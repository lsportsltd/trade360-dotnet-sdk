using FluentAssertions;
using Trade360SDK.Common.Entities.Markets;
using System.Collections.Generic;
using System.Linq;

namespace Trade360SDK.Common.Entities.Tests.Entities.Markets;

/// <summary>
/// Comprehensive tests for Market class covering all properties,
/// collections, and edge cases.
/// </summary>
public class MarketComprehensiveTests
{
    [Fact]
    public void Market_DefaultConstructor_ShouldInitializeWithDefaultValues()
    {
        // Act
        var market = new Market();

        // Assert
        market.Id.Should().Be(0);
        market.Name.Should().BeNull();
        market.Bets.Should().BeNull();
        market.ProviderMarkets.Should().BeNull();
        market.MainLine.Should().BeNull();
    }

    [Fact]
    public void Market_Id_ShouldSetAndGetCorrectly()
    {
        // Arrange
        var market = new Market();
        var expectedId = 12345;

        // Act
        market.Id = expectedId;

        // Assert
        market.Id.Should().Be(expectedId);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(999999)]
    [InlineData(-1)]
    [InlineData(int.MaxValue)]
    [InlineData(int.MinValue)]
    public void Market_Id_ShouldHandleVariousIntegerValues(int id)
    {
        // Arrange
        var market = new Market();

        // Act
        market.Id = id;

        // Assert
        market.Id.Should().Be(id);
    }

    [Fact]
    public void Market_Name_ShouldSetAndGetCorrectly()
    {
        // Arrange
        var market = new Market();
        var expectedName = "Over/Under 2.5 Goals";

        // Act
        market.Name = expectedName;

        // Assert
        market.Name.Should().Be(expectedName);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("Simple Market")]
    [InlineData("Market with Special Characters: !@#$%^&*()")]
    [InlineData("Market with Unicode: 测试市场")]
    [InlineData("Very Long Market Name That Exceeds Normal Length Expectations And Contains Multiple Words And Phrases")]
    public void Market_Name_ShouldHandleVariousStringValues(string name)
    {
        // Arrange
        var market = new Market();

        // Act
        market.Name = name;

        // Assert
        market.Name.Should().Be(name);
    }

    [Fact]
    public void Market_Name_ShouldHandleNullValue()
    {
        // Arrange
        var market = new Market();

        // Act
        market.Name = null;

        // Assert
        market.Name.Should().BeNull();
    }

    [Fact]
    public void Market_Bets_ShouldSetAndGetCorrectly()
    {
        // Arrange
        var market = new Market();
        var bets = new List<Bet>
        {
            new Bet { Id = 1, Name = "Home Win" },
            new Bet { Id = 2, Name = "Draw" },
            new Bet { Id = 3, Name = "Away Win" }
        };

        // Act
        market.Bets = bets;

        // Assert
        market.Bets.Should().NotBeNull();
        market.Bets.Should().HaveCount(3);
        market.Bets.Should().BeEquivalentTo(bets);
    }

    [Fact]
    public void Market_Bets_ShouldHandleEmptyCollection()
    {
        // Arrange
        var market = new Market();
        var emptyBets = new List<Bet>();

        // Act
        market.Bets = emptyBets;

        // Assert
        market.Bets.Should().NotBeNull();
        market.Bets.Should().BeEmpty();
    }

    [Fact]
    public void Market_Bets_ShouldHandleNullValue()
    {
        // Arrange
        var market = new Market();

        // Act
        market.Bets = null;

        // Assert
        market.Bets.Should().BeNull();
    }

    [Fact]
    public void Market_Bets_ShouldHandleLargeCollection()
    {
        // Arrange
        var market = new Market();
        var largeBetCollection = Enumerable.Range(1, 1000)
            .Select(i => new Bet { Id = i, Name = $"Bet {i}" })
            .ToList();

        // Act
        market.Bets = largeBetCollection;

        // Assert
        market.Bets.Should().NotBeNull();
        market.Bets.Should().HaveCount(1000);
        market.Bets!.First().Id.Should().Be(1);
        market.Bets!.Last().Id.Should().Be(1000);
    }

    [Fact]
    public void Market_ProviderMarkets_ShouldSetAndGetCorrectly()
    {
        // Arrange
        var market = new Market();
        var providerMarkets = new List<ProviderMarket>
        {
            new ProviderMarket(),
            new ProviderMarket()
        };

        // Act
        market.ProviderMarkets = providerMarkets;

        // Assert
        market.ProviderMarkets.Should().NotBeNull();
        market.ProviderMarkets.Should().HaveCount(2);
        market.ProviderMarkets.Should().BeEquivalentTo(providerMarkets);
    }

    [Fact]
    public void Market_ProviderMarkets_ShouldHandleEmptyCollection()
    {
        // Arrange
        var market = new Market();
        var emptyProviderMarkets = new List<ProviderMarket>();

        // Act
        market.ProviderMarkets = emptyProviderMarkets;

        // Assert
        market.ProviderMarkets.Should().NotBeNull();
        market.ProviderMarkets.Should().BeEmpty();
    }

    [Fact]
    public void Market_ProviderMarkets_ShouldHandleNullValue()
    {
        // Arrange
        var market = new Market();

        // Act
        market.ProviderMarkets = null;

        // Assert
        market.ProviderMarkets.Should().BeNull();
    }

