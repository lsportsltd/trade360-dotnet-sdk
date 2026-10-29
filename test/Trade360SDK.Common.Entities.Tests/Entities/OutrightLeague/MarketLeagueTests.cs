using System;
using System.Collections.Generic;
using System.Text.Json;
using Trade360SDK.Common.Entities.Enums;
using Trade360SDK.Common.Entities.OutrightLeague;
using Trade360SDK.Common.Entities.Markets;
using Xunit;

namespace Trade360SDK.Common.Tests
{
    public class MarketLeagueTests
    {
        [Fact]
        public void Properties_ShouldGetAndSetValues()
        {
            var bets = new List<Bet> { new Bet { Id = 1 } };
            var providerMarkets = new List<ProviderMarket>
            {
                new ProviderMarket
                {
                    Id = 100,
                    Name = "ProviderMarketName",
                    LastUpdate = new DateTime(2025, 1, 1, 12, 0, 0),
                    Bets = new List<ProviderBet> { new ProviderBet { Id = 1001 } }
                }
            };
            var league = new MarketLeague
            {
                Id = 10,
                Name = "LeagueName",
                Bets = bets,
                ProviderMarkets = providerMarkets,
                MainLine = "2.5",
                Status = MarketStatus.Suspended
            };
            Assert.Equal(10, league.Id);
            Assert.Equal("LeagueName", league.Name);
            Assert.Equal(bets, league.Bets);
            Assert.Equal(providerMarkets, league.ProviderMarkets);
            Assert.Equal("2.5", league.MainLine);
            Assert.Equal(MarketStatus.Suspended, league.Status);
        }

        [Fact]
        public void Properties_ShouldAllowNulls()
        {
            var league = new MarketLeague();
            Assert.Null(league.Name);
            Assert.Null(league.Bets);
            Assert.Null(league.ProviderMarkets);
            Assert.Null(league.MainLine);
            Assert.Equal(MarketStatus.NotSet, league.Status);
        }

        [Fact]
        public void DeserializeMarketStatusFromJson_MapsStatusProperty()
        {
            var market = JsonSerializer.Deserialize<MarketLeague>(
                "{\"Id\":590,\"Name\":\"To Be Relegated\",\"Status\":2,\"Bets\":[]}");

            Assert.NotNull(market);
            Assert.Equal(590, market!.Id);
            Assert.Equal("To Be Relegated", market.Name);
            Assert.Equal(MarketStatus.Suspended, market.Status);
        }

        [Fact]
        public void DeserializeClosedMarketStatusFromJson_MapsStatusProperty()
        {
            var market = JsonSerializer.Deserialize<MarketLeague>(
                "{\"Id\":590,\"Name\":\"To Be Relegated\",\"Status\":4,\"Bets\":[]}");

            Assert.NotNull(market);
            Assert.Equal(MarketStatus.Closed, market!.Status);
        }
    }
} 