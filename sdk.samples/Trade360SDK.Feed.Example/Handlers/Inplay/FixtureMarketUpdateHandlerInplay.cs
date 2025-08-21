using Trade360SDK.Common.Entities.MessageTypes;
using Trade360SDK.Common.Models;
using Trade360SDK.Feed.FeedType;

namespace Trade360SDK.Feed.Example.Handlers.Inplay;

internal class FixtureMarketUpdateHandlerInplay : IEntityHandler<MarketUpdate, InPlay>
{
    public Task ProcessAsync(TransportMessageHeaders? transportMessageHeaders, MessageHeader? header, MarketUpdate? entity)
    {
        Console.WriteLine("MarketUpdate received");
        return Task.CompletedTask;
    }
}