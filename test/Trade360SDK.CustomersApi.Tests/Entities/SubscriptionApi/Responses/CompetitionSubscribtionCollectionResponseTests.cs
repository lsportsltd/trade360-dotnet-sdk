using Trade360SDK.CustomersApi.Entities.SubscriptionApi.Responses;
using Xunit;

namespace Trade360SDK.CustomersApi.Tests.Entities.SubscriptionApi.Responses
{
    public class CompetitionSubscriptionCollectionResponseTests
    {
        [Fact]
        public void CanInstantiateCompetitionSubscriptionCollectionResponse()
        {
            var response = new CompetitionSubscriptionCollectionResponse();
            Assert.NotNull(response);
        }
    }
} 