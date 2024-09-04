using Trade360SDK.Common.Entities.MessageTypes;
using Trade360SDK.Common.Models;
using Trade360SDK.Feed.FeedType;

namespace Trade360SDK.Feed.Example.Handlers.Prematch;

internal class LivescoreUpdateHandlerPrematch : IEntityHandler<LivescoreUpdate, PreMatch>
{
    public Task ProcessAsync(LivescoreUpdate entity)
    {
        Console.WriteLine("LivescoreUpdate received");
        return Task.CompletedTask;
    }

    public async Task ProcessAsync(object entity, MessageHeader header)
    {
        await ProcessAsync((LivescoreUpdate)entity);
    }
}