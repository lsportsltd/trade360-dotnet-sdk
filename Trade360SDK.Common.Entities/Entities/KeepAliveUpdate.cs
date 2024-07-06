using Trade360SDK.Common.Attributes;
using Trade360SDK.Common.Entities.KeepAlives;

namespace Trade360SDK.Common.Entities
{
    [Trade360Entity(31)]
    public class KeepAliveUpdate
    {
        public KeepAlive? KeepAlive { get; set; }
    }
}
