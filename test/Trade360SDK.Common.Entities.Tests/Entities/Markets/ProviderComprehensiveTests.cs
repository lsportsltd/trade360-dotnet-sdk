using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Trade360SDK.Common.Entities.Markets;
using Xunit;

namespace Trade360SDK.Common.Entities.Tests.Entities.Markets
{
    public class ProviderComprehensiveTests
    {
        #region Constructor Tests

        [Fact]
        public void Provider_DefaultConstructor_ShouldInitializeCorrectly()
        {
            // Act
            var provider = new Provider();

            // Assert
            provider.Should().NotBeNull();
            provider.Id.Should().Be(0);
            provider.Name.Should().BeNull();
            provider.LastUpdate.Should().Be(default(DateTime));
            provider.ProviderFixtureId.Should().BeNull();
            provider.ProviderLeagueId.Should().BeNull();
            provider.ProviderMarketId.Should().BeNull();
            provider.Bets.Should().BeNull();
        }

        #endregion

        #region Property Tests

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(100)]
        [InlineData(999999)]
        [InlineData(-1)]
        [InlineData(int.MaxValue)]
        [InlineData(int.MinValue)]
        public void Provider_IdProperty_ShouldSetAndGetCorrectly(int id)
        {
            // Arrange
            var provider = new Provider();

            // Act
            provider.Id = id;

            // Assert
            provider.Id.Should().Be(id);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("Bet365")]
        [InlineData("William Hill")]
        [InlineData("Pinnacle Sports")]
        [InlineData("Very Long Provider Name That Might Be Used In Real World")]
        [InlineData("Provider123")]
        [InlineData("Provider-With-Dashes")]
        [InlineData("Provider_With_Underscores")]
        [InlineData("Provider.With.Dots")]
        [InlineData("Provider With Spaces")]
        public void Provider_NameProperty_ShouldSetAndGetCorrectly(string? name)
        {
            // Arrange
            var provider = new Provider();

            // Act
            provider.Name = name;

            // Assert
            provider.Name.Should().Be(name);
        }

        [Fact]
        public void Provider_LastUpdateProperty_ShouldSetAndGetCorrectly()
        {
            // Arrange
            var provider = new Provider();
            var testDate = new DateTime(2024, 6, 25, 14, 30, 45);

            // Act
            provider.LastUpdate = testDate;

            // Assert
            provider.LastUpdate.Should().Be(testDate);
        }

        [Fact]
        public void Provider_LastUpdateProperty_WithMinValue_ShouldSetCorrectly()
        {
            // Arrange
            var provider = new Provider();

            // Act
            provider.LastUpdate = DateTime.MinValue;

            // Assert
            provider.LastUpdate.Should().Be(DateTime.MinValue);
        }

        [Fact]
        public void Provider_LastUpdateProperty_WithMaxValue_ShouldSetCorrectly()
        {
            // Arrange
            var provider = new Provider();

            // Act
            provider.LastUpdate = DateTime.MaxValue;

            // Assert
            provider.LastUpdate.Should().Be(DateTime.MaxValue);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("12345")]
        [InlineData("FIXTURE_123")]
        [InlineData("fixture-456")]
        [InlineData("Fix.789")]
        [InlineData("Very_Long_Provider_Fixture_Id_That_Might_Be_Used")]
        public void Provider_ProviderFixtureIdProperty_ShouldSetAndGetCorrectly(string? fixtureId)
        {
            // Arrange
            var provider = new Provider();

            // Act
            provider.ProviderFixtureId = fixtureId;

            // Assert
            provider.ProviderFixtureId.Should().Be(fixtureId);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("67890")]
        [InlineData("LEAGUE_ABC")]
        [InlineData("league-def")]
        [InlineData("Lea.gue123")]
        [InlineData("Premier_League_2024")]
        public void Provider_ProviderLeagueIdProperty_ShouldSetAndGetCorrectly(string? leagueId)
        {
            // Arrange
            var provider = new Provider();

            // Act
            provider.ProviderLeagueId = leagueId;

            // Assert
            provider.ProviderLeagueId.Should().Be(leagueId);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("MKT123")]
        [InlineData("MARKET_XYZ")]
        [InlineData("market-789")]
        [InlineData("Mkt.456")]
        [InlineData("Over_Under_2_5_Goals")]
        public void Provider_ProviderMarketIdProperty_ShouldSetAndGetCorrectly(string? marketId)
        {
            // Arrange
            var provider = new Provider();

            // Act
            provider.ProviderMarketId = marketId;

            // Assert
            provider.ProviderMarketId.Should().Be(marketId);
        }

        [Fact]
        public void Provider_BetsProperty_WithNull_ShouldSetAndGetCorrectly()
        {
            // Arrange
            var provider = new Provider();

            // Act
            provider.Bets = null;

            // Assert
            provider.Bets.Should().BeNull();
        }

        [Fact]
        public void Provider_BetsProperty_WithEmptyCollection_ShouldSetAndGetCorrectly()
        {
            // Arrange
            var provider = new Provider();
            var emptyBets = new List<Bet>();

            // Act
            provider.Bets = emptyBets;

            // Assert
            provider.Bets.Should().NotBeNull();
            provider.Bets.Should().BeEmpty();
            provider.Bets.Should().BeSameAs(emptyBets);
        }

