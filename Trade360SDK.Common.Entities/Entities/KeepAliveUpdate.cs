using Trade360SDK.Common.Attributes;

namespace Trade360SDK.Common.Entities
{
    [Trade360Entity(31)]
    public class KeepAliveUpdate
    {
        public KeepAlive.KeepAlive? KeepAlive { get; set; }
    }
}
