using System;
using FluentAssertions;
using Xunit;
using Trade360SDK.Common.Models;

namespace Trade360SDK.Common.Entities.Tests.Models
{
    public class MessageHeaderTests
    {
        [Fact]
        public void MessageHeader_DefaultConstructor_ShouldCreateInstanceWithNullProperties()
        {
            // Act
            var header = new MessageHeader();

            // Assert
            header.Should().NotBeNull();
            header.CreationDate.Should().BeNull();
            header.Type.Should().Be(0);
            header.MsgSeq.Should().BeNull();
            header.MsgGuid.Should().BeNull();
            header.ServerTimestamp.Should().BeNull();
            header.MessageBrokerTimestamp.Should().BeNull();
            header.MessageTimestamp.Should().BeNull();
        }

        [Fact]
        public void MessageHeader_SetCreationDate_ShouldSetValue()
        {
            // Arrange
            var header = new MessageHeader();
            var creationDate = "2023-12-01T10:30:00Z";

            // Act
            header.CreationDate = creationDate;

            // Assert
            header.CreationDate.Should().Be(creationDate);
        }

        [Fact]
        public void MessageHeader_SetType_ShouldSetValue()
        {
            // Arrange
            var header = new MessageHeader();
            var type = 42;

            // Act
            header.Type = type;

            // Assert
            header.Type.Should().Be(type);
        }

        [Fact]
        public void MessageHeader_SetMsgSeq_ShouldSetValue()
        {
            // Arrange
            var header = new MessageHeader();
            var msgSeq = 123;

            // Act
            header.MsgSeq = msgSeq;

            // Assert
            header.MsgSeq.Should().Be(msgSeq);
        }

        [Fact]
        public void MessageHeader_SetMsgGuid_ShouldSetValue()
        {
            // Arrange
            var header = new MessageHeader();
            var msgGuid = "550e8400-e29b-41d4-a716-446655440000";

            // Act
            header.MsgGuid = msgGuid;

            // Assert
            header.MsgGuid.Should().Be(msgGuid);
        }

        [Fact]
        public void MessageHeader_SetServerTimestamp_ShouldSetValue()
        {
            // Arrange
            var header = new MessageHeader();
            var timestamp = 1638360600000L; // Unix timestamp

            // Act
            header.ServerTimestamp = timestamp;

            // Assert
            header.ServerTimestamp.Should().Be(timestamp);
        }

        [Fact]
        public void MessageHeader_SetMessageBrokerTimestamp_ShouldSetValue()
        {
            // Arrange
            var header = new MessageHeader();
            var timestamp = new DateTime(2023, 12, 1, 10, 30, 0, DateTimeKind.Utc);

            // Act
            header.MessageBrokerTimestamp = timestamp;

            // Assert
            header.MessageBrokerTimestamp.Should().Be(timestamp);
        }

        [Fact]
        public void MessageHeader_SetMessageTimestamp_ShouldSetValue()
        {
            // Arrange
            var header = new MessageHeader();
            var timestamp = new DateTime(2023, 12, 1, 10, 30, 0, DateTimeKind.Utc);

            // Act
            header.MessageTimestamp = timestamp;

            // Assert
            header.MessageTimestamp.Should().Be(timestamp);
        }

        [Fact]
        public void MessageHeader_SetAllProperties_ShouldSetAllValues()
        {
            // Arrange
            var header = new MessageHeader();
            var creationDate = "2023-12-01T10:30:00Z";
            var type = 42;
            var msgSeq = 123;
            var msgGuid = "550e8400-e29b-41d4-a716-446655440000";
            var serverTimestamp = 1638360600000L;
            var messageBrokerTimestamp = new DateTime(2023, 12, 1, 10, 30, 0, DateTimeKind.Utc);
            var messageTimestamp = new DateTime(2023, 12, 1, 10, 35, 0, DateTimeKind.Utc);

            // Act
            header.CreationDate = creationDate;
            header.Type = type;
            header.MsgSeq = msgSeq;
            header.MsgGuid = msgGuid;
            header.ServerTimestamp = serverTimestamp;
            header.MessageBrokerTimestamp = messageBrokerTimestamp;
            header.MessageTimestamp = messageTimestamp;

            // Assert
            header.CreationDate.Should().Be(creationDate);
            header.Type.Should().Be(type);
            header.MsgSeq.Should().Be(msgSeq);
            header.MsgGuid.Should().Be(msgGuid);
            header.ServerTimestamp.Should().Be(serverTimestamp);
            header.MessageBrokerTimestamp.Should().Be(messageBrokerTimestamp);
            header.MessageTimestamp.Should().Be(messageTimestamp);
        }

        [Fact]
        public void MessageHeader_SetNegativeType_ShouldSetValue()
        {
            // Arrange
            var header = new MessageHeader();
            var type = -1;

            // Act
            header.Type = type;

            // Assert
            header.Type.Should().Be(type);
        }

