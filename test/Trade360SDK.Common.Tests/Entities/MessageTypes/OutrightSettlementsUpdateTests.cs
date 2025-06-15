using Trade360SDK.Common.Entities.MessageTypes;
using Trade360SDK.Common.Entities.Markets;
using Trade360SDK.Common.Entities.OutrightSport;
using Xunit;

namespace Trade360SDK.Common.Tests.Entities.MessageTypes
{
    public class OutrightSettlementsUpdateTests
    {
        [Fact]
        public void Properties_ShouldGetAndSetValues()
        {
            var competition = new OutrightCompetition<MarketEvent>();
            var update = new OutrightSettlementsUpdate
            {
                Competition = competition
            };
            Assert.Equal(competition, update.Competition);
        }

        [Fact]
        public void Properties_ShouldAllowNullsAndDefaults()
        {
            var update = new OutrightSettlementsUpdate();
            Assert.Null(update.Competition);
        }
    }
} 