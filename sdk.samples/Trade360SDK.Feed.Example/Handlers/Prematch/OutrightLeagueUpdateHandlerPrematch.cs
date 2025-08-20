using Trade360SDK.Common.Entities.MessageTypes;
using Trade360SDK.Common.Models;
using Trade360SDK.Feed.FeedType;

namespace Trade360SDK.Feed.Example.Handlers.Prematch
{
    internal class OutrightLeagueUpdateHandlerPrematch : IEntityHandler<OutrightLeagueUpdate, PreMatch>
    {
        public Task ProcessAsync(RabbitMessageProperties? rabbitHeaders, MessageHeader? header, OutrightLeagueUpdate? entity)
        {
            Console.WriteLine("OutrightLeagueUpdate received");
            return Task.CompletedTask;
        }
    }
}
