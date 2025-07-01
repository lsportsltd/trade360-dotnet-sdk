using FluentAssertions;
using Trade360SDK.Common.Entities.Markets;
using Trade360SDK.Common.Entities.OutrightLeague;
using Xunit;
using System.Collections.Generic;
using System.Linq;

namespace Trade360SDK.Common.Entities.Tests.Entities.OutrightLeague
{
    public class MarketLeagueComprehensiveTests
    {
        [Fact]
        public void MarketLeague_DefaultConstructor_ShouldInitializeWithDefaults()
        {
            // Arrange & Act
            var marketLeague = new MarketLeague();

            // Assert
            marketLeague.Id.Should().Be(0);
            marketLeague.Name.Should().BeNull();
            marketLeague.Bets.Should().BeNull();
            marketLeague.MainLine.Should().BeNull();
        }

        [Fact]
        public void Id_WhenSet_ShouldReturnExpectedValue()
        {
            // Arrange
            var marketLeague = new MarketLeague();
            const int expectedId = 12345;

            // Act
            marketLeague.Id = expectedId;

            // Assert
            marketLeague.Id.Should().Be(expectedId);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(-1)]
        [InlineData(int.MaxValue)]
        [InlineData(int.MinValue)]
        public void Id_WithVariousValues_ShouldStoreCorrectly(int id)
        {
            // Arrange
            var marketLeague = new MarketLeague();

            // Act
            marketLeague.Id = id;

            // Assert
            marketLeague.Id.Should().Be(id);
        }

        [Fact]
        public void Name_WhenSet_ShouldReturnExpectedValue()
        {
            // Arrange
            var marketLeague = new MarketLeague();
            const string expectedName = "Premier League";

            // Act
            marketLeague.Name = expectedName;

            // Assert
            marketLeague.Name.Should().Be(expectedName);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("Short")]
        [InlineData("Very long league name with special characters !@#$%^&*()")]
        public void Name_WithVariousValues_ShouldStoreCorrectly(string name)
        {
            // Arrange
            var marketLeague = new MarketLeague();

            // Act
            marketLeague.Name = name;

            // Assert
            marketLeague.Name.Should().Be(name);
        }

        [Fact]
        public void MainLine_WhenSet_ShouldReturnExpectedValue()
        {
            // Arrange
            var marketLeague = new MarketLeague();
            const string expectedMainLine = "2.5";

            // Act
            marketLeague.MainLine = expectedMainLine;

            // Assert
            marketLeague.MainLine.Should().Be(expectedMainLine);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("0")]
        [InlineData("1.5")]
        [InlineData("-1.5")]
        [InlineData("Draw")]
        public void MainLine_WithVariousValues_ShouldStoreCorrectly(string mainLine)
        {
            // Arrange
            var marketLeague = new MarketLeague();

            // Act
            marketLeague.MainLine = mainLine;

            // Assert
            marketLeague.MainLine.Should().Be(mainLine);
        }

        [Fact]
        public void Bets_WhenSetToCollection_ShouldReturnExpectedValue()
        {
            // Arrange
            var marketLeague = new MarketLeague();
            var bets = new List<Bet>
            {
                new Bet { Id = 1, ProviderBetId = "BET-001" },
                new Bet { Id = 2, ProviderBetId = "BET-002" }
            };

            // Act
            marketLeague.Bets = bets;

            // Assert
            marketLeague.Bets.Should().NotBeNull();
            marketLeague.Bets.Should().HaveCount(2);
            marketLeague.Bets.Should().ContainEquivalentOf(bets[0]);
            marketLeague.Bets.Should().ContainEquivalentOf(bets[1]);
        }

        [Fact]
        public void Bets_WhenSetToEmptyCollection_ShouldHandleGracefully()
        {
            // Arrange
            var marketLeague = new MarketLeague();
            var emptyBets = new List<Bet>();

            // Act
            marketLeague.Bets = emptyBets;

            // Assert
            marketLeague.Bets.Should().NotBeNull().And.BeEmpty();
        }

        [Fact]
        public void MarketLeague_WithAllPropertiesSet_ShouldRetainAllValues()
        {
            // Arrange
            const int expectedId = 999;
            const string expectedName = "Champions League";
            const string expectedMainLine = "Over 2.5";
            var expectedBets = new List<Bet>
            {
                new Bet { Id = 100, ProviderBetId = "PROV-100" }
            };

            var marketLeague = new MarketLeague
            {
                Id = expectedId,
                Name = expectedName,
                MainLine = expectedMainLine,
                Bets = expectedBets
            };

            // Act & Assert
            marketLeague.Id.Should().Be(expectedId);
            marketLeague.Name.Should().Be(expectedName);
            marketLeague.MainLine.Should().Be(expectedMainLine);
            marketLeague.Bets.Should().BeEquivalentTo(expectedBets);
        }

        [Fact]
        public void MarketLeague_PropertiesCanBeOverwritten()
        {
            // Arrange
            var marketLeague = new MarketLeague
            {
                Id = 123,
                Name = "Initial Name",
                MainLine = "Initial Line",
                Bets = new List<Bet> { new Bet { Id = 999 } }
            };

            const int newId = 456;
            const string newName = "Updated Name";
            const string newMainLine = "Updated Line";
            var newBets = new List<Bet> { new Bet { Id = 888 } };

            // Act
            marketLeague.Id = newId;
            marketLeague.Name = newName;
            marketLeague.MainLine = newMainLine;
            marketLeague.Bets = newBets;

            // Assert
            marketLeague.Id.Should().Be(newId);
            marketLeague.Name.Should().Be(newName);
            marketLeague.MainLine.Should().Be(newMainLine);
            marketLeague.Bets.Should().BeEquivalentTo(newBets);
        }

        [Fact]
        public void MarketLeague_CollectionProperties_CanBeSetToNull()
        {
            // Arrange
            var marketLeague = new MarketLeague
            {
                Bets = new List<Bet> { new Bet() }
            };

            // Act
            marketLeague.Bets = null;

            // Assert
            marketLeague.Bets.Should().BeNull();
        }

        [Fact]
        public void MarketLeague_StringProperties_CanBeSetToNull()
        {
            // Arrange
            var marketLeague = new MarketLeague
            {
                Name = "Some Name",
                MainLine = "Some Line"
            };

            // Act
            marketLeague.Name = null;
            marketLeague.MainLine = null;

            // Assert
            marketLeague.Name.Should().BeNull();
            marketLeague.MainLine.Should().BeNull();
        }
    }
} 