        [Fact]
        public void MessageHeader_SetNegativeMsgSeq_ShouldSetValue()
        {
            // Arrange
            var header = new MessageHeader();
            var msgSeq = -1;

            // Act
            header.MsgSeq = msgSeq;

            // Assert
            header.MsgSeq.Should().Be(msgSeq);
        }

        [Fact]
        public void MessageHeader_SetNegativeServerTimestamp_ShouldSetValue()
        {
            // Arrange
            var header = new MessageHeader();
            var timestamp = -1L;

            // Act
            header.ServerTimestamp = timestamp;

            // Assert
            header.ServerTimestamp.Should().Be(timestamp);
        }

        [Fact]
        public void MessageHeader_SetEmptyStringValues_ShouldSetValues()
        {
            // Arrange
            var header = new MessageHeader();

            // Act
            header.CreationDate = string.Empty;
            header.MsgGuid = string.Empty;

            // Assert
            header.CreationDate.Should().Be(string.Empty);
            header.MsgGuid.Should().Be(string.Empty);
        }

        [Fact]
        public void MessageHeader_SetWhitespaceStringValues_ShouldSetValues()
        {
            // Arrange
            var header = new MessageHeader();

            // Act
            header.CreationDate = "   ";
            header.MsgGuid = "   ";

            // Assert
            header.CreationDate.Should().Be("   ");
            header.MsgGuid.Should().Be("   ");
        }

        [Fact]
        public void MessageHeader_SetMinMaxValues_ShouldSetValues()
        {
            // Arrange
            var header = new MessageHeader();

            // Act
            header.Type = int.MaxValue;
            header.MsgSeq = int.MaxValue;
            header.ServerTimestamp = long.MaxValue;

            // Assert
            header.Type.Should().Be(int.MaxValue);
            header.MsgSeq.Should().Be(int.MaxValue);
            header.ServerTimestamp.Should().Be(long.MaxValue);
        }

        [Fact]
        public void MessageHeader_SetMinValues_ShouldSetValues()
        {
            // Arrange
            var header = new MessageHeader();

            // Act
            header.Type = int.MinValue;
            header.MsgSeq = int.MinValue;
            header.ServerTimestamp = long.MinValue;

            // Assert
            header.Type.Should().Be(int.MinValue);
            header.MsgSeq.Should().Be(int.MinValue);
            header.ServerTimestamp.Should().Be(long.MinValue);
        }

        [Fact]
        public void MessageHeader_SetDateTimeMinMaxValues_ShouldSetValues()
        {
            // Arrange
            var header = new MessageHeader();

            // Act
            header.MessageBrokerTimestamp = DateTime.MinValue;
            header.MessageTimestamp = DateTime.MaxValue;

            // Assert
            header.MessageBrokerTimestamp.Should().Be(DateTime.MinValue);
            header.MessageTimestamp.Should().Be(DateTime.MaxValue);
        }

        [Theory]
        [InlineData("2023-12-01T10:30:00Z")]
        [InlineData("01/12/2023 10:30:00")]
        [InlineData("Dec 1, 2023")]
        [InlineData("invalid-date")]
        public void MessageHeader_SetVariousCreationDateFormats_ShouldSetValue(string creationDate)
        {
            // Arrange
            var header = new MessageHeader();

            // Act
            header.CreationDate = creationDate;

            // Assert
            header.CreationDate.Should().Be(creationDate);
        }

        [Theory]
        [InlineData("550e8400-e29b-41d4-a716-446655440000")]
        [InlineData("not-a-guid")]
        [InlineData("12345")]
        [InlineData("")]
        public void MessageHeader_SetVariousGuidFormats_ShouldSetValue(string msgGuid)
        {
            // Arrange
            var header = new MessageHeader();

            // Act
            header.MsgGuid = msgGuid;

            // Assert
            header.MsgGuid.Should().Be(msgGuid);
        }

        [Fact]
        public void MessageHeader_PropertySettersGetters_ShouldWorkCorrectly()
        {
            // Arrange
            var header = new MessageHeader();

            // Act & Assert - Test that we can set and get each property multiple times
            header.CreationDate = "test1";
            header.CreationDate.Should().Be("test1");
            header.CreationDate = "test2";
            header.CreationDate.Should().Be("test2");

            header.Type = 1;
            header.Type.Should().Be(1);
            header.Type = 2;
            header.Type.Should().Be(2);

            header.MsgSeq = 100;
            header.MsgSeq.Should().Be(100);
            header.MsgSeq = null;
            header.MsgSeq.Should().BeNull();

            header.ServerTimestamp = 1000L;
            header.ServerTimestamp.Should().Be(1000L);
            header.ServerTimestamp = null;
            header.ServerTimestamp.Should().BeNull();
        }
    }
} 