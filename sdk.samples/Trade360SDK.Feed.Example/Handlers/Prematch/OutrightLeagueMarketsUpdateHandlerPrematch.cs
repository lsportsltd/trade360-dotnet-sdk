using Trade360SDK.Common.Entities.MessageTypes;

namespace Trade360SDK.Feed.Example.Handlers.Prematch
{
    public class OutrightLeagueMarketsUpdateHandlerPrematch : IEntityHandler<OutrightLeagueMarketUpdate>
    {
        public Task ProcessAsync(OutrightLeagueMarketUpdate entity)
        {
            Console.WriteLine("OutrightLeagueMarketUpdate received");
            return Task.CompletedTask;
        }
    }
}
