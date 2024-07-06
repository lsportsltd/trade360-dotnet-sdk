using System.Collections.Generic;

namespace Trade360SDK.Api.Abstraction.SubscriptionApi.Responses
{
    public class FixtureSubscribtionCollectionResponse
    {
        public List<FixtureSubsription> Fixtures { get; set; }
    }

    public class FixtureSubsription
    {
        public int FixtureId { get; set; }
        public bool Success { get; set; }
    }
}
