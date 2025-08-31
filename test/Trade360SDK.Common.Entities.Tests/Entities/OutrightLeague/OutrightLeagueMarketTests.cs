using System.Collections.Generic;
using System.Linq;
using Trade360SDK.Common.Entities.Markets;
using Trade360SDK.Common.Entities.OutrightLeague;
using Xunit;

namespace Trade360SDK.Common.Tests
{
    public class OutrightLeagueMarketTests
    {
        [Fact]
        public void Properties_ShouldGetAndSetValues()
        {
            var bets = new List<Bet> { new Bet { Id = 1, Name = "Test Bet" } };
            var market = new OutrightLeagueMarket
            {
                Id = 123,
                Name = "Test Market",
                Bets = bets
            };
            
            Assert.Equal(123, market.Id);
            Assert.Equal("Test Market", market.Name);
            Assert.Equal(bets, market.Bets);
        }

        [Fact]
        public void Properties_ShouldAllowNullsAndDefaults()
        {
            var market = new OutrightLeagueMarket();
            
            Assert.Equal(0, market.Id); // default int
            Assert.Null(market.Name);
            Assert.Null(market.Bets);
        }

        [Fact]
        public void Name_ShouldAcceptEmptyString()
        {
            var market = new OutrightLeagueMarket
            {
                Name = ""
            };
            
            Assert.Equal("", market.Name);
        }

        [Fact]
        public void Bets_ShouldAcceptEmptyCollection()
        {
            var emptyBets = new List<Bet>();
            var market = new OutrightLeagueMarket
            {
                Bets = emptyBets
            };
            
            Assert.NotNull(market.Bets);
            Assert.Empty(market.Bets);
        }

        [Fact]
        public void Bets_ShouldHandleMultipleBets()
        {
            var bets = new List<Bet>
            {
                new Bet { Id = 1, Name = "Bet 1" },
                new Bet { Id = 2, Name = "Bet 2" },
                new Bet { Id = 3, Name = "Bet 3" }
            };
            
            var market = new OutrightLeagueMarket
            {
                Bets = bets
            };
            
            Assert.Equal(3, market.Bets.Count());
            Assert.Equal("Bet 1", market.Bets.First().Name);
            Assert.Equal("Bet 3", market.Bets.Last().Name);
        }

        [Fact]
        public void Id_ShouldAcceptNegativeValues()
        {
            var market = new OutrightLeagueMarket
            {
                Id = -1
            };
            
            Assert.Equal(-1, market.Id);
        }

        [Fact]
        public void Properties_ShouldBeIndependentlySettable()
        {
            var market = new OutrightLeagueMarket();
            
            market.Id = 456;
            Assert.Equal(456, market.Id);
            Assert.Null(market.Name);
            Assert.Null(market.Bets);
            
            market.Name = "Updated Market";
            Assert.Equal("Updated Market", market.Name);
            Assert.Equal(456, market.Id);
            Assert.Null(market.Bets);
            
            var newBets = new List<Bet> { new Bet { Id = 10 } };
            market.Bets = newBets;
            Assert.Equal(newBets, market.Bets);
            Assert.Equal("Updated Market", market.Name);
            Assert.Equal(456, market.Id);
        }
    }
}

