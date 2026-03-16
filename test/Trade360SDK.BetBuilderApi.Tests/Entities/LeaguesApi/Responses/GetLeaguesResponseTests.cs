using FluentAssertions;
using Trade360SDK.BetBuilderApi.Entities.LeaguesApi.Responses;
using Xunit;

namespace Trade360SDK.BetBuilderApi.Tests.Entities.LeaguesApi.Responses
{
    public class GetLeaguesResponseTests
    {
        [Fact]
        public void GetLeaguesResponse_DefaultConstructor_ShouldCreateInstanceWithDefaultValues()
        {
            var response = new GetLeaguesResponse();

            response.Should().NotBeNull();
            response.Leagues.Should().BeNull();
            response.CurrentVersion.Should().BeNull();
        }

        [Fact]
        public void LeagueEntry_DefaultConstructor_ShouldCreateInstanceWithDefaultValues()
        {
            var entry = new LeagueEntry();

            entry.Should().NotBeNull();
            entry.SportId.Should().Be(0);
            entry.SportName.Should().BeNull();
            entry.LeagueId.Should().Be(0);
            entry.LeagueName.Should().BeNull();
            entry.FormatName.Should().BeNull();
            entry.FormatType.Should().BeNull();
            entry.League.Should().BeNull();
            entry.SupportsExtraTime.Should().BeFalse();
            entry.SupportsPlayoff.Should().BeFalse();
            entry.Periods.Should().BeNull();
            entry.Status.Should().BeNull();
            entry.Version.Should().BeNull();
        }

        [Fact]
        public void LeagueEntry_SetProperties_ShouldReturnCorrectValues()
        {
            var entry = new LeagueEntry
            {
                SportId = 6046,
                SportName = "Football",
                LeagueId = 75,
                LeagueName = "NFL",
                SupportsExtraTime = true,
                SupportsPlayoff = true
            };

            entry.SportId.Should().Be(6046);
            entry.SportName.Should().Be("Football");
            entry.LeagueId.Should().Be(75);
            entry.LeagueName.Should().Be("NFL");
            entry.SupportsExtraTime.Should().BeTrue();
            entry.SupportsPlayoff.Should().BeTrue();
        }
    }
}
