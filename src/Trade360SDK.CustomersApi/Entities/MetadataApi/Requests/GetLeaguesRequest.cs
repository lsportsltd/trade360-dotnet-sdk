using System.Collections.Generic;
using Trade360SDK.Common.Entities.Enums;

namespace Trade360SDK.CustomersApi.Entities.MetadataApi.Requests
{
    public class GetLeaguesRequest : BaseMetadataRequest
    {
        public IEnumerable<int>? SportIds { get; set; }
        public IEnumerable<int>? LocationIds { get; set; }
        public SubscriptionState SubscriptionStatus { get; set; }

    }
}
