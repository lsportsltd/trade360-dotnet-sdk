using System;
using FluentAssertions;
using Trade360SDK.Common.Models;
using Xunit;

namespace Trade360SDK.Common.Entities.Tests.Models
{
    public class WrappedMessageTests
    {
        [Fact]
        public void WrappedMessage_DefaultConstructor_ShouldInitializeWithNullValues()
        {
            // Act
            var wrappedMessage = new WrappedMessage();

            // Assert
            wrappedMessage.Should().NotBeNull();
            wrappedMessage.Header.Should().BeNull();
            wrappedMessage.Body.Should().BeNull();
        }

        [Fact]
        public void WrappedMessage_HeaderProperty_ShouldBeSettableAndGettable()
        {
            // Arrange
            var wrappedMessage = new WrappedMessage();
            var header = new MessageHeader
            {
                Type = 1,
                MsgSeq = 12345,
                CreationDate = "2024-01-01T10:30:00Z"
            };

            // Act
            wrappedMessage.Header = header;

            // Assert
            wrappedMessage.Header.Should().NotBeNull();
            wrappedMessage.Header.Should().BeSameAs(header);
            wrappedMessage.Header.Type.Should().Be(1);
            wrappedMessage.Header.MsgSeq.Should().Be(12345);
            wrappedMessage.Header.CreationDate.Should().Be("2024-01-01T10:30:00Z");
        }

        [Fact]
        public void WrappedMessage_BodyProperty_ShouldBeSettableAndGettable()
        {
            // Arrange
            var wrappedMessage = new WrappedMessage();
            var body = "{\"Events\": [{\"Id\": 12345, \"Status\": 1}]}";

            // Act
            wrappedMessage.Body = body;

            // Assert
            wrappedMessage.Body.Should().NotBeNull();
            wrappedMessage.Body.Should().Be(body);
            wrappedMessage.Body.Should().Contain("Events");
            wrappedMessage.Body.Should().Contain("12345");
        }

        [Fact]
        public void WrappedMessage_WithCompleteData_ShouldMaintainAllProperties()
        {
            // Arrange
            var header = new MessageHeader
            {
                Type = 3,
                MsgSeq = 99999,
                CreationDate = "2024-12-25T15:30:45.123Z",
                MsgGuid = "550e8400-e29b-41d4-a716-446655440000",
                ServerTimestamp = 1704105000000,
                MessageBrokerTimestamp = DateTime.UtcNow,
                MessageTimestamp = DateTime.UtcNow.AddSeconds(1)
            };
            var body = "{\"Events\": [{\"Id\": 12345, \"Status\": 1, \"Markets\": []}]}";

            // Act
            var wrappedMessage = new WrappedMessage
            {
                Header = header,
                Body = body
            };

            // Assert
            wrappedMessage.Should().NotBeNull();
            wrappedMessage.Header.Should().NotBeNull();
            wrappedMessage.Header.Should().BeSameAs(header);
            wrappedMessage.Body.Should().Be(body);
            
            // Verify all header properties are preserved
            wrappedMessage.Header.Type.Should().Be(3);
            wrappedMessage.Header.MsgSeq.Should().Be(99999);
            wrappedMessage.Header.CreationDate.Should().Be("2024-12-25T15:30:45.123Z");
            wrappedMessage.Header.MsgGuid.Should().Be("550e8400-e29b-41d4-a716-446655440000");
            wrappedMessage.Header.ServerTimestamp.Should().Be(1704105000000);
            wrappedMessage.Header.MessageBrokerTimestamp.Should().NotBeNull();
            wrappedMessage.Header.MessageTimestamp.Should().NotBeNull();
        }

        [Fact]
        public void WrappedMessage_HeaderCanBeSetToNull_ShouldAllowNullHeader()
        {
            // Arrange
            var wrappedMessage = new WrappedMessage
            {
                Header = new MessageHeader { Type = 1 }
            };

            // Act
            wrappedMessage.Header = null;

            // Assert
            wrappedMessage.Header.Should().BeNull();
        }

        [Fact]
        public void WrappedMessage_BodyCanBeSetToNull_ShouldAllowNullBody()
        {
            // Arrange
            var wrappedMessage = new WrappedMessage
            {
                Body = "test body"
            };

            // Act
            wrappedMessage.Body = null;

            // Assert
            wrappedMessage.Body.Should().BeNull();
        }

        [Fact]
        public void WrappedMessage_WithEmptyStringBody_ShouldPreserveEmptyString()
        {
            // Arrange
            var wrappedMessage = new WrappedMessage();

            // Act
            wrappedMessage.Body = "";

            // Assert
            wrappedMessage.Body.Should().NotBeNull();
            wrappedMessage.Body.Should().BeEmpty();
        }

