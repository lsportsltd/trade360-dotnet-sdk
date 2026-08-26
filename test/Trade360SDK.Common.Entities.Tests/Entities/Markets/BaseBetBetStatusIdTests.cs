using System.Text.Json;
using FluentAssertions;
using Trade360SDK.Common.Entities.Enums;
using Trade360SDK.Common.Entities.Markets;
using Xunit;

namespace Trade360SDK.Common.Tests
{
    public class BaseBetBetStatusIdTests
    {
        [Fact]
        public void Deserialize_ClosedMarketBet_ShouldKeepLegacyStatus()
        {
            const string json = """
                {
                  "Id": 1,
                  "Name": "Home",
                  "Status": 2,
                  "BetStatusId": 4
                }
                """;

            var bet = JsonSerializer.Deserialize<Bet>(json);

            bet.Should().NotBeNull();
            bet!.Status.Should().Be(BetStatus.Suspended);
            bet.BetStatusId.Should().Be(BetStatusId.Closed);
        }

        [Theory]
        [InlineData(1, BetStatus.Open, BetStatusId.Open)]
        [InlineData(2, BetStatus.Suspended, BetStatusId.Suspended)]
        [InlineData(3, BetStatus.Settled, BetStatusId.Settled)]
        public void Deserialize_OpenMarketBet_ShouldCopyStatusOntoBetStatusId(
            int statusValue, BetStatus expectedStatus, BetStatusId expectedBetStatusId)
        {
            var json = $$"""
                {
                  "Id": 1,
                  "Status": {{statusValue}},
                  "BetStatusId": {{statusValue}}
                }
                """;

            var bet = JsonSerializer.Deserialize<Bet>(json);

            bet.Should().NotBeNull();
            bet!.Status.Should().Be(expectedStatus);
            bet.BetStatusId.Should().Be(expectedBetStatusId);
        }

        [Fact]
        public void Deserialize_SettlementBet_ShouldKeepOutcomeAndSetSettled()
        {
            const string json = """
                {
                  "Id": 1,
                  "Name": "Home",
                  "Status": 3,
                  "BetStatusId": 3,
                  "Settlement": 2
                }
                """;

            var bet = JsonSerializer.Deserialize<Bet>(json);

            bet.Should().NotBeNull();
            bet!.Status.Should().Be(BetStatus.Settled);
            bet.BetStatusId.Should().Be(BetStatusId.Settled);
            bet.Settlement.Should().Be(SettlementType.Winner);
        }

        [Fact]
        public void Deserialize_LegacyPayloadWithoutBetStatusId_ShouldLeavePropertyNull()
        {
            const string json = """
                {
                  "Id": 1,
                  "Status": 1,
                  "Settlement": 1
                }
                """;

            var bet = JsonSerializer.Deserialize<Bet>(json);

            bet.Should().NotBeNull();
            bet!.Status.Should().Be(BetStatus.Open);
            bet.BetStatusId.Should().BeNull();
            bet.Settlement.Should().Be(SettlementType.Loser);
        }
    }
}
