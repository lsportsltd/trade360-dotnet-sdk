using System;
using System.Text.Json;
using System.Threading.Tasks;
using Trade360SDK.Feed.Interfaces;

namespace Trade360SDK.Feed.Handlers
{
    internal class BodyHandler<T> : IBodyHandler
    {
        private readonly IEntityHandler<T> _entityHandler;
        private readonly ILogger? _logger;

        public BodyHandler(IEntityHandler<T> entityHandler, ILogger? logger)
        {
            _entityHandler = entityHandler;
            _logger = logger;
        }

        public Task Process(string? body)
        {
            var entity = body == null ? Activator.CreateInstance<T>() : JsonSerializer.Deserialize<T>(body);
            if (entity == null)
            {
                _logger?.WriteError($"Failed to deserialize {typeof(T).Name} entity");
                return Task.CompletedTask;
            }
            return _entityHandler.ProcessAsync(entity);
        }
    }
}
