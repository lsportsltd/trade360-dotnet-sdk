using Trade360SDK.Common.Entities.MessageTypes;
using Trade360SDK.Common.Models;
using Trade360SDK.Feed.FeedType;

namespace Trade360SDK.Feed.Example.Handlers.Inplay;

public class OutrightLeagueSettlementsUpdateHandlerInplay  : IEntityHandler<OutrightLeagueSettlementUpdate, InPlay >
{
    public Task ProcessAsync(TransportMessageHeaders? transportMessageHeaders, MessageHeader? header,
        OutrightLeagueSettlementUpdate? entity)
    {
        Console.WriteLine("OutrightLeagueSettlementsUpdate received");
        return Task.CompletedTask;
    }
}