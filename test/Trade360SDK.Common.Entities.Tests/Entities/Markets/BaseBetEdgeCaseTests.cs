using System;
using System.Globalization;
using FluentAssertions;
using Trade360SDK.Common.Entities.Enums;
using Trade360SDK.Common.Entities.Markets;
using Xunit;

namespace Trade360SDK.Common.Entities.Tests.Entities.Markets
{
    public class BaseBetEdgeCaseTests
    {
        [Fact]
        public void BaseBet_WithLongId_ShouldHandleMaxValues()
        {
            // Arrange
            var baseBet = new BaseBet();

            // Act
            baseBet.Id = long.MaxValue;

            // Assert
            baseBet.Id.Should().Be(long.MaxValue);
        }

        [Fact]
        public void BaseBet_WithNegativeId_ShouldAllowNegativeValues()
        {
            // Arrange
            var baseBet = new BaseBet();

            // Act
            baseBet.Id = -1L;

            // Assert
            baseBet.Id.Should().Be(-1L);
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("\t")]
        [InlineData("\n")]
        [InlineData("\r\n")]
        [InlineData("   \t  \n  ")]
        public void BaseBet_WithWhitespaceNames_ShouldAcceptWhitespace(string name)
        {
            // Arrange
            var baseBet = new BaseBet();

            // Act
            baseBet.Name = name;

            // Assert
            baseBet.Name.Should().Be(name);
        }

        [Theory]
        [InlineData("0")]
        [InlineData("0.0")]
        [InlineData("0.00")]
        [InlineData("1.0")]
        [InlineData("1.00")]
        [InlineData("1.01")]
        [InlineData("999.99")]
        [InlineData("1000000.00")]
        public void BaseBet_WithVariousPriceFormats_ShouldAcceptAllFormats(string price)
        {
            // Arrange
            var baseBet = new BaseBet();

            // Act
            baseBet.Price = price;
            baseBet.StartPrice = price;
            baseBet.PriceIN = price;
            baseBet.PriceUS = price;
            baseBet.PriceUK = price;
            baseBet.PriceMA = price;
            baseBet.PriceHK = price;

            // Assert
            baseBet.Price.Should().Be(price);
            baseBet.StartPrice.Should().Be(price);
            baseBet.PriceIN.Should().Be(price);
            baseBet.PriceUS.Should().Be(price);
            baseBet.PriceUK.Should().Be(price);
            baseBet.PriceMA.Should().Be(price);
            baseBet.PriceHK.Should().Be(price);
        }

        [Theory]
        [InlineData("+100")]
        [InlineData("-110")]
        [InlineData("+200")]
        [InlineData("-200")]
        [InlineData("EVEN")]
        [InlineData("EVENS")]
        public void BaseBet_WithAmericanOddsFormat_ShouldAcceptAmericanFormat(string americanOdds)
        {
            // Arrange
            var baseBet = new BaseBet();

            // Act
            baseBet.PriceUS = americanOdds;

            // Assert
            baseBet.PriceUS.Should().Be(americanOdds);
        }

        [Theory]
        [InlineData("1/2")]
        [InlineData("2/1")]
        [InlineData("5/4")]
        [InlineData("11/10")]
        [InlineData("100/30")]
        [InlineData("EVS")]
        [InlineData("EVENS")]
        public void BaseBet_WithFractionalOddsFormat_ShouldAcceptFractionalFormat(string fractionalOdds)
        {
            // Arrange
            var baseBet = new BaseBet();

            // Act
            baseBet.PriceUK = fractionalOdds;

            // Assert
            baseBet.PriceUK.Should().Be(fractionalOdds);
        }

        [Theory]
        [InlineData("0")]
        [InlineData("1000")]
        [InlineData("10000")]
        [InlineData("100000")]
        [InlineData("1000000")]
        [InlineData("999999999")]
        public void BaseBet_WithVariousPriceVolumes_ShouldAcceptAllVolumes(string volume)
        {
            // Arrange
            var baseBet = new BaseBet();

            // Act
            baseBet.PriceVolume = volume;

            // Assert
            baseBet.PriceVolume.Should().Be(volume);
        }

        [Fact]
        public void BaseBet_WithMinDateTime_ShouldAcceptMinValue()
        {
            // Arrange
            var baseBet = new BaseBet();

            // Act
            baseBet.LastUpdate = DateTime.MinValue;

            // Assert
            baseBet.LastUpdate.Should().Be(DateTime.MinValue);
        }

