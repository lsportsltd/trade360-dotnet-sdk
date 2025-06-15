using Trade360SDK.CustomersApi.Entities.SubscriptionApi.Requests;
using Xunit;

namespace Trade360SDK.CustomersApi.Tests.Entities.SubscriptionApi.Requests
{
    public class GetFixtureScheduleRequestTests
    {
        [Fact]
        public void CanInstantiateGetFixtureScheduleRequest()
        {
            var request = new GetFixtureScheduleRequest();
            Assert.NotNull(request);
        }
    }
} 