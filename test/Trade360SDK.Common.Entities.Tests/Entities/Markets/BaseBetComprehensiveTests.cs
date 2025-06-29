using FluentAssertions;
using Trade360SDK.Common.Entities.Markets;
using Trade360SDK.Common.Entities.Enums;
using System;

namespace Trade360SDK.Common.Entities.Tests.Entities.Markets;

/// <summary>
/// Comprehensive tests for BaseBet class covering all properties,
/// enums, edge cases, and value validation.
/// </summary>
public class BaseBetComprehensiveTests
{
    [Fact]
    public void BaseBet_DefaultConstructor_ShouldInitializeWithDefaultValues()
    {
        // Act
        var baseBet = new BaseBet();

        // Assert
        baseBet.Id.Should().Be(0);
        baseBet.Name.Should().BeNull();
        baseBet.Line.Should().BeNull();
        baseBet.BaseLine.Should().BeNull();
        baseBet.Status.Should().Be(BetStatus.NotSet);
        baseBet.StartPrice.Should().BeNull();
        baseBet.Price.Should().BeNull();
        baseBet.PriceIN.Should().BeNull();
        baseBet.PriceUS.Should().BeNull();
        baseBet.PriceUK.Should().BeNull();
        baseBet.PriceMA.Should().BeNull();
        baseBet.PriceHK.Should().BeNull();
        baseBet.PriceVolume.Should().BeNull();
        baseBet.Settlement.Should().BeNull();
        baseBet.SuspensionReason.Should().BeNull();
        baseBet.LastUpdate.Should().Be(default(DateTime));
        baseBet.Probability.Should().Be(0.0);
        baseBet.ParticipantId.Should().BeNull();
        baseBet.PlayerName.Should().BeNull();
    }

    [Fact]
    public void BaseBet_Id_ShouldSetAndGetCorrectly()
    {
        // Arrange
        var baseBet = new BaseBet();
        var expectedId = 123456789L;

        // Act
        baseBet.Id = expectedId;

        // Assert
        baseBet.Id.Should().Be(expectedId);
    }

    [Theory]
    [InlineData(0L)]
    [InlineData(1L)]
    [InlineData(-1L)]
    [InlineData(long.MaxValue)]
    [InlineData(long.MinValue)]
    [InlineData(999999999999L)]
    public void BaseBet_Id_ShouldHandleVariousLongValues(long id)
    {
        // Arrange
        var baseBet = new BaseBet();

        // Act
        baseBet.Id = id;

        // Assert
        baseBet.Id.Should().Be(id);
    }

    [Fact]
    public void BaseBet_Name_ShouldSetAndGetCorrectly()
    {
        // Arrange
        var baseBet = new BaseBet();
        var expectedName = "Home Team Win";

        // Act
        baseBet.Name = expectedName;

        // Assert
        baseBet.Name.Should().Be(expectedName);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("Simple Bet")]
    [InlineData("Bet with Special Characters: !@#$%^&*()")]
    [InlineData("Bet with Unicode: 赌注")]
    [InlineData("Very Long Bet Name That Exceeds Normal Length Expectations")]
    public void BaseBet_Name_ShouldHandleVariousStringValues(string name)
    {
        // Arrange
        var baseBet = new BaseBet();

        // Act
        baseBet.Name = name;

        // Assert
        baseBet.Name.Should().Be(name);
    }

    [Fact]
    public void BaseBet_Name_ShouldHandleNullValue()
    {
        // Arrange
        var baseBet = new BaseBet();

        // Act
        baseBet.Name = null;

        // Assert
        baseBet.Name.Should().BeNull();
    }

    [Theory]
    [InlineData("")]
    [InlineData("0")]
    [InlineData("2.5")]
    [InlineData("-1.5")]
    [InlineData("+1")]
    [InlineData("Asian Handicap")]
    public void BaseBet_Line_ShouldHandleVariousStringValues(string line)
    {
        // Arrange
        var baseBet = new BaseBet();

        // Act
        baseBet.Line = line;

        // Assert
        baseBet.Line.Should().Be(line);
    }

    [Fact]
    public void BaseBet_Line_ShouldHandleNullValue()
    {
        // Arrange
        var baseBet = new BaseBet();

        // Act
        baseBet.Line = null;

        // Assert
        baseBet.Line.Should().BeNull();
    }

    [Theory]
    [InlineData("")]
    [InlineData("0")]
    [InlineData("1.5")]
    [InlineData("-2.5")]
    [InlineData("Base Line Value")]
    public void BaseBet_BaseLine_ShouldHandleVariousStringValues(string baseLine)
    {
        // Arrange
        var baseBet = new BaseBet();

        // Act
        baseBet.BaseLine = baseLine;

        // Assert
        baseBet.BaseLine.Should().Be(baseLine);
    }

