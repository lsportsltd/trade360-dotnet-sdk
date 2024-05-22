using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using RabbitMQ.Client;
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


    }
}
