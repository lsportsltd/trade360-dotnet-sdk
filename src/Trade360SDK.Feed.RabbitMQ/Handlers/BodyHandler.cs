using Microsoft.Extensions.Logging;
using System;
using System.Text.Json;
using System.Threading.Tasks;
using Trade360SDK.Feed.RabbitMQ.Interfaces;

namespace Trade360SDK.Feed.RabbitMQ.Handlers
{
    internal class BodyHandler<T> : IBodyHandler where T : new()
    {
        private readonly IEntityHandler<T> _entityHandler;
        private readonly ILogger _logger;

        public BodyHandler(IEntityHandler<T> entityHandler, ILogger logger)
        {
            _entityHandler = entityHandler ?? throw new ArgumentNullException(nameof(entityHandler));
            _logger = (logger ?? throw new ArgumentNullException(nameof(logger)));
        }

        public Task ProcessAsync(string? body)
        {
            var entity = default(T);
            
            try
            {
                entity = body == null ? new T() : JsonSerializer.Deserialize<T>(body);
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
            
            return _entityHandler.ProcessAsync(entity);
        }
    }
}
