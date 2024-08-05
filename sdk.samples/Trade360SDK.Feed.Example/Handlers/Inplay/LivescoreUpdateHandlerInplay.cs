using Trade360SDK.Common.Entities.MessageTypes;
using Trade360SDK.Common.Models;

namespace Trade360SDK.Feed.Example.Handlers.Inplay;

public class LivescoreUpdateHandlerInplay : IEntityHandler<LivescoreUpdate>
{
    public Task ProcessAsync(LivescoreUpdate entity, MessageHeader header)
    {
        Console.WriteLine("LivescoreUpdate received");
        return Task.CompletedTask;
    }
}