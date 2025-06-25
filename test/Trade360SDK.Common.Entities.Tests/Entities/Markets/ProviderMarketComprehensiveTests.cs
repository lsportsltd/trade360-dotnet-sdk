using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Trade360SDK.Common.Entities.Markets;
using Xunit;

namespace Trade360SDK.Common.Entities.Tests.Entities.Markets
{
    public class ProviderMarketComprehensiveTests
    {
        #region Constructor Tests

        [Fact]
        public void ProviderMarket_DefaultConstructor_ShouldInitializeCorrectly()
        {
            // Act
            var providerMarket = new ProviderMarket();

            // Assert
            providerMarket.Should().NotBeNull();
            providerMarket.Id.Should().Be(0);
            providerMarket.Name.Should().BeNull();
            providerMarket.Bets.Should().BeNull();
            providerMarket.LastUpdate.Should().Be(default(DateTime));
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
        public void ProviderMarket_IdProperty_ShouldSetAndGetCorrectly(int id)
        {
            // Arrange
            var providerMarket = new ProviderMarket();

            // Act
            providerMarket.Id = id;

            // Assert
            providerMarket.Id.Should().Be(id);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("Match Winner")]
        [InlineData("Over/Under 2.5")]
        [InlineData("Asian Handicap")]
        [InlineData("Very Long Market Name That Might Be Used In Real World Scenarios")]
        [InlineData("Market123")]
        [InlineData("Market-With-Dashes")]
        [InlineData("Market_With_Underscores")]
        [InlineData("Market.With.Dots")]
        [InlineData("Market With Spaces")]
        public void ProviderMarket_NameProperty_ShouldSetAndGetCorrectly(string? name)
        {
            // Arrange
            var providerMarket = new ProviderMarket();

            // Act
            providerMarket.Name = name;

            // Assert
            providerMarket.Name.Should().Be(name);
        }

        [Fact]
        public void ProviderMarket_LastUpdateProperty_ShouldSetAndGetCorrectly()
        {
            // Arrange
            var providerMarket = new ProviderMarket();
            var testDate = new DateTime(2024, 6, 25, 14, 30, 45);

            // Act
            providerMarket.LastUpdate = testDate;

            // Assert
            providerMarket.LastUpdate.Should().Be(testDate);
        }

        [Fact]
        public void ProviderMarket_LastUpdateProperty_WithMinValue_ShouldSetCorrectly()
        {
            // Arrange
            var providerMarket = new ProviderMarket();

            // Act
            providerMarket.LastUpdate = DateTime.MinValue;

            // Assert
            providerMarket.LastUpdate.Should().Be(DateTime.MinValue);
        }

        [Fact]
        public void ProviderMarket_LastUpdateProperty_WithMaxValue_ShouldSetCorrectly()
        {
            // Arrange
            var providerMarket = new ProviderMarket();

            // Act
            providerMarket.LastUpdate = DateTime.MaxValue;

            // Assert
            providerMarket.LastUpdate.Should().Be(DateTime.MaxValue);
        }

        [Fact]
        public void ProviderMarket_BetsProperty_WithNull_ShouldSetAndGetCorrectly()
        {
            // Arrange
            var providerMarket = new ProviderMarket();

            // Act
            providerMarket.Bets = null;

            // Assert
            providerMarket.Bets.Should().BeNull();
        }

        [Fact]
        public void ProviderMarket_BetsProperty_WithEmptyCollection_ShouldSetAndGetCorrectly()
        {
            // Arrange
            var providerMarket = new ProviderMarket();
            var emptyBets = new List<ProviderBet>();

            // Act
            providerMarket.Bets = emptyBets;

            // Assert
            providerMarket.Bets.Should().NotBeNull();
            providerMarket.Bets.Should().BeEmpty();
            providerMarket.Bets.Should().BeSameAs(emptyBets);
        }

        [Fact]
        public void ProviderMarket_BetsProperty_WithSingleBet_ShouldSetAndGetCorrectly()
        {
            // Arrange
            var providerMarket = new ProviderMarket();
            var bet = new ProviderBet { Id = 1, Name = "Home Win" };
            var bets = new List<ProviderBet> { bet };

            // Act
            providerMarket.Bets = bets;

            // Assert
            providerMarket.Bets.Should().NotBeNull();
            providerMarket.Bets.Should().HaveCount(1);
            providerMarket.Bets.Should().Contain(bet);
            providerMarket.Bets.First().Id.Should().Be(1);
            providerMarket.Bets.First().Name.Should().Be("Home Win");
        }

        [Fact]
        public void ProviderMarket_BetsProperty_WithMultipleBets_ShouldSetAndGetCorrectly()
        {
            // Arrange
            var providerMarket = new ProviderMarket();
            var bet1 = new ProviderBet { Id = 1, Name = "Home Win" };
            var bet2 = new ProviderBet { Id = 2, Name = "Draw" };
            var bet3 = new ProviderBet { Id = 3, Name = "Away Win" };
            var bets = new List<ProviderBet> { bet1, bet2, bet3 };

            // Act
            providerMarket.Bets = bets;

            // Assert
            providerMarket.Bets.Should().NotBeNull();
            providerMarket.Bets.Should().HaveCount(3);
            providerMarket.Bets.Should().Contain(bet1);
            providerMarket.Bets.Should().Contain(bet2);
            providerMarket.Bets.Should().Contain(bet3);
        }

        #endregion

        #region Integration Tests

        [Fact]
        public void ProviderMarket_SetAllProperties_ShouldSetAllValues()
        {
            // Arrange
            var providerMarket = new ProviderMarket();
            var id = 999;
            var name = "Test Market";
            var lastUpdate = new DateTime(2024, 6, 25, 14, 30, 45);
            var bets = new List<ProviderBet>
            {
                new ProviderBet { Id = 1, Name = "Bet 1" },
                new ProviderBet { Id = 2, Name = "Bet 2" }
            };

            // Act
            providerMarket.Id = id;
            providerMarket.Name = name;
            providerMarket.LastUpdate = lastUpdate;
            providerMarket.Bets = bets;

            // Assert
            providerMarket.Id.Should().Be(id);
            providerMarket.Name.Should().Be(name);
            providerMarket.LastUpdate.Should().Be(lastUpdate);
            providerMarket.Bets.Should().BeEquivalentTo(bets);
        }

        [Fact]
        public void ProviderMarket_SetAllPropertiesToNull_ShouldSetNullValues()
        {
            // Arrange
            var providerMarket = new ProviderMarket
            {
                Id = 123,
                Name = "Initial Name",
                LastUpdate = DateTime.Now,
                Bets = new List<ProviderBet> { new ProviderBet { Id = 1 } }
            };

            // Act
            providerMarket.Name = null;
            providerMarket.Bets = null;

            // Assert
            providerMarket.Id.Should().Be(123); // Value type, can't be null
            providerMarket.Name.Should().BeNull();
            providerMarket.Bets.Should().BeNull();
        }

        #endregion

        #region Edge Case Tests

        [Fact]
        public void ProviderMarket_WithUtcDateTime_ShouldHandleCorrectly()
        {
            // Arrange
            var providerMarket = new ProviderMarket();
            var utcTime = DateTime.UtcNow;

            // Act
            providerMarket.LastUpdate = utcTime;

            // Assert
            providerMarket.LastUpdate.Should().Be(utcTime);
        }

        [Fact]
        public void ProviderMarket_WithLocalDateTime_ShouldHandleCorrectly()
        {
            // Arrange
            var providerMarket = new ProviderMarket();
            var localTime = DateTime.Now;

            // Act
            providerMarket.LastUpdate = localTime;

            // Assert
            providerMarket.LastUpdate.Should().Be(localTime);
        }

        [Fact]
        public void ProviderMarket_WithSpecificDateTime_ShouldPreservePrecision()
        {
            // Arrange
            var providerMarket = new ProviderMarket();
            var specificTime = new DateTime(2024, 6, 25, 14, 30, 45, 123);

            // Act
            providerMarket.LastUpdate = specificTime;

            // Assert
            providerMarket.LastUpdate.Should().Be(specificTime);
            providerMarket.LastUpdate.Millisecond.Should().Be(123);
        }

        [Fact]
        public void ProviderMarket_WithLargeBetCollection_ShouldHandleCorrectly()
        {
            // Arrange
            var providerMarket = new ProviderMarket();
            var largeBetCollection = Enumerable.Range(1, 1000)
                .Select(i => new ProviderBet { Id = i, Name = $"Bet {i}" })
                .ToList();

            // Act
            providerMarket.Bets = largeBetCollection;

            // Assert
            providerMarket.Bets.Should().NotBeNull();
            providerMarket.Bets.Should().HaveCount(1000);
            providerMarket.Bets.First().Id.Should().Be(1);
            providerMarket.Bets.Last().Id.Should().Be(1000);
        }

        [Theory]
        [InlineData("")]
        [InlineData("   ")]
        [InlineData("\t")]
        [InlineData("\n")]
        [InlineData("\r\n")]
        public void ProviderMarket_WithWhitespaceStrings_ShouldPreserveWhitespace(string whitespace)
        {
            // Arrange
            var providerMarket = new ProviderMarket();

            // Act
            providerMarket.Name = whitespace;

            // Assert
            providerMarket.Name.Should().Be(whitespace);
        }

        [Fact]
        public void ProviderMarket_WithUnicodeStrings_ShouldHandleCorrectly()
        {
            // Arrange
            var providerMarket = new ProviderMarket();
            var unicodeName = "Market™ 中文 🎯 ñáéíóú";

            // Act
            providerMarket.Name = unicodeName;

            // Assert
            providerMarket.Name.Should().Be(unicodeName);
        }

        #endregion

        #region Behavior Tests

        [Fact]
        public void ProviderMarket_MultipleAssignments_ShouldOverwritePreviousValues()
        {
            // Arrange
            var providerMarket = new ProviderMarket();

            // Act & Assert - Multiple assignments to same property
            providerMarket.Name = "First Name";
            providerMarket.Name.Should().Be("First Name");

            providerMarket.Name = "Second Name";
            providerMarket.Name.Should().Be("Second Name");

            providerMarket.Name = null;
            providerMarket.Name.Should().BeNull();

            providerMarket.Name = "Final Name";
            providerMarket.Name.Should().Be("Final Name");
        }

        [Fact]
        public void ProviderMarket_BetsCollectionReassignment_ShouldReplaceCollection()
        {
            // Arrange
            var providerMarket = new ProviderMarket();
            var firstCollection = new List<ProviderBet> { new ProviderBet { Id = 1 } };
            var secondCollection = new List<ProviderBet> { new ProviderBet { Id = 2 }, new ProviderBet { Id = 3 } };

            // Act & Assert
            providerMarket.Bets = firstCollection;
            providerMarket.Bets.Should().HaveCount(1);
            providerMarket.Bets.Should().BeSameAs(firstCollection);

            providerMarket.Bets = secondCollection;
            providerMarket.Bets.Should().HaveCount(2);
            providerMarket.Bets.Should().BeSameAs(secondCollection);
            providerMarket.Bets.Should().NotBeSameAs(firstCollection);
        }

        [Fact]
        public void ProviderMarket_PropertyIndependence_ShouldNotAffectOtherProperties()
        {
            // Arrange
            var providerMarket = new ProviderMarket
            {
                Id = 100,
                Name = "Test Market",
                LastUpdate = DateTime.Now,
                Bets = new List<ProviderBet> { new ProviderBet { Id = 1 } }
            };

            var originalValues = new
            {
                Id = providerMarket.Id,
                Name = providerMarket.Name,
                LastUpdate = providerMarket.LastUpdate,
                BetsCount = providerMarket.Bets?.Count()
            };

            // Act - Change one property
            providerMarket.Name = "Changed Name";

            // Assert - Other properties should remain unchanged
            providerMarket.Id.Should().Be(originalValues.Id);
            providerMarket.LastUpdate.Should().Be(originalValues.LastUpdate);
            providerMarket.Bets?.Count().Should().Be(originalValues.BetsCount);
            
            // Only the changed property should be different
            providerMarket.Name.Should().Be("Changed Name");
            providerMarket.Name.Should().NotBe(originalValues.Name);
        }

        #endregion

        #region Type Tests

        [Fact]
        public void ProviderMarket_ShouldBePublicClass()
        {
            // Arrange
            var type = typeof(ProviderMarket);

            // Assert
            type.IsPublic.Should().BeTrue();
            type.IsClass.Should().BeTrue();
            type.IsAbstract.Should().BeFalse();
        }

        [Fact]
        public void ProviderMarket_ShouldHaveParameterlessConstructor()
        {
            // Arrange
            var type = typeof(ProviderMarket);

            // Act
            var constructor = type.GetConstructor(Type.EmptyTypes);

            // Assert
            constructor.Should().NotBeNull();
            constructor!.IsPublic.Should().BeTrue();
        }

        [Fact]
        public void ProviderMarket_ShouldBeInCorrectNamespace()
        {
            // Arrange
            var type = typeof(ProviderMarket);

            // Assert
            type.Namespace.Should().Be("Trade360SDK.Common.Entities.Markets");
        }

        #endregion
    }
} 