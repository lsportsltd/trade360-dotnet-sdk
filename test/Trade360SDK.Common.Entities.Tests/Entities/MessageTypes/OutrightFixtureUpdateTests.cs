using Trade360SDK.Common.Entities.MessageTypes;
using Trade360SDK.Common.Entities.OutrightSport;
using Xunit;

namespace Trade360SDK.Common.Tests.Entities.MessageTypes
{
    public class OutrightFixtureUpdateTests
    {
        [Fact]
        public void Properties_ShouldGetAndSetValues()
        {
            var competition = new OutrightCompetition<OutrightFixtureEvent>();
            var update = new OutrightFixtureUpdate
            {
                Competition = competition
            };
            Assert.Equal(competition, update.Competition);
        }

        [Fact]
        public void Properties_ShouldAllowNullsAndDefaults()
        {
            var update = new OutrightFixtureUpdate();
            Assert.Null(update.Competition);
        }
    }
} 