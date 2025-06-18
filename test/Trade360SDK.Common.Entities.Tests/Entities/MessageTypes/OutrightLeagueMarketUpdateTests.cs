using Trade360SDK.Common.Entities.MessageTypes;
using Trade360SDK.Common.Entities.OutrightLeague;
using Xunit;

namespace Trade360SDK.Common.Tests.Entities.MessageTypes
{
    public class OutrightLeagueMarketUpdateTests
    {
        [Fact]
        public void Properties_ShouldGetAndSetValues()
        {
            var competition = new OutrightLeagueCompetitionWrapper<OutrightLeagueMarketEvent>();
            var update = new OutrightLeagueMarketUpdate
            {
                Competition = competition
            };
            Assert.Equal(competition, update.Competition);
        }

        [Fact]
        public void Properties_ShouldAllowNullsAndDefaults()
        {
            var update = new OutrightLeagueMarketUpdate();
            Assert.Null(update.Competition);
        }
    }
} 