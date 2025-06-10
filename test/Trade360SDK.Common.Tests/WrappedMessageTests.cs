using Trade360SDK.Common.Models;
using Xunit;

namespace Trade360SDK.Common.Tests
{
    public class WrappedMessageTests
    {
        [Fact]
        public void Can_Set_And_Get_Properties()
        {
            var header = new MessageHeader { Type = 2 };
            var wrapped = new WrappedMessage
            {
                Header = header,
                Body = "Test body"
            };

            Assert.Equal(header, wrapped.Header);
            Assert.Equal("Test body", wrapped.Body);
        }

        [Fact]
        public void Default_Values_Are_Null()
        {
            var wrapped = new WrappedMessage();
            Assert.Null(wrapped.Header);
            Assert.Null(wrapped.Body);
        }
    }
} 