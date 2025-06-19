using System;
using FluentAssertions;
using Xunit;
using Trade360SDK.Common.Entities.Markets;
using Trade360SDK.Common.Entities.Enums;

namespace Trade360SDK.Common.Entities.Tests.Entities.Markets
{
    public class BaseBetComprehensiveTests
    {
        [Fact]
        public void BaseBet_DefaultConstructor_ShouldCreateInstanceWithDefaultValues()
        {
            // Act
            var baseBet = new BaseBet();

            // Assert
            baseBet.Should().NotBeNull();
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
        public void BaseBet_SetId_ShouldSetValue()
        {
            // Arrange
            var baseBet = new BaseBet();
            var id = 123456789L;

            // Act
            baseBet.Id = id;

            // Assert
            baseBet.Id.Should().Be(id);
        }

        [Theory]
        [InlineData(0L)]
        [InlineData(1L)]
        [InlineData(-1L)]
        [InlineData(long.MaxValue)]
        [InlineData(long.MinValue)]
        [InlineData(999999999L)]
        public void BaseBet_SetVariousIds_ShouldSetValue(long id)
        {
            // Arrange
            var baseBet = new BaseBet();

            // Act
            baseBet.Id = id;

            // Assert
            baseBet.Id.Should().Be(id);
        }

        [Fact]
        public void BaseBet_SetName_ShouldSetValue()
        {
            // Arrange
            var baseBet = new BaseBet();
            var name = "Manchester United";

            // Act
            baseBet.Name = name;

            // Assert
            baseBet.Name.Should().Be(name);
        }

        [Theory]
        [InlineData("")]
        [InlineData("   ")]
        [InlineData("Home")]
        [InlineData("Away")]
        [InlineData("Draw")]
        [InlineData("Over 2.5")]
        [InlineData("Under 2.5")]
        [InlineData("Both Teams To Score - Yes")]
        [InlineData("Both Teams To Score - No")]
        public void BaseBet_SetVariousNames_ShouldSetValue(string name)
        {
            // Arrange
            var baseBet = new BaseBet();

            // Act
            baseBet.Name = name;

            // Assert
            baseBet.Name.Should().Be(name);
        }

        [Fact]
        public void BaseBet_SetLine_ShouldSetValue()
        {
            // Arrange
            var baseBet = new BaseBet();
            var line = "2.5";

            // Act
            baseBet.Line = line;

            // Assert
            baseBet.Line.Should().Be(line);
        }

        [Theory]
        [InlineData("")]
        [InlineData("0")]
        [InlineData("0.5")]
        [InlineData("1.5")]
        [InlineData("2.5")]
        [InlineData("-1")]
        [InlineData("+1")]
        [InlineData("-0.5")]
        [InlineData("+0.5")]
        public void BaseBet_SetVariousLines_ShouldSetValue(string line)
        {
            // Arrange
            var baseBet = new BaseBet();

            // Act
            baseBet.Line = line;

            // Assert
            baseBet.Line.Should().Be(line);
        }

        [Fact]
        public void BaseBet_SetBaseLine_ShouldSetValue()
        {
            // Arrange
            var baseBet = new BaseBet();
            var baseLine = "1.5";

            // Act
            baseBet.BaseLine = baseLine;

            // Assert
            baseBet.BaseLine.Should().Be(baseLine);
        }

        [Fact]
        public void BaseBet_SetStatus_ShouldSetValue()
        {
            // Arrange
            var baseBet = new BaseBet();
            var status = BetStatus.Open;

            // Act
            baseBet.Status = status;

            // Assert
            baseBet.Status.Should().Be(status);
        }

        [Theory]
        [InlineData(BetStatus.NotSet)]
        [InlineData(BetStatus.Open)]
        [InlineData(BetStatus.Suspended)]
        [InlineData(BetStatus.Settled)]
        public void BaseBet_SetVariousStatuses_ShouldSetValue(BetStatus status)
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
        [InlineData("3.25")]
        [InlineData("10.50")]
        [InlineData("1.01")]
        [InlineData("999.99")]
        public void BaseBet_SetVariousPrices_ShouldSetValue(string price)
        {
            // Arrange
            var baseBet = new BaseBet();

            // Act
            baseBet.StartPrice = price;
            baseBet.Price = price;
            baseBet.PriceIN = price;
            baseBet.PriceUS = price;
            baseBet.PriceUK = price;
            baseBet.PriceMA = price;
            baseBet.PriceHK = price;

            // Assert
            baseBet.StartPrice.Should().Be(price);
            baseBet.Price.Should().Be(price);
            baseBet.PriceIN.Should().Be(price);
            baseBet.PriceUS.Should().Be(price);
            baseBet.PriceUK.Should().Be(price);
            baseBet.PriceMA.Should().Be(price);
            baseBet.PriceHK.Should().Be(price);
        }

        [Fact]
        public void BaseBet_SetPriceVolume_ShouldSetValue()
        {
            // Arrange
            var baseBet = new BaseBet();
            var priceVolume = "10000.50";

            // Act
            baseBet.PriceVolume = priceVolume;

            // Assert
            baseBet.PriceVolume.Should().Be(priceVolume);
        }

        [Fact]
        public void BaseBet_SetSettlement_ShouldSetValue()
        {
            // Arrange
            var baseBet = new BaseBet();
            var settlement = SettlementType.Winner;

            // Act
            baseBet.Settlement = settlement;

            // Assert
            baseBet.Settlement.Should().Be(settlement);
        }

        [Fact]
        public void BaseBet_SetSuspensionReason_ShouldSetValue()
        {
            // Arrange
            var baseBet = new BaseBet();
            var suspensionReason = 5;

            // Act
            baseBet.SuspensionReason = suspensionReason;

            // Assert
            baseBet.SuspensionReason.Should().Be(suspensionReason);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(-1)]
        [InlineData(100)]
        [InlineData(int.MaxValue)]
        [InlineData(int.MinValue)]
        public void BaseBet_SetVariousSuspensionReasons_ShouldSetValue(int suspensionReason)
        {
            // Arrange
            var baseBet = new BaseBet();

            // Act
            baseBet.SuspensionReason = suspensionReason;

            // Assert
            baseBet.SuspensionReason.Should().Be(suspensionReason);
        }

        [Fact]
        public void BaseBet_SetLastUpdate_ShouldSetValue()
        {
            // Arrange
            var baseBet = new BaseBet();
            var lastUpdate = DateTime.UtcNow;

            // Act
            baseBet.LastUpdate = lastUpdate;

            // Assert
            baseBet.LastUpdate.Should().Be(lastUpdate);
        }

        [Fact]
        public void BaseBet_SetProbability_ShouldSetValue()
        {
            // Arrange
            var baseBet = new BaseBet();
            var probability = 0.75;

            // Act
            baseBet.Probability = probability;

            // Assert
            baseBet.Probability.Should().Be(probability);
        }

        [Theory]
        [InlineData(0.0)]
        [InlineData(0.5)]
        [InlineData(1.0)]
        [InlineData(0.25)]
        [InlineData(0.75)]
        [InlineData(0.999)]
        [InlineData(double.MaxValue)]
        [InlineData(double.MinValue)]
        public void BaseBet_SetVariousProbabilities_ShouldSetValue(double probability)
        {
            // Arrange
            var baseBet = new BaseBet();

            // Act
            baseBet.Probability = probability;

            // Assert
            baseBet.Probability.Should().Be(probability);
        }

        [Fact]
        public void BaseBet_SetParticipantId_ShouldSetValue()
        {
            // Arrange
            var baseBet = new BaseBet();
            var participantId = 12345;

            // Act
            baseBet.ParticipantId = participantId;

            // Assert
            baseBet.ParticipantId.Should().Be(participantId);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(-1)]
        [InlineData(int.MaxValue)]
        [InlineData(int.MinValue)]
        public void BaseBet_SetVariousParticipantIds_ShouldSetValue(int participantId)
        {
            // Arrange
            var baseBet = new BaseBet();

            // Act
            baseBet.ParticipantId = participantId;

            // Assert
            baseBet.ParticipantId.Should().Be(participantId);
        }

        [Fact]
        public void BaseBet_SetPlayerName_ShouldSetValue()
        {
            // Arrange
            var baseBet = new BaseBet();
            var playerName = "Cristiano Ronaldo";

            // Act
            baseBet.PlayerName = playerName;

            // Assert
            baseBet.PlayerName.Should().Be(playerName);
        }

        [Theory]
        [InlineData("")]
        [InlineData("   ")]
        [InlineData("Lionel Messi")]
        [InlineData("Cristiano Ronaldo")]
        [InlineData("Kevin De Bruyne")]
        [InlineData("Mohamed Salah")]
        public void BaseBet_SetVariousPlayerNames_ShouldSetValue(string playerName)
        {
            // Arrange
            var baseBet = new BaseBet();

            // Act
            baseBet.PlayerName = playerName;

            // Assert
            baseBet.PlayerName.Should().Be(playerName);
        }

        [Fact]
        public void BaseBet_SetAllProperties_ShouldSetAllValues()
        {
            // Arrange
            var baseBet = new BaseBet();
            var id = 123456L;
            var name = "Test Bet";
            var line = "1.5";
            var baseLine = "1.0";
            var status = BetStatus.Open;
            var startPrice = "2.50";
            var price = "2.60";
            var priceIN = "2.55";
            var priceUS = "+160";
            var priceUK = "8/5";
            var priceMA = "1.60";
            var priceHK = "0.60";
            var priceVolume = "50000";
            var settlement = SettlementType.Winner;
            var suspensionReason = 3;
            var lastUpdate = DateTime.UtcNow;
            var probability = 0.65;
            var participantId = 789;
            var playerName = "Test Player";

            // Act
            baseBet.Id = id;
            baseBet.Name = name;
            baseBet.Line = line;
            baseBet.BaseLine = baseLine;
            baseBet.Status = status;
            baseBet.StartPrice = startPrice;
            baseBet.Price = price;
            baseBet.PriceIN = priceIN;
            baseBet.PriceUS = priceUS;
            baseBet.PriceUK = priceUK;
            baseBet.PriceMA = priceMA;
            baseBet.PriceHK = priceHK;
            baseBet.PriceVolume = priceVolume;
            baseBet.Settlement = settlement;
            baseBet.SuspensionReason = suspensionReason;
            baseBet.LastUpdate = lastUpdate;
            baseBet.Probability = probability;
            baseBet.ParticipantId = participantId;
            baseBet.PlayerName = playerName;

            // Assert
            baseBet.Id.Should().Be(id);
            baseBet.Name.Should().Be(name);
            baseBet.Line.Should().Be(line);
            baseBet.BaseLine.Should().Be(baseLine);
            baseBet.Status.Should().Be(status);
            baseBet.StartPrice.Should().Be(startPrice);
            baseBet.Price.Should().Be(price);
            baseBet.PriceIN.Should().Be(priceIN);
            baseBet.PriceUS.Should().Be(priceUS);
            baseBet.PriceUK.Should().Be(priceUK);
            baseBet.PriceMA.Should().Be(priceMA);
            baseBet.PriceHK.Should().Be(priceHK);
            baseBet.PriceVolume.Should().Be(priceVolume);
            baseBet.Settlement.Should().Be(settlement);
            baseBet.SuspensionReason.Should().Be(suspensionReason);
            baseBet.LastUpdate.Should().Be(lastUpdate);
            baseBet.Probability.Should().Be(probability);
            baseBet.ParticipantId.Should().Be(participantId);
            baseBet.PlayerName.Should().Be(playerName);
        }

        [Fact]
        public void BaseBet_SetNullValues_ShouldSetNulls()
        {
            // Arrange
            var baseBet = new BaseBet();

            // Act
            baseBet.Name = null;
            baseBet.Line = null;
            baseBet.BaseLine = null;
            baseBet.StartPrice = null;
            baseBet.Price = null;
            baseBet.PriceIN = null;
            baseBet.PriceUS = null;
            baseBet.PriceUK = null;
            baseBet.PriceMA = null;
            baseBet.PriceHK = null;
            baseBet.PriceVolume = null;
            baseBet.Settlement = null;
            baseBet.SuspensionReason = null;
            baseBet.ParticipantId = null;
            baseBet.PlayerName = null;

            // Assert
            baseBet.Name.Should().BeNull();
            baseBet.Line.Should().BeNull();
            baseBet.BaseLine.Should().BeNull();
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
            baseBet.ParticipantId.Should().BeNull();
            baseBet.PlayerName.Should().BeNull();
        }

        [Fact]
        public void BaseBet_PropertySettersGetters_ShouldWorkCorrectly()
        {
            // Arrange
            var baseBet = new BaseBet();

            // Act & Assert - Test that we can set and get each property multiple times
            baseBet.Id = 1L;
            baseBet.Id.Should().Be(1L);
            baseBet.Id = 2L;
            baseBet.Id.Should().Be(2L);

            baseBet.Name = "Name1";
            baseBet.Name.Should().Be("Name1");
            baseBet.Name = "Name2";
            baseBet.Name.Should().Be("Name2");
            baseBet.Name = null;
            baseBet.Name.Should().BeNull();

            baseBet.Status = BetStatus.Open;
            baseBet.Status.Should().Be(BetStatus.Open);
            baseBet.Status = BetStatus.Suspended;
            baseBet.Status.Should().Be(BetStatus.Suspended);

            baseBet.Probability = 0.5;
            baseBet.Probability.Should().Be(0.5);
            baseBet.Probability = 0.8;
            baseBet.Probability.Should().Be(0.8);
        }

        [Fact]
        public void BaseBet_ToString_ShouldReturnStringRepresentation()
        {
            // Arrange
            var baseBet = new BaseBet
            {
                Id = 1,
                Name = "Test Bet"
            };

            // Act
            var result = baseBet.ToString();

            // Assert
            result.Should().NotBeNullOrEmpty();
            result.Should().Contain("BaseBet");
        }

        [Fact]
        public void BaseBet_GetHashCode_ShouldReturnConsistentValue()
        {
            // Arrange
            var baseBet = new BaseBet
            {
                Id = 1,
                Name = "Test Bet"
            };

            // Act
            var hashCode1 = baseBet.GetHashCode();
            var hashCode2 = baseBet.GetHashCode();

            // Assert
            hashCode1.Should().Be(hashCode2);
        }

        [Fact]
        public void BaseBet_Type_ShouldBeCorrect()
        {
            // Arrange & Act
            var baseBet = new BaseBet();

            // Assert
            baseBet.GetType().Should().Be(typeof(BaseBet));
            baseBet.GetType().Name.Should().Be("BaseBet");
            baseBet.GetType().Namespace.Should().Be("Trade360SDK.Common.Entities.Markets");
        }
    }
} 