        [Fact]
        public void WrappedMessage_WithLargeBody_ShouldHandleLargeContent()
        {
            // Arrange
            var largeBody = new string('A', 100000); // 100KB string
            var wrappedMessage = new WrappedMessage();

            // Act
            wrappedMessage.Body = largeBody;

            // Assert
            wrappedMessage.Body.Should().NotBeNull();
            wrappedMessage.Body.Should().HaveLength(100000);
            wrappedMessage.Body.Should().Be(largeBody);
        }

        [Fact]
        public void WrappedMessage_PropertyChanges_ShouldNotAffectEachOther()
        {
            // Arrange
            var wrappedMessage = new WrappedMessage();
            var originalHeader = new MessageHeader { Type = 1 };
            var originalBody = "original body";

            // Act
            wrappedMessage.Header = originalHeader;
            wrappedMessage.Body = originalBody;
            
            var newHeader = new MessageHeader { Type = 2 };
            var newBody = "new body";
            
            wrappedMessage.Header = newHeader;
            wrappedMessage.Body = newBody;

            // Assert
            wrappedMessage.Header.Should().BeSameAs(newHeader);
            wrappedMessage.Header.Type.Should().Be(2);
            wrappedMessage.Body.Should().Be(newBody);
            
            // Original objects should remain unchanged
            originalHeader.Type.Should().Be(1);
        }
    }

    public class MessageHeaderTests
    {
        [Fact]
        public void MessageHeader_DefaultConstructor_ShouldInitializeWithDefaultValues()
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
        public void MessageHeader_CreationDateProperty_ShouldBeSettableAndGettable()
        {
            // Arrange
            var header = new MessageHeader();
            var creationDate = "2024-01-01T10:30:00Z";

            // Act
            header.CreationDate = creationDate;

            // Assert
            header.CreationDate.Should().Be(creationDate);
        }

        [Fact]
        public void MessageHeader_TypeProperty_ShouldBeSettableAndGettable()
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
        public void MessageHeader_MsgSeqProperty_ShouldBeSettableAndGettable()
        {
            // Arrange
            var header = new MessageHeader();
            var msgSeq = 12345;

            // Act
            header.MsgSeq = msgSeq;

            // Assert
            header.MsgSeq.Should().Be(msgSeq);
        }

        [Fact]
        public void MessageHeader_MsgGuidProperty_ShouldBeSettableAndGettable()
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
        public void MessageHeader_ServerTimestampProperty_ShouldBeSettableAndGettable()
        {
            // Arrange
            var header = new MessageHeader();
            var serverTimestamp = 1704105000000L;

            // Act
            header.ServerTimestamp = serverTimestamp;

            // Assert
            header.ServerTimestamp.Should().Be(serverTimestamp);
        }

        [Fact]
        public void MessageHeader_MessageBrokerTimestampProperty_ShouldBeSettableAndGettable()
        {
            // Arrange
            var header = new MessageHeader();
            var timestamp = DateTime.UtcNow;

            // Act
            header.MessageBrokerTimestamp = timestamp;

            // Assert
            header.MessageBrokerTimestamp.Should().Be(timestamp);
        }

        [Fact]
        public void MessageHeader_MessageTimestampProperty_ShouldBeSettableAndGettable()
        {
            // Arrange
            var header = new MessageHeader();
            var timestamp = DateTime.UtcNow;

            // Act
            header.MessageTimestamp = timestamp;

            // Assert
            header.MessageTimestamp.Should().Be(timestamp);
        }

        [Fact]
        public void MessageHeader_WithAllProperties_ShouldMaintainAllValues()
        {
            // Arrange
            var creationDate = "2024-12-25T15:30:45.123Z";
            var type = 3;
            var msgSeq = 99999;
            var msgGuid = "550e8400-e29b-41d4-a716-446655440000";
            var serverTimestamp = 1704105000000L;
            var messageBrokerTimestamp = DateTime.UtcNow;
            var messageTimestamp = DateTime.UtcNow.AddSeconds(1);

            // Act
            var header = new MessageHeader
            {
                CreationDate = creationDate,
                Type = type,
                MsgSeq = msgSeq,
                MsgGuid = msgGuid,
                ServerTimestamp = serverTimestamp,
                MessageBrokerTimestamp = messageBrokerTimestamp,
                MessageTimestamp = messageTimestamp
            };

            // Assert
            header.Should().NotBeNull();
            header.CreationDate.Should().Be(creationDate);
            header.Type.Should().Be(type);
            header.MsgSeq.Should().Be(msgSeq);
            header.MsgGuid.Should().Be(msgGuid);
            header.ServerTimestamp.Should().Be(serverTimestamp);
            header.MessageBrokerTimestamp.Should().Be(messageBrokerTimestamp);
            header.MessageTimestamp.Should().Be(messageTimestamp);
        }

