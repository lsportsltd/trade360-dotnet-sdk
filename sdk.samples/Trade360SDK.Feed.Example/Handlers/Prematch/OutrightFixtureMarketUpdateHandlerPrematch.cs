using Trade360SDK.Common.Entities.MessageTypes;
using Trade360SDK.Common.Models;

namespace Trade360SDK.Feed.Example.Handlers.Prematch
{
    internal class OutrightFixtureMarketUpdateHandlerPrematch : IEntityHandler<OutrightFixtureMarketUpdate>
    {
        public Task ProcessAsync(OutrightFixtureMarketUpdate entity, MessageHeader header)
        {
            Console.WriteLine("LivescoreUpdate received");
            return Task.CompletedTask;
        }
    }
}
