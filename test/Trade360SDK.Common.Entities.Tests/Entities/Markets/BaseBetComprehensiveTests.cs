using System;
using FluentAssertions;
using Trade360SDK.Common.Entities.Enums;
using Trade360SDK.Common.Entities.Markets;
using Xunit;

namespace Trade360SDK.Common.Entities.Tests.Entities.Markets;

public class BaseBetComprehensiveTests
{
    [Fact]
    public void Constructor_ShouldCreateInstanceSuccessfully()
    {
        // Act
        var baseBet = new BaseBet();

        // Assert
        baseBet.Should().NotBeNull();
        baseBet.Id.Should().Be(0); // Default long value
        baseBet.Status.Should().Be(BetStatus.NotSet); // Default enum value
        baseBet.Probability.Should().BeNull(); // Default nullable double value
        baseBet.LastUpdate.Should().Be(default(DateTime)); // Default DateTime
    }

    [Theory]
    [InlineData(1L)]
    [InlineData(long.MaxValue)]
    [InlineData(long.MinValue)]
    [InlineData(42L)]
    [InlineData(0L)]
    public void Id_Property_ShouldAcceptValidValues(long expectedId)
    {
        // Arrange
        var baseBet = new BaseBet();

        // Act
        baseBet.Id = expectedId;

        // Assert
        baseBet.Id.Should().Be(expectedId);
    }

