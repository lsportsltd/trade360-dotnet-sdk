using Trade360SDK.Common.Entities.MessageTypes;

namespace Trade360SDK.Feed.Example.Handlers.Prematch
{
    internal class HeartbeatHandlerPrematch : IEntityHandler<HeartbeatUpdate>
    {
        public Task ProcessAsync(HeartbeatUpdate entity)
        {
            Console.WriteLine("Heartbeat received");
            return Task.CompletedTask;
        }
    }
}
