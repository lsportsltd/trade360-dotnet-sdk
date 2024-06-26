using Trade360SDK.Feed.Entities;

namespace Trade360SDK.Feed.Example.Handlers;

public class LivescoreUpdateHandler: IEntityHandler<LivescoreUpdate>
{
    public Task ProcessAsync(LivescoreUpdate entity)
    {
        System.Console.WriteLine("LivescoreUpdate received");
        return Task.CompletedTask;
    }
}