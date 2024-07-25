using Trade360SDK.Common.Entities.MessageTypes;

namespace Trade360SDK.Feed.Example.Handlers.Prematch
{
    public class OutrightLeagueUpdateHandlerPrematch : IEntityHandler<OutrightLeagueUpdate>
    {
        public Task ProcessAsync(OutrightLeagueUpdate entity)
        {
            Console.WriteLine("OutrightLeagueUpdate received");
            return Task.CompletedTask;
        }
    }
}
