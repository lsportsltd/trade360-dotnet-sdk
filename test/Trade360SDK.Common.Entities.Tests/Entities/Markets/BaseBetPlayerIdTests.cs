using FluentAssertions;
using Trade360SDK.Common.Entities.Markets;

namespace Trade360SDK.Common.Entities.Tests.Entities.Markets;

public class BaseBetPlayerIdTests
{
    [Fact]
    public void PlayerId_ShouldBeStringType()
    {
        var baseBet = new BaseBet();
        
        baseBet.PlayerId.Should().BeNull();
        baseBet.PlayerId = "123";
        baseBet.PlayerId.Should().BeOfType<string>();
    }

    [Theory]
    [InlineData("299919")]
    [InlineData("123")]
    [InlineData("1")]
    [InlineData("9999999999")]
    public void PlayerId_ShouldAcceptNumericStrings(string playerId)
    {
        var baseBet = new BaseBet { PlayerId = playerId };
        
        baseBet.PlayerId.Should().Be(playerId);
        baseBet.PlayerId.Should().BeOfType<string>();
    }

    [Theory]
    [InlineData("ABC123")]
    [InlineData("player-123")]
    [InlineData("P_456")]
    [InlineData("PLAYER_ID_789")]
    public void PlayerId_ShouldAcceptAlphanumericStrings(string playerId)
    {
        var baseBet = new BaseBet { PlayerId = playerId };
        
        baseBet.PlayerId.Should().Be(playerId);
    }

    [Fact]
    public void PlayerId_ShouldAcceptNull()
    {
        var baseBet = new BaseBet { PlayerId = null };
        
        baseBet.PlayerId.Should().BeNull();
    }

    [Fact]
    public void PlayerId_ShouldAcceptEmptyString()
    {
        var baseBet = new BaseBet { PlayerId = "" };
        
        baseBet.PlayerId.Should().BeEmpty();
    }

    [Theory]
    [InlineData("   ")]
    [InlineData("\t")]
    [InlineData("\n")]
    public void PlayerId_ShouldAcceptWhitespaceStrings(string playerId)
    {
        var baseBet = new BaseBet { PlayerId = playerId };
        
        baseBet.PlayerId.Should().Be(playerId);
    }

    [Fact]
    public void PlayerId_ShouldBeSettableAndGettable()
    {
        var baseBet = new BaseBet();
        var expectedPlayerId = "299919";
        
        baseBet.PlayerId = expectedPlayerId;
        
        baseBet.PlayerId.Should().Be(expectedPlayerId);
    }

    [Fact]
    public void PlayerId_ShouldAllowReassignment()
    {
        var baseBet = new BaseBet { PlayerId = "123" };
        
        baseBet.PlayerId = "456";
        
        baseBet.PlayerId.Should().Be("456");
    }

    [Fact]
    public void PlayerId_ShouldAllowReassignmentToNull()
    {
        var baseBet = new BaseBet { PlayerId = "123" };
        
        baseBet.PlayerId = null;
        
        baseBet.PlayerId.Should().BeNull();
    }

    [Fact]
    public void PlayerId_WhenSetToNumericString_ShouldNotBeConvertibleToInt()
    {
        var baseBet = new BaseBet { PlayerId = "12345" };
        
        // This should not compile if attempted - PlayerId is string, not int
        baseBet.PlayerId.Should().BeOfType<string>();
        baseBet.PlayerId.Should().NotBeNull();
        
        // But we can parse it if needed
        int.TryParse(baseBet.PlayerId, out var parsed).Should().BeTrue();
        parsed.Should().Be(12345);
    }

    [Fact]
    public void PlayerId_WhenSetToAlphanumeric_ShouldNotBeConvertibleToInt()
    {
        var baseBet = new BaseBet { PlayerId = "ABC123" };
        
        baseBet.PlayerId.Should().BeOfType<string>();
        int.TryParse(baseBet.PlayerId, out _).Should().BeFalse();
    }

    [Fact]
    public void PlayerId_ShouldWorkWithObjectInitializer()
    {
        var baseBet = new BaseBet
        {
            Id = 123456789,
            PlayerId = "299919",
            Name = "Test Player"
        };
        
        baseBet.PlayerId.Should().Be("299919");
        baseBet.Id.Should().Be(123456789);
        baseBet.Name.Should().Be("Test Player");
    }

    [Fact]
    public void PlayerId_DefaultValue_ShouldBeNull()
    {
        var baseBet = new BaseBet();
        
        baseBet.PlayerId.Should().BeNull();
    }

    [Theory]
    [InlineData("0")]
    [InlineData("00000")]
    [InlineData("-1")]
    [InlineData("-999")]
    public void PlayerId_ShouldAcceptZeroAndNegativeNumericStrings(string playerId)
    {
        var baseBet = new BaseBet { PlayerId = playerId };
        
        baseBet.PlayerId.Should().Be(playerId);
    }

    [Fact]
    public void PlayerId_ShouldPreserveLeadingZeros()
    {
        var baseBet = new BaseBet { PlayerId = "00123" };
        
        baseBet.PlayerId.Should().Be("00123");
        baseBet.PlayerId.Should().NotBe("123");
    }

    [Fact]
    public void PlayerId_ShouldHandleVeryLongStrings()
    {
        var longPlayerId = new string('1', 1000);
        var baseBet = new BaseBet { PlayerId = longPlayerId };
        
        baseBet.PlayerId.Should().Be(longPlayerId);
        baseBet.PlayerId.Should().HaveLength(1000);
    }

    [Theory]
    [InlineData("Player-123")]
    [InlineData("player_456")]
    [InlineData("PLAYER#789")]
    [InlineData("player@999")]
    public void PlayerId_ShouldAcceptSpecialCharacters(string playerId)
    {
        var baseBet = new BaseBet { PlayerId = playerId };
        
        baseBet.PlayerId.Should().Be(playerId);
    }

    [Fact]
    public void PlayerId_ShouldWorkWithMultipleInstances()
    {
        var bet1 = new BaseBet { PlayerId = "111" };
        var bet2 = new BaseBet { PlayerId = "222" };
        var bet3 = new BaseBet { PlayerId = "333" };
        
        bet1.PlayerId.Should().Be("111");
        bet2.PlayerId.Should().Be("222");
        bet3.PlayerId.Should().Be("333");
    }

    [Fact]
    public void PlayerId_IndependentFromOtherProperties()
    {
        var baseBet = new BaseBet
        {
            Id = 999,
            PlayerId = "123",
            Name = "Test",
            ParticipantId = 456
        };
        
        baseBet.PlayerId.Should().Be("123");
        baseBet.Id.Should().Be(999);
        baseBet.ParticipantId.Should().Be(456);
        
        // Changing PlayerId shouldn't affect other properties
        baseBet.PlayerId = "789";
        baseBet.Id.Should().Be(999);
        baseBet.ParticipantId.Should().Be(456);
    }
}

