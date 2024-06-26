using Trade360SDK.Feed.Entities;

namespace Trade360SDK.Feed.Console.Sample.Handlers
{
    internal class HeartbeatHandler : IEntityHandler<Heartbeat>
    {
        public Task ProcessAsync(Heartbeat entity)
        {
            System.Console.WriteLine("Heartbeat received");
            return Task.CompletedTask;
        }
    }
}
