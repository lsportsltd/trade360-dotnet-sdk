using FluentAssertions;
using Trade360SDK.Common.Entities.Markets;

namespace Trade360SDK.Common.Entities.Tests.Entities.Markets;

public class BaseBetOrderTests
{
    [Fact]
    public void Order_ShouldBeNullableInt()
    {
        var baseBet = new BaseBet();
        
        baseBet.Order.Should().BeNull();
    }

    [Fact]
    public void Order_ShouldAcceptIntValues()
    {
        var baseBet = new BaseBet { Order = 1 };
        
        baseBet.Order.Should().Be(1);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(5)]
    [InlineData(10)]
    [InlineData(100)]
    public void Order_ShouldAcceptPositiveValues(int order)
    {
        var baseBet = new BaseBet { Order = order };
        
        baseBet.Order.Should().Be(order);
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(-10)]
    [InlineData(-100)]
    public void Order_ShouldAcceptNegativeValues(int order)
    {
        var baseBet = new BaseBet { Order = order };
        
        baseBet.Order.Should().Be(order);
    }

    [Fact]
    public void Order_ShouldAllowNull()
    {
        var baseBet = new BaseBet { Order = null };
        
        baseBet.Order.Should().BeNull();
    }

    [Fact]
    public void Order_DefaultValue_ShouldBeNull()
    {
        var baseBet = new BaseBet();
        
        baseBet.Order.Should().BeNull();
    }

    [Fact]
    public void Order_ShouldBeSettableAndGettable()
    {
        var baseBet = new BaseBet();
        
        baseBet.Order = 5;
        
        baseBet.Order.Should().Be(5);
    }

    [Fact]
    public void Order_ShouldAllowReassignment()
    {
        var baseBet = new BaseBet { Order = 1 };
        
        baseBet.Order = 2;
        
        baseBet.Order.Should().Be(2);
    }

    [Fact]
    public void Order_ShouldAllowReassignmentToNull()
    {
        var baseBet = new BaseBet { Order = 1 };
        
        baseBet.Order = null;
        
        baseBet.Order.Should().BeNull();
    }

    [Fact]
    public void Order_HasValue_ShouldBeTrueWhenSet()
    {
        var baseBet = new BaseBet { Order = 1 };
        
        baseBet.Order.HasValue.Should().BeTrue();
        baseBet.Order.Value.Should().Be(1);
    }

    [Fact]
    public void Order_HasValue_ShouldBeFalseWhenNull()
    {
        var baseBet = new BaseBet { Order = null };
        
        baseBet.Order.HasValue.Should().BeFalse();
    }

    [Fact]
    public void Order_ShouldWorkWithObjectInitializer()
    {
        var baseBet = new BaseBet
        {
            Id = 123,
            Order = 1,
            Name = "Test"
        };
        
        baseBet.Order.Should().Be(1);
        baseBet.Id.Should().Be(123);
    }

    [Fact]
    public void Order_ShouldAllowZeroValue()
    {
        var baseBet = new BaseBet { Order = 0 };
        
        baseBet.Order.Should().Be(0);
        baseBet.Order.Should().NotBeNull();
    }

    [Fact]
    public void Order_ShouldSupportComparison()
    {
        var bet1 = new BaseBet { Order = 1 };
        var bet2 = new BaseBet { Order = 2 };
        var bet3 = new BaseBet { Order = 1 };
        
        bet1.Order.Value.Should().BeLessThan(bet2.Order.Value);
        bet1.Order.Should().Be(bet3.Order);
    }

    [Fact]
    public void Order_ShouldWorkForSortingBets()
    {
        var bets = new[]
        {
            new BaseBet { Id = 1, Order = 3 },
            new BaseBet { Id = 2, Order = 1 },
            new BaseBet { Id = 3, Order = 2 },
            new BaseBet { Id = 4, Order = null }
        };

        var sorted = bets.Where(b => b.Order.HasValue)
                        .OrderBy(b => b.Order)
                        .ToList();

        sorted[0].Order.Should().Be(1);
        sorted[1].Order.Should().Be(2);
        sorted[2].Order.Should().Be(3);
    }

    [Fact]
    public void Order_IndependentFromOtherProperties()
    {
        var baseBet = new BaseBet
        {
            Id = 999,
            Order = 1,
            PlayerId = "123",
            Name = "Test"
        };
        
        baseBet.Order.Should().Be(1);
        baseBet.Id.Should().Be(999);
        
        // Changing Order shouldn't affect other properties
        baseBet.Order = 2;
        baseBet.Id.Should().Be(999);
        baseBet.PlayerId.Should().Be("123");
    }

    [Fact]
    public void Order_ShouldAcceptMaxIntValue()
    {
        var baseBet = new BaseBet { Order = int.MaxValue };
        
        baseBet.Order.Should().Be(int.MaxValue);
    }

    [Fact]
    public void Order_ShouldAcceptMinIntValue()
    {
        var baseBet = new BaseBet { Order = int.MinValue };
        
        baseBet.Order.Should().Be(int.MinValue);
    }

    [Fact]
    public void Order_WhenNull_ShouldNotEqualZero()
    {
        var bet1 = new BaseBet { Order = null };
        var bet2 = new BaseBet { Order = 0 };
        
        bet1.Order.Should().NotBe(bet2.Order);
        bet1.Order.Should().BeNull();
        bet2.Order.Should().Be(0);
    }

    [Fact]
    public void Order_ShouldWorkWithMultipleBets()
    {
        var bet1 = new BaseBet { Order = 1, Name = "Bet1" };
        var bet2 = new BaseBet { Order = 2, Name = "Bet2" };
        var bet3 = new BaseBet { Order = null, Name = "Bet3" };
        
        bet1.Order.Should().Be(1);
        bet2.Order.Should().Be(2);
        bet3.Order.Should().BeNull();
    }

    [Fact]
    public void Order_ShouldWorkInConditionals()
    {
        var baseBet = new BaseBet { Order = 1 };
        
        if (baseBet.Order.HasValue && baseBet.Order.Value > 0)
        {
            baseBet.Order.Should().Be(1);
        }
        else
        {
            Assert.Fail("Conditional should have been true");
        }
    }

    [Fact]
    public void Order_WhenUsedWithPlayerId_ShouldWorkTogether()
    {
        var baseBet = new BaseBet
        {
            Order = 1,
            PlayerId = "299919",
            Name = "Player Bet"
        };
        
        baseBet.Order.Should().Be(1);
        baseBet.PlayerId.Should().Be("299919");
        baseBet.Name.Should().Be("Player Bet");
    }
}

