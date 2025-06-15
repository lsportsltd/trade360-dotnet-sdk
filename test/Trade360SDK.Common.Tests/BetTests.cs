using Trade360SDK.Common.Entities.Markets;
using Xunit;

namespace Trade360SDK.Common.Tests
{
    public class BetTests
    {
        [Fact]
        public void Properties_ShouldGetAndSetValues()
        {
            var bet = new Bet
            {
                Id = 2,
                Name = "Bet2",
                ProviderBetId = "PBID123"
            };
            Assert.Equal(2, bet.Id);
            Assert.Equal("Bet2", bet.Name);
            Assert.Equal("PBID123", bet.ProviderBetId);
        }

        [Fact]
        public void ProviderBetId_ShouldAllowNull()
        {
            var bet = new Bet();
            Assert.Null(bet.ProviderBetId);
        }
    }
} 