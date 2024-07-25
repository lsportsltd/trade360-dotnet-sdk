using Trade360SDK.Common.Entities.MessageTypes;

namespace Trade360SDK.Feed.Example.Handlers.Prematch
{
    public class SettlementUpdateHandlerPrematch : IEntityHandler<SettlementUpdate>
    {
        public Task ProcessAsync(SettlementUpdate entity)
        {
            Console.WriteLine("SettlementUpdate received");
            return Task.CompletedTask;
        }
    }
}
