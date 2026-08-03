using Trade360SDK.Common.Entities.MessageTypes;
using Trade360SDK.Common.Models;
using Trade360SDK.Feed.FeedType;

namespace Trade360SDK.Feed.Example.Handlers.Inplay
{
    internal class HeartbeatHandlerInplay : IEntityHandler<HeartbeatUpdate, InPlay>
    {
        public Task ProcessAsync(TransportMessageHeaders? transportMessageHeaders, MessageHeader? header, HeartbeatUpdate? entity)
        {
            Console.WriteLine(
                $"[FIH] HeartbeatUpdate InPlay: FeedInterruptedDomains=[{string.Join(',', entity?.FeedInterrupted ?? [])}] (empty=normal)");
            return Task.CompletedTask;
        }
    }
}
