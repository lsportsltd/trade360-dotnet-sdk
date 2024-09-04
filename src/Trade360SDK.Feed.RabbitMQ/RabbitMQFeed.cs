﻿using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using System;
using System.Threading;
using System.Threading.Tasks;
using Trade360SDK.Common.Configuration;
using Trade360SDK.Common.Entities.Enums;
using Trade360SDK.CustomersApi.Interfaces;
using Trade360SDK.Feed.Configuration;
using Trade360SDK.Feed.RabbitMQ.Consumers;
using Trade360SDK.Feed.RabbitMQ.Exceptions;
using Trade360SDK.Feed.RabbitMQ.Resolvers;
using Trade360SDK.Feed.RabbitMQ.Validators;

namespace Trade360SDK.Feed.RabbitMQ
{
    public class RabbitMqFeed : IFeed
    {
        private readonly MessageConsumer _consumer;
        private readonly ILogger _logger;
        private IConnection? _connection;
        private IModel? _channel;
        private string? _consumerTag;
        private readonly RmqConnectionSettings _settings;
        private readonly IPackageDistributionApiClient _packageDistributionApiClient;
        private readonly IHandlerTypeResolver _handlerTypeResolver;

        public RabbitMqFeed(RmqConnectionSettings settings, IHandlerTypeResolver handlerTypeResolver, FlowType flowType, ILoggerFactory loggerFactory,
            ICustomersApiFactory customersApiFactory)
        {
            _logger = (loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory))).CreateLogger(this.GetType());
            _consumer = new MessageConsumer(handlerTypeResolver, flowType, loggerFactory);
            _settings = settings ?? throw new ArgumentNullException(nameof(settings));
            // Validate settings
            RmqConnectionSettingsValidator.Validate(_settings);
            _packageDistributionApiClient = customersApiFactory.CreatePackageDistributionHttpClient(settings.BaseCustomersApi, new PackageCredentials()
            {
                PackageId = settings.PackageId,
                Password = settings.Password,
                Username = settings.UserName
            });
        }

        public async Task StartAsync(bool connectAtStart, CancellationToken cancellationToken)
        {
            try
            {
                if (connectAtStart)
                {
                    await EnsureDistributionStartedAsync(cancellationToken);
                }

                _connection = CreateConnection(_settings);

                if (!_connection.IsOpen)
                {
                    _logger.LogError("Failed to connect to RabbitMQ.");
                    throw new InvalidOperationException("Failed to connect to RabbitMQ.");
                }

                _channel = _connection.CreateModel();
                _channel.BasicQos(prefetchSize: 0, prefetchCount: _settings.PrefetchCount, global: false);

                _consumer.Model = _channel;

                _consumerTag = _channel.BasicConsume(
                    queue: $"_{_settings.PackageId}_",
                    autoAck: _settings.AutoAck,
                    consumer: _consumer);

                _logger.LogInformation("Connected to RabbitMQ and started consuming.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error starting RabbitMQFeed.");
                throw new RabbitMqFeedException("An error occurred while starting the RabbitMQ feed.", ex);
            }

        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            try
            {
                if (_channel != null && !string.IsNullOrEmpty(_consumerTag))
                {
                    _channel.BasicCancel(_consumerTag);
                }

                _connection?.Close();
                _logger.LogInformation("RabbitMQ connection closed.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error stopping RabbitMQFeed.");
                throw new RabbitMqFeedException("An error occurred while stopping the RabbitMQ feed.", ex);
            }

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            try
            {
                _logger.LogInformation("Disposing RabbitMQ resources...");
                _connection?.Dispose();
                _channel?.Dispose();
                _logger.LogInformation("RabbitMQFeed disposed successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while disposing RabbitMQFeed resources. Ensure that all resources are properly released.");
                throw new RabbitMqFeedException("An error occurred while disposing the RabbitMQ feed. See inner exception for details.", ex);
            }
        }

        private async Task EnsureDistributionStartedAsync(CancellationToken cancellationToken)
        {
            const int maxRetries = 5;
            const int delayMilliseconds = 2000;

            for (int attempt = 0; attempt < maxRetries; attempt++)
            {
                var result = await _packageDistributionApiClient.GetDistributionStatusAsync(cancellationToken);
                if (result.IsDistributionOn)
                {
                    _logger.LogInformation("Distribution is already on.");
                    return;
                }

                _logger.LogInformation("Distribution is off. Attempting to start...");
                await _packageDistributionApiClient.StartDistributionAsync(cancellationToken);

                await Task.Delay(delayMilliseconds, cancellationToken);

                result = await _packageDistributionApiClient.GetDistributionStatusAsync(cancellationToken);
                if (result.IsDistributionOn)
                {
                    _logger.LogInformation("Successfully started distribution.");
                    return;
                }

                _logger.LogWarning($"Attempt {attempt + 1} to start distribution failed.");
            }

            throw new InvalidOperationException("Failed to start distribution after multiple attempts.");
        }

        private IConnection CreateConnection(RmqConnectionSettings settings)
        {
            var factory = new ConnectionFactory
            {
                HostName = settings.Host,
                Port = settings.Port,
                VirtualHost = settings.VirtualHost,
                UserName = settings.UserName,
                Password = settings.Password,
                RequestedHeartbeat = TimeSpan.FromSeconds(settings.RequestedHeartbeatSeconds),
                NetworkRecoveryInterval = TimeSpan.FromSeconds(settings.NetworkRecoveryInterval),
                DispatchConsumersAsync = settings.DispatchConsumersAsync,
                AutomaticRecoveryEnabled = settings.AutomaticRecoveryEnabled
            };

            return factory.CreateConnection();
        }
    }
}
