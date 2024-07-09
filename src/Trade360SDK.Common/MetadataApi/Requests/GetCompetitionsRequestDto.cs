using System.Collections.Generic;
using Trade360SDK.Api.Abstraction.Enums;

namespace Trade360SDK.Api.Abstraction.MetadataApi.Requests
{
    public class GetCompetitionsRequestDto : BaseRequest
    {
        public IEnumerable<int>? SportIds { get; set; }
        public IEnumerable<int>? LocationIds { get; set; }
        public SubscriptionStatusEnum SubscriptionStatus { get; set; }

    }
}
