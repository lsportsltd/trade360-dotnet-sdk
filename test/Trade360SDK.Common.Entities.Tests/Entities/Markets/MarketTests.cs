using System.Collections.Generic;
using System.Text.Json;
using Trade360SDK.Common.Entities.Enums;
using Trade360SDK.Common.Entities.Markets;
using Xunit;

namespace Trade360SDK.Common.Tests
{
    public class MarketTests
    {
        [Fact]
        public void Properties_ShouldGetAndSetValues()
        {
            var bets = new List<Bet> { new Bet { Id = 1 } };
            var providerMarkets = new List<ProviderMarket> { new ProviderMarket { Id = 2 } };
            var market = new Market
            {
                Id = 10,
                Name = "MarketName",
                Bets = bets,
                ProviderMarkets = providerMarkets,
                MainLine = "2.5"
            };
            Assert.Equal(10, market.Id);
            Assert.Equal("MarketName", market.Name);
            Assert.Equal(bets, market.Bets);
            Assert.Equal(providerMarkets, market.ProviderMarkets);
            Assert.Equal("2.5", market.MainLine);
        }

        [Fact]
        public void Properties_ShouldAllowNulls()
        {
            var market = new Market();
            Assert.Null(market.Name);
            Assert.Null(market.Bets);
            Assert.Null(market.ProviderMarkets);
            Assert.Null(market.MainLine);
        }

        [Fact]
        public void DeserializeMarketStatusFromJson_MapsStatusProperty()
        {
            var market = JsonSerializer.Deserialize<Market>("{\"Id\":52,\"Name\":\"1X2\",\"Status\":2,\"Bets\":[]}");

            Assert.NotNull(market);
            Assert.Equal(52, market!.Id);
            Assert.Equal("1X2", market.Name);
            Assert.Equal(MarketStatus.Suspended, market.Status);
        }

        [Fact]
        public void DeserializeMarketStatusFromJson_MapsMarketStatusProperty()
        {
            var market = JsonSerializer.Deserialize<Market>("{\"Id\":52,\"Name\":\"1X2\",\"MarketStatus\":2,\"Bets\":[]}");

            Assert.NotNull(market);
            Assert.Equal(MarketStatus.Suspended, market!.Status);
        }
    }
}