    [Fact]
    public void BaseBet_BaseLine_ShouldHandleNullValue()
    {
        // Arrange
        var baseBet = new BaseBet();

        // Act
        baseBet.BaseLine = null;

        // Assert
        baseBet.BaseLine.Should().BeNull();
    }

    [Theory]
    [InlineData(BetStatus.NotSet)]
    [InlineData(BetStatus.Open)]
    [InlineData(BetStatus.Suspended)]
    [InlineData(BetStatus.Settled)]
    public void BaseBet_Status_ShouldHandleAllBetStatusValues(BetStatus status)
    {
        // Arrange
        var baseBet = new BaseBet();

        // Act
        baseBet.Status = status;

        // Assert
        baseBet.Status.Should().Be(status);
    }

    [Theory]
    [InlineData("")]
    [InlineData("1.50")]
    [InlineData("2.00")]
    [InlineData("10.5")]
    [InlineData("1000")]
    public void BaseBet_StartPrice_ShouldHandleVariousStringValues(string startPrice)
    {
        // Arrange
        var baseBet = new BaseBet();

        // Act
        baseBet.StartPrice = startPrice;

        // Assert
        baseBet.StartPrice.Should().Be(startPrice);
    }

    [Fact]
    public void BaseBet_StartPrice_ShouldHandleNullValue()
    {
        // Arrange
        var baseBet = new BaseBet();

        // Act
        baseBet.StartPrice = null;

        // Assert
        baseBet.StartPrice.Should().BeNull();
    }

    [Theory]
    [InlineData("")]
    [InlineData("1.50")]
    [InlineData("2.00")]
    [InlineData("10.5")]
    [InlineData("1000")]
    public void BaseBet_Price_ShouldHandleVariousStringValues(string price)
    {
        // Arrange
        var baseBet = new BaseBet();

        // Act
        baseBet.Price = price;

        // Assert
        baseBet.Price.Should().Be(price);
    }

    [Fact]
    public void BaseBet_Price_ShouldHandleNullValue()
    {
        // Arrange
        var baseBet = new BaseBet();

        // Act
        baseBet.Price = null;

        // Assert
        baseBet.Price.Should().BeNull();
    }

    [Theory]
    [InlineData("1.50")]
    [InlineData("2.00")]
    [InlineData("10.5")]
    public void BaseBet_PriceIN_ShouldHandleVariousStringValues(string priceIN)
    {
        // Arrange
        var baseBet = new BaseBet();

        // Act
        baseBet.PriceIN = priceIN;

        // Assert
        baseBet.PriceIN.Should().Be(priceIN);
    }

    [Theory]
    [InlineData("+150")]
    [InlineData("-200")]
    [InlineData("100")]
    public void BaseBet_PriceUS_ShouldHandleVariousStringValues(string priceUS)
    {
        // Arrange
        var baseBet = new BaseBet();

        // Act
        baseBet.PriceUS = priceUS;

        // Assert
        baseBet.PriceUS.Should().Be(priceUS);
    }

    [Theory]
    [InlineData("1/2")]
    [InlineData("5/1")]
    [InlineData("10/3")]
    public void BaseBet_PriceUK_ShouldHandleVariousStringValues(string priceUK)
    {
        // Arrange
        var baseBet = new BaseBet();

        // Act
        baseBet.PriceUK = priceUK;

        // Assert
        baseBet.PriceUK.Should().Be(priceUK);
    }

    [Theory]
    [InlineData("0.50")]
    [InlineData("1.00")]
    [InlineData("2.50")]
    public void BaseBet_PriceMA_ShouldHandleVariousStringValues(string priceMA)
    {
        // Arrange
        var baseBet = new BaseBet();

        // Act
        baseBet.PriceMA = priceMA;

        // Assert
        baseBet.PriceMA.Should().Be(priceMA);
    }

    [Theory]
    [InlineData("1.50")]
    [InlineData("2.00")]
    [InlineData("0.90")]
    public void BaseBet_PriceHK_ShouldHandleVariousStringValues(string priceHK)
    {
        // Arrange
        var baseBet = new BaseBet();

        // Act
        baseBet.PriceHK = priceHK;

        // Assert
        baseBet.PriceHK.Should().Be(priceHK);
    }

    [Theory]
    [InlineData("1000")]
    [InlineData("50000")]
    [InlineData("250000")]
    public void BaseBet_PriceVolume_ShouldHandleVariousStringValues(string priceVolume)
    {
        // Arrange
        var baseBet = new BaseBet();

        // Act
        baseBet.PriceVolume = priceVolume;

        // Assert
        baseBet.PriceVolume.Should().Be(priceVolume);
    }

