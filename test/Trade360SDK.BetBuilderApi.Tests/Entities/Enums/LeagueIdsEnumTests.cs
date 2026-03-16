using FluentAssertions;
using Trade360SDK.BetBuilderApi.Entities.Enums;
using Xunit;

namespace Trade360SDK.BetBuilderApi.Tests.Entities.Enums
{
    public class LeagueIdsEnumTests
    {
        [Fact]
        public void LeagueIdsEnum_NFL_ShouldHaveCorrectValue()
        {
            ((int)LeagueIdsEnum.NFL).Should().Be(75);
        }

        [Fact]
        public void LeagueIdsEnum_NBA_ShouldHaveCorrectValue()
        {
            ((int)LeagueIdsEnum.NBA).Should().Be(64);
        }

        [Fact]
        public void LeagueIdsEnum_NCAA_ShouldHaveCorrectValue()
        {
            ((int)LeagueIdsEnum.NCAA).Should().Be(32505);
        }

        [Fact]
        public void LeagueIdsEnum_WNBA_ShouldHaveCorrectValue()
        {
            ((int)LeagueIdsEnum.WNBA).Should().Be(761);
        }

        [Fact]
        public void LeagueIdsEnum_Euroleague_ShouldHaveCorrectValue()
        {
            ((int)LeagueIdsEnum.Euroleague).Should().Be(33249);
        }

        [Fact]
        public void LeagueIdsEnum_NCAABasketball_ShouldHaveCorrectValue()
        {
            ((int)LeagueIdsEnum.NCAABasketball).Should().Be(4045);
        }
    }
}
