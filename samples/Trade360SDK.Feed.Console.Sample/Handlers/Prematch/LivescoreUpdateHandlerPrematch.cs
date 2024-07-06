using Trade360SDK.Common.Entities;

namespace Trade360SDK.Feed.Example.Handlers.Prematch;

public class LivescoreUpdateHandlerPrematch : IEntityHandler<LivescoreUpdate>
{
    public Task ProcessAsync(LivescoreUpdate entity)
    {
        Console.WriteLine("LivescoreUpdate received");
        return Task.CompletedTask;
    }
}