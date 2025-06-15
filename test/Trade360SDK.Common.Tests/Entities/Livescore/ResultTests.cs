using Trade360SDK.Common.Entities.Livescore;
using Xunit;

namespace Trade360SDK.Common.Tests
{
    public class ResultTests
    {
        [Fact]
        public void Properties_ShouldGetAndSetValues()
        {
            var result = new Result
            {
                Position = "Forward",
                Value = "1"
            };
            Assert.Equal("Forward", result.Position);
            Assert.Equal("1", result.Value);
        }

        [Fact]
        public void Properties_ShouldAllowNulls()
        {
            var result = new Result();
            Assert.Null(result.Position);
            Assert.Null(result.Value);
        }
    }
} 