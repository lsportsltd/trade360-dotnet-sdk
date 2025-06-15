using Trade360SDK.CustomersApi.Entities.SubscriptionApi.Responses;
using Xunit;

namespace Trade360SDK.CustomersApi.Tests.Entities.SubscriptionApi.Responses
{
    public class FixtureScheduleCollectionResponseTests
    {
        [Fact]
        public void CanInstantiateFixtureScheduleCollectionResponse()
        {
            var response = new FixtureScheduleCollectionResponse();
            Assert.NotNull(response);
        }
    }
} 