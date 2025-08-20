using Trade360SDK.Common.Entities.MessageTypes;
using Trade360SDK.Common.Models;
using Trade360SDK.Feed.FeedType;

namespace Trade360SDK.Feed.Example.Handlers.Inplay;

internal class FixtureMetadataUpdateHandlerInplay : IEntityHandler<FixtureMetadataUpdate, InPlay>
{
    public Task ProcessAsync(RabbitMessageProperties? rabbitHeaders, MessageHeader? header, FixtureMetadataUpdate? entity)
    {
        Console.WriteLine("FixtureMetadataUpdate received");
        return Task.CompletedTask;
    }
}