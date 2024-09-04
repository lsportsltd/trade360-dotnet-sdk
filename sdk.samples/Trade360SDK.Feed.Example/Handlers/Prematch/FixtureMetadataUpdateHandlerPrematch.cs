using Trade360SDK.Common.Entities.MessageTypes;
using Trade360SDK.Common.Models;
using Trade360SDK.Feed.FeedType;

namespace Trade360SDK.Feed.Example.Handlers.Prematch;

internal class FixtureMetadataUpdateHandlerPrematch : IEntityHandler<FixtureMetadataUpdate, PreMatch>
{
    public Task ProcessAsync(FixtureMetadataUpdate entity)
    {
        Console.WriteLine("FixtureMetadataUpdate received");
        return Task.CompletedTask;
    }

    public async Task ProcessAsync(object entity, MessageHeader header)
    {
        await ProcessAsync((FixtureMetadataUpdate)entity);
    }
}