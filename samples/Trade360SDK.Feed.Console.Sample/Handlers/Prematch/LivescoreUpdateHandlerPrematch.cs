using Trade360SDK.Feed.Entities;

namespace Trade360SDK.Feed.Example.Handlers.Prematch;

public class LivescoreUpdateHandlerPrematch : IEntityHandler<LivescoreUpdate>
{
    public Task ProcessAsync(LivescoreUpdate entity)
    {
        Console.WriteLine("LivescoreUpdate received");
        return Task.CompletedTask;
    }
}