using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using Trade360SDK.Common.Attributes;
using Trade360SDK.Common.Models;
using Trade360SDK.Feed.Converters;
using Trade360SDK.Feed.RabbitMQ.Handlers;

namespace Trade360SDK.Feed.RabbitMQ.Consumers
{
    internal class MessageConsumer : AsyncDefaultBasicConsumer
    {
        private readonly ConcurrentDictionary<int, IMessageTypeHandler> _bodyHandlers = new ConcurrentDictionary<int, IMessageTypeHandler>();
        private readonly ILogger _logger;
        private readonly string? _messageFormat;

        public MessageConsumer(ILoggerFactory? loggerFactory, string? messageFormat)
        {
            _logger = (loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory))).CreateLogger(this.GetType());
            _messageFormat = messageFormat;
        }

        public override async Task HandleBasicDeliver(string consumerTag, ulong deliveryTag, bool redelivered,
            string exchange, string routingKey, IBasicProperties properties, ReadOnlyMemory<byte> body)
        {
            try
            {
                var rawMessage = Encoding.UTF8.GetString(body.Span);
                WrappedMessage wrappedMessage = null!;

                if (_messageFormat.Equals("json", StringComparison.OrdinalIgnoreCase))
                {
                    wrappedMessage = JsonWrappedMessageObjectConverter.ConvertJsonToMessage(rawMessage);
                }
                else if (_messageFormat.Equals("xml", StringComparison.OrdinalIgnoreCase))
                {
                    wrappedMessage = XmlWrappedMessageObjectConverter.ConvertXmlToMessage(rawMessage);
                }

                if (wrappedMessage?.Header == null)
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
            }
            catch (JsonException jsonEx)
            {
                _logger.LogError(jsonEx, "JSON processing error");
            }
            catch (InvalidOperationException xmlEx) when (xmlEx.InnerException is XmlException)
            {
                _logger.LogError(xmlEx, "XML processing error");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error handling message delivery: {ex.Message}");
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

            var newBodyHandler = new MessageTypeHandler<TEntity>(entityHandler, _logger, _messageFormat);
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
