using Trade360SDK.Common.Entities.MessageTypes;
using Trade360SDK.Common.Entities.OutrightLeague;
using Xunit;

namespace Trade360SDK.Common.Tests.Entities.MessageTypes
{
    public class OutrightLeagueUpdateTests
    {
        [Fact]
        public void Properties_ShouldGetAndSetValues()
        {
            var competition = new OutrightLeagueCompetitionWrapper<OutrightLeagueEvent>();
            var update = new OutrightLeagueUpdate
            {
                Competition = competition
            };
            Assert.Equal(competition, update.Competition);
        }

        [Fact]
        public void Properties_ShouldAllowNullsAndDefaults()
        {
            var update = new OutrightLeagueUpdate();
            Assert.Null(update.Competition);
        }
    }
} 