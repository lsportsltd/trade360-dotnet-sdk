using Trade360SDK.Common.Entities;

namespace Trade360SDK.Feed.Example.Handlers.Inplay;

public class FixtureMetadataUpdateHandlerInplay : IEntityHandler<FixtureMetadataUpdate>
{
    public Task ProcessAsync(FixtureMetadataUpdate entity)
    {
        Console.WriteLine("FixtureMetadataUpdate received");
        return Task.CompletedTask;
    }
}