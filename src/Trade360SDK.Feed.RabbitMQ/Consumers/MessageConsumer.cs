using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using Trade360SDK.Common.Attributes;
using Trade360SDK.Common.Entities.Enums;
using Trade360SDK.Common.Helpers;
using Trade360SDK.Feed.Converters;
using Trade360SDK.Feed.Configuration;
using Trade360SDK.Feed.RabbitMQ.Resolvers;

namespace Trade360SDK.Feed.RabbitMQ.Consumers
{
    internal class MessageConsumer : AsyncDefaultBasicConsumer
    {
        private readonly ILogger _logger;
        private readonly IMessageProcessorContainer _messageProcessorContainer;
        private readonly RmqConnectionSettings _settings;
        private readonly Dictionary<int, Type> _messageUpdateTypes;
        private readonly DateTime unixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        public MessageConsumer(IMessageProcessorContainer messageProcessorContainer, RmqConnectionSettings settings, ILoggerFactory? loggerFactory)
        {
            _messageUpdateTypes = Trade360AttributeHelper.GetAttributes()
                .ToDictionary(t => t.GetCustomAttribute<Trade360EntityAttribute>().EntityKey, t => t);
            _messageProcessorContainer = (messageProcessorContainer ?? throw new ArgumentNullException(nameof(messageProcessorContainer)));
            _settings = settings;
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

                if (properties.Headers.TryGetValue("ServerTimestamp", out var timestampObj) && timestampObj != null)
                {
                    // Directly cast to long since you know it's always a long
                    var rmqTimestampInMs = (long)timestampObj;
                    var platformTimestamp = DateTimeOffset.FromUnixTimeMilliseconds(rmqTimestampInMs).UtcDateTime;
                    wrappedMessage.Header.MessageBrokerTimestamp = platformTimestamp;
                }

                wrappedMessage.Header.MessageTimestamp = DateTime.UtcNow;
                
                var id = wrappedMessage.Header.Type;
                
                Type? type = IdentifyType(id);

                if (type != null)
                {
                    var messageProcessor = _messageProcessorContainer.GetMessageProcessor(id);
                    await messageProcessor.ProcessAsync(type, wrappedMessage?.Header, wrappedMessage?.Body);
                }
                
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

        private Type? IdentifyType(int entityType)
        {
            _messageUpdateTypes.TryGetValue(entityType, out var messageType);
            if (messageType == null)
            {
                HandleUnknownEntityType(entityType);
            }

            return messageType;
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
