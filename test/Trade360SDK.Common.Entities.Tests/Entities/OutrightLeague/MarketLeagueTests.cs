using System;
using System.Collections.Generic;
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
                MainLine = "2.5"
            };
            Assert.Equal(10, league.Id);
            Assert.Equal("LeagueName", league.Name);
            Assert.Equal(bets, league.Bets);
            Assert.Equal(providerMarkets, league.ProviderMarkets);
            Assert.Equal("2.5", league.MainLine);
        }

        [Fact]
        public void Properties_ShouldAllowNulls()
        {
            var league = new MarketLeague();
            Assert.Null(league.Name);
            Assert.Null(league.Bets);
            Assert.Null(league.ProviderMarkets);
            Assert.Null(league.MainLine);
        }
    }
} 