using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using System;
using System.Threading;
using System.Threading.Tasks;
using Trade360SDK.Common.Configuration;
using Trade360SDK.Common.Entities.Enums;
using Trade360SDK.CustomersApi.Entities.MetadataApi.Responses;
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
        private readonly CancellationTokenSource _cts = new CancellationTokenSource();
        private readonly ConnectionFactory _factory;
        private readonly IHandlerTypeResolver _handlerTypeResolver;
        private bool _isReconnecting; // Flag to prevent multiple reconnections
        private readonly object _reconnectionLock = new object(); // Lock for thread safety

        public RabbitMqFeed(RmqConnectionSettings settings, Trade360Settings trade360Settings, IHandlerTypeResolver handlerTypeResolver, FlowType flowType, ILoggerFactory loggerFactory,
            ICustomersApiFactory customersApiFactory)
        {
            _logger = (loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory))).CreateLogger(this.GetType());
            _consumer = new MessageConsumer(handlerTypeResolver, settings, loggerFactory);
            _settings = settings ?? throw new ArgumentNullException(nameof(settings));

            // Validate settings
            RmqConnectionSettingsValidator.Validate(_settings);
            
            if (trade360Settings != null)
            {
                _packageDistributionApiClient = customersApiFactory.CreatePackageDistributionHttpClient(
                    trade360Settings.CustomersApiBaseUrl, new PackageCredentials()
                    {
                        PackageId = flowType == FlowType.InPlay
                            ? trade360Settings.InplayPackageCredentials.PackageId
                            : flowType == FlowType.PreMatch ? trade360Settings.PrematchPackageCredentials.PackageId : throw new ArgumentException("Not recognized flow type"),
                        Password = flowType == FlowType.InPlay
                            ? trade360Settings.InplayPackageCredentials.Password
                            : flowType == FlowType.PreMatch ? trade360Settings.PrematchPackageCredentials.Password : throw new ArgumentException("Not recognized flow type"),
                        Username = flowType == FlowType.InPlay
                            ? trade360Settings.InplayPackageCredentials.Username
                            : flowType == FlowType.PreMatch ? trade360Settings.PrematchPackageCredentials.Username : throw new ArgumentException("Not recognized flow type")
                    });
            }
            
            // Initialize connection factory
            _factory = new ConnectionFactory
            {
                HostName = _settings.Host,
                Port = _settings.Port,
                VirtualHost = _settings.VirtualHost,
                UserName = _settings.UserName,
                Password = _settings.Password,
                RequestedHeartbeat = TimeSpan.FromSeconds(_settings.RequestedHeartbeatSeconds),
                NetworkRecoveryInterval = TimeSpan.FromSeconds(_settings.NetworkRecoveryInterval),
                DispatchConsumersAsync = _settings.DispatchConsumersAsync,
                AutomaticRecoveryEnabled = true, // Enable automatic connection recovery
                TopologyRecoveryEnabled = true // Disable topology recovery to catch the event ourselves
            };

            CreateAndSetupConnection();
        }

        public async Task StartAsync(bool connectAtStart, CancellationToken cancellationToken)
        {
            try
            {
                if (connectAtStart)
                {
                    if (_packageDistributionApiClient == null)
                    {
                        throw new ArgumentException("No CustomersApi configuration specified. See CustomersApi sample service.");
                    }
                    await EnsureDistributionStartedAsync(cancellationToken);
                }

                _consumerTag = _channel.BasicConsume(
                    queue: $"_{_settings.PackageId}_",
                    autoAck: _settings.AutoAck,
                    consumer: _consumer);

                _logger.LogInformation("Connected to RabbitMQ and started consuming.");
            }
            catch (OperationCanceledException)
            {
                _logger.LogInformation("RabbitMQ feed start operation was canceled.");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error starting RabbitMQFeed.");
                throw new RabbitMqFeedException("An error occurred while starting the RabbitMQ feed.", ex);
            }
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            try
            {
                _cts.Cancel(); // Cancel any ongoing recovery attempts
                
                if (_channel != null && !string.IsNullOrEmpty(_consumerTag))
                {
                    _channel.BasicCancel(_consumerTag);
                }

                if (_connection?.IsOpen == true)
                {
                    _connection.Close(); // Close the connection before disposing
                }

                _logger.LogInformation("RabbitMQ connection closed.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error stopping RabbitMQFeed.");
                throw new RabbitMqFeedException("An error occurred while stopping the RabbitMQ feed.", ex);
            }

            await Task.CompletedTask;
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
                if (await GetDistributionEnabled("Distribution is already on.", cancellationToken)) return;

                if (cancellationToken.IsCancellationRequested)
                {
                    _logger.LogInformation("Distribution start operation was canceled.");
                    cancellationToken.ThrowIfCancellationRequested();
                }

                _logger.LogInformation("Distribution is off. Attempting to start...");
                
                await StartDistribution(cancellationToken);

                await Task.Delay(delayMilliseconds, cancellationToken);
                
                if (await GetDistributionEnabled("Successfully started distribution.", cancellationToken)) return;

                _logger.LogWarning($"Attempt {attempt + 1} to start distribution failed.");
            }

            throw new InvalidOperationException("Failed to start distribution after multiple attempts.");
        }


        private async Task StartDistribution(CancellationToken cancellationToken)
        {
            try
            {
                await _packageDistributionApiClient.StartDistributionAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed StartDistribution. {ex}");
            }
        }

        private async Task<bool> GetDistributionEnabled(string successfulLogMessage, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _packageDistributionApiClient.GetDistributionStatusAsync(cancellationToken);
                if (result.IsDistributionOn)
                {
                    _logger.LogInformation(successfulLogMessage);
                    return true;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Got inappropriate GetDistributionEnabled response. Check configuration. {ex}");
            }
            return false;
        }
        
        private void CreateAndSetupConnection()
        {
            _connection = _factory.CreateConnection();
            _connection.ConnectionShutdown += OnConnectionShutdown;

            // Create and configure the channel
            _channel = _connection.CreateModel();

            _channel.BasicQos(prefetchSize: 0, prefetchCount: _settings.PrefetchCount, global: false);
            _consumer.Model = _channel;
        }
        
        private void OnConnectionShutdown(object? sender, ShutdownEventArgs e)
        {
            if (e.ReplyCode == 200) // Normal shutdown
            {
                _logger.LogInformation("Connection closed by server.");
                return;
            }

            _logger.LogWarning($"Connection shutdown. ReplyCode: {e.ReplyCode}, ReplyText: {e.ReplyText}");
            _ = RetryConnectionAsync();
        }

        private async Task RetryConnectionAsync()
        {
            lock (_reconnectionLock)
            {
                if (_isReconnecting) return; // Already reconnecting, exit to prevent multiple attempts
                _isReconnecting = true;
            }

            const int maxRetries = 12; // Retry for 2 minutes (12 * 10 seconds)
            const int delayMilliseconds = 10000; // 10-second delay between retries

            for (var attempt = 0; attempt < maxRetries; attempt++)
            {
                if (_cts.Token.IsCancellationRequested)
                {
                    _logger.LogInformation("RabbitMQ feed recovery operation was canceled.");
                    _isReconnecting = false;
                    return;
                }

                try
                {
                    CreateAndSetupConnection();

                    _consumerTag = _channel.BasicConsume(
                        queue: $"_{_settings.PackageId}_",
                        autoAck: _settings.AutoAck,
                        consumer: _consumer);

                    _logger.LogInformation("Reconnected to RabbitMQ and resumed consuming.");
                    _isReconnecting = false;
                    return;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Attempt {attempt + 1} to reconnect to RabbitMQ failed. Retrying in {delayMilliseconds / 1000} seconds...");
                }

                await Task.Delay(delayMilliseconds, _cts.Token);
            }

            _logger.LogError("Failed to reconnect to RabbitMQ after multiple attempts.");
            _isReconnecting = false;
            throw new RabbitMqFeedException("Failed to reconnect to RabbitMQ after multiple attempts.");
        }
    }
}
