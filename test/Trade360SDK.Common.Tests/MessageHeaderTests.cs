using System;
using Trade360SDK.Common.Models;
using Xunit;

namespace Trade360SDK.Common.Tests
{
    public class MessageHeaderTests
    {
        [Fact]
        public void Can_Set_And_Get_Properties()
        {
            var now = DateTime.UtcNow;
            var header = new MessageHeader
            {
                CreationDate = "2024-01-01T00:00:00Z",
                Type = 1,
                MsgSeq = 42,
                MsgGuid = "abc-123",
                ServerTimestamp = 1234567890,
                MessageBrokerTimestamp = now,
                MessageTimestamp = now.AddMinutes(1)
            };

            Assert.Equal("2024-01-01T00:00:00Z", header.CreationDate);
            Assert.Equal(1, header.Type);
            Assert.Equal(42, header.MsgSeq);
            Assert.Equal("abc-123", header.MsgGuid);
            Assert.Equal(1234567890, header.ServerTimestamp);
            Assert.Equal(now, header.MessageBrokerTimestamp);
            Assert.Equal(now.AddMinutes(1), header.MessageTimestamp);
        }

        [Fact]
        public void Default_Values_Are_Null_Or_Zero()
        {
            var header = new MessageHeader();
            Assert.Null(header.CreationDate);
            Assert.Equal(0, header.Type);
            Assert.Null(header.MsgSeq);
            Assert.Null(header.MsgGuid);
            Assert.Null(header.ServerTimestamp);
            Assert.Null(header.MessageBrokerTimestamp);
            Assert.Null(header.MessageTimestamp);
        }
    }
} 