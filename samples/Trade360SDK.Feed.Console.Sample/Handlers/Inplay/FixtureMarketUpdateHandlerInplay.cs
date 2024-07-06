using Trade360SDK.Common.Entities;

namespace Trade360SDK.Feed.Example.Handlers.Inplay;

public class FixtureMarketUpdateHandlerInplay : IEntityHandler<MarketUpdate>
{
    public Task ProcessAsync(MarketUpdate entity)
    {
        Console.WriteLine("FixtureMetadataUpdate received");
        return Task.CompletedTask;
    }
}