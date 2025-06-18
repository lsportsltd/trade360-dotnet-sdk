using Trade360SDK.Common.Entities.MessageTypes;
using Trade360SDK.Common.Entities.OutrightSport;
using Xunit;

namespace Trade360SDK.Common.Tests.Entities.MessageTypes
{
    public class OutrightScoreUpdateTests
    {
        [Fact]
        public void Properties_ShouldGetAndSetValues()
        {
            var competition = new OutrightCompetition<OutrightScoreEvent>();
            var update = new OutrightScoreUpdate
            {
                Competition = competition
            };
            Assert.Equal(competition, update.Competition);
        }

        [Fact]
        public void Properties_ShouldAllowNullsAndDefaults()
        {
            var update = new OutrightScoreUpdate();
            Assert.Null(update.Competition);
        }
    }
} 