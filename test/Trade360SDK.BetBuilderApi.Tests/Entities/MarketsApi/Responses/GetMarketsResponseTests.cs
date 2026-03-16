using FluentAssertions;
using Trade360SDK.BetBuilderApi.Entities.MarketsApi.Responses;
using Xunit;

namespace Trade360SDK.BetBuilderApi.Tests.Entities.MarketsApi.Responses
{
    public class GetMarketsResponseTests
    {
        [Fact]
        public void GetMarketsResponse_DefaultConstructor_ShouldCreateInstanceWithDefaultValues()
        {
            var response = new GetMarketsResponse();

            response.Should().NotBeNull();
            response.Markets.Should().BeNull();
        }

        [Fact]
        public void Market_DefaultConstructor_ShouldCreateInstanceWithDefaultValues()
        {
            var market = new Market();

            market.Should().NotBeNull();
            market.SportLookUp.Should().BeNull();
            market.MarketLookUp.Should().BeNull();
            market.DependentMarketLookUps.Should().BeNull();
        }

        [Fact]
        public void MarketLookUp_SetProperties_ShouldReturnCorrectValues()
        {
            var lookUp = new MarketLookUp { Id = 1, Name = "Total" };

            lookUp.Id.Should().Be(1);
            lookUp.Name.Should().Be("Total");
        }

        [Fact]
        public void SportLookUp_SetProperties_ShouldReturnCorrectValues()
        {
            var lookUp = new SportLookUp { Id = 6046, Name = "Football" };

            lookUp.Id.Should().Be(6046);
            lookUp.Name.Should().Be("Football");
        }
    }
}
