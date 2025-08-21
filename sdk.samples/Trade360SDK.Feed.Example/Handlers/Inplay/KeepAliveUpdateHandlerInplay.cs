using Trade360SDK.Common.Entities.MessageTypes;
using Trade360SDK.Common.Models;
using Trade360SDK.Feed.FeedType;

namespace Trade360SDK.Feed.Example.Handlers.Inplay
{
    internal class KeepAliveUpdateHandlerInplay : IEntityHandler<KeepAliveUpdate, InPlay>
    {
        public Task ProcessAsync(TransportMessageHeaders? transportMessageHeaders, MessageHeader? header, KeepAliveUpdate? entity)
        {
            Console.WriteLine("KeepAliveUpdate received");
            return Task.CompletedTask;
        }
    }
}
