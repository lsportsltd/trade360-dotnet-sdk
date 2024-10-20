using Trade360SDK.Common.Entities.MessageTypes;
using Trade360SDK.Common.Models;
using Trade360SDK.Feed.FeedType;

namespace Trade360SDK.Feed.Example.Handlers.Inplay
{
    internal class SettlementUpdateHandlerInplay : IEntityHandler<SettlementUpdate, InPlay>
    {
        public Task ProcessAsync(MessageHeader? header, SettlementUpdate? entity)
        {
            Console.WriteLine("SettlementUpdate received");
            return Task.CompletedTask;
        }
    }
}
