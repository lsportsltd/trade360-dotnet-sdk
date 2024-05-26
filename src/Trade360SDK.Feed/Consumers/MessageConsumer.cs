using System;
using System.Collections.Concurrent;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using RabbitMQ.Client;
using Trade360SDK.Feed.Attributes;
using Trade360SDK.Feed.Handlers;
using Trade360SDK.Feed.Interfaces;
using Trade360SDK.Feed.Models;

namespace Trade360SDK.Feed.Consumers
{
    internal class MessageConsumer : AsyncDefaultBasicConsumer
    {
        private readonly ConcurrentDictionary<int, IBodyHandler> bodyHandlers = new ConcurrentDictionary<int, IBodyHandler>();
        private readonly ILogger? _logger;

        public MessageConsumer(IModel model, ILogger? logger) : base(model)
        {
            _logger = logger;
        }

        public override async Task HandleBasicDeliver(string consumerTag, ulong deliveryTag, bool redelivered, string exchange, string routingKey, IBasicProperties properties, ReadOnlyMemory<byte> body)
        {
            var rawMessage = Encoding.UTF8.GetString(body.Span);
            var wrappedMessage = JsonSerializer.Deserialize<WrappedMessage>(rawMessage);
            if (wrappedMessage == null || wrappedMessage.Header == null)
            {
                _logger?.WriteError("Invalid message format");
                return;
            }

            var entityType = wrappedMessage.Header.Type;
            if (!bodyHandlers.TryGetValue(entityType, out IBodyHandler bodyHandler))
            {
                _logger?.WriteWarning($"Handler for {entityType} not configured");
                return;
            }

            await bodyHandler.Process(wrappedMessage.Body);
        }

        public void RegisterEntityHandler<TEntity>(IEntityHandler<TEntity> entityHandler)
        {
            var entityType = typeof(TEntity);
            var entityAttribute = entityType.GetCustomAttribute<Trade360EntityAttribute>();
            if (entityAttribute == null)
            {
                throw new InvalidOperationException($"{entityType.FullName} isn't trade360 entity. You should use only entities from Trade360SDK.Feed.Entities namespace");
            }

            var newBodyHandler = new BodyHandler<TEntity>(entityHandler, _logger);
            bodyHandlers.AddOrUpdate(entityAttribute.EntityKey, (_) => newBodyHandler, (_, __) => newBodyHandler);
        }
    }
}
