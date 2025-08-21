using Trade360SDK.Common.Entities.MessageTypes;
using Trade360SDK.Common.Models;
using Trade360SDK.Feed.FeedType;

namespace Trade360SDK.Feed.Example.Handlers.Prematch
{
    internal class OutrightSettlementsUpdateHandlerPrematch : IEntityHandler<OutrightSettlementsUpdate, PreMatch>
    {
        public Task ProcessAsync(TransportMessageHeaders? transportMessageHeaders, MessageHeader? header,
            OutrightSettlementsUpdate? entity)
        {
            Console.WriteLine("OutrightSettlementsUpdate received");
            return Task.CompletedTask;
        }
    }
}
