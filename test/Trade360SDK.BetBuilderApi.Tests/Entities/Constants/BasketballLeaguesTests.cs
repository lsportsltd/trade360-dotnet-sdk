using FluentAssertions;
using Trade360SDK.BetBuilderApi.Entities.Constants;
using Xunit;

namespace Trade360SDK.BetBuilderApi.Tests.Entities.Constants
{
    public class BasketballLeaguesTests
    {
        [Fact]
        public void NBA_ShouldHaveCorrectValue()
        {
            BasketballLeagues.NBA.Should().Be("NBA");
        }

        [Fact]
        public void NCAA_ShouldHaveCorrectValue()
        {
            BasketballLeagues.NCAA.Should().Be("NCAA");
        }

        [Fact]
        public void WNBA_ShouldHaveCorrectValue()
        {
            BasketballLeagues.WNBA.Should().Be("WNBA");
        }

        [Fact]
        public void Euroleague_ShouldHaveCorrectValue()
        {
            BasketballLeagues.Euroleague.Should().Be("EURO");
        }
    }
}
