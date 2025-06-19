using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Xunit;
using Trade360SDK.Common.Entities.Markets;

namespace Trade360SDK.Common.Entities.Tests.Entities.Markets
{
    public class MarketComprehensiveTests
    {
        [Fact]
        public void Market_DefaultConstructor_ShouldCreateInstanceWithDefaultValues()
        {
            // Act
            var market = new Market();

            // Assert
            market.Should().NotBeNull();
            market.Id.Should().Be(0);
            market.Name.Should().BeNull();
            market.Bets.Should().BeNull();
            market.ProviderMarkets.Should().BeNull();
            market.MainLine.Should().BeNull();
        }

        [Fact]
        public void Market_SetId_ShouldSetValue()
        {
            // Arrange
            var market = new Market();
            var id = 12345;

            // Act
            market.Id = id;

            // Assert
            market.Id.Should().Be(id);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(-1)]
        [InlineData(int.MaxValue)]
        [InlineData(int.MinValue)]
        [InlineData(999999)]
        public void Market_SetVariousIds_ShouldSetValue(int id)
        {
            // Arrange
            var market = new Market();

            // Act
            market.Id = id;

            // Assert
            market.Id.Should().Be(id);
        }

        [Fact]
        public void Market_SetName_ShouldSetValue()
        {
            // Arrange
            var market = new Market();
            var name = "Match Winner";

            // Act
            market.Name = name;

            // Assert
            market.Name.Should().Be(name);
        }

        [Theory]
        [InlineData("")]
        [InlineData("   ")]
        [InlineData("Match Winner")]
        [InlineData("Over/Under 2.5 Goals")]
        [InlineData("Both Teams To Score")]
        [InlineData("Correct Score")]
        [InlineData("Asian Handicap")]
        [InlineData("Double Chance")]
        public void Market_SetVariousNames_ShouldSetValue(string name)
        {
            // Arrange
            var market = new Market();

            // Act
            market.Name = name;

            // Assert
            market.Name.Should().Be(name);
        }

        [Fact]
        public void Market_SetBets_ShouldSetValue()
        {
            // Arrange
            var market = new Market();
            var bets = new List<Bet>
            {
                new Bet { Id = 1, Name = "Home" },
                new Bet { Id = 2, Name = "Draw" },
                new Bet { Id = 3, Name = "Away" }
            };

            // Act
            market.Bets = bets;

            // Assert
            market.Bets.Should().BeEquivalentTo(bets);
            market.Bets.Should().HaveCount(3);
        }

        [Fact]
        public void Market_SetProviderMarkets_ShouldSetValue()
        {
            // Arrange
            var market = new Market();
            var providerMarkets = new List<ProviderMarket>
            {
                new ProviderMarket { Id = 1 },
                new ProviderMarket { Id = 2 }
            };

            // Act
            market.ProviderMarkets = providerMarkets;

            // Assert
            market.ProviderMarkets.Should().BeEquivalentTo(providerMarkets);
            market.ProviderMarkets.Should().HaveCount(2);
        }

        [Fact]
        public void Market_SetMainLine_ShouldSetValue()
        {
            // Arrange
            var market = new Market();
            var mainLine = "2.5";

            // Act
            market.MainLine = mainLine;

            // Assert
            market.MainLine.Should().Be(mainLine);
        }

        [Theory]
        [InlineData("")]
        [InlineData("   ")]
        [InlineData("0")]
        [InlineData("0.5")]
        [InlineData("1.5")]
        [InlineData("2.5")]
        [InlineData("-1")]
        [InlineData("+1")]
        [InlineData("Asian Handicap -0.5")]
        public void Market_SetVariousMainLines_ShouldSetValue(string mainLine)
        {
            // Arrange
            var market = new Market();

            // Act
            market.MainLine = mainLine;

            // Assert
            market.MainLine.Should().Be(mainLine);
        }

        [Fact]
        public void Market_SetAllProperties_ShouldSetAllValues()
        {
            // Arrange
            var market = new Market();
            var id = 100;
            var name = "Total Goals";
            var bets = new List<Bet> { new Bet { Id = 1, Name = "Over" }, new Bet { Id = 2, Name = "Under" } };
            var providerMarkets = new List<ProviderMarket> { new ProviderMarket { Id = 10 } };
            var mainLine = "2.5";

            // Act
            market.Id = id;
            market.Name = name;
            market.Bets = bets;
            market.ProviderMarkets = providerMarkets;
            market.MainLine = mainLine;

            // Assert
            market.Id.Should().Be(id);
            market.Name.Should().Be(name);
            market.Bets.Should().BeEquivalentTo(bets);
            market.ProviderMarkets.Should().BeEquivalentTo(providerMarkets);
            market.MainLine.Should().Be(mainLine);
        }

