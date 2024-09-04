using Trade360SDK.Common.Entities.Enums;
using Trade360SDK.Common.Entities.MessageTypes;
using Trade360SDK.Common.Models;
using Trade360SDK.Feed.FeedType;

namespace Trade360SDK.Feed.Example.Handlers.Inplay
{
    internal class KeepAliveUpdateHandlerInplay : IEntityHandler<KeepAliveUpdate, InPlay>
    {
        public Task ProcessAsync(KeepAliveUpdate entity)
        {
            Console.WriteLine("KeepAliveUpdate received");
            return Task.CompletedTask;
        }

        public async Task ProcessAsync(object entity, MessageHeader header)
        {
            await ProcessAsync((KeepAliveUpdate)entity);
        }
    }
}
