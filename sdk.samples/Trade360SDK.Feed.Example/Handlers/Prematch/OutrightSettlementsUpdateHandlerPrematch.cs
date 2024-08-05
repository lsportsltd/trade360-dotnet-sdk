using Trade360SDK.Common.Entities.MessageTypes;
using Trade360SDK.Common.Models;

namespace Trade360SDK.Feed.Example.Handlers.Prematch
{
    public class OutrightSettlementsUpdateHandlerPrematch : IEntityHandler<OutrightSettlementsUpdate>
    {
        public Task ProcessAsync(OutrightSettlementsUpdate entity, MessageHeader header)
        {
            Console.WriteLine("OutrightSettlementsUpdate received");
            return Task.CompletedTask;
        }
    }
}
