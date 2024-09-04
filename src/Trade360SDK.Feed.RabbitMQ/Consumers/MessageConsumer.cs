using System;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using Trade360SDK.Common.Attributes;
using Trade360SDK.Common.Entities.Enums;
using Trade360SDK.Feed.Converters;
using Trade360SDK.Feed.RabbitMQ.Resolvers;

namespace Trade360SDK.Feed.RabbitMQ.Consumers
{
    internal class MessageConsumer : AsyncDefaultBasicConsumer
    {
        private readonly ILogger _logger;
        private readonly IHandlerTypeResolver _handlerTypeResolver;

        public MessageConsumer(IHandlerTypeResolver handlerTypeResolver, FlowType type, ILoggerFactory? loggerFactory)
        {
            _handlerTypeResolver = (handlerTypeResolver ?? throw new ArgumentNullException(nameof(handlerTypeResolver)));
            _logger =
                (loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory))).CreateLogger(this.GetType());
        }

        public override async Task HandleBasicDeliver(string consumerTag, ulong deliveryTag, bool redelivered,
            string exchange, string routingKey, IBasicProperties properties, ReadOnlyMemory<byte> body)
        {
            try
            {
                var rawMessage = Encoding.UTF8.GetString(body.Span);
                var wrappedMessage = JsonWrappedMessageObjectConverter.ConvertJsonToMessage(rawMessage);

                if (wrappedMessage.Header == null)
                {
                    _logger.LogError("Invalid message format");
                    return;
                }

                wrappedMessage.Header.ReceivedTimestamp = DateTime.UtcNow;
                wrappedMessage.Header.SourceTimestamp =
                    DateTimeOffset.FromUnixTimeSeconds(properties.Timestamp.UnixTime).UtcDateTime;

                var entityType = wrappedMessage.Header.Type;

                Type type = IdentifyType(entityType);

                IHandler handler = IdentifyHandler(entityType, type);
                
                var entity = wrappedMessage.Body != null ? JsonSerializer.Deserialize(wrappedMessage.Body, type) : null;
                
                await handler.ProcessAsync(entity, wrappedMessage.Header);
            }
            catch (JsonException jsonEx)
            {
                _logger.LogError(jsonEx, "JSON processing error");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error handling message delivery: {ex.Message}");
            }
        }

        private Type IdentifyType(int entityType)
        {
            return _handlerTypeResolver.ResolveMessageType(entityType);
        }

        private IHandler IdentifyHandler(int entityType, Type type)
        {
            try
            {
                return _handlerTypeResolver.GetHandler(type);
            }
            catch (Exception e)
            {
                HandleUnknownEntityType(entityType);
                return null;
            }
        }

        private void HandleUnknownEntityType(int entityType)
        {
            var missedEntityType = Assembly.GetExecutingAssembly().GetTypes()
                .FirstOrDefault(x =>
                    x.Namespace == "Trade360SDK.Feed.Entities" &&
                    x.GetCustomAttribute<Trade360EntityAttribute>()?.EntityKey == entityType);

            _logger.LogWarning(missedEntityType != null
                ? $"Handler for {missedEntityType.FullName} is not configured"
                : $"Received unknown entity type {entityType}");
        }
    }
}