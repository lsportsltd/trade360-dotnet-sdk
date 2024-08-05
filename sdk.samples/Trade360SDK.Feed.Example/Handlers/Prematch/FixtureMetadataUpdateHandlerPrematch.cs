using Trade360SDK.Common.Entities.MessageTypes;
using Trade360SDK.Common.Models;

namespace Trade360SDK.Feed.Example.Handlers.Prematch;

public class FixtureMetadataUpdateHandlerPrematch : IEntityHandler<FixtureMetadataUpdate>
{
    public Task ProcessAsync(FixtureMetadataUpdate entity, MessageHeader header)
    {
        Console.WriteLine("FixtureMetadataUpdate received");
        return Task.CompletedTask;
    }
}