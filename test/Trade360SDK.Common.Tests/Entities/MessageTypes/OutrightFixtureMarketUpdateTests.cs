using Trade360SDK.Common.Entities.Markets;
using Trade360SDK.Common.Entities.MessageTypes;
using Trade360SDK.Common.Entities.OutrightSport;
using Xunit;

namespace Trade360SDK.Common.Tests.Entities.MessageTypes
{
    public class OutrightFixtureMarketUpdateTests
    {
        [Fact]
        public void Properties_ShouldGetAndSetValues()
        {
            var competition = new OutrightCompetition<MarketEvent>();
            var update = new OutrightFixtureMarketUpdate
            {
                Competition = competition
            };
            Assert.Equal(competition, update.Competition);
        }

        [Fact]
        public void Properties_ShouldAllowNullsAndDefaults()
        {
            var update = new OutrightFixtureMarketUpdate();
            Assert.Null(update.Competition);
        }
    }
} 