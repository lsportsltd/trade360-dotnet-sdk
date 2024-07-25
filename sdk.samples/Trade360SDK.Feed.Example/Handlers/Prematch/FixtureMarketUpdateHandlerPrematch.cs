using Trade360SDK.Common.Entities.MessageTypes;

namespace Trade360SDK.Feed.Example.Handlers.Prematch
{
    public class FixtureMarketUpdateHandlerPrematch : IEntityHandler<MarketUpdate>
    {
        public Task ProcessAsync(MarketUpdate entity)
        {
            Console.WriteLine("MarketUpdate received");
            return Task.CompletedTask;
        }
    }
}