        [Fact]
        public void BaseBet_WithMaxDateTime_ShouldAcceptMaxValue()
        {
            // Arrange
            var baseBet = new BaseBet();

            // Act
            baseBet.LastUpdate = DateTime.MaxValue;

            // Assert
            baseBet.LastUpdate.Should().Be(DateTime.MaxValue);
        }

        [Theory]
        [InlineData(0.0)]
        [InlineData(0.1)]
        [InlineData(0.01)]
        [InlineData(0.001)]
        [InlineData(0.9999)]
        [InlineData(1.0)]
        [InlineData(100.0)]
        [InlineData(double.MaxValue)]
        [InlineData(double.MinValue)]
        public void BaseBet_WithExtremeProbabilities_ShouldAcceptAllValues(double probability)
        {
            // Arrange
            var baseBet = new BaseBet();

            // Act
            baseBet.Probability = probability;

            // Assert
            baseBet.Probability.Should().Be(probability);
        }

        [Fact]
        public void BaseBet_WithNaN_ShouldAcceptNaN()
        {
            // Arrange
            var baseBet = new BaseBet();

            // Act
            baseBet.Probability = double.NaN;

            // Assert
            baseBet.Probability.Should().Be(double.NaN);
        }

        [Fact]
        public void BaseBet_WithPositiveInfinity_ShouldAcceptPositiveInfinity()
        {
            // Arrange
            var baseBet = new BaseBet();

            // Act
            baseBet.Probability = double.PositiveInfinity;

            // Assert
            baseBet.Probability.Should().Be(double.PositiveInfinity);
        }

        [Fact]
        public void BaseBet_WithNegativeInfinity_ShouldAcceptNegativeInfinity()
        {
            // Arrange
            var baseBet = new BaseBet();

            // Act
            baseBet.Probability = double.NegativeInfinity;

            // Assert
            baseBet.Probability.Should().Be(double.NegativeInfinity);
        }

        [Theory]
        [InlineData(int.MaxValue)]
        [InlineData(int.MinValue)]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(999999)]
        public void BaseBet_WithExtremeParticipantIds_ShouldAcceptAllValues(int participantId)
        {
            // Arrange
            var baseBet = new BaseBet();

            // Act
            baseBet.ParticipantId = participantId;

            // Assert
            baseBet.ParticipantId.Should().Be(participantId);
        }

