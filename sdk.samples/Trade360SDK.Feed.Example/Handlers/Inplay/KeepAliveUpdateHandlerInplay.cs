using Trade360SDK.Common.Entities.MessageTypes;
using Trade360SDK.Common.Models;

namespace Trade360SDK.Feed.Example.Handlers.Inplay
{
    public class KeepAliveUpdateHandlerInplay : IEntityHandler<KeepAliveUpdate>
    {
        public Task ProcessAsync(KeepAliveUpdate entity, MessageHeader header)
        {
            Console.WriteLine("KeepAliveUpdate received");
            return Task.CompletedTask;
        }
    }
}
