using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using System.Threading.Tasks;
using System.Threading;
using System;
using Trade360SDK.Feed.Configuration;
using Trade360SDK.Feed.RabbitMQ.Consumers;
using Trade360SDK.Feed.RabbitMQ.Exceptions;
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

        public RabbitMqFeed(RmqConnectionSettings settings, ILoggerFactory loggerFactory)
        {
            _logger = (loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory))).CreateLogger(this.GetType());
            _consumer = new MessageConsumer(loggerFactory);
            _settings = settings ?? throw new ArgumentNullException(nameof(settings));
            // Validate settings
            RmqConnectionSettingsValidator.Validate(_settings);
        }

        public void AddEntityHandler<TEntity>(IEntityHandler<TEntity> entityHandler)
        {
            _consumer.RegisterEntityHandler(entityHandler);
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            try
            {
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
                    _consumer);

                _logger.LogInformation("Connected to RabbitMQ and started consuming.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error starting RabbitMQFeed.");
                throw new RabbitMQFeedException("An error occurred while starting the RabbitMQ feed.", ex);
            }

            return Task.CompletedTask;
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
                throw new RabbitMQFeedException("An error occurred while stopping the RabbitMQ feed.", ex);
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
                throw new RabbitMQFeedException("An error occurred while disposing the RabbitMQ feed. See inner exception for details.", ex);
            }
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
