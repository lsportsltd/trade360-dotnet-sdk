using System.Collections.Generic;
using Trade360SDK.Common.Entities.Enums;
using Trade360SDK.CustomersApi.Entities.Base;

namespace Trade360SDK.CustomersApi.Entities.MetadataApi.Requests
{
    public class GetCompetitionsRequest : BaseRequest
    {
        public IEnumerable<int>? SportIds { get; set; }
        public IEnumerable<int>? LocationIds { get; set; }
        public SubscriptionState SubscriptionStatus { get; set; }

    }
}