        [Fact]
        public void Market_SetNullValues_ShouldSetNulls()
        {
            // Arrange
            var market = new Market();

            // Act
            market.Name = null;
            market.Bets = null;
            market.ProviderMarkets = null;
            market.MainLine = null;

            // Assert
            market.Name.Should().BeNull();
            market.Bets.Should().BeNull();
            market.ProviderMarkets.Should().BeNull();
            market.MainLine.Should().BeNull();
        }

        [Fact]
        public void Market_SetEmptyCollections_ShouldSetEmptyCollections()
        {
            // Arrange
            var market = new Market();
            var emptyBets = new List<Bet>();
            var emptyProviderMarkets = new List<ProviderMarket>();

            // Act
            market.Bets = emptyBets;
            market.ProviderMarkets = emptyProviderMarkets;

            // Assert
            market.Bets.Should().BeEmpty();
            market.ProviderMarkets.Should().BeEmpty();
        }

        [Fact]
        public void Market_PropertySettersGetters_ShouldWorkCorrectly()
        {
            // Arrange
            var market = new Market();

            // Act & Assert - Test that we can set and get each property multiple times
            market.Id = 1;
            market.Id.Should().Be(1);
            market.Id = 2;
            market.Id.Should().Be(2);

            market.Name = "Name1";
            market.Name.Should().Be("Name1");
            market.Name = "Name2";
            market.Name.Should().Be("Name2");
            market.Name = null;
            market.Name.Should().BeNull();

            market.MainLine = "1.5";
            market.MainLine.Should().Be("1.5");
            market.MainLine = "2.5";
            market.MainLine.Should().Be("2.5");
            market.MainLine = null;
            market.MainLine.Should().BeNull();
        }

        [Fact]
        public void Market_WithComplexBetData_ShouldStoreCorrectly()
        {
            // Arrange
            var market = new Market();
            var complexBets = new List<Bet>
            {
                new Bet 
                { 
                    Id = 1, 
                    Name = "Manchester United", 
                    Price = "2.50",
                    Line = "0"
                },
                new Bet 
                { 
                    Id = 2, 
                    Name = "Draw", 
                    Price = "3.20",
                    Line = null
                },
                new Bet 
                { 
                    Id = 3, 
                    Name = "Liverpool", 
                    Price = "2.80",
                    Line = "0"
                }
            };

            // Act
            market.Id = 1;
            market.Name = "Match Winner";
            market.Bets = complexBets;
            market.MainLine = "0";

            // Assert
            market.Id.Should().Be(1);
            market.Name.Should().Be("Match Winner");
            market.Bets.Should().HaveCount(3);
            market.Bets.First().Name.Should().Be("Manchester United");
            market.Bets.Skip(1).First().Name.Should().Be("Draw");
            market.Bets.Last().Name.Should().Be("Liverpool");
            market.MainLine.Should().Be("0");
        }

        [Fact]
        public void Market_ToString_ShouldReturnStringRepresentation()
        {
            // Arrange
            var market = new Market
            {
                Id = 1,
                Name = "Test Market"
            };

            // Act
            var result = market.ToString();

            // Assert
            result.Should().NotBeNullOrEmpty();
            result.Should().Contain("Market");
        }

        [Fact]
        public void Market_GetHashCode_ShouldReturnConsistentValue()
        {
            // Arrange
            var market = new Market
            {
                Id = 1,
                Name = "Test Market"
            };

            // Act
            var hashCode1 = market.GetHashCode();
            var hashCode2 = market.GetHashCode();

            // Assert
            hashCode1.Should().Be(hashCode2);
        }

        [Fact]
        public void Market_Type_ShouldBeCorrect()
        {
            // Arrange & Act
            var market = new Market();

            // Assert
            market.GetType().Should().Be(typeof(Market));
            market.GetType().Name.Should().Be("Market");
            market.GetType().Namespace.Should().Be("Trade360SDK.Common.Entities.Markets");
        }
    }
} 