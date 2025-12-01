using Trade360SDK.CustomersApi.Entities.SubscriptionApi.Requests;
using Xunit;

namespace Trade360SDK.CustomersApi.Tests.Entities.SubscriptionApi.Requests
{
    public class CompetitionSubscriptionRequestTests
    {
        [Fact]
        public void CanInstantiateCompetitionSubscriptionRequest()
        {
            var request = new CompetitionSubscriptionRequest();
            Assert.NotNull(request);
        }
    }
} 