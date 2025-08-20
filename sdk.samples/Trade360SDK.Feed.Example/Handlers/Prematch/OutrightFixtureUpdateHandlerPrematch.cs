using Trade360SDK.Common.Entities.MessageTypes;
using Trade360SDK.Common.Models;
using Trade360SDK.Feed.FeedType;

namespace Trade360SDK.Feed.Example.Handlers.Prematch
{
    internal class OutrightFixtureUpdateHandlerPrematch : IEntityHandler<OutrightFixtureUpdate, PreMatch>
    {
        public Task ProcessAsync(RabbitMessageProperties? rabbitHeaders, MessageHeader? header, OutrightFixtureUpdate? entity)
        {
            Console.WriteLine("OutrightFixtureUpdate received");
            return Task.CompletedTask;
        }
    }
}