    [Fact]
    public void Market_MainLine_ShouldSetAndGetCorrectly()
    {
        // Arrange
        var market = new Market();
        var expectedMainLine = "2.5";

        // Act
        market.MainLine = expectedMainLine;

        // Assert
        market.MainLine.Should().Be(expectedMainLine);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("0")]
    [InlineData("2.5")]
    [InlineData("-1.5")]
    [InlineData("Over 2.5")]
    [InlineData("Asian Handicap +1")]
    public void Market_MainLine_ShouldHandleVariousStringValues(string mainLine)
    {
        // Arrange
        var market = new Market();

        // Act
        market.MainLine = mainLine;

        // Assert
        market.MainLine.Should().Be(mainLine);
    }

    [Fact]
    public void Market_MainLine_ShouldHandleNullValue()
    {
        // Arrange
        var market = new Market();

        // Act
        market.MainLine = null;

        // Assert
        market.MainLine.Should().BeNull();
    }

    [Fact]
    public void Market_AllProperties_ShouldSetAndGetCorrectly()
    {
        // Arrange
        var market = new Market();
        var expectedId = 100;
        var expectedName = "Total Goals";
        var expectedMainLine = "2.5";
        var expectedBets = new List<Bet>
        {
            new Bet { Id = 1, Name = "Over" },
            new Bet { Id = 2, Name = "Under" }
        };
        var expectedProviderMarkets = new List<ProviderMarket>
        {
            new ProviderMarket()
        };

        // Act
        market.Id = expectedId;
        market.Name = expectedName;
        market.MainLine = expectedMainLine;
        market.Bets = expectedBets;
        market.ProviderMarkets = expectedProviderMarkets;

        // Assert
        market.Id.Should().Be(expectedId);
        market.Name.Should().Be(expectedName);
        market.MainLine.Should().Be(expectedMainLine);
        market.Bets.Should().BeEquivalentTo(expectedBets);
        market.ProviderMarkets.Should().BeEquivalentTo(expectedProviderMarkets);
    }

    [Fact]
    public void Market_PropertyChanges_ShouldNotAffectOtherProperties()
    {
        // Arrange
        var market = new Market
        {
            Id = 1,
            Name = "Original Name",
            MainLine = "Original Line"
        };

        // Act
        market.Id = 999;

        // Assert
        market.Id.Should().Be(999);
        market.Name.Should().Be("Original Name");
        market.MainLine.Should().Be("Original Line");
    }

    [Fact]
    public void Market_Collections_ShouldAllowModificationAfterAssignment()
    {
        // Arrange
        var market = new Market();
        var betsList = new List<Bet>
        {
            new Bet { Id = 1, Name = "Initial Bet" }
        };
        market.Bets = betsList;

        // Act
        betsList.Add(new Bet { Id = 2, Name = "Added Bet" });

        // Assert
        market.Bets.Should().HaveCount(2);
        market.Bets!.Should().Contain(b => b.Name == "Initial Bet");
        market.Bets!.Should().Contain(b => b.Name == "Added Bet");
    }

    [Fact]
    public void Market_ObjectInstantiation_ShouldCreateIndependentInstances()
    {
        // Arrange & Act
        var market1 = new Market { Id = 1, Name = "Market 1" };
        var market2 = new Market { Id = 2, Name = "Market 2" };

        // Assert
        market1.Id.Should().NotBe(market2.Id);
        market1.Name.Should().NotBe(market2.Name);
    }

    [Fact]
    public void Market_WithComplexBetStructure_ShouldHandleCorrectly()
    {
        // Arrange
        var market = new Market();
        var complexBets = new List<Bet>
        {
            new Bet 
            { 
                Id = 1, 
                Name = "Complex Bet 1",
                Line = "1.5",
                Price = "2.50",
                Status = Trade360SDK.Common.Entities.Enums.BetStatus.Open
            },
            new Bet 
            { 
                Id = 2, 
                Name = "Complex Bet 2",
                Line = "-1.5",
                Price = "1.80",
                Status = Trade360SDK.Common.Entities.Enums.BetStatus.Suspended
            }
        };

        // Act
        market.Bets = complexBets;

        // Assert
        market.Bets.Should().HaveCount(2);
        market.Bets!.Should().Contain(b => b.Line == "1.5" && b.Price == "2.50");
        market.Bets!.Should().Contain(b => b.Line == "-1.5" && b.Price == "1.80");
    }

    [Fact]
    public void Market_ReferenceTypes_ShouldHandleNullAndNonNullCorrectly()
    {
        // Arrange
        var market = new Market();

        // Act & Assert - Initial state
        market.Name.Should().BeNull();
        market.Bets.Should().BeNull();
        market.ProviderMarkets.Should().BeNull();
        market.MainLine.Should().BeNull();

        // Act - Set non-null values
        market.Name = "Test Market";
        market.Bets = new List<Bet>();
        market.ProviderMarkets = new List<ProviderMarket>();
        market.MainLine = "Test Line";

        // Assert - Non-null state
        market.Name.Should().NotBeNull();
        market.Bets.Should().NotBeNull();
        market.ProviderMarkets.Should().NotBeNull();
        market.MainLine.Should().NotBeNull();

        // Act - Set back to null
        market.Name = null;
        market.Bets = null;
        market.ProviderMarkets = null;
        market.MainLine = null;

        // Assert - Back to null state
        market.Name.Should().BeNull();
        market.Bets.Should().BeNull();
        market.ProviderMarkets.Should().BeNull();
        market.MainLine.Should().BeNull();
    }
} 