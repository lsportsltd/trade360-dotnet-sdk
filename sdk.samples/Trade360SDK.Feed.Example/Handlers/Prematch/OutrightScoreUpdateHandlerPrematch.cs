using Trade360SDK.Common.Entities.MessageTypes;
using Trade360SDK.Common.Models;

namespace Trade360SDK.Feed.Example.Handlers.Prematch
{
    public class OutrightScoreUpdateHandlerPrematch : IEntityHandler<OutrightScoreUpdate>
    {
        public Task ProcessAsync(OutrightScoreUpdate entity, MessageHeader header)
        {
            Console.WriteLine("OutrightLeagueUpdate received");
            return Task.CompletedTask;
        }
    }
}
