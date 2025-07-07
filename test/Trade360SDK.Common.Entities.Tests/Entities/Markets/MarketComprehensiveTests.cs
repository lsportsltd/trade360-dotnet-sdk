using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Trade360SDK.Common.Entities.Markets;
using Xunit;

namespace Trade360SDK.Common.Entities.Tests.Entities.Markets;

public class MarketComprehensiveTests
{
    [Fact]
    public void Constructor_ShouldCreateInstanceSuccessfully()
    {
        // Act
        var market = new Market();

        // Assert
        market.Should().NotBeNull();
        market.Id.Should().Be(0); // Default int value
        market.Name.Should().BeNull();
        market.Bets.Should().BeNull();
        market.ProviderMarkets.Should().BeNull();
        market.MainLine.Should().BeNull();
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(100)]
    [InlineData(int.MaxValue)]
    [InlineData(int.MinValue)]
    [InlineData(-1)]
    public void Id_Property_ShouldAcceptIntValues(int expectedId)
    {
        // Arrange
        var market = new Market();

        // Act
        market.Id = expectedId;

        // Assert
        market.Id.Should().Be(expectedId);
    }

    [Theory]
    [InlineData("Match Winner")]
    [InlineData("Over/Under 2.5 Goals")]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("Very Long Market Name With Special Characters @#$%")]
    [InlineData("Asian Handicap +1.5")]
    public void Name_Property_ShouldAcceptNullableStringValues(string? expectedName)
    {
        // Arrange
        var market = new Market();

        // Act
        market.Name = expectedName;

        // Assert
        market.Name.Should().Be(expectedName);
    }

    [Fact]
    public void Bets_Property_ShouldAcceptNullValue()
    {
        // Arrange
        var market = new Market();

        // Act
        market.Bets = null;

        // Assert
        market.Bets.Should().BeNull();
    }

    [Fact]
    public void Bets_Property_ShouldAcceptEmptyCollection()
    {
        // Arrange
        var market = new Market();
        var emptyBets = new List<Bet>();

        // Act
        market.Bets = emptyBets;

        // Assert
        market.Bets.Should().NotBeNull();
        market.Bets.Should().BeEmpty();
        market.Bets.Should().BeSameAs(emptyBets);
    }

    [Fact]
    public void Bets_Property_ShouldAcceptCollectionWithSingleBet()
    {
        // Arrange
        var market = new Market();
        var bet = new Bet { Id = 123, Name = "Home Win" };
        var bets = new List<Bet> { bet };

        // Act
        market.Bets = bets;

        // Assert
        market.Bets.Should().NotBeNull();
        market.Bets.Should().HaveCount(1);
        market.Bets!.First().Should().BeSameAs(bet);
        market.Bets.First().Id.Should().Be(123);
        market.Bets.First().Name.Should().Be("Home Win");
    }

    [Fact]
    public void Bets_Property_ShouldAcceptCollectionWithMultipleBets()
    {
        // Arrange
        var market = new Market();
        var bet1 = new Bet { Id = 1, Name = "Home" };
        var bet2 = new Bet { Id = 2, Name = "Draw" };
        var bet3 = new Bet { Id = 3, Name = "Away" };
        var bets = new List<Bet> { bet1, bet2, bet3 };

        // Act
        market.Bets = bets;

        // Assert
        market.Bets.Should().NotBeNull();
        market.Bets.Should().HaveCount(3);
        market.Bets!.Should().Contain(bet1);
        market.Bets.Should().Contain(bet2);
        market.Bets.Should().Contain(bet3);
    }

    [Fact]
    public void ProviderMarkets_Property_ShouldAcceptNullValue()
    {
        // Arrange
        var market = new Market();

        // Act
        market.ProviderMarkets = null;

        // Assert
        market.ProviderMarkets.Should().BeNull();
    }

    [Fact]
    public void ProviderMarkets_Property_ShouldAcceptEmptyCollection()
    {
        // Arrange
        var market = new Market();
        var emptyProviderMarkets = new List<ProviderMarket>();

        // Act
        market.ProviderMarkets = emptyProviderMarkets;

        // Assert
        market.ProviderMarkets.Should().NotBeNull();
        market.ProviderMarkets.Should().BeEmpty();
        market.ProviderMarkets.Should().BeSameAs(emptyProviderMarkets);
    }

    [Fact]
    public void ProviderMarkets_Property_ShouldAcceptCollectionWithSingleProviderMarket()
    {
        // Arrange
        var market = new Market();
        var providerMarket = new ProviderMarket { Id = 456, Name = "Provider Match Winner" };
        var providerMarkets = new List<ProviderMarket> { providerMarket };

        // Act
        market.ProviderMarkets = providerMarkets;

        // Assert
        market.ProviderMarkets.Should().NotBeNull();
        market.ProviderMarkets.Should().HaveCount(1);
        market.ProviderMarkets!.First().Should().BeSameAs(providerMarket);
        market.ProviderMarkets.First().Id.Should().Be(456);
        market.ProviderMarkets.First().Name.Should().Be("Provider Match Winner");
    }

    [Fact]
    public void ProviderMarkets_Property_ShouldAcceptCollectionWithMultipleProviderMarkets()
    {
        // Arrange
        var market = new Market();
        var providerMarket1 = new ProviderMarket { Id = 10, Name = "Provider 1" };
        var providerMarket2 = new ProviderMarket { Id = 20, Name = "Provider 2" };
        var providerMarkets = new List<ProviderMarket> { providerMarket1, providerMarket2 };

        // Act
        market.ProviderMarkets = providerMarkets;

        // Assert
        market.ProviderMarkets.Should().NotBeNull();
        market.ProviderMarkets.Should().HaveCount(2);
        market.ProviderMarkets!.Should().Contain(providerMarket1);
        market.ProviderMarkets.Should().Contain(providerMarket2);
    }

    [Theory]
    [InlineData("0.0")]
    [InlineData("1.5")]
    [InlineData("-1.5")]
    [InlineData("+2.5")]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("Asian Handicap Main Line")]
    public void MainLine_Property_ShouldAcceptNullableStringValues(string? expectedMainLine)
    {
        // Arrange
        var market = new Market();

        // Act
        market.MainLine = expectedMainLine;

        // Assert
        market.MainLine.Should().Be(expectedMainLine);
    }

    [Fact]
    public void AllProperties_ShouldWorkIndependently()
    {
        // Arrange
        var market = new Market();
        var bet = new Bet { Id = 999, Name = "Test Bet" };
        var bets = new List<Bet> { bet };
        var providerMarket = new ProviderMarket { Id = 888, Name = "Test Provider Market" };
        var providerMarkets = new List<ProviderMarket> { providerMarket };

        // Act
        market.Id = 12345;
        market.Name = "Test Market";
        market.Bets = bets;
        market.ProviderMarkets = providerMarkets;
        market.MainLine = "2.5";

        // Assert
        market.Id.Should().Be(12345);
        market.Name.Should().Be("Test Market");
        market.Bets.Should().BeSameAs(bets);
        market.Bets!.Should().HaveCount(1);
        market.Bets.First().Id.Should().Be(999);
        market.ProviderMarkets.Should().BeSameAs(providerMarkets);
        market.ProviderMarkets!.Should().HaveCount(1);
        market.ProviderMarkets.First().Id.Should().Be(888);
        market.MainLine.Should().Be("2.5");
    }

    [Fact]
    public void Object_ShouldBeInstantiable()
    {
        // Act & Assert
        var market = new Market();
        market.Should().BeOfType<Market>();
        market.Should().NotBeNull();
    }

    [Fact]
    public void Properties_ShouldBeGettableAndSettable()
    {
        // Arrange
        var market = new Market();

        // Act & Assert - Test that all properties can be read and written
        var id = market.Id;
        market.Id = 999;
        market.Id.Should().Be(999);

        var name = market.Name;
        market.Name = "New Market Name";
        market.Name.Should().Be("New Market Name");

        var bets = market.Bets;
        var newBets = new List<Bet> { new Bet { Id = 1 } };
        market.Bets = newBets;
        market.Bets.Should().BeSameAs(newBets);

        var providerMarkets = market.ProviderMarkets;
        var newProviderMarkets = new List<ProviderMarket> { new ProviderMarket { Id = 1 } };
        market.ProviderMarkets = newProviderMarkets;
        market.ProviderMarkets.Should().BeSameAs(newProviderMarkets);

        var mainLine = market.MainLine;
        market.MainLine = "1.0";
        market.MainLine.Should().Be("1.0");
    }

    [Fact]
    public void Bets_ShouldSupportDifferentCollectionTypes()
    {
        // Arrange
        var market = new Market();
        var bet1 = new Bet { Id = 1, Name = "Bet 1" };
        var bet2 = new Bet { Id = 2, Name = "Bet 2" };

        // Test with different collection types
        var list = new List<Bet> { bet1, bet2 };
        var array = new Bet[] { bet1, bet2 };

        // Act & Assert - List
        market.Bets = list;
        market.Bets.Should().BeSameAs(list);
        market.Bets.Should().HaveCount(2);

        // Act & Assert - Array
        market.Bets = array;
        market.Bets.Should().BeSameAs(array);
        market.Bets.Should().HaveCount(2);
    }

    [Fact]
    public void ProviderMarkets_ShouldSupportDifferentCollectionTypes()
    {
        // Arrange
        var market = new Market();
        var providerMarket1 = new ProviderMarket { Id = 1, Name = "Provider 1" };
        var providerMarket2 = new ProviderMarket { Id = 2, Name = "Provider 2" };

        // Test with different collection types
        var list = new List<ProviderMarket> { providerMarket1, providerMarket2 };
        var array = new ProviderMarket[] { providerMarket1, providerMarket2 };

        // Act & Assert - List
        market.ProviderMarkets = list;
        market.ProviderMarkets.Should().BeSameAs(list);
        market.ProviderMarkets.Should().HaveCount(2);

        // Act & Assert - Array
        market.ProviderMarkets = array;
        market.ProviderMarkets.Should().BeSameAs(array);
        market.ProviderMarkets.Should().HaveCount(2);
    }

    [Fact]
    public void Market_WithComplexData_ShouldHandleCorrectly()
    {
        // Arrange
        var market = new Market();
        var bets = new List<Bet>
        {
            new Bet { Id = 1, Name = "Home Win", Price = "2.50" },
            new Bet { Id = 2, Name = "Draw", Price = "3.20" },
            new Bet { Id = 3, Name = "Away Win", Price = "2.80" }
        };
        var providerMarkets = new List<ProviderMarket>
        {
            new ProviderMarket { Id = 100, Name = "Bet365 Market" },
            new ProviderMarket { Id = 200, Name = "William Hill Market" }
        };

        // Act
        market.Id = 54321;
        market.Name = "Match Winner";
        market.Bets = bets;
        market.ProviderMarkets = providerMarkets;
        market.MainLine = "0";

        // Assert
        market.Id.Should().Be(54321);
        market.Name.Should().Be("Match Winner");
        market.Bets.Should().HaveCount(3);
        market.Bets!.Should().Contain(b => b.Name == "Home Win");
        market.Bets.Should().Contain(b => b.Name == "Draw");
        market.Bets.Should().Contain(b => b.Name == "Away Win");
        market.ProviderMarkets.Should().HaveCount(2);
        market.ProviderMarkets!.Should().Contain(pm => pm.Name == "Bet365 Market");
        market.ProviderMarkets.Should().Contain(pm => pm.Name == "William Hill Market");
        market.MainLine.Should().Be("0");
    }

    [Fact]
    public void Market_ReassignProperties_ShouldUpdateCorrectly()
    {
        // Arrange
        var market = new Market
        {
            Id = 111,
            Name = "Old Market",
            Bets = new List<Bet> { new Bet { Id = 1 } },
            ProviderMarkets = new List<ProviderMarket> { new ProviderMarket { Id = 1 } },
            MainLine = "old line"
        };

        var newBets = new List<Bet> { new Bet { Id = 2, Name = "New Bet" } };
        var newProviderMarkets = new List<ProviderMarket> { new ProviderMarket { Id = 2, Name = "New Provider" } };

        // Act
        market.Id = 222;
        market.Name = "New Market";
        market.Bets = newBets;
        market.ProviderMarkets = newProviderMarkets;
        market.MainLine = "new line";

        // Assert
        market.Id.Should().Be(222);
        market.Name.Should().Be("New Market");
        market.Bets.Should().BeSameAs(newBets);
        market.Bets!.First().Name.Should().Be("New Bet");
        market.ProviderMarkets.Should().BeSameAs(newProviderMarkets);
        market.ProviderMarkets!.First().Name.Should().Be("New Provider");
        market.MainLine.Should().Be("new line");
    }

    [Fact]
    public void Market_NullAssignments_ShouldWork()
    {
        // Arrange
        var market = new Market
        {
            Id = 123,
            Name = "Test Market",
            Bets = new List<Bet> { new Bet() },
            ProviderMarkets = new List<ProviderMarket> { new ProviderMarket() },
            MainLine = "test line"
        };

        // Act
        market.Name = null;
        market.Bets = null;
        market.ProviderMarkets = null;
        market.MainLine = null;

        // Assert
        market.Id.Should().Be(123); // Id should remain unchanged
        market.Name.Should().BeNull();
        market.Bets.Should().BeNull();
        market.ProviderMarkets.Should().BeNull();
        market.MainLine.Should().BeNull();
    }
} 