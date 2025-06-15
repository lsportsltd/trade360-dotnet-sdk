using Trade360SDK.Common.Entities.Markets;
using Xunit;

namespace Trade360SDK.Common.Tests
{
    public class ProviderBetTests
    {
        [Fact]
        public void Properties_ShouldGetAndSetValues()
        {
            var bet = new ProviderBet { Id = 3, Name = "ProviderBetName" };
            Assert.Equal(3, bet.Id);
            Assert.Equal("ProviderBetName", bet.Name);
        }

        [Fact]
        public void Properties_ShouldAllowNulls()
        {
            var bet = new ProviderBet();
            Assert.Null(bet.Name);
        }
    }
} 