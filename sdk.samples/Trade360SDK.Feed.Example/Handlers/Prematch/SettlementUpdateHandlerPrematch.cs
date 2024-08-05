using Trade360SDK.Common.Entities.MessageTypes;
using Trade360SDK.Common.Models;

namespace Trade360SDK.Feed.Example.Handlers.Prematch
{
    public class SettlementUpdateHandlerPrematch : IEntityHandler<SettlementUpdate>
    {
        public Task ProcessAsync(SettlementUpdate entity, MessageHeader header)
        {
            Console.WriteLine("SettlementUpdate received");
            return Task.CompletedTask;
        }
    }
}
