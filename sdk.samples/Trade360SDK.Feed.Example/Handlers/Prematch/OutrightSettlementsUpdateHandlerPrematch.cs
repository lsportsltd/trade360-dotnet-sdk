using Trade360SDK.Common.Entities.MessageTypes;
using Trade360SDK.Common.Models;
using Trade360SDK.Feed.FeedType;

namespace Trade360SDK.Feed.Example.Handlers.Prematch
{
    internal class OutrightSettlementsUpdateHandlerPrematch : IEntityHandler<OutrightSettlementsUpdate, PreMatch>
    {
        public Task ProcessAsync(OutrightSettlementsUpdate entity)
        {
            Console.WriteLine("OutrightSettlementsUpdate received");
            return Task.CompletedTask;
        }

        public async Task ProcessAsync(object entity, MessageHeader header)
        {
            await ProcessAsync((OutrightSettlementsUpdate)entity);
        }
    }
}
