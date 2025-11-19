using System;
using Trade360SDK.Common.Entities.Markets;
using Trade360SDK.Common.Entities.Enums;
using Xunit;

namespace Trade360SDK.Common.Tests
{
    public class BaseBetTests
    {
        [Fact]
        public void Properties_ShouldGetAndSetValues()
        {
            var bet = new BaseBet
            {
                Id = 1,
                Name = "BetName",
                Line = "1.5",
                BaseLine = "2.0",
                Status = BetStatus.Open,
                StartPrice = "2.5",
                Price = "2.0",
                PriceIN = "1.9",
                PriceUS = "-110",
                PriceUK = "10/11",
                PriceMA = "1.91",
                PriceHK = "0.91",
                PriceVolume = "1000",
                Settlement = SettlementType.Winner,
                SuspensionReason = 5,
                LastUpdate = DateTime.UtcNow,
                Probability = 0.75,
                ParticipantId = 42,
                PlayerId = "12345",
                PlayerName = "Player",
                Order = 1
            };
            Assert.Equal(1, bet.Id);
            Assert.Equal("BetName", bet.Name);
            Assert.Equal("1.5", bet.Line);
            Assert.Equal("2.0", bet.BaseLine);
            Assert.Equal(BetStatus.Open, bet.Status);
            Assert.Equal("2.5", bet.StartPrice);
            Assert.Equal("2.0", bet.Price);
            Assert.Equal("1.9", bet.PriceIN);
            Assert.Equal("-110", bet.PriceUS);
            Assert.Equal("10/11", bet.PriceUK);
            Assert.Equal("1.91", bet.PriceMA);
            Assert.Equal("0.91", bet.PriceHK);
            Assert.Equal("1000", bet.PriceVolume);
            Assert.Equal(SettlementType.Winner, bet.Settlement);
            Assert.Equal(5, bet.SuspensionReason);
            Assert.NotEqual(default, bet.LastUpdate);
            Assert.Equal(0.75, bet.Probability);
            Assert.Equal(42, bet.ParticipantId);
            Assert.Equal("12345", bet.PlayerId);
            Assert.Equal("Player", bet.PlayerName);
            Assert.Equal(1, bet.Order);
        }

        [Fact]
        public void Properties_ShouldAllowNulls()
        {
            var bet = new BaseBet();
            Assert.Null(bet.Name);
            Assert.Null(bet.Line);
            Assert.Null(bet.BaseLine);
            Assert.Null(bet.StartPrice);
            Assert.Null(bet.Price);
            Assert.Null(bet.PriceIN);
            Assert.Null(bet.PriceUS);
            Assert.Null(bet.PriceUK);
            Assert.Null(bet.PriceMA);
            Assert.Null(bet.PriceHK);
            Assert.Null(bet.PriceVolume);
            Assert.Null(bet.Settlement);
            Assert.Null(bet.SuspensionReason);
            Assert.Null(bet.ParticipantId);
            Assert.Null(bet.PlayerId);
            Assert.Null(bet.PlayerName);
            Assert.Null(bet.Order);
        }
    }
} 