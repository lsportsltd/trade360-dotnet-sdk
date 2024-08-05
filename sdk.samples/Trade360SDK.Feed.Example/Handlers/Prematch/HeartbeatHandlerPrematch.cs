using Trade360SDK.Common.Entities.MessageTypes;
using Trade360SDK.Common.Models;

namespace Trade360SDK.Feed.Example.Handlers.Prematch
{
    internal class HeartbeatHandlerPrematch : IEntityHandler<HeartbeatUpdate>
    {
        public Task ProcessAsync(HeartbeatUpdate entity, MessageHeader header)
        {
            Console.WriteLine("Heartbeat received");
            return Task.CompletedTask;
        }
    }
}
