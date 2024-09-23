using Trade360SDK.Common.Entities.MessageTypes;
using Trade360SDK.Common.Models;
using Trade360SDK.Feed.FeedType;

namespace Trade360SDK.Feed.Example.Handlers.Prematch
{
    internal class SettlementUpdateHandlerPrematch : IEntityHandler<SettlementUpdate, PreMatch>
    {
        public Task ProcessAsync(SettlementUpdate entity)
        {
            Console.WriteLine("SettlementUpdate received");
            return Task.CompletedTask;
        }

        public async Task ProcessAsync(object entity, MessageHeader header)
        {
            await ProcessAsync((SettlementUpdate)entity);
        }
    }
}
