using Trade360SDK.Common.Entities.Markets;
using Xunit;
using System.Collections.Generic;

namespace Trade360SDK.Common.Tests
{
    public class MarketEventTests
    {
        [Fact]
        public void Properties_ShouldGetAndSetValues()
        {
            var markets = new List<Market>();
            var evt = new MarketEvent { FixtureId = 7, Markets = markets };
            Assert.Equal(7, evt.FixtureId);
            Assert.Equal(markets, evt.Markets);
        }

        [Fact]
        public void Properties_ShouldAllowNulls()
        {
            var evt = new MarketEvent();
            Assert.Null(evt.Markets);
        }
    }
} 