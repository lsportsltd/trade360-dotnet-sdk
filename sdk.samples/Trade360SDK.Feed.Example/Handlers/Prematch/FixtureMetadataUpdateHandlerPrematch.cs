using Trade360SDK.Common.Entities.MessageTypes;

namespace Trade360SDK.Feed.Example.Handlers.Prematch;

public class FixtureMetadataUpdateHandlerPrematch : IEntityHandler<FixtureMetadataUpdate>
{
    public Task ProcessAsync(FixtureMetadataUpdate entity)
    {
        Console.WriteLine("FixtureMetadataUpdate received");
        return Task.CompletedTask;
    }
}