using System;
using System.Threading;
using System.Threading.Tasks;
using Trade360SDK.Common.Entities.Enums;

namespace Trade360SDK.Feed
{
    public interface IFeed : IDisposable
    {
        Task StartAsync(bool connectAtStart, CancellationToken cancellationToken);
        Task StopAsync(CancellationToken cancellationToken);
    }
}
