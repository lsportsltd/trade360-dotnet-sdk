using FluentAssertions;
using Trade360SDK.Common.Entities.Enums;
using Trade360SDK.Common.Entities.Markets;
using Xunit;

namespace Trade360SDK.Common.Entities.Tests.Entities.Markets
{
    public class BetComprehensiveTests
    {
        [Fact]
        public void Bet_ShouldInheritFromBaseBet()
        {
            // Arrange & Act
            var bet = new Bet();

            // Assert
            bet.Should().BeAssignableTo<BaseBet>();
        }

        [Fact]
        public void ProviderBetId_WhenSet_ShouldReturnExpectedValue()
        {
            // Arrange
            var bet = new Bet();
            const string expectedProviderBetId = "PROVIDER-123";

            // Act
            bet.ProviderBetId = expectedProviderBetId;

            // Assert
            bet.ProviderBetId.Should().Be(expectedProviderBetId);
        }

        [Fact]
        public void ProviderBetId_WhenNotSet_ShouldBeNull()
        {
            // Arrange & Act
            var bet = new Bet();

            // Assert
            bet.ProviderBetId.Should().BeNull();
        }

        [Fact]
        public void Bet_ShouldHaveAllBaseBetProperties()
        {
            // Arrange
            var bet = new Bet
            {
                Id = 12345L,
                Name = "Test Bet",
                Line = "1.5",
                BaseLine = "1.0",
                Status = BetStatus.Open,
                StartPrice = "2.50",
                Price = "2.75",
                PriceIN = "1.75",
                PriceUS = "+175",
                PriceUK = "7/4",
                PriceMA = "1.75",
                PriceHK = "0.75",
                PriceVolume = "10000",
                Settlement = SettlementType.Winner,
                SuspensionReason = 1,
                LastUpdate = DateTime.UtcNow,
                Probability = 0.36,
                ParticipantId = 999,
                PlayerName = "John Doe",
                ProviderBetId = "PROV-456"
            };

            // Act & Assert
            bet.Id.Should().Be(12345L);
            bet.Name.Should().Be("Test Bet");
            bet.Line.Should().Be("1.5");
            bet.BaseLine.Should().Be("1.0");
            bet.Status.Should().Be(BetStatus.Open);
            bet.StartPrice.Should().Be("2.50");
            bet.Price.Should().Be("2.75");
            bet.PriceIN.Should().Be("1.75");
            bet.PriceUS.Should().Be("+175");
            bet.PriceUK.Should().Be("7/4");
            bet.PriceMA.Should().Be("1.75");
            bet.PriceHK.Should().Be("0.75");
            bet.PriceVolume.Should().Be("10000");
            bet.Settlement.Should().Be(SettlementType.Winner);
            bet.SuspensionReason.Should().Be(1);
            bet.LastUpdate.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
            bet.Probability.Should().Be(0.36);
            bet.ParticipantId.Should().Be(999);
            bet.PlayerName.Should().Be("John Doe");
            bet.ProviderBetId.Should().Be("PROV-456");
        }

        [Fact]
        public void Bet_WithNullValues_ShouldHandleGracefully()
        {
            // Arrange & Act
            var bet = new Bet
            {
                Id = 0,
                Name = null,
                Line = null,
                BaseLine = null,
                Status = BetStatus.Suspended,
                StartPrice = null,
                Price = null,
                PriceIN = null,
                PriceUS = null,
                PriceUK = null,
                PriceMA = null,
                PriceHK = null,
                PriceVolume = null,
                Settlement = null,
                SuspensionReason = null,
                LastUpdate = default,
                Probability = 0.0,
                ParticipantId = null,
                PlayerName = null,
                ProviderBetId = null
            };

            // Assert
            bet.Id.Should().Be(0);
            bet.Name.Should().BeNull();
            bet.Line.Should().BeNull();
            bet.BaseLine.Should().BeNull();
            bet.Status.Should().Be(BetStatus.Suspended);
            bet.StartPrice.Should().BeNull();
            bet.Price.Should().BeNull();
            bet.PriceIN.Should().BeNull();
            bet.PriceUS.Should().BeNull();
            bet.PriceUK.Should().BeNull();
            bet.PriceMA.Should().BeNull();
            bet.PriceHK.Should().BeNull();
            bet.PriceVolume.Should().BeNull();
            bet.Settlement.Should().BeNull();
            bet.SuspensionReason.Should().BeNull();
            bet.LastUpdate.Should().Be(default);
            bet.Probability.Should().Be(0.0);
            bet.ParticipantId.Should().BeNull();
            bet.PlayerName.Should().BeNull();
            bet.ProviderBetId.Should().BeNull();
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("PROVIDER-001")]
        [InlineData("very-long-provider-bet-id-with-special-characters-123_@#$")]
        public void ProviderBetId_WithVariousValues_ShouldStoreCorrectly(string providerBetId)
        {
            // Arrange
            var bet = new Bet();

            // Act
            bet.ProviderBetId = providerBetId;

            // Assert
            bet.ProviderBetId.Should().Be(providerBetId);
        }

        [Fact]
        public void Bet_DefaultConstructor_ShouldInitializeWithDefaults()
        {
            // Arrange & Act
            var bet = new Bet();

            // Assert
            bet.Id.Should().Be(0);
            bet.Name.Should().BeNull();
            bet.Line.Should().BeNull();
            bet.BaseLine.Should().BeNull();
            bet.Status.Should().Be(default(BetStatus));
            bet.StartPrice.Should().BeNull();
            bet.Price.Should().BeNull();
            bet.PriceIN.Should().BeNull();
            bet.PriceUS.Should().BeNull();
            bet.PriceUK.Should().BeNull();
            bet.PriceMA.Should().BeNull();
            bet.PriceHK.Should().BeNull();
            bet.PriceVolume.Should().BeNull();
            bet.Settlement.Should().BeNull();
            bet.SuspensionReason.Should().BeNull();
            bet.LastUpdate.Should().Be(default);
            bet.Probability.Should().Be(0.0);
            bet.ParticipantId.Should().BeNull();
            bet.PlayerName.Should().BeNull();
            bet.ProviderBetId.Should().BeNull();
        }
    }
} 