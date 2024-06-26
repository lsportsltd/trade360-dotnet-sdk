using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using RabbitMQ.Client;
using Trade360SDK.Feed.Attributes;
using Trade360SDK.Feed.RabbitMQ.Handlers;
using Trade360SDK.Feed.RabbitMQ.Interfaces;
using Trade360SDK.Feed.RabbitMQ.Models;

namespace Trade360SDK.Feed.RabbitMQ.Consumers
{
    internal class MessageConsumer : AsyncDefaultBasicConsumer
    {
        private readonly ConcurrentDictionary<int, IBodyHandler> bodyHandlers = new ConcurrentDictionary<int, IBodyHandler>();
        private readonly ILogger? _logger;

        public MessageConsumer(ILogger? logger)
        {
            _logger = logger;
        }

        public override async Task HandleBasicDeliver(string consumerTag, ulong deliveryTag, bool redelivered, string exchange, string routingKey, IBasicProperties properties, ReadOnlyMemory<byte> body)
        {
            var rawMessage = Encoding.UTF8.GetString(body.Span);

            var wrappedMessageJsonObject = JsonWrappedMessageJsonObjectConverter
                .ConvertJsonToMessage(rawMessage);
            
            var header = JsonSerializer
                .Deserialize<MessageHeader>(wrappedMessageJsonObject.Header);
            
            var wrappedMessage = new WrappedMessage
            {
                Header = header,
                Body = wrappedMessageJsonObject.Body
            };
            
            if (wrappedMessage == null || wrappedMessage.Header == null)
            {
                _logger?.WriteError("Invalid message format");
                return;
            }

            var entityType = wrappedMessage.Header.Type;
            if (!bodyHandlers.TryGetValue(entityType, out IBodyHandler bodyHandler))
            {
                var missedEntityType = Assembly.GetExecutingAssembly().GetTypes()
                    .FirstOrDefault(x => x.Namespace == "Trade360SDK.Feed.Entities" && x.GetCustomAttribute<Trade360EntityAttribute>()?.EntityKey == entityType);
                if (missedEntityType != null)
                {
                    // _logger?.WriteWarning($"Handler for {missedEntityType.FullName} is not configured");
                }
                else
                {
                    //_logger?.WriteError($"Received unknown entity type {entityType}");
                }
                return;
            }

            await bodyHandler.ProcessAsync(wrappedMessage.Body);
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
    
    internal class JsonWrappedMessageJsonObjectConverter
    {
        public static WrappedMessageJsonObject ConvertJsonToMessage(string rawJson)
        {
            using (JsonDocument doc = JsonDocument.Parse(rawJson))
            {
                JsonElement root = doc.RootElement;
                WrappedMessageJsonObject message = new WrappedMessageJsonObject
                {
                    Header = root.GetProperty("Header").ToString(),
                    Body = root.GetProperty("Body").ToString()
                };
                return message;
            }
        }
    }

    internal class WrappedMessageJsonObject
    {
        public string Header { get; set; }
        
        public string Body { get; set; }
    }
}
