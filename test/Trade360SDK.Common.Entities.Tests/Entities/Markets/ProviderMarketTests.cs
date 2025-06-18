using System;
using System.Collections.Generic;
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
    }
} 