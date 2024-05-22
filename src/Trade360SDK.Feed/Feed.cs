using System;
using RabbitMQ.Client;

namespace Trade360SDK.Feed
{
    public class Feed : IDisposable
    {
        private const int Port = 5672;

        private readonly ConnectionFactory _connectionFactory;
        private readonly int _packageId;
        private readonly int _prefetchCount;

        private IConnection? _connection;
        private IModel? _channel;
        private string? _consumerTag;

        public Feed(
            string rmqHost,
            string username, string password,
            int packageId,
            int prefetchCount, TimeSpan recoveryTime)
        {
            _connectionFactory = new ConnectionFactory
            {
                HostName = rmqHost,
                Port = 5672,
                UserName = username,
                Password = password,
                DispatchConsumersAsync = true,
                VirtualHost = "StmInPlay",
                AutomaticRecoveryEnabled = true,
                RequestedHeartbeat = TimeSpan.FromSeconds(60),
                NetworkRecoveryInterval = recoveryTime,
            };

            _packageId = packageId;
            _prefetchCount = prefetchCount;
        }

        public void Start()
        {
            _connection = _connectionFactory.CreateConnection();
            if (!_connection.IsOpen)
            {
                throw new Exception("Failed to connect");
            }
            _channel = _connection.CreateModel();

            _consumerTag = _channel.BasicConsume(
                queue: $"_{_packageId}_",
                autoAck: true,
                consumer: null);
        }

        public void Stop()
        {
            _channel?.BasicCancel(_consumerTag);
            _connection?.Close();
        }

        public void Dispose()
        {
            _connection?.Dispose();
            _channel?.Dispose();
        }
    }
}
