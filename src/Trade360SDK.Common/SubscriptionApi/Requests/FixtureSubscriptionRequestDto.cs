using System.Collections.Generic;
using Trade360SDK.Api.Common.Models.Requests.Base;

namespace Trade360SDK.Api.Abstraction.SubscriptionApi.Requests
{
    public class FixtureSubscriptionRequest : BaseRequest
    {
        public IEnumerable<int>? Fixtures { get; set; }
    }
}
