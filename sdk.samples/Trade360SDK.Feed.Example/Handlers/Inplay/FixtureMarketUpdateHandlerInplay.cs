using RabbitMQ.Client;
using Trade360SDK.Common.Entities.MessageTypes;
using Trade360SDK.Common.Models;
namespace Trade360SDK.Feed.Example.Handlers.Inplay;

public class FixtureMarketUpdateHandlerInplay : IEntityHandler<MarketUpdate>
{
    public Task ProcessAsync(MarketUpdate entity, MessageHeader header)
    {
        Console.WriteLine("FixtureMetadataUpdate received");
        return Task.CompletedTask;
    }
}