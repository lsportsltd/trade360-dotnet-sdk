using Trade360SDK.Common.Entities.Shared;
using Xunit;

namespace Trade360SDK.Common.Tests
{
    public class NameValuePairTests
    {
        [Fact]
        public void Properties_ShouldGetAndSetValues()
        {
            var pair = new NameValuePair
            {
                Name = "TestName",
                Value = "TestValue"
            };
            Assert.Equal("TestName", pair.Name);
            Assert.Equal("TestValue", pair.Value);
        }

        [Fact]
        public void Properties_ShouldAllowNulls()
        {
            var pair = new NameValuePair();
            Assert.Null(pair.Name);
            Assert.Null(pair.Value);
        }
    }
} 