using Trade360SDK.Common.Entities.Markets;
using Xunit;

namespace Trade360SDK.Common.Tests
{
    public class ProviderTests
    {
        [Fact]
        public void Properties_ShouldGetAndSetValues()
        {
            var provider = new Provider { Id = 11, Name = "ProviderName" };
            Assert.Equal(11, provider.Id);
            Assert.Equal("ProviderName", provider.Name);
        }

        [Fact]
        public void Properties_ShouldAllowNulls()
        {
            var provider = new Provider();
            Assert.Null(provider.Name);
        }
    }
} 