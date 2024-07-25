using Trade360SDK.Common.Entities.MessageTypes;

namespace Trade360SDK.Feed.Example.Handlers.Prematch
{
    public class OutrightSettlementsUpdateHandlerPrematch : IEntityHandler<OutrightSettlementsUpdate>
    {
        public Task ProcessAsync(OutrightSettlementsUpdate entity)
        {
            Console.WriteLine("OutrightSettlementsUpdate received");
            return Task.CompletedTask;
        }
    }
}
