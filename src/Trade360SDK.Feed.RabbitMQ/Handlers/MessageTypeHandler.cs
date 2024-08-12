using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Trade360SDK.Common.Models;

namespace Trade360SDK.Feed.RabbitMQ.Handlers
{
    internal class MessageTypeHandler<T> : IMessageTypeHandler where T : new()
    {
        private readonly IEntityHandler<T> _entityHandler;
        private readonly ILogger _logger;
        private readonly string _messageFormat;

        public MessageTypeHandler(IEntityHandler<T> entityHandler, ILogger logger, string? messageFormat)
        {
            _entityHandler = entityHandler ?? throw new ArgumentNullException(nameof(entityHandler));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _messageFormat = messageFormat ?? throw new ArgumentNullException(nameof(messageFormat));
        }

        public Task ProcessAsync(string? body, MessageHeader header)
        {
            var entity = default(T);

            try
            {
                if (body != null)
                {
                    if (_messageFormat.Equals("json", StringComparison.OrdinalIgnoreCase))
                    {
                        entity = JsonSerializer.Deserialize<T>(body);
                    }
                    else if (_messageFormat.Equals("xml", StringComparison.OrdinalIgnoreCase))
                    {
                        var serializer = new XmlSerializer(typeof(T));
                        using var reader = new StringReader(body);
                        entity = (T)serializer.Deserialize(reader);
                    }
                }
                else
                {
                    entity = new T();
                }
            }
            catch (Exception e)
            {
                _logger.LogWarning($"Failed to deserialize {typeof(T).Name} entity, Due to: {e.Message}");
            }

            if (entity == null)
            {
                _logger.LogError($"Failed to deserialize {typeof(T).Name} entity");
                return Task.CompletedTask;
            }

            return _entityHandler.ProcessAsync(entity, header);
        }
    }
}