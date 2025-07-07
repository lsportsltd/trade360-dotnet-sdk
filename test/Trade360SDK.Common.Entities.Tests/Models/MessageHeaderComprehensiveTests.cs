using System;
using FluentAssertions;
using Trade360SDK.Common.Models;
using Xunit;

namespace Trade360SDK.Common.Entities.Tests.Models;

public class MessageHeaderComprehensiveTests
{
    [Fact]
    public void Constructor_ShouldCreateInstanceSuccessfully()
    {
        // Act
        var messageHeader = new MessageHeader();

        // Assert
        messageHeader.Should().NotBeNull();
        messageHeader.CreationDate.Should().BeNull();
        messageHeader.Type.Should().Be(0); // Default int value
        messageHeader.MsgSeq.Should().BeNull();
        messageHeader.MsgGuid.Should().BeNull();
        messageHeader.ServerTimestamp.Should().BeNull();
        messageHeader.MessageBrokerTimestamp.Should().BeNull();
        messageHeader.MessageTimestamp.Should().BeNull();
    }

    [Theory]
    [InlineData("2023-12-25T10:30:45Z")]
    [InlineData("2024-01-01T00:00:00")]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("2023-12-31T23:59:59.999Z")]
    [InlineData("invalid date format")]
    public void CreationDate_Property_ShouldAcceptNullableStringValues(string? expectedCreationDate)
    {
        // Arrange
        var messageHeader = new MessageHeader();

        // Act
        messageHeader.CreationDate = expectedCreationDate;

        // Assert
        messageHeader.CreationDate.Should().Be(expectedCreationDate);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(100)]
    [InlineData(int.MaxValue)]
    [InlineData(int.MinValue)]
    [InlineData(-1)]
    [InlineData(2)] // LivescoreUpdate
    [InlineData(3)] // MarketUpdate
    [InlineData(31)] // KeepAliveUpdate
    public void Type_Property_ShouldAcceptIntValues(int expectedType)
    {
        // Arrange
        var messageHeader = new MessageHeader();

        // Act
        messageHeader.Type = expectedType;

        // Assert
        messageHeader.Type.Should().Be(expectedType);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(100)]
    [InlineData(null)]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(int.MaxValue)]
    [InlineData(int.MinValue)]
    public void MsgSeq_Property_ShouldAcceptNullableIntValues(int? expectedMsgSeq)
    {
        // Arrange
        var messageHeader = new MessageHeader();

        // Act
        messageHeader.MsgSeq = expectedMsgSeq;

        // Assert
        messageHeader.MsgSeq.Should().Be(expectedMsgSeq);
    }

    [Theory]
    [InlineData("550e8400-e29b-41d4-a716-446655440000")]
    [InlineData("6ba7b810-9dad-11d1-80b4-00c04fd430c8")]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("not-a-guid")]
    [InlineData("12345")]
    [InlineData("message-guid-123")]
    public void MsgGuid_Property_ShouldAcceptNullableStringValues(string? expectedMsgGuid)
    {
        // Arrange
        var messageHeader = new MessageHeader();

        // Act
        messageHeader.MsgGuid = expectedMsgGuid;

        // Assert
        messageHeader.MsgGuid.Should().Be(expectedMsgGuid);
    }

    [Theory]
    [InlineData(1640995200000L)] // 2022-01-01 00:00:00 UTC
    [InlineData(0L)]
    [InlineData(null)]
    [InlineData(-1L)]
    [InlineData(long.MaxValue)]
    [InlineData(long.MinValue)]
    [InlineData(1703980800000L)] // 2023-12-31 00:00:00 UTC
    public void ServerTimestamp_Property_ShouldAcceptNullableLongValues(long? expectedServerTimestamp)
    {
        // Arrange
        var messageHeader = new MessageHeader();

        // Act
        messageHeader.ServerTimestamp = expectedServerTimestamp;

        // Assert
        messageHeader.ServerTimestamp.Should().Be(expectedServerTimestamp);
    }

    [Fact]
    public void MessageBrokerTimestamp_Property_ShouldAcceptNullValue()
    {
        // Arrange
        var messageHeader = new MessageHeader();

        // Act
        messageHeader.MessageBrokerTimestamp = null;

        // Assert
        messageHeader.MessageBrokerTimestamp.Should().BeNull();
    }

    [Fact]
    public void MessageBrokerTimestamp_Property_ShouldAcceptDateTimeValues()
    {
        // Arrange
        var messageHeader = new MessageHeader();
        var expectedDateTime = new DateTime(2023, 12, 25, 10, 30, 45, DateTimeKind.Utc);

        // Act
        messageHeader.MessageBrokerTimestamp = expectedDateTime;

        // Assert
        messageHeader.MessageBrokerTimestamp.Should().Be(expectedDateTime);
    }

    [Theory]
    [InlineData("2023-01-01T00:00:00Z")]
    [InlineData("2023-12-31T23:59:59.999Z")]
    [InlineData("2024-06-15T12:30:45Z")]
    public void MessageBrokerTimestamp_Property_ShouldAcceptVariousDateTimeValues(string dateTimeString)
    {
        // Arrange
        var messageHeader = new MessageHeader();
        var expectedDateTime = DateTime.Parse(dateTimeString);

        // Act
        messageHeader.MessageBrokerTimestamp = expectedDateTime;

        // Assert
        messageHeader.MessageBrokerTimestamp.Should().Be(expectedDateTime);
    }

    [Fact]
    public void MessageTimestamp_Property_ShouldAcceptNullValue()
    {
        // Arrange
        var messageHeader = new MessageHeader();

        // Act
        messageHeader.MessageTimestamp = null;

        // Assert
        messageHeader.MessageTimestamp.Should().BeNull();
    }

    [Fact]
    public void MessageTimestamp_Property_ShouldAcceptDateTimeValues()
    {
        // Arrange
        var messageHeader = new MessageHeader();
        var expectedDateTime = new DateTime(2023, 12, 25, 15, 30, 45, DateTimeKind.Local);

        // Act
        messageHeader.MessageTimestamp = expectedDateTime;

        // Assert
        messageHeader.MessageTimestamp.Should().Be(expectedDateTime);
    }

    [Theory]
    [InlineData("2023-01-01T00:00:00")]
    [InlineData("2023-12-31T23:59:59")]
    [InlineData("2024-07-20T08:15:30")]
    public void MessageTimestamp_Property_ShouldAcceptVariousDateTimeValues(string dateTimeString)
    {
        // Arrange
        var messageHeader = new MessageHeader();
        var expectedDateTime = DateTime.Parse(dateTimeString);

        // Act
        messageHeader.MessageTimestamp = expectedDateTime;

        // Assert
        messageHeader.MessageTimestamp.Should().Be(expectedDateTime);
    }

    [Fact]
    public void AllProperties_ShouldWorkIndependently()
    {
        // Arrange
        var messageHeader = new MessageHeader();
        var creationDate = "2023-12-25T10:30:45Z";
        var type = 3; // MarketUpdate
        var msgSeq = 12345;
        var msgGuid = "550e8400-e29b-41d4-a716-446655440000";
        var serverTimestamp = 1703515845000L;
        var messageBrokerTimestamp = new DateTime(2023, 12, 25, 10, 30, 45, DateTimeKind.Utc);
        var messageTimestamp = new DateTime(2023, 12, 25, 10, 30, 46, DateTimeKind.Local);

        // Act
        messageHeader.CreationDate = creationDate;
        messageHeader.Type = type;
        messageHeader.MsgSeq = msgSeq;
        messageHeader.MsgGuid = msgGuid;
        messageHeader.ServerTimestamp = serverTimestamp;
        messageHeader.MessageBrokerTimestamp = messageBrokerTimestamp;
        messageHeader.MessageTimestamp = messageTimestamp;

        // Assert
        messageHeader.CreationDate.Should().Be(creationDate);
        messageHeader.Type.Should().Be(type);
        messageHeader.MsgSeq.Should().Be(msgSeq);
        messageHeader.MsgGuid.Should().Be(msgGuid);
        messageHeader.ServerTimestamp.Should().Be(serverTimestamp);
        messageHeader.MessageBrokerTimestamp.Should().Be(messageBrokerTimestamp);
        messageHeader.MessageTimestamp.Should().Be(messageTimestamp);
    }

    [Fact]
    public void Object_ShouldBeInstantiable()
    {
        // Act & Assert
        var messageHeader = new MessageHeader();
        messageHeader.Should().BeOfType<MessageHeader>();
        messageHeader.Should().NotBeNull();
    }

    [Fact]
    public void Properties_ShouldBeGettableAndSettable()
    {
        // Arrange
        var messageHeader = new MessageHeader();

        // Act & Assert - Test that all properties can be read and written
        var creationDate = messageHeader.CreationDate;
        messageHeader.CreationDate = "new date";
        messageHeader.CreationDate.Should().Be("new date");

        var type = messageHeader.Type;
        messageHeader.Type = 999;
        messageHeader.Type.Should().Be(999);

        var msgSeq = messageHeader.MsgSeq;
        messageHeader.MsgSeq = 555;
        messageHeader.MsgSeq.Should().Be(555);

        var msgGuid = messageHeader.MsgGuid;
        messageHeader.MsgGuid = "new-guid";
        messageHeader.MsgGuid.Should().Be("new-guid");

        var serverTimestamp = messageHeader.ServerTimestamp;
        messageHeader.ServerTimestamp = 123456789L;
        messageHeader.ServerTimestamp.Should().Be(123456789L);

        var messageBrokerTimestamp = messageHeader.MessageBrokerTimestamp;
        var newBrokerTimestamp = DateTime.UtcNow;
        messageHeader.MessageBrokerTimestamp = newBrokerTimestamp;
        messageHeader.MessageBrokerTimestamp.Should().Be(newBrokerTimestamp);

        var messageTimestamp = messageHeader.MessageTimestamp;
        var newMessageTimestamp = DateTime.Now;
        messageHeader.MessageTimestamp = newMessageTimestamp;
        messageHeader.MessageTimestamp.Should().Be(newMessageTimestamp);
    }

    [Fact]
    public void MessageHeader_WithCompleteData_ShouldHandleCorrectly()
    {
        // Arrange & Act
        var messageHeader = new MessageHeader
        {
            CreationDate = "2023-12-25T10:30:45.123Z",
            Type = 2, // LivescoreUpdate
            MsgSeq = 98765,
            MsgGuid = "6ba7b810-9dad-11d1-80b4-00c04fd430c8",
            ServerTimestamp = 1703515845123L,
            MessageBrokerTimestamp = new DateTime(2023, 12, 25, 10, 30, 45, 123, DateTimeKind.Utc),
            MessageTimestamp = new DateTime(2023, 12, 25, 10, 30, 45, 456, DateTimeKind.Local)
        };

        // Assert
        messageHeader.CreationDate.Should().Be("2023-12-25T10:30:45.123Z");
        messageHeader.Type.Should().Be(2);
        messageHeader.MsgSeq.Should().Be(98765);
        messageHeader.MsgGuid.Should().Be("6ba7b810-9dad-11d1-80b4-00c04fd430c8");
        messageHeader.ServerTimestamp.Should().Be(1703515845123L);
        messageHeader.MessageBrokerTimestamp.Should().Be(new DateTime(2023, 12, 25, 10, 30, 45, 123, DateTimeKind.Utc));
        messageHeader.MessageTimestamp.Should().Be(new DateTime(2023, 12, 25, 10, 30, 45, 456, DateTimeKind.Local));
    }

    [Fact]
    public void MessageHeader_ReassignProperties_ShouldUpdateCorrectly()
    {
        // Arrange
        var messageHeader = new MessageHeader
        {
            CreationDate = "old date",
            Type = 1,
            MsgSeq = 111,
            MsgGuid = "old-guid",
            ServerTimestamp = 111111L,
            MessageBrokerTimestamp = new DateTime(2023, 1, 1),
            MessageTimestamp = new DateTime(2023, 1, 2)
        };

        var newBrokerTimestamp = new DateTime(2023, 12, 25);
        var newMessageTimestamp = new DateTime(2023, 12, 26);

        // Act
        messageHeader.CreationDate = "new date";
        messageHeader.Type = 2;
        messageHeader.MsgSeq = 222;
        messageHeader.MsgGuid = "new-guid";
        messageHeader.ServerTimestamp = 222222L;
        messageHeader.MessageBrokerTimestamp = newBrokerTimestamp;
        messageHeader.MessageTimestamp = newMessageTimestamp;

        // Assert
        messageHeader.CreationDate.Should().Be("new date");
        messageHeader.Type.Should().Be(2);
        messageHeader.MsgSeq.Should().Be(222);
        messageHeader.MsgGuid.Should().Be("new-guid");
        messageHeader.ServerTimestamp.Should().Be(222222L);
        messageHeader.MessageBrokerTimestamp.Should().Be(newBrokerTimestamp);
        messageHeader.MessageTimestamp.Should().Be(newMessageTimestamp);
    }

    [Fact]
    public void MessageHeader_NullAssignments_ShouldWork()
    {
        // Arrange
        var messageHeader = new MessageHeader
        {
            CreationDate = "test date",
            Type = 5,
            MsgSeq = 333,
            MsgGuid = "test-guid",
            ServerTimestamp = 333333L,
            MessageBrokerTimestamp = DateTime.UtcNow,
            MessageTimestamp = DateTime.Now
        };

        // Act
        messageHeader.CreationDate = null;
        messageHeader.MsgSeq = null;
        messageHeader.MsgGuid = null;
        messageHeader.ServerTimestamp = null;
        messageHeader.MessageBrokerTimestamp = null;
        messageHeader.MessageTimestamp = null;

        // Assert
        messageHeader.CreationDate.Should().BeNull();
        messageHeader.Type.Should().Be(5); // Should remain unchanged
        messageHeader.MsgSeq.Should().BeNull();
        messageHeader.MsgGuid.Should().BeNull();
        messageHeader.ServerTimestamp.Should().BeNull();
        messageHeader.MessageBrokerTimestamp.Should().BeNull();
        messageHeader.MessageTimestamp.Should().BeNull();
    }

    [Fact]
    public void MessageHeader_EdgeCaseValues_ShouldBeHandled()
    {
        // Arrange
        var messageHeader = new MessageHeader();
        var extremeDateTime = DateTime.MaxValue;
        var minDateTime = DateTime.MinValue;

        // Act & Assert - Extreme DateTime values
        messageHeader.MessageBrokerTimestamp = extremeDateTime;
        messageHeader.MessageBrokerTimestamp.Should().Be(extremeDateTime);

        messageHeader.MessageTimestamp = minDateTime;
        messageHeader.MessageTimestamp.Should().Be(minDateTime);

        // Act & Assert - Extreme long values
        messageHeader.ServerTimestamp = long.MaxValue;
        messageHeader.ServerTimestamp.Should().Be(long.MaxValue);

        messageHeader.ServerTimestamp = long.MinValue;
        messageHeader.ServerTimestamp.Should().Be(long.MinValue);

        // Act & Assert - Extreme int values
        messageHeader.Type = int.MaxValue;
        messageHeader.Type.Should().Be(int.MaxValue);

        messageHeader.MsgSeq = int.MinValue;
        messageHeader.MsgSeq.Should().Be(int.MinValue);
    }

    [Fact]
    public void MessageHeader_LongStrings_ShouldBeHandled()
    {
        // Arrange
        var messageHeader = new MessageHeader();
        var longString = new string('a', 10000); // 10k characters

        // Act
        messageHeader.CreationDate = longString;
        messageHeader.MsgGuid = longString;

        // Assert
        messageHeader.CreationDate.Should().Be(longString);
        messageHeader.MsgGuid.Should().Be(longString);
    }

    [Fact]
    public void MessageHeader_SpecialCharacters_ShouldBeHandled()
    {
        // Arrange
        var messageHeader = new MessageHeader();
        var specialString = "test@#$%^&*()_+-=[]{}|;':\",./<>?`~";

        // Act
        messageHeader.CreationDate = specialString;
        messageHeader.MsgGuid = specialString;

        // Assert
        messageHeader.CreationDate.Should().Be(specialString);
        messageHeader.MsgGuid.Should().Be(specialString);
    }

    [Fact]
    public void MessageHeader_UnicodeStrings_ShouldBeHandled()
    {
        // Arrange
        var messageHeader = new MessageHeader();
        var unicodeString = "测试数据 🎯 ñáéíóú αβγδε русский";

        // Act
        messageHeader.CreationDate = unicodeString;
        messageHeader.MsgGuid = unicodeString;

        // Assert
        messageHeader.CreationDate.Should().Be(unicodeString);
        messageHeader.MsgGuid.Should().Be(unicodeString);
    }
} 