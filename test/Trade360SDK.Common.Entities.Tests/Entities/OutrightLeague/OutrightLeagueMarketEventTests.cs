using System.Collections.Generic;
using System.Text.Json;
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
                FixtureName = "Premier League 2023/2024 Outright Winner",
                Markets = markets
            };
            Assert.Equal(123, evt.FixtureId);
            Assert.Equal("Premier League 2023/2024 Outright Winner", evt.FixtureName);
            Assert.Equal(markets, evt.Markets);
        }

        [Fact]
        public void Properties_ShouldAllowNullsAndDefaults()
        {
            var evt = new OutrightLeagueMarketEvent();
            Assert.Equal(0, evt.FixtureId); // default int
            Assert.Null(evt.FixtureName);
            Assert.Null(evt.Markets);
        }

        [Fact]
        public void FixtureName_EmptyOrWhitespace_ShouldNormalizeToNull()
        {
            var evt = new OutrightLeagueMarketEvent
            {
                FixtureName = ""
            };
            Assert.Null(evt.FixtureName);

            evt.FixtureName = "Premier League 2023/2024 Outright Winner";
            evt.FixtureName = null;
            Assert.Null(evt.FixtureName);
        }

        [Fact]
        public void JsonSerialization_WhenFixtureNameNull_ShouldOmitProperty()
        {
            var evt = new OutrightLeagueMarketEvent
            {
                FixtureId = 24603148,
                FixtureName = null
            };

            var json = JsonSerializer.Serialize(evt);

            Assert.DoesNotContain("FixtureName", json);
            Assert.Contains("\"FixtureId\":24603148", json);
        }

        [Fact]
        public void JsonSerialization_WhenFixtureNamePresent_ShouldIncludeProperty()
        {
            var evt = new OutrightLeagueMarketEvent
            {
                FixtureId = 24603148,
                FixtureName = "Premier League 2023/2024 Outright Winner"
            };

            var json = JsonSerializer.Serialize(evt);

            Assert.Contains("\"FixtureName\":\"Premier League 2023/2024 Outright Winner\"", json);
        }
    }
} 