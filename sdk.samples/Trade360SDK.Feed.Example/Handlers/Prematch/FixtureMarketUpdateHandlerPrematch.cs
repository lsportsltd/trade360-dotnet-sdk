using Trade360SDK.Common.Entities.MessageTypes;
using Trade360SDK.Common.Models;

namespace Trade360SDK.Feed.Example.Handlers.Prematch
{
    public class FixtureMarketUpdateHandlerPrematch : IEntityHandler<MarketUpdate>
    {
        public Task ProcessAsync(MarketUpdate entity, MessageHeader header)
        {
            Console.WriteLine("MarketUpdate received");
            return Task.CompletedTask;
        }
    }
}
