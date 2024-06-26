using Trade360SDK.Feed.Attributes;
using Trade360SDK.Feed.Entities.KeepAlives;

namespace Trade360SDK.Feed.Entities
{
    [Trade360Entity(31)]
    public class KeepAliveUpdate
    {
        public KeepAlive? KeepAlive { get; set; }
    }
}
