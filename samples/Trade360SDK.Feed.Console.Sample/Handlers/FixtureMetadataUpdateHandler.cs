using Trade360SDK.Feed.Entities;

namespace Trade360SDK.Feed.Example.Handlers;

public class FixtureMetadataUpdateHandler : IEntityHandler<FixtureMetadataUpdate>
{
    public Task ProcessAsync(FixtureMetadataUpdate entity)
    {
        System.Console.WriteLine("FixtureMetadataUpdate received");
        return Task.CompletedTask;
    }
}