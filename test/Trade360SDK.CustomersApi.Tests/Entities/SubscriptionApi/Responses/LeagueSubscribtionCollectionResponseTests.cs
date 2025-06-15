using Trade360SDK.CustomersApi.Entities.SubscriptionApi.Responses;
using Xunit;

namespace Trade360SDK.CustomersApi.Tests.Entities.SubscriptionApi.Responses
{
    public class LeagueSubscriptionCollectionResponseTests
    {
        [Fact]
        public void CanInstantiateLeagueSubscriptionCollectionResponse()
        {
            var response = new LeagueSubscriptionCollectionResponse();
            Assert.NotNull(response);
        }
    }
} 