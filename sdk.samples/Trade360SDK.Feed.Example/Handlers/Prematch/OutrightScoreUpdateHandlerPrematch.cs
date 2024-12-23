using Trade360SDK.Common.Entities.MessageTypes;
using Trade360SDK.Common.Models;
using Trade360SDK.Feed.FeedType;

namespace Trade360SDK.Feed.Example.Handlers.Prematch
{
    internal class OutrightScoreUpdateHandlerPrematch : IEntityHandler<OutrightScoreUpdate, PreMatch>
    {
        public Task ProcessAsync(MessageHeader? header, OutrightScoreUpdate? entity)
        {
            Console.WriteLine("OutrightLeagueUpdate received");
            return Task.CompletedTask;
        }
    }
}
