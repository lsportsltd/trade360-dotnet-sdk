using Trade360SDK.Common.Entities.MessageTypes;
using Trade360SDK.Common.Models;
using Trade360SDK.Feed.FeedType;

namespace Trade360SDK.Feed.Example.Handlers.Inplay;

internal class LivescoreUpdateHandlerInplay : IEntityHandler<LivescoreUpdate, InPlay>
{
    public Task ProcessAsync(TransportMessageHeaders? transportMessageHeaders, MessageHeader? header, LivescoreUpdate? entity)
    {
        Console.WriteLine("LivescoreUpdate received");
        return Task.CompletedTask;
    }
}