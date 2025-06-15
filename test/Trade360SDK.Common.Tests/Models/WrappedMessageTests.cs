using Trade360SDK.Common.Models;
using Xunit;

namespace Trade360SDK.Common.Tests
{
    public class WrappedMessageTests
    {
        [Fact]
        public void Properties_ShouldGetAndSetValues()
        {
            var header = new MessageHeader { CreationDate = "2024-01-01T00:00:00Z" };
            var wrapped = new WrappedMessage
            {
                Header = header,
                Body = "test body"
            };
            Assert.Equal(header, wrapped.Header);
            Assert.Equal("test body", wrapped.Body);
        }

        [Fact]
        public void Properties_ShouldAllowNulls()
        {
            var wrapped = new WrappedMessage();
            Assert.Null(wrapped.Header);
            Assert.Null(wrapped.Body);
        }
    }
} 