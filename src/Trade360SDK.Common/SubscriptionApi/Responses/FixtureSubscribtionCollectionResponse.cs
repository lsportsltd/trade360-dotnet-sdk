using System.Collections.Generic;

namespace Trade360SDK.Api.Abstraction.SubscriptionApi.Responses
{
    public class FixtureSubscriptionCollectionResponse
    {
        public List<FixtureSubscription> Fixtures { get; set; }
    }

    public class FixtureSubscription
    {
        public int FixtureId { get; set; }
        public bool Success { get; set; }
    }
}