        [Fact]
        public void Provider_BetsProperty_WithSingleBet_ShouldSetAndGetCorrectly()
        {
            // Arrange
            var provider = new Provider();
            var bet = new Bet { Id = 1, Name = "Home Win" };
            var bets = new List<Bet> { bet };

            // Act
            provider.Bets = bets;

            // Assert
            provider.Bets.Should().NotBeNull();
            provider.Bets.Should().HaveCount(1);
            provider.Bets.Should().Contain(bet);
            provider.Bets.First().Id.Should().Be(1);
            provider.Bets.First().Name.Should().Be("Home Win");
        }

        [Fact]
        public void Provider_BetsProperty_WithMultipleBets_ShouldSetAndGetCorrectly()
        {
            // Arrange
            var provider = new Provider();
            var bet1 = new Bet { Id = 1, Name = "Home Win" };
            var bet2 = new Bet { Id = 2, Name = "Draw" };
            var bet3 = new Bet { Id = 3, Name = "Away Win" };
            var bets = new List<Bet> { bet1, bet2, bet3 };

            // Act
            provider.Bets = bets;

            // Assert
            provider.Bets.Should().NotBeNull();
            provider.Bets.Should().HaveCount(3);
            provider.Bets.Should().Contain(bet1);
            provider.Bets.Should().Contain(bet2);
            provider.Bets.Should().Contain(bet3);
        }

        #endregion

        #region Integration Tests

        [Fact]
        public void Provider_SetAllProperties_ShouldSetAllValues()
        {
            // Arrange
            var provider = new Provider();
            var id = 999;
            var name = "Test Provider";
            var lastUpdate = new DateTime(2024, 6, 25, 14, 30, 45);
            var fixtureId = "FIXTURE_123";
            var leagueId = "LEAGUE_456";
            var marketId = "MARKET_789";
            var bets = new List<Bet>
            {
                new Bet { Id = 1, Name = "Bet 1" },
                new Bet { Id = 2, Name = "Bet 2" }
            };

            // Act
            provider.Id = id;
            provider.Name = name;
            provider.LastUpdate = lastUpdate;
            provider.ProviderFixtureId = fixtureId;
            provider.ProviderLeagueId = leagueId;
            provider.ProviderMarketId = marketId;
            provider.Bets = bets;

            // Assert
            provider.Id.Should().Be(id);
            provider.Name.Should().Be(name);
            provider.LastUpdate.Should().Be(lastUpdate);
            provider.ProviderFixtureId.Should().Be(fixtureId);
            provider.ProviderLeagueId.Should().Be(leagueId);
            provider.ProviderMarketId.Should().Be(marketId);
            provider.Bets.Should().BeEquivalentTo(bets);
        }

        [Fact]
        public void Provider_SetAllPropertiesToNull_ShouldSetNullValues()
        {
            // Arrange
            var provider = new Provider
            {
                Id = 123,
                Name = "Initial Name",
                LastUpdate = DateTime.Now,
                ProviderFixtureId = "Initial Fixture",
                ProviderLeagueId = "Initial League",
                ProviderMarketId = "Initial Market",
                Bets = new List<Bet> { new Bet { Id = 1 } }
            };

            // Act
            provider.Name = null;
            provider.ProviderFixtureId = null;
            provider.ProviderLeagueId = null;
            provider.ProviderMarketId = null;
            provider.Bets = null;

            // Assert
            provider.Id.Should().Be(123); // Value type, can't be null
            provider.Name.Should().BeNull();
            provider.ProviderFixtureId.Should().BeNull();
            provider.ProviderLeagueId.Should().BeNull();
            provider.ProviderMarketId.Should().BeNull();
            provider.Bets.Should().BeNull();
        }

        #endregion

        #region Edge Case Tests

        [Fact]
        public void Provider_WithUtcDateTime_ShouldHandleCorrectly()
        {
            // Arrange
            var provider = new Provider();
            var utcTime = DateTime.UtcNow;

            // Act
            provider.LastUpdate = utcTime;

            // Assert
            provider.LastUpdate.Should().Be(utcTime);
        }

        [Fact]
        public void Provider_WithLocalDateTime_ShouldHandleCorrectly()
        {
            // Arrange
            var provider = new Provider();
            var localTime = DateTime.Now;

            // Act
            provider.LastUpdate = localTime;

            // Assert
            provider.LastUpdate.Should().Be(localTime);
        }

        [Fact]
        public void Provider_WithSpecificDateTime_ShouldPreservePrecision()
        {
            // Arrange
            var provider = new Provider();
            var specificTime = new DateTime(2024, 6, 25, 14, 30, 45, 123);

            // Act
            provider.LastUpdate = specificTime;

            // Assert
            provider.LastUpdate.Should().Be(specificTime);
            provider.LastUpdate.Millisecond.Should().Be(123);
        }

