using Trade360SDK.Common.Attributes;
using Xunit;

namespace Trade360SDK.Common.Tests
{
    public class Trade360EntityAttributeTests
    {
        [Fact]
        public void EntityKey_Is_Set_Correctly()
        {
            var attr = new Trade360EntityAttribute(42);
            Assert.Equal(42, attr.EntityKey);
        }
    }
} 