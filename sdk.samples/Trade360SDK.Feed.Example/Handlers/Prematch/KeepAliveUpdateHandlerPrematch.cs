using Trade360SDK.Common.Entities.MessageTypes;
using Trade360SDK.Common.Models;
using Trade360SDK.Feed.FeedType;

namespace Trade360SDK.Feed.Example.Handlers.Prematch
{
    internal class KeepAliveUpdateHandlerPrematch : IEntityHandler<KeepAliveUpdate, PreMatch>
    {
        public Task ProcessAsync(TransportMessageHeaders? transportMessageHeaders, MessageHeader? header, KeepAliveUpdate? entity)
        {
            Console.WriteLine("KeepAliveUpdate received");
            return Task.CompletedTask;
        }
    }
}
