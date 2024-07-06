using System.Collections.Generic;

namespace Trade360SDK.Api.Abstraction.SubscriptionApi.Responses
{
    public class GetSubscriptionResponse
    {
        public IEnumerable<int>? Fixtures { get; set; }
    }
}