    [Theory]
    [InlineData(SettlementType.Cancelled)]
    [InlineData(SettlementType.NotSettled)]
    [InlineData(SettlementType.Loser)]
    [InlineData(SettlementType.Winner)]
    [InlineData(SettlementType.Refund)]
    [InlineData(SettlementType.HalfLost)]
    [InlineData(SettlementType.HalfWon)]
    public void BaseBet_Settlement_ShouldHandleAllSettlementTypeValues(SettlementType settlementType)
    {
        // Arrange
        var baseBet = new BaseBet();

        // Act
        baseBet.Settlement = settlementType;

        // Assert
        baseBet.Settlement.Should().Be(settlementType);
    }

    [Fact]
    public void BaseBet_Settlement_ShouldHandleNullValue()
    {
        // Arrange
        var baseBet = new BaseBet();

        // Act
        baseBet.Settlement = null;

        // Assert
        baseBet.Settlement.Should().BeNull();
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(100)]
    [InlineData(-1)]
    [InlineData(int.MaxValue)]
    [InlineData(int.MinValue)]
    public void BaseBet_SuspensionReason_ShouldHandleVariousIntegerValues(int suspensionReason)
    {
        // Arrange
        var baseBet = new BaseBet();

        // Act
        baseBet.SuspensionReason = suspensionReason;

        // Assert
        baseBet.SuspensionReason.Should().Be(suspensionReason);
    }

    [Fact]
    public void BaseBet_SuspensionReason_ShouldHandleNullValue()
    {
        // Arrange
        var baseBet = new BaseBet();

        // Act
        baseBet.SuspensionReason = null;

        // Assert
        baseBet.SuspensionReason.Should().BeNull();
    }

    [Fact]
    public void BaseBet_LastUpdate_ShouldSetAndGetCorrectly()
    {
        // Arrange
        var baseBet = new BaseBet();
        var expectedDateTime = new DateTime(2024, 1, 15, 10, 30, 45);

        // Act
        baseBet.LastUpdate = expectedDateTime;

        // Assert
        baseBet.LastUpdate.Should().Be(expectedDateTime);
    }

    [Theory]
    [InlineData(0.0)]
    [InlineData(0.5)]
    [InlineData(1.0)]
    [InlineData(100.0)]
    [InlineData(-1.0)]
    [InlineData(double.MaxValue)]
    [InlineData(double.MinValue)]
    public void BaseBet_Probability_ShouldHandleVariousDoubleValues(double probability)
    {
        // Arrange
        var baseBet = new BaseBet();

        // Act
        baseBet.Probability = probability;

        // Assert
        baseBet.Probability.Should().Be(probability);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(12345)]
    [InlineData(-1)]
    [InlineData(int.MaxValue)]
    [InlineData(int.MinValue)]
    public void BaseBet_ParticipantId_ShouldHandleVariousIntegerValues(int participantId)
    {
        // Arrange
        var baseBet = new BaseBet();

        // Act
        baseBet.ParticipantId = participantId;

        // Assert
        baseBet.ParticipantId.Should().Be(participantId);
    }

    [Fact]
    public void BaseBet_ParticipantId_ShouldHandleNullValue()
    {
        // Arrange
        var baseBet = new BaseBet();

        // Act
        baseBet.ParticipantId = null;

        // Assert
        baseBet.ParticipantId.Should().BeNull();
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("John Doe")]
    [InlineData("Player with Special Characters: !@#$%")]
    [InlineData("Player with Unicode: 球员")]
    [InlineData("Very Long Player Name That Exceeds Normal Length")]
    public void BaseBet_PlayerName_ShouldHandleVariousStringValues(string playerName)
    {
        // Arrange
        var baseBet = new BaseBet();

        // Act
        baseBet.PlayerName = playerName;

        // Assert
        baseBet.PlayerName.Should().Be(playerName);
    }

    [Fact]
    public void BaseBet_PlayerName_ShouldHandleNullValue()
    {
        // Arrange
        var baseBet = new BaseBet();

        // Act
        baseBet.PlayerName = null;

        // Assert
        baseBet.PlayerName.Should().BeNull();
    }

