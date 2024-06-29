using System;
using System.Threading;
using System.Threading.Tasks;
using Trade360SDK.Feed.RabbitMQ;

namespace Trade360SDK.Feed
{
    public interface IRabbitMQFeed : IDisposable
    {
        Task StartAsync(CancellationToken cancellationToken);
        Task StopAsync(CancellationToken cancellationToken);
        void AddEntityHandler<TEntity>(IEntityHandler<TEntity> entityHandler);
    }
}
