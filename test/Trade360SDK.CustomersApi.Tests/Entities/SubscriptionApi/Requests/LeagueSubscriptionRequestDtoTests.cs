using Trade360SDK.CustomersApi.Entities.SubscriptionApi.Requests;
using Xunit;

namespace Trade360SDK.CustomersApi.Tests.Entities.SubscriptionApi.Requests
{
    public class LeagueSubscriptionRequestDtoTests
    {
        [Fact]
        public void CanInstantiateLeagueSubscriptionRequestDto()
        {
            var dto = new LeagueSubscriptionRequestDto();
            Assert.NotNull(dto);
        }
    }
} 