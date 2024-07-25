using Trade360SDK.Common.Entities.MessageTypes;

namespace Trade360SDK.Feed.Example.Handlers.Prematch
{
    public class OutrightFixtureUpdateHandlerPrematch : IEntityHandler<OutrightFixtureUpdate>
    {
        public Task ProcessAsync(OutrightFixtureUpdate entity)
        {
            Console.WriteLine("OutrightFixtureUpdate received");
            return Task.CompletedTask;
        }
    }
}
