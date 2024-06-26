using System.Collections.Generic;
using Trade360SDK.Feed.Entities.Shared;

namespace Trade360SDK.Feed.Entities.KeepAlives
{
    public class KeepAlive
    {
        public IEnumerable<int>? ActiveEvents { get; set; }

        public IEnumerable<NameValuePair>? ExtraData { get; set; }

        public int? ProviderId { get; set; }
    }
}
