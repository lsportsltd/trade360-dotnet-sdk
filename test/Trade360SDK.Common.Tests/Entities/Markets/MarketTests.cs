using System.Collections.Generic;
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
    }
} 