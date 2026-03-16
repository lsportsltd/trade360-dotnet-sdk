using FluentAssertions;
using Trade360SDK.BetBuilderApi.Entities.Constants;
using Xunit;

namespace Trade360SDK.BetBuilderApi.Tests.Entities.Constants
{
    public class AmericanFootballLeaguesTests
    {
        [Fact]
        public void NFL_ShouldHaveCorrectValue()
        {
            AmericanFootballLeagues.NFL.Should().Be("NFL");
        }

        [Fact]
        public void NFLRegularSeason_ShouldHaveCorrectValue()
        {
            AmericanFootballLeagues.NFLRegularSeason.Should().Be("NFLR");
        }

        [Fact]
        public void NCAA_ShouldHaveCorrectValue()
        {
            AmericanFootballLeagues.NCAA.Should().Be("NCAA");
        }
    }
}