    [Theory]
    [InlineData("Test Bet Name")]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("Very Long Bet Name With Special Characters @#$%")]
    public void Name_Property_ShouldAcceptNullableStringValues(string? expectedName)
    {
        // Arrange
        var baseBet = new BaseBet();

        // Act
        baseBet.Name = expectedName;

        // Assert
        baseBet.Name.Should().Be(expectedName);
    }

    [Theory]
    [InlineData("1.5")]
    [InlineData("+2.5")]
    [InlineData("-1.5")]
    [InlineData("")]
    [InlineData(null)]
    public void Line_Property_ShouldAcceptNullableStringValues(string? expectedLine)
    {
        // Arrange
        var baseBet = new BaseBet();

        // Act
        baseBet.Line = expectedLine;

        // Assert
        baseBet.Line.Should().Be(expectedLine);
    }

    [Theory]
    [InlineData("0.0")]
    [InlineData("1.0")]
    [InlineData("-0.5")]
    [InlineData("")]
    [InlineData(null)]
    public void BaseLine_Property_ShouldAcceptNullableStringValues(string? expectedBaseLine)
    {
        // Arrange
        var baseBet = new BaseBet();

        // Act
        baseBet.BaseLine = expectedBaseLine;

        // Assert
        baseBet.BaseLine.Should().Be(expectedBaseLine);
    }

    [Theory]
    [InlineData(BetStatus.NotSet)]
    [InlineData(BetStatus.Open)]
    [InlineData(BetStatus.Suspended)]
    [InlineData(BetStatus.Settled)]
    public void Status_Property_ShouldAcceptValidBetStatusValues(BetStatus expectedStatus)
    {
        // Arrange
        var baseBet = new BaseBet();

        // Act
        baseBet.Status = expectedStatus;

        // Assert
        baseBet.Status.Should().Be(expectedStatus);
    }

    [Theory]
    [InlineData("1.50")]
    [InlineData("2.00")]
    [InlineData("10.5")]
    [InlineData("")]
    [InlineData(null)]
    public void StartPrice_Property_ShouldAcceptNullableStringValues(string? expectedStartPrice)
    {
        // Arrange
        var baseBet = new BaseBet();

        // Act
        baseBet.StartPrice = expectedStartPrice;

        // Assert
        baseBet.StartPrice.Should().Be(expectedStartPrice);
    }

    [Theory]
    [InlineData("1.75")]
    [InlineData("3.50")]
    [InlineData("100.00")]
    [InlineData("")]
    [InlineData(null)]
    public void Price_Property_ShouldAcceptNullableStringValues(string? expectedPrice)
    {
        // Arrange
        var baseBet = new BaseBet();

        // Act
        baseBet.Price = expectedPrice;

        // Assert
        baseBet.Price.Should().Be(expectedPrice);
    }

    [Theory]
    [InlineData("1.80")]
    [InlineData("2.25")]
    [InlineData("")]
    [InlineData(null)]
    public void PriceIN_Property_ShouldAcceptNullableStringValues(string? expectedPriceIN)
    {
        // Arrange
        var baseBet = new BaseBet();

        // Act
        baseBet.PriceIN = expectedPriceIN;

        // Assert
        baseBet.PriceIN.Should().Be(expectedPriceIN);
    }

    [Theory]
    [InlineData("+150")]
    [InlineData("-200")]
    [InlineData("100")]
    [InlineData("")]
    [InlineData(null)]
    public void PriceUS_Property_ShouldAcceptNullableStringValues(string? expectedPriceUS)
    {
        // Arrange
        var baseBet = new BaseBet();

        // Act
        baseBet.PriceUS = expectedPriceUS;

        // Assert
        baseBet.PriceUS.Should().Be(expectedPriceUS);
    }

    [Theory]
    [InlineData("3/4")]
    [InlineData("1/2")]
    [InlineData("5/1")]
    [InlineData("")]
    [InlineData(null)]
    public void PriceUK_Property_ShouldAcceptNullableStringValues(string? expectedPriceUK)
    {
        // Arrange
        var baseBet = new BaseBet();

        // Act
        baseBet.PriceUK = expectedPriceUK;

        // Assert
        baseBet.PriceUK.Should().Be(expectedPriceUK);
    }

    [Theory]
    [InlineData("0.75")]
    [InlineData("1.25")]
    [InlineData("")]
    [InlineData(null)]
    public void PriceMA_Property_ShouldAcceptNullableStringValues(string? expectedPriceMA)
    {
        // Arrange
        var baseBet = new BaseBet();

        // Act
        baseBet.PriceMA = expectedPriceMA;

        // Assert
        baseBet.PriceMA.Should().Be(expectedPriceMA);
    }

    [Theory]
    [InlineData("0.80")]
    [InlineData("1.50")]
    [InlineData("")]
    [InlineData(null)]
    public void PriceHK_Property_ShouldAcceptNullableStringValues(string? expectedPriceHK)
    {
        // Arrange
        var baseBet = new BaseBet();

        // Act
        baseBet.PriceHK = expectedPriceHK;

        // Assert
        baseBet.PriceHK.Should().Be(expectedPriceHK);
    }

    [Theory]
    [InlineData("10000")]
    [InlineData("50000")]
    [InlineData("")]
    [InlineData(null)]
    public void PriceVolume_Property_ShouldAcceptNullableStringValues(string? expectedPriceVolume)
    {
        // Arrange
        var baseBet = new BaseBet();

        // Act
        baseBet.PriceVolume = expectedPriceVolume;

        // Assert
        baseBet.PriceVolume.Should().Be(expectedPriceVolume);
    }

    [Theory]
    [InlineData(SettlementType.NotSettled)]
    [InlineData(SettlementType.Winner)]
    [InlineData(SettlementType.Loser)]
    [InlineData(SettlementType.Refund)]
    [InlineData(SettlementType.HalfWon)]
    [InlineData(SettlementType.HalfLost)]
    [InlineData(SettlementType.Cancelled)]
    [InlineData(null)]
    public void Settlement_Property_ShouldAcceptNullableSettlementTypeValues(SettlementType? expectedSettlement)
    {
        // Arrange
        var baseBet = new BaseBet();

        // Act
        baseBet.Settlement = expectedSettlement;

        // Assert
        baseBet.Settlement.Should().Be(expectedSettlement);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(100)]
    [InlineData(null)]
    [InlineData(0)]
    [InlineData(-1)]
    public void SuspensionReason_Property_ShouldAcceptNullableIntValues(int? expectedSuspensionReason)
    {
        // Arrange
        var baseBet = new BaseBet();

        // Act
        baseBet.SuspensionReason = expectedSuspensionReason;

        // Assert
        baseBet.SuspensionReason.Should().Be(expectedSuspensionReason);
    }

    [Fact]
    public void LastUpdate_Property_ShouldAcceptDateTimeValues()
    {
        // Arrange
        var baseBet = new BaseBet();
        var expectedDateTime = new DateTime(2023, 12, 25, 10, 30, 45);

        // Act
        baseBet.LastUpdate = expectedDateTime;

        // Assert
        baseBet.LastUpdate.Should().Be(expectedDateTime);
    }

    [Theory]
    [InlineData(0.0)]
    [InlineData(0.5)]
    [InlineData(1.0)]
    [InlineData(0.25)]
    [InlineData(0.75)]
    [InlineData(100.0)]
    [InlineData(-1.0)]
    public void Probability_Property_ShouldAcceptDoubleValues(double expectedProbability)
    {
        // Arrange
        var baseBet = new BaseBet();

        // Act
        baseBet.Probability = expectedProbability;

        // Assert
        baseBet.Probability.Should().Be(expectedProbability);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(1000)]
    [InlineData(null)]
    [InlineData(0)]
    [InlineData(-1)]
    public void ParticipantId_Property_ShouldAcceptNullableIntValues(int? expectedParticipantId)
    {
        // Arrange
        var baseBet = new BaseBet();

        // Act
        baseBet.ParticipantId = expectedParticipantId;

        // Assert
        baseBet.ParticipantId.Should().Be(expectedParticipantId);
    }

    [Theory]
    [InlineData("123")]
    [InlineData("456789")]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("P12345")]
    public void PlayerId_Property_ShouldAcceptNullableStringValues(string? expectedPlayerId)
    {
        // Arrange
        var baseBet = new BaseBet();

        // Act
        baseBet.PlayerId = expectedPlayerId;

        // Assert
        baseBet.PlayerId.Should().Be(expectedPlayerId);
    }

    [Theory]
    [InlineData("John Doe")]
    [InlineData("Player Name")]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("Very Long Player Name With Special Characters")]
    public void PlayerName_Property_ShouldAcceptNullableStringValues(string? expectedPlayerName)
    {
        // Arrange
        var baseBet = new BaseBet();

        // Act
        baseBet.PlayerName = expectedPlayerName;

        // Assert
        baseBet.PlayerName.Should().Be(expectedPlayerName);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(10)]
    [InlineData(100)]
    [InlineData(null)]
    [InlineData(0)]
    [InlineData(-1)]
    public void Order_Property_ShouldAcceptNullableIntValues(int? expectedOrder)
    {
        // Arrange
        var baseBet = new BaseBet();

        // Act
        baseBet.Order = expectedOrder;

        // Assert
        baseBet.Order.Should().Be(expectedOrder);
    }

    [Fact]
    public void AllProperties_ShouldWorkIndependently()
    {
        // Arrange
        var baseBet = new BaseBet();
        var testDateTime = DateTime.UtcNow;

        // Act
        baseBet.Id = 12345L;
        baseBet.Name = "Test Bet";
        baseBet.Line = "1.5";
        baseBet.BaseLine = "0.0";
        baseBet.Status = BetStatus.Open;
        baseBet.StartPrice = "2.00";
        baseBet.Price = "1.95";
        baseBet.PriceIN = "1.90";
        baseBet.PriceUS = "+100";
        baseBet.PriceUK = "1/1";
        baseBet.PriceMA = "1.00";
        baseBet.PriceHK = "1.00";
        baseBet.PriceVolume = "50000";
        baseBet.Settlement = SettlementType.NotSettled;
        baseBet.SuspensionReason = 5;
        baseBet.LastUpdate = testDateTime;
        baseBet.Probability = 0.52;
        baseBet.ParticipantId = 789;
        baseBet.PlayerId = "456";
        baseBet.PlayerName = "John Smith";
        baseBet.Order = 3;

        // Assert
        baseBet.Id.Should().Be(12345L);
        baseBet.Name.Should().Be("Test Bet");
        baseBet.Line.Should().Be("1.5");
        baseBet.BaseLine.Should().Be("0.0");
        baseBet.Status.Should().Be(BetStatus.Open);
        baseBet.StartPrice.Should().Be("2.00");
        baseBet.Price.Should().Be("1.95");
        baseBet.PriceIN.Should().Be("1.90");
        baseBet.PriceUS.Should().Be("+100");
        baseBet.PriceUK.Should().Be("1/1");
        baseBet.PriceMA.Should().Be("1.00");
        baseBet.PriceHK.Should().Be("1.00");
        baseBet.PriceVolume.Should().Be("50000");
        baseBet.Settlement.Should().Be(SettlementType.NotSettled);
        baseBet.SuspensionReason.Should().Be(5);
        baseBet.LastUpdate.Should().Be(testDateTime);
        baseBet.Probability.Should().Be(0.52);
        baseBet.ParticipantId.Should().Be(789);
        baseBet.PlayerId.Should().Be("456");
        baseBet.PlayerName.Should().Be("John Smith");
        baseBet.Order.Should().Be(3);
    }

    [Fact]
    public void Object_ShouldBeInstantiable()
    {
        // Act & Assert
        var baseBet = new BaseBet();
        baseBet.Should().BeOfType<BaseBet>();
        baseBet.Should().NotBeNull();
    }

    [Fact]
    public void Properties_ShouldBeGettableAndSettable()
    {
        // Arrange
        var baseBet = new BaseBet();

        // Act & Assert - Test that all properties can be read and written
        var id = baseBet.Id;
        baseBet.Id = 999L;
        baseBet.Id.Should().Be(999L);

        var name = baseBet.Name;
        baseBet.Name = "New Name";
        baseBet.Name.Should().Be("New Name");

        var line = baseBet.Line;
        baseBet.Line = "2.5";
        baseBet.Line.Should().Be("2.5");

        var baseLine = baseBet.BaseLine;
        baseBet.BaseLine = "1.0";
        baseBet.BaseLine.Should().Be("1.0");

        var status = baseBet.Status;
        baseBet.Status = BetStatus.Suspended;
        baseBet.Status.Should().Be(BetStatus.Suspended);

        var probability = baseBet.Probability;
        baseBet.Probability = 0.85;
        baseBet.Probability.Should().Be(0.85);
    }
} 