using Trade360SDK.Common.Entities.MessageTypes;
using Trade360SDK.Common.Models;

namespace Trade360SDK.Feed.Example.Handlers.Prematch
{
    public class OutrightLeagueMarketsUpdateHandlerPrematch : IEntityHandler<OutrightLeagueMarketUpdate>
    {
        public Task ProcessAsync(OutrightLeagueMarketUpdate entity, MessageHeader header)
        {
            Console.WriteLine("OutrightLeagueMarketUpdate received");
            return Task.CompletedTask;
        }
    }
}
