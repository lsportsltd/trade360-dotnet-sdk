using System;
using System.Collections.Generic;
using System.Text.Json;
using Trade360SDK.Common.Entities.Enums;
using Trade360SDK.Common.Entities.Markets;
using Xunit;

namespace Trade360SDK.Common.Tests
{
    public class ProviderMarketTests
    {
        [Fact]
        public void Properties_ShouldGetAndSetValues()
        {
            var bets = new List<ProviderBet> { new ProviderBet { Id = 1 } };
            var market = new ProviderMarket
            {
                Id = 5,
                Name = "ProviderMarketName",
                Bets = bets,
                LastUpdate = DateTime.UtcNow
            };
            Assert.Equal(5, market.Id);
            Assert.Equal("ProviderMarketName", market.Name);
            Assert.Equal(bets, market.Bets);
            Assert.NotEqual(default, market.LastUpdate);
        }

        [Fact]
        public void Properties_ShouldAllowNulls()
        {
            var market = new ProviderMarket();
            Assert.Null(market.Name);
            Assert.Null(market.Bets);
        }

        [Fact]
        public void DeserializeMarketStatusFromJson_MapsMarketStatusProperty()
        {
            var providerMarket = JsonSerializer.Deserialize<ProviderMarket>(
                "{\"Id\":57,\"Name\":\"Bet365\",\"MarketStatus\":2,\"Bets\":[]}");

            Assert.NotNull(providerMarket);
            Assert.Equal(57, providerMarket!.Id);
            Assert.Equal("Bet365", providerMarket.Name);
            Assert.Equal(MarketStatus.Suspended, providerMarket.MarketStatus);
        }

        [Fact]
        public void DeserializeClosedMarketStatusFromJson_MapsMarketStatusProperty()
        {
            var providerMarket = JsonSerializer.Deserialize<ProviderMarket>(
                "{\"Id\":13,\"Name\":\"BWin\",\"MarketStatus\":4,\"Bets\":[]}");

            Assert.NotNull(providerMarket);
            Assert.Equal(13, providerMarket!.Id);
            Assert.Equal("BWin", providerMarket.Name);
            Assert.Equal(MarketStatus.Closed, providerMarket.MarketStatus);
        }
    }
} 