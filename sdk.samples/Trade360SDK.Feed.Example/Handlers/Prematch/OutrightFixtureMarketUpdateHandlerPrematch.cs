using Trade360SDK.Common.Entities.MessageTypes;

namespace Trade360SDK.Feed.Example.Handlers.Prematch
{
    internal class OutrightFixtureMarketUpdateHandlerPrematch : IEntityHandler<OutrightFixtureMarketUpdate>
    {
        public Task ProcessAsync(OutrightFixtureMarketUpdate entity)
        {
            Console.WriteLine("LivescoreUpdate received");
            return Task.CompletedTask;
        }
    }
}