    [Fact]
    public void BaseBet_AllProperties_ShouldSetAndGetCorrectly()
    {
        // Arrange
        var baseBet = new BaseBet();
        var expectedDateTime = DateTime.UtcNow;

        // Act
        baseBet.Id = 123456L;
        baseBet.Name = "Test Bet";
        baseBet.Line = "2.5";
        baseBet.BaseLine = "1.5";
        baseBet.Status = BetStatus.Open;
        baseBet.StartPrice = "2.00";
        baseBet.Price = "1.90";
        baseBet.PriceIN = "1.90";
        baseBet.PriceUS = "+190";
        baseBet.PriceUK = "9/10";
        baseBet.PriceMA = "0.90";
        baseBet.PriceHK = "0.90";
        baseBet.PriceVolume = "50000";
        baseBet.Settlement = SettlementType.Winner;
        baseBet.SuspensionReason = 1;
        baseBet.LastUpdate = expectedDateTime;
        baseBet.Probability = 0.526;
        baseBet.ParticipantId = 789;
        baseBet.PlayerName = "Test Player";

        // Assert
        baseBet.Id.Should().Be(123456L);
        baseBet.Name.Should().Be("Test Bet");
        baseBet.Line.Should().Be("2.5");
        baseBet.BaseLine.Should().Be("1.5");
        baseBet.Status.Should().Be(BetStatus.Open);
        baseBet.StartPrice.Should().Be("2.00");
        baseBet.Price.Should().Be("1.90");
        baseBet.PriceIN.Should().Be("1.90");
        baseBet.PriceUS.Should().Be("+190");
        baseBet.PriceUK.Should().Be("9/10");
        baseBet.PriceMA.Should().Be("0.90");
        baseBet.PriceHK.Should().Be("0.90");
        baseBet.PriceVolume.Should().Be("50000");
        baseBet.Settlement.Should().Be(SettlementType.Winner);
        baseBet.SuspensionReason.Should().Be(1);
        baseBet.LastUpdate.Should().Be(expectedDateTime);
        baseBet.Probability.Should().Be(0.526);
        baseBet.ParticipantId.Should().Be(789);
        baseBet.PlayerName.Should().Be("Test Player");
    }

    [Fact]
    public void BaseBet_PropertyChanges_ShouldNotAffectOtherProperties()
    {
        // Arrange
        var baseBet = new BaseBet
        {
            Id = 1L,
            Name = "Original Name",
            Price = "1.50"
        };

        // Act
        baseBet.Id = 999L;

        // Assert
        baseBet.Id.Should().Be(999L);
        baseBet.Name.Should().Be("Original Name");
        baseBet.Price.Should().Be("1.50");
    }

    [Fact]
    public void BaseBet_ObjectInstantiation_ShouldCreateIndependentInstances()
    {
        // Arrange & Act
        var baseBet1 = new BaseBet { Id = 1L, Name = "Bet 1" };
        var baseBet2 = new BaseBet { Id = 2L, Name = "Bet 2" };

        // Assert
        baseBet1.Id.Should().NotBe(baseBet2.Id);
        baseBet1.Name.Should().NotBe(baseBet2.Name);
    }

    [Fact]
    public void BaseBet_AllPriceFormats_ShouldHandleCorrectly()
    {
        // Arrange
        var baseBet = new BaseBet();

        // Act
        baseBet.Price = "2.00";      // Decimal
        baseBet.PriceIN = "2.00";    // Indonesian
        baseBet.PriceUS = "+100";    // American
        baseBet.PriceUK = "1/1";     // Fractional
        baseBet.PriceMA = "1.00";    // Malay
        baseBet.PriceHK = "1.00";    // Hong Kong

        // Assert
        baseBet.Price.Should().Be("2.00");
        baseBet.PriceIN.Should().Be("2.00");
        baseBet.PriceUS.Should().Be("+100");
        baseBet.PriceUK.Should().Be("1/1");
        baseBet.PriceMA.Should().Be("1.00");
        baseBet.PriceHK.Should().Be("1.00");
    }

    [Fact]
    public void BaseBet_NullableProperties_ShouldHandleNullAndNonNullCorrectly()
    {
        // Arrange
        var baseBet = new BaseBet();

        // Act & Assert - Initial null state
        baseBet.Name.Should().BeNull();
        baseBet.Settlement.Should().BeNull();
        baseBet.SuspensionReason.Should().BeNull();
        baseBet.ParticipantId.Should().BeNull();
        baseBet.PlayerName.Should().BeNull();

        // Act - Set non-null values
        baseBet.Name = "Test Name";
        baseBet.Settlement = SettlementType.Winner;
        baseBet.SuspensionReason = 1;
        baseBet.ParticipantId = 123;
        baseBet.PlayerName = "Test Player";

        // Assert - Non-null state
        baseBet.Name.Should().NotBeNull();
        baseBet.Settlement.Should().NotBeNull();
        baseBet.SuspensionReason.Should().NotBeNull();
        baseBet.ParticipantId.Should().NotBeNull();
        baseBet.PlayerName.Should().NotBeNull();

        // Act - Set back to null
        baseBet.Name = null;
        baseBet.Settlement = null;
        baseBet.SuspensionReason = null;
        baseBet.ParticipantId = null;
        baseBet.PlayerName = null;

        // Assert - Back to null state
        baseBet.Name.Should().BeNull();
        baseBet.Settlement.Should().BeNull();
        baseBet.SuspensionReason.Should().BeNull();
        baseBet.ParticipantId.Should().BeNull();
        baseBet.PlayerName.Should().BeNull();
    }
} 