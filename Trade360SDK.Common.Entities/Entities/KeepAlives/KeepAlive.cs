using System.Collections.Generic;
using Trade360SDK.Common.Entities.Shared;

namespace Trade360SDK.Common.Entities.KeepAlives
{
    public class KeepAlive
    {
        public IEnumerable<int>? ActiveEvents { get; set; }

        public IEnumerable<NameValuePair>? ExtraData { get; set; }

        public int? ProviderId { get; set; }
    }
}