        [Theory]
        [InlineData(int.MaxValue)]
        [InlineData(int.MinValue)]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(999)]
        public void BaseBet_WithExtremeSuspensionReasons_ShouldAcceptAllValues(int suspensionReason)
        {
            // Arrange
            var baseBet = new BaseBet();

            // Act
            baseBet.SuspensionReason = suspensionReason;

            // Assert
            baseBet.SuspensionReason.Should().Be(suspensionReason);
        }

        [Fact]
        public void BaseBet_WithUnicodeCharacters_ShouldHandleUnicode()
        {
            // Arrange
            var baseBet = new BaseBet();
            var unicodeName = "测试球员 🏈 ⚽ 🏀";
            var unicodePlayerName = "Ñoël Müñõz García 🇪🇸";

            // Act
            baseBet.Name = unicodeName;
            baseBet.PlayerName = unicodePlayerName;

            // Assert
            baseBet.Name.Should().Be(unicodeName);
            baseBet.PlayerName.Should().Be(unicodePlayerName);
        }

        [Fact]
        public void BaseBet_WithVeryLongStrings_ShouldHandleLongContent()
        {
            // Arrange
            var baseBet = new BaseBet();
            var longString = new string('A', 10000);

            // Act
            baseBet.Name = longString;
            baseBet.Line = longString;
            baseBet.BaseLine = longString;
            baseBet.StartPrice = longString;
            baseBet.Price = longString;
            baseBet.PriceIN = longString;
            baseBet.PriceUS = longString;
            baseBet.PriceUK = longString;
            baseBet.PriceMA = longString;
            baseBet.PriceHK = longString;
            baseBet.PriceVolume = longString;
            baseBet.PlayerName = longString;

            // Assert
            baseBet.Name.Should().Be(longString);
            baseBet.Line.Should().Be(longString);
            baseBet.BaseLine.Should().Be(longString);
            baseBet.StartPrice.Should().Be(longString);
            baseBet.Price.Should().Be(longString);
            baseBet.PriceIN.Should().Be(longString);
            baseBet.PriceUS.Should().Be(longString);
            baseBet.PriceUK.Should().Be(longString);
            baseBet.PriceMA.Should().Be(longString);
            baseBet.PriceHK.Should().Be(longString);
            baseBet.PriceVolume.Should().Be(longString);
            baseBet.PlayerName.Should().Be(longString);
        }

        [Fact]
        public void BaseBet_PropertyAssignmentChaining_ShouldWorkCorrectly()
        {
            // Arrange & Act
            var baseBet = new BaseBet
            {
                Id = 123,
                Name = "Test Bet"
            };
            baseBet.Status = BetStatus.Open;
            baseBet.Settlement = SettlementType.Winner;

            // Assert
            baseBet.Id.Should().Be(123);
            baseBet.Name.Should().Be("Test Bet");
            baseBet.Status.Should().Be(BetStatus.Open);
            baseBet.Settlement.Should().Be(SettlementType.Winner);
        }

        [Fact]
        public void BaseBet_MultiplePropertyAssignments_ShouldRetainLatestValues()
        {
            // Arrange
            var baseBet = new BaseBet();

            // Act
            baseBet.Id = 1;
            baseBet.Id = 2;
            baseBet.Id = 3;

            baseBet.Name = "First";
            baseBet.Name = "Second";
            baseBet.Name = "Third";

            // Assert
            baseBet.Id.Should().Be(3);
            baseBet.Name.Should().Be("Third");
        }

        [Fact]
        public void BaseBet_WithAllSettlementTypes_ShouldAcceptAllTypes()
        {
            // Arrange
            var baseBet = new BaseBet();

            // Act & Assert
            foreach (SettlementType settlement in Enum.GetValues<SettlementType>())
            {
                baseBet.Settlement = settlement;
                baseBet.Settlement.Should().Be(settlement);
            }
        }

        [Fact]
        public void BaseBet_WithAllBetStatuses_ShouldAcceptAllStatuses()
        {
            // Arrange
            var baseBet = new BaseBet();

            // Act & Assert
            foreach (BetStatus status in Enum.GetValues<BetStatus>())
            {
                baseBet.Status = status;
                baseBet.Status.Should().Be(status);
            }
        }

        [Fact]
        public void BaseBet_ResetToDefaults_ShouldResetCorrectly()
        {
            // Arrange
            var baseBet = new BaseBet
            {
                Id = 123,
                Name = "Test",
                Status = BetStatus.Open,
                Settlement = SettlementType.Winner,
                Probability = 0.5,
                ParticipantId = 456,
                SuspensionReason = 789
            };

            // Act
            baseBet.Id = 0;
            baseBet.Name = null;
            baseBet.Status = BetStatus.NotSet;
            baseBet.Settlement = null;
            baseBet.Probability = 0.0;
            baseBet.ParticipantId = null;
            baseBet.SuspensionReason = null;

            // Assert
            baseBet.Id.Should().Be(0);
            baseBet.Name.Should().BeNull();
            baseBet.Status.Should().Be(BetStatus.NotSet);
            baseBet.Settlement.Should().BeNull();
            baseBet.Probability.Should().Be(0.0);
            baseBet.ParticipantId.Should().BeNull();
            baseBet.SuspensionReason.Should().BeNull();
        }

        [Fact]
        public void BaseBet_DateTimeKinds_ShouldPreserveKind()
        {
            // Arrange
            var baseBet = new BaseBet();
            var utcTime = DateTime.SpecifyKind(new DateTime(2023, 1, 1, 12, 0, 0), DateTimeKind.Utc);
            var localTime = DateTime.SpecifyKind(new DateTime(2023, 1, 1, 12, 0, 0), DateTimeKind.Local);
            var unspecifiedTime = DateTime.SpecifyKind(new DateTime(2023, 1, 1, 12, 0, 0), DateTimeKind.Unspecified);

            // Act & Assert
            baseBet.LastUpdate = utcTime;
            baseBet.LastUpdate.Kind.Should().Be(DateTimeKind.Utc);

            baseBet.LastUpdate = localTime;
            baseBet.LastUpdate.Kind.Should().Be(DateTimeKind.Local);

            baseBet.LastUpdate = unspecifiedTime;
            baseBet.LastUpdate.Kind.Should().Be(DateTimeKind.Unspecified);
        }

        [Fact]
        public void BaseBet_TypeChecking_ShouldBeCorrectType()
        {
            // Arrange
            var baseBet = new BaseBet();

            // Assert
            baseBet.Should().BeOfType<BaseBet>();
            baseBet.Should().BeAssignableTo<BaseBet>();
        }
    }
} 