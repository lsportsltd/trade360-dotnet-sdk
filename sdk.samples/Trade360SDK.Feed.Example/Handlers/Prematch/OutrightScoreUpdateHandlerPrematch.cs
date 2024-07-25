using Trade360SDK.Common.Entities.MessageTypes;

namespace Trade360SDK.Feed.Example.Handlers.Prematch
{
    public class OutrightScoreUpdateHandlerPrematch : IEntityHandler<OutrightScoreUpdate>
    {
        public Task ProcessAsync(OutrightScoreUpdate entity)
        {
            Console.WriteLine("OutrightLeagueUpdate received");
            return Task.CompletedTask;
        }
    }
}
