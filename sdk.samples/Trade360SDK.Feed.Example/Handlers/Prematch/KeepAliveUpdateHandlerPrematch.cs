using Trade360SDK.Common.Entities.MessageTypes;

namespace Trade360SDK.Feed.Example.Handlers.Prematch
{
    public class KeepAliveUpdateHandlerPrematchPrematch : IEntityHandler<KeepAliveUpdate>
    {
        public Task ProcessAsync(KeepAliveUpdate entity)
        {
            Console.WriteLine("KeepAliveUpdate received");
            return Task.CompletedTask;
        }
    }
}
