﻿using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using RabbitMQ.Client;
using Trade360SDK.Common;
using Trade360SDK.Common.Enums;
using Trade360SDK.Feed.RabbitMQ.Consumers;

namespace Trade360SDK.Feed.RabbitMQ
{
    public class RabbitMQFeed : BaseHttpClient, IFeed, IDisposable
    {
        private const int Port = 5672;

        private readonly ConnectionFactory _connectionFactory;
        private readonly int _packageId;
        private readonly int _prefetchCount;
        private readonly MessageConsumer _consumer;

        private IConnection? _connection;
        private IModel? _channel;
        private string? _consumerTag;

        public RabbitMQFeed(
            string rmqHost,
            string rmqVhost,
            int rmqPort,
            
            string username,
            string password,
            
            int prefetchCount, 
            TimeSpan recoveryTime,
            
            int packageId,
            string customersApi,
            ILogger logger) 
            : base(customersApi, packageId, username, password)
        {
            _connectionFactory = new ConnectionFactory
            {
                HostName = rmqHost,
                Port = rmqPort,
                VirtualHost = rmqVhost,
                
                UserName = username,
                Password = password,
                
                RequestedHeartbeat = TimeSpan.FromSeconds(60),
                NetworkRecoveryInterval = recoveryTime,
                
                DispatchConsumersAsync = true,
                AutomaticRecoveryEnabled = true,
            };

            _packageId = packageId;
            _prefetchCount = prefetchCount;

            _consumer = new MessageConsumer(logger);
        }

        public RabbitMQFeed(
            HttpClient httpClient,
            string rmqHost,
            string username, string password,
            int packageId, PackageType packageType,
            int prefetchCount, TimeSpan recoveryTime,
            ILogger logger) : base(httpClient, packageId, username, password)
        {
            _connectionFactory = new ConnectionFactory
            {
                HostName = rmqHost,
                Port = 5672,
                UserName = username,
                Password = password,
                DispatchConsumersAsync = true,
                VirtualHost = packageType == PackageType.InPlay ? "StmInPlay" : "StmPreMatch",
                AutomaticRecoveryEnabled = true,
                RequestedHeartbeat = TimeSpan.FromSeconds(60),
                NetworkRecoveryInterval = recoveryTime,
            };

            _packageId = packageId;
            _prefetchCount = prefetchCount;

            _consumer = new MessageConsumer(logger);
        }

        public void AddEntityHandler<TEntity>(IEntityHandler<TEntity> entityHandler)
        {
            _consumer.RegisterEntityHandler(entityHandler);
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            // need to validate distribution status and activation status,
            // in case fail to send request - package activation  - contact support, 
            // in case the Distribution disabled, start distribution then connect.
            
            _connection = _connectionFactory
                .CreateConnection();
            
            if (!_connection.IsOpen)
            {
                throw new Exception("Failed to connect");
            }
            
            _channel = _connection
                .CreateModel();
            
            _consumer.Model = _channel;

            _consumerTag = _channel
                .BasicConsume(
                    queue: $"_{_packageId}_",
                    autoAck: true,
                    _consumer);
        }

        public async Task StopAsync(CancellationToken cancellationToken)
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