        [Fact]
        public void MessageHeader_NullableProperties_ShouldAcceptNullValues()
        {
            // Arrange
            var header = new MessageHeader
            {
                CreationDate = "test",
                MsgSeq = 123,
                MsgGuid = "test-guid",
                ServerTimestamp = 123456789,
                MessageBrokerTimestamp = DateTime.UtcNow,
                MessageTimestamp = DateTime.UtcNow
            };

            // Act
            header.CreationDate = null;
            header.MsgSeq = null;
            header.MsgGuid = null;
            header.ServerTimestamp = null;
            header.MessageBrokerTimestamp = null;
            header.MessageTimestamp = null;

            // Assert
            header.CreationDate.Should().BeNull();
            header.MsgSeq.Should().BeNull();
            header.MsgGuid.Should().BeNull();
            header.ServerTimestamp.Should().BeNull();
            header.MessageBrokerTimestamp.Should().BeNull();
            header.MessageTimestamp.Should().BeNull();
        }

        [Fact]
        public void MessageHeader_TypeProperty_ShouldHandleBoundaryValues()
        {
            // Arrange
            var header = new MessageHeader();

            // Act & Assert
            header.Type = int.MinValue;
            header.Type.Should().Be(int.MinValue);

            header.Type = int.MaxValue;
            header.Type.Should().Be(int.MaxValue);

            header.Type = 0;
            header.Type.Should().Be(0);
        }

        [Fact]
        public void MessageHeader_MsgSeqProperty_ShouldHandleBoundaryValues()
        {
            // Arrange
            var header = new MessageHeader();

            // Act & Assert
            header.MsgSeq = int.MinValue;
            header.MsgSeq.Should().Be(int.MinValue);

            header.MsgSeq = int.MaxValue;
            header.MsgSeq.Should().Be(int.MaxValue);

            header.MsgSeq = 0;
            header.MsgSeq.Should().Be(0);
        }

        [Fact]
        public void MessageHeader_ServerTimestampProperty_ShouldHandleBoundaryValues()
        {
            // Arrange
            var header = new MessageHeader();

            // Act & Assert
            header.ServerTimestamp = long.MinValue;
            header.ServerTimestamp.Should().Be(long.MinValue);

            header.ServerTimestamp = long.MaxValue;
            header.ServerTimestamp.Should().Be(long.MaxValue);

            header.ServerTimestamp = 0L;
            header.ServerTimestamp.Should().Be(0L);
        }

        [Fact]
        public void MessageHeader_StringProperties_ShouldHandleEmptyStrings()
        {
            // Arrange
            var header = new MessageHeader();

            // Act
            header.CreationDate = "";
            header.MsgGuid = "";

            // Assert
            header.CreationDate.Should().BeEmpty();
            header.MsgGuid.Should().BeEmpty();
        }

        [Fact]
        public void MessageHeader_StringProperties_ShouldHandleLargeStrings()
        {
            // Arrange
            var header = new MessageHeader();
            var largeString = new string('A', 10000);

            // Act
            header.CreationDate = largeString;
            header.MsgGuid = largeString;

            // Assert
            header.CreationDate.Should().Be(largeString);
            header.MsgGuid.Should().Be(largeString);
        }

        [Fact]
        public void MessageHeader_DateTimeProperties_ShouldHandlePreciseTimes()
        {
            // Arrange
            var header = new MessageHeader();
            var preciseTime = new DateTime(2024, 12, 25, 15, 30, 45, 123, DateTimeKind.Utc);

            // Act
            header.MessageBrokerTimestamp = preciseTime;
            header.MessageTimestamp = preciseTime;

            // Assert
            header.MessageBrokerTimestamp.Should().Be(preciseTime);
            header.MessageTimestamp.Should().Be(preciseTime);
        }

        [Fact]
        public void MessageHeader_PropertyChanges_ShouldBeIndependent()
        {
            // Arrange
            var header = new MessageHeader();

            // Act
            header.Type = 1;
            header.MsgSeq = 100;
            header.CreationDate = "2024-01-01";
            
            header.Type = 2;
            header.MsgSeq = 200;
            header.CreationDate = "2024-01-02";

            // Assert
            header.Type.Should().Be(2);
            header.MsgSeq.Should().Be(200);
            header.CreationDate.Should().Be("2024-01-02");
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(31)]
        [InlineData(32)]
        [InlineData(35)]
        [InlineData(38)]
        [InlineData(40)]
        public void MessageHeader_WithKnownMessageTypes_ShouldHandleCorrectly(int messageType)
        {
            // Arrange
            var header = new MessageHeader();

            // Act
            header.Type = messageType;

            // Assert
            header.Type.Should().Be(messageType);
        }
    }
} 