using Trade360SDK.Common.Entities.MessageTypes;
using Trade360SDK.Common.Models;

namespace Trade360SDK.Feed.Example.Handlers.Prematch
{
    public class OutrightFixtureUpdateHandlerPrematch : IEntityHandler<OutrightFixtureUpdate>
    {
        public Task ProcessAsync(OutrightFixtureUpdate entity, MessageHeader header)
        {
            Console.WriteLine("OutrightFixtureUpdate received");
            return Task.CompletedTask;
        }
    }
}
