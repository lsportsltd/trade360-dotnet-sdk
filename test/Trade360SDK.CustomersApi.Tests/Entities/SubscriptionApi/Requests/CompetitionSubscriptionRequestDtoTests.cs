using Trade360SDK.CustomersApi.Entities.SubscriptionApi.Requests;
using Xunit;

namespace Trade360SDK.CustomersApi.Tests.Entities.SubscriptionApi.Requests
{
    public class CompetitionSubscriptionRequestDtoTests
    {
        [Fact]
        public void CanInstantiateCompetitionSubscriptionRequestDto()
        {
            var dto = new CompetitionSubscriptionRequestDto();
            Assert.NotNull(dto);
        }
    }
} 