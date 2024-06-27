using Trade360SDK.Feed.Entities;

namespace Trade360SDK.Feed.Console.Sample.Handlers
{
    internal class LivescoreHandler : IEntityHandler<LivescoreUpdate>
    {
        public Task ProcessAsync(LivescoreUpdate livescoreUpdate)
        {
            if (livescoreUpdate.Events == null)
            {
                return Task.CompletedTask;
            }

            foreach (var livescoreEvent in livescoreUpdate.Events)
            {
                System.Console.WriteLine($"Received livescore update for fixture {livescoreEvent.FixtureId}");
            }
            return Task.CompletedTask;
        }
    }
}
