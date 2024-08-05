using Trade360SDK.Common.Entities.MessageTypes;
using Trade360SDK.Common.Models;

namespace Trade360SDK.Feed.Example.Handlers.Prematch
{
    public class OutrightLeagueUpdateHandlerPrematch : IEntityHandler<OutrightLeagueUpdate>
    {
        public Task ProcessAsync(OutrightLeagueUpdate entity, MessageHeader header)
        {
            Console.WriteLine("OutrightLeagueUpdate received");
            return Task.CompletedTask;
        }
    }
}
