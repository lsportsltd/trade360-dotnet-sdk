using Trade360SDK.Common.Entities;

namespace Trade360SDK.Feed.Example.Handlers.Inplay;

public class LivescoreUpdateHandlerInplay : IEntityHandler<LivescoreUpdate>
{
    public Task ProcessAsync(LivescoreUpdate entity)
    {
        Console.WriteLine("LivescoreUpdate received");
        return Task.CompletedTask;
    }
}