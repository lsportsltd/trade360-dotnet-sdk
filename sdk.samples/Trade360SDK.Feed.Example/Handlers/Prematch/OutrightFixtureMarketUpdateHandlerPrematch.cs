using Trade360SDK.Common.Entities.MessageTypes;
using Trade360SDK.Common.Models;
using Trade360SDK.Feed.FeedType;

namespace Trade360SDK.Feed.Example.Handlers.Prematch
{
    internal class OutrightFixtureMarketUpdateHandlerPrematch : IEntityHandler<OutrightFixtureMarketUpdate, PreMatch>
    {
        public Task ProcessAsync(RabbitMessageProperties? rabbitHeaders, MessageHeader? header, OutrightFixtureMarketUpdate? entity)
        {
            Console.WriteLine("LivescoreUpdate received");
            return Task.CompletedTask;
        }
    }
}
