using System.Collections.Generic;
using Trade360SDK.Api.Common.Models.Requests.Base;

namespace Trade360SDK.Api.Abstraction.SubscriptionApi.Requests
{
    public class GetSubscriptionRequest : BaseRequest
    {
        public IEnumerable<int>? SportIds { get; set; }
        public IEnumerable<int>? LocationIds { get; set; }
        public IEnumerable<int>? LeagueIds { get; set; }
    }
}
