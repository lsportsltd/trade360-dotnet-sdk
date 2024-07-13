using System;
using System.Threading;
using System.Threading.Tasks;

namespace Trade360SDK.Feed
{
    public interface IFeed : IDisposable
    {
        Task StartAsync(bool connectAtStart, CancellationToken cancellationToken);
        Task StopAsync(CancellationToken cancellationToken);
        void AddEntityHandler<TEntity>(IEntityHandler<TEntity> entityHandler);
    }
}
