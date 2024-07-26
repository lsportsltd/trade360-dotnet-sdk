using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using Trade360SDK.Common.Attributes;
using Trade360SDK.Common.Models;
using Trade360SDK.Feed.RabbitMQ.Handlers;
using Trade360SDK.Feed.RabbitMQ.Interfaces;

namespace Trade360SDK.Feed.RabbitMQ.Consumers
{
    internal class MessageConsumer : AsyncDefaultBasicConsumer
    {
        private readonly ConcurrentDictionary<int, IBodyHandler> _bodyHandlers = new ConcurrentDictionary<int, IBodyHandler>();
        private readonly ILogger _logger;

        public MessageConsumer(ILoggerFactory? loggerFactory)
        {
            _logger = (loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory))).CreateLogger(this.GetType());
        }

        public override async Task HandleBasicDeliver(string consumerTag, ulong deliveryTag, bool redelivered, string exchange, string routingKey, IBasicProperties properties, ReadOnlyMemory<byte> body)
        {
            try
            {
                var rawMessage = Encoding.UTF8.GetString(body.Span);

                var wrappedMessageJsonObject = JsonWrappedMessageJsonObjectConverter.ConvertJsonToMessage(rawMessage);

                var header = JsonSerializer.Deserialize<MessageHeader>(wrappedMessageJsonObject.Header);

                var wrappedMessage = new WrappedMessage
                {
                    Header = header,
                    Body = wrappedMessageJsonObject.Body
                };

                if (wrappedMessage.Header == null)
                {
                    _logger.LogError("Invalid message format");
                    return;
                }

                var entityType = wrappedMessage.Header.Type;
                if (!_bodyHandlers.TryGetValue(entityType, out IBodyHandler bodyHandler))
                {
                    var missedEntityType = Assembly.GetExecutingAssembly().GetTypes()
                        .FirstOrDefault(x => x.Namespace == "Trade360SDK.Feed.Entities" && x.GetCustomAttribute<Trade360EntityAttribute>()?.EntityKey == entityType);
                    if (missedEntityType != null)
                    {
                        _logger.LogWarning($"Handler for {missedEntityType.FullName} is not configured");
                    }
                    else
                    {
                        _logger.LogWarning($"Received unknown entity type {entityType}");
                    }
                    return;
                }

                await bodyHandler.ProcessAsync(wrappedMessage.Body);
            }
            catch (JsonException jsonEx)
            {
                _logger.LogError(jsonEx, "JSON processing error");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error handling message delivery Exception: {ex.Message} , StackTrace: {ex.StackTrace}");
            }
        }

        public void RegisterEntityHandler<TEntity>(IEntityHandler<TEntity> entityHandler) where TEntity : new()
        {
            var entityType = typeof(TEntity);
            var entityAttribute = entityType.GetCustomAttribute<Trade360EntityAttribute>();
            if (entityAttribute == null)
            {
                throw new InvalidOperationException($"{entityType.FullName} isn't trade360 entity. You should use only entities from Trade360SDK.Feed.Entities namespace");
            }

            var newBodyHandler = new BodyHandler<TEntity>(entityHandler, _logger);
            _bodyHandlers[entityAttribute.EntityKey] = newBodyHandler;
        }
    }
    
    internal class JsonWrappedMessageJsonObjectConverter
    {
        public static WrappedMessageJsonObject ConvertJsonToMessage(string rawJson)
        {
            using JsonDocument doc = JsonDocument.Parse(rawJson);
            JsonElement root = doc.RootElement;
            WrappedMessageJsonObject message = new WrappedMessageJsonObject
            {
                Header = root.GetProperty("Header").ToString(),
                Body = root.TryGetProperty("Body", out JsonElement bodyElement) ? bodyElement.ToString() : null
            };
            return message;
        }
    }

    internal class WrappedMessageJsonObject
    {
        public string? Header { get; set; }
        
        public string? Body { get; set; }
    }
}
