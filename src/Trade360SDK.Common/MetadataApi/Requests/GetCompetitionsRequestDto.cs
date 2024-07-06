using System.Collections.Generic;
using Trade360SDK.Api.Common.Models.Requests.Base;
using Trade360SDK.Metadata.Enums;

namespace Trade360SDK.Common.Metadata.Requests
{
    public class GetCompetitionsRequestDto : BaseRequest
    {
        public IEnumerable<int>? SportIds { get; set; }
        public IEnumerable<int>? LocationIds { get; set; }
        public SubscriptionStatusEnum SubscriptionStatus { get; set; }

    }
}
