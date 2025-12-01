using System.Collections.Generic;
using Trade360SDK.Common.Entities.OutrightLeague;
using Xunit;

namespace Trade360SDK.Common.Tests
{
    public class OutrightLeagueMarketEventTests
    {
        [Fact]
        public void Properties_ShouldGetAndSetValues()
        {
            var markets = new List<MarketLeague> { new MarketLeague { Id = 1 } };
            var evt = new OutrightLeagueMarketEvent
            {
                FixtureId = 123,
                Markets = markets
            };
            Assert.Equal(123, evt.FixtureId);
            Assert.Equal(markets, evt.Markets);
        }

        [Fact]
        public void Properties_ShouldAllowNullsAndDefaults()
        {
            var evt = new OutrightLeagueMarketEvent();
            Assert.Equal(0, evt.FixtureId); // default int
            Assert.Null(evt.Markets);
        }
    }
} 