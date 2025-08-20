using Trade360SDK.Common.Entities.MessageTypes;
using Trade360SDK.Common.Models;
using Trade360SDK.Feed.FeedType;

namespace Trade360SDK.Feed.Example.Handlers.Inplay
{
    internal class HeartbeatHandlerInplay : IEntityHandler<HeartbeatUpdate, InPlay>
    {
        public Task ProcessAsync(RabbitMessageProperties? rabbitHeaders, MessageHeader? header, HeartbeatUpdate? entity)
        {
            Console.WriteLine("Heartbeat received");
            return Task.CompletedTask;
        }
    }
}
