using System;
using Trade360SDK.Common.Models;
using Xunit;

namespace Trade360SDK.Common.Tests
{
    public class MessageHeaderTests
    {
        [Fact]
        public void Properties_ShouldGetAndSetValues()
        {
            var header = new MessageHeader
            {
                CreationDate = "2024-01-01T00:00:00Z",
                Type = 1,
                MsgSeq = 42,
                MsgGuid = "guid-123",
                ServerTimestamp = 1234567890,
                MessageBrokerTimestamp = DateTime.UtcNow,
                MessageTimestamp = DateTime.UtcNow.AddMinutes(-1)
            };

            Assert.Equal("2024-01-01T00:00:00Z", header.CreationDate);
            Assert.Equal(1, header.Type);
            Assert.Equal(42, header.MsgSeq);
            Assert.Equal("guid-123", header.MsgGuid);
            Assert.Equal(1234567890, header.ServerTimestamp);
            Assert.NotNull(header.MessageBrokerTimestamp);
            Assert.NotNull(header.MessageTimestamp);
        }

        [Fact]
        public void Properties_ShouldAllowNulls()
        {
            var header = new MessageHeader();
            Assert.Null(header.CreationDate);
            Assert.Null(header.MsgGuid);
            Assert.Null(header.ServerTimestamp);
            Assert.Null(header.MessageBrokerTimestamp);
            Assert.Null(header.MessageTimestamp);
        }
    }
} 