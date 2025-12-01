using Trade360SDK.CustomersApi.Entities.SubscriptionApi.Requests;
using Xunit;

namespace Trade360SDK.CustomersApi.Tests.Entities.SubscriptionApi.Requests
{
    public class LeagueSubscriptionRequestTests
    {
        [Fact]
        public void CanInstantiateLeagueSubscriptionRequest()
        {
            var request = new LeagueSubscriptionRequest();
            Assert.NotNull(request);
        }
    }
} 