        [Fact]
        public void Provider_WithLargeBetCollection_ShouldHandleCorrectly()
        {
            // Arrange
            var provider = new Provider();
            var largeBetCollection = Enumerable.Range(1, 1000)
                .Select(i => new Bet { Id = i, Name = $"Bet {i}" })
                .ToList();

            // Act
            provider.Bets = largeBetCollection;

            // Assert
            provider.Bets.Should().NotBeNull();
            provider.Bets.Should().HaveCount(1000);
            provider.Bets.First().Id.Should().Be(1);
            provider.Bets.Last().Id.Should().Be(1000);
        }

        [Theory]
        [InlineData("")]
        [InlineData("   ")]
        [InlineData("\t")]
        [InlineData("\n")]
        [InlineData("\r\n")]
        public void Provider_WithWhitespaceStrings_ShouldPreserveWhitespace(string whitespace)
        {
            // Arrange
            var provider = new Provider();

            // Act
            provider.Name = whitespace;
            provider.ProviderFixtureId = whitespace;
            provider.ProviderLeagueId = whitespace;
            provider.ProviderMarketId = whitespace;

            // Assert
            provider.Name.Should().Be(whitespace);
            provider.ProviderFixtureId.Should().Be(whitespace);
            provider.ProviderLeagueId.Should().Be(whitespace);
            provider.ProviderMarketId.Should().Be(whitespace);
        }

        [Fact]
        public void Provider_WithUnicodeStrings_ShouldHandleCorrectly()
        {
            // Arrange
            var provider = new Provider();
            var unicodeName = "Provider™ 中文 🎯 ñáéíóú";
            var unicodeFixture = "Fixture™ 中文 🎯";
            var unicodeLeague = "League™ 中文 🎯";
            var unicodeMarket = "Market™ 中文 🎯";

            // Act
            provider.Name = unicodeName;
            provider.ProviderFixtureId = unicodeFixture;
            provider.ProviderLeagueId = unicodeLeague;
            provider.ProviderMarketId = unicodeMarket;

            // Assert
            provider.Name.Should().Be(unicodeName);
            provider.ProviderFixtureId.Should().Be(unicodeFixture);
            provider.ProviderLeagueId.Should().Be(unicodeLeague);
            provider.ProviderMarketId.Should().Be(unicodeMarket);
        }

        #endregion

        #region Behavior Tests

        [Fact]
        public void Provider_MultipleAssignments_ShouldOverwritePreviousValues()
        {
            // Arrange
            var provider = new Provider();

            // Act & Assert - Multiple assignments to same property
            provider.Name = "First Name";
            provider.Name.Should().Be("First Name");

            provider.Name = "Second Name";
            provider.Name.Should().Be("Second Name");

            provider.Name = null;
            provider.Name.Should().BeNull();

            provider.Name = "Final Name";
            provider.Name.Should().Be("Final Name");
        }

        [Fact]
        public void Provider_BetsCollectionReassignment_ShouldReplaceCollection()
        {
            // Arrange
            var provider = new Provider();
            var firstCollection = new List<Bet> { new Bet { Id = 1 } };
            var secondCollection = new List<Bet> { new Bet { Id = 2 }, new Bet { Id = 3 } };

            // Act & Assert
            provider.Bets = firstCollection;
            provider.Bets.Should().HaveCount(1);
            provider.Bets.Should().BeSameAs(firstCollection);

            provider.Bets = secondCollection;
            provider.Bets.Should().HaveCount(2);
            provider.Bets.Should().BeSameAs(secondCollection);
            provider.Bets.Should().NotBeSameAs(firstCollection);
        }

        [Fact]
        public void Provider_PropertyIndependence_ShouldNotAffectOtherProperties()
        {
            // Arrange
            var provider = new Provider
            {
                Id = 100,
                Name = "Test Provider",
                LastUpdate = DateTime.Now,
                ProviderFixtureId = "FIX123",
                ProviderLeagueId = "LEA456",
                ProviderMarketId = "MKT789",
                Bets = new List<Bet> { new Bet { Id = 1 } }
            };

            var originalValues = new
            {
                Id = provider.Id,
                Name = provider.Name,
                LastUpdate = provider.LastUpdate,
                ProviderFixtureId = provider.ProviderFixtureId,
                ProviderLeagueId = provider.ProviderLeagueId,
                ProviderMarketId = provider.ProviderMarketId,
                BetsCount = provider.Bets?.Count()
            };

            // Act - Change one property
            provider.Name = "Changed Name";

            // Assert - Other properties should remain unchanged
            provider.Id.Should().Be(originalValues.Id);
            provider.LastUpdate.Should().Be(originalValues.LastUpdate);
            provider.ProviderFixtureId.Should().Be(originalValues.ProviderFixtureId);
            provider.ProviderLeagueId.Should().Be(originalValues.ProviderLeagueId);
            provider.ProviderMarketId.Should().Be(originalValues.ProviderMarketId);
            provider.Bets?.Count().Should().Be(originalValues.BetsCount);
            
            // Only the changed property should be different
            provider.Name.Should().Be("Changed Name");
            provider.Name.Should().NotBe(originalValues.Name);
        }

        #endregion
    }
} 