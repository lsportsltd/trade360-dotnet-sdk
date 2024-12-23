using Trade360SDK.Common.Entities.MessageTypes;
using Trade360SDK.Common.Models;
using Trade360SDK.Feed.FeedType;

namespace Trade360SDK.Feed.Example.Handlers.Prematch
{
    internal class OutrightLeagueMarketsUpdateHandlerPrematch : IEntityHandler<OutrightLeagueMarketUpdate, PreMatch>
    {
        public Task ProcessAsync(MessageHeader? header, OutrightLeagueMarketUpdate? entity)
        {
            Console.WriteLine("OutrightLeagueMarketUpdate received");
            return Task.CompletedTask;
        }

    }
}
