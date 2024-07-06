using System.Collections.Generic;

namespace Trade360SDK.Api.Abstraction.SubscriptionApi.Requests
{
    public class FixtureSubscriptionRequestDto
    {
        public IEnumerable<int>? Fixtures { get; set; }
    }
}
