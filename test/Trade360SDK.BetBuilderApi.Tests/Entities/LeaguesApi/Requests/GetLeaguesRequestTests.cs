using FluentAssertions;
using Trade360SDK.BetBuilderApi.Entities.LeaguesApi.Requests;
using Xunit;

namespace Trade360SDK.BetBuilderApi.Tests.Entities.LeaguesApi.Requests
{
    public class GetLeaguesRequestTests
    {
        [Fact]
        public void GetLeaguesRequest_DefaultConstructor_ShouldCreateInstanceWithDefaultValues()
        {
            var request = new GetLeaguesRequest();

            request.Should().NotBeNull();
            request.SportId.Should().Be(0);
            request.LeagueId.Should().BeNull();
        }

        [Fact]
        public void GetLeaguesRequest_SetProperties_ShouldReturnCorrectValues()
        {
            var request = new GetLeaguesRequest
            {
                SportId = 6046,
                LeagueId = 75
            };

            request.SportId.Should().Be(6046);
            request.LeagueId.Should().Be(75);
        }
    }
}
