using Trade360SDK.Feed.Entities;

namespace Trade360SDK.Feed.Example.Handlers.Inplay;

public class FixtureMetadataUpdateHandlerInplay : IEntityHandler<FixtureMetadataUpdate>
{
    public Task ProcessAsync(FixtureMetadataUpdate entity)
    {
        Console.WriteLine("FixtureMetadataUpdate received");
        return Task.CompletedTask;
    }
}