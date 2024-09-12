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
using Trade360SDK.Feed.Converters;
using Trade360SDK.Feed.Configuration;
using Trade360SDK.Feed.RabbitMQ.Handlers;

namespace Trade360SDK.Feed.RabbitMQ.Consumers
{
    internal class MessageConsumer : AsyncDefaultBasicConsumer
    {
        private readonly ConcurrentDictionary<int, IMessageTypeHandler> _bodyHandlers = new ConcurrentDictionary<int, IMessageTypeHandler>();
        private readonly RmqConnectionSettings _settings;
        private readonly ILogger _logger;

        public MessageConsumer(ILoggerFactory? loggerFactory, RmqConnectionSettings settings)
        {
            _settings = settings;
            _logger = (loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory))).CreateLogger(this.GetType());
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
                wrappedMessage.Header.SourceTimestamp = DateTimeOffset.FromUnixTimeSeconds(properties.Timestamp.UnixTime).UtcDateTime;

                var entityType = wrappedMessage.Header.Type;
                if (!_bodyHandlers.TryGetValue(entityType, out IMessageTypeHandler messageTypeHandler))
                {
                    HandleUnknownEntityType(entityType);
                    return;
                }

                await messageTypeHandler.ProcessAsync(wrappedMessage.Body, wrappedMessage.Header);
                
                if (_settings.AutoAck == false)
                    Model.BasicAck(deliveryTag, false);
            }
            catch (JsonException jsonEx)
            {
                _logger.LogError(jsonEx, "JSON processing error");

                if (_settings.AutoAck == false)
                    Model.BasicReject(deliveryTag, true);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error handling message delivery: {ex.Message}");

                if (_settings.AutoAck == false)
                    Model.BasicReject(deliveryTag, true);
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

            var newBodyHandler = new MessageTypeHandler<TEntity>(entityHandler, _logger);
            _bodyHandlers[entityAttribute.EntityKey] = newBodyHandler;
        }

        private void HandleUnknownEntityType(int entityType)
        {
            var missedEntityType = Assembly.GetExecutingAssembly().GetTypes()
                .FirstOrDefault(x => x.Namespace == "Trade360SDK.Feed.Entities" && x.GetCustomAttribute<Trade360EntityAttribute>()?.EntityKey == entityType);

            _logger.LogWarning(missedEntityType != null
                ? $"Handler for {missedEntityType.FullName} is not configured"
                : $"Received unknown entity type {entityType}");
        }
    }
}
