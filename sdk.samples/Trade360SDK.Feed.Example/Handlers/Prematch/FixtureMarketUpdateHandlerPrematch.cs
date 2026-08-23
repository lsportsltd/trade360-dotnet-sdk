using Trade360SDK.Common.Entities.MessageTypes;
using Trade360SDK.Common.Models;
using Trade360SDK.Feed.FeedType;

namespace Trade360SDK.Feed.Example.Handlers.Prematch
{
    internal class FixtureMarketUpdateHandlerPrematch : IEntityHandler<MarketUpdate, PreMatch>
    {
        public Task ProcessAsync(TransportMessageHeaders? transportMessageHeaders, MessageHeader? header, MarketUpdate? entity)
        {
            Console.WriteLine("MarketUpdate received");
            if (entity?.Events != null)
            {
                foreach (var marketEvent in entity.Events)
                {
                    if (marketEvent.Markets == null)
                    {
                        continue;
                    }

                    foreach (var market in marketEvent.Markets)
                    {
                        if (market.Bets == null)
                        {
                            continue;
                        }

                        foreach (var bet in market.Bets)
                        {
                            Console.WriteLine($"Bet {bet.Id} Status={bet.Status} BetStatusId={bet.BetStatusId}");
                        }
                    }
                }
            }
            return Task.CompletedTask;
        }
    }
}
