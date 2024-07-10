using System.Collections.Generic;
using Trade360SDK.Common.Entities.Enums;

namespace Trade360SDK.CustomersApi.Entities.MetadataApi.Requests
{
    public class GetLeaguesRequestDto
    {
        public IEnumerable<int>? SportIds { get; set; }
        public IEnumerable<int>? LocationIds { get; set; }
        public SubscriptionStatusEnum SubscriptionStatus { get; set; }
    }
}
