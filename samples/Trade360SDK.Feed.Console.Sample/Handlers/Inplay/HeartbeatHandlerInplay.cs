using Trade360SDK.Common.Entities.MessageTypes;

namespace Trade360SDK.Feed.Example.Handlers.Inplay
{
    internal class HeartbeatHandlerInplay : IEntityHandler<HeartbeatUpdate>
    {
        public Task ProcessAsync(HeartbeatUpdate entity)
        {
            Console.WriteLine("Heartbeat received");
            return Task.CompletedTask;
        }
    }
}
