using FluentAssertions;
using Trade360SDK.Common.Models;
using Xunit;
using System;
using System.Linq;

namespace Trade360SDK.Common.Tests.Models
{
    public class WrappedMessageTests
    {
        [Fact]
        public void Constructor_ShouldCreateInstanceSuccessfully()
        {
            // Act
            var message = new WrappedMessage();

            // Assert
            message.Should().NotBeNull();
            message.Header.Should().BeNull();
            message.Body.Should().BeNull();
        }

        [Fact]
        public void Header_ShouldGetAndSetCorrectly()
        {
            // Arrange
            var message = new WrappedMessage();
            var header = new MessageHeader
            {
                MsgGuid = "test-message-id",
                Type = 1
            };

            // Act
            message.Header = header;

            // Assert
            message.Header.Should().BeSameAs(header);
            message.Header.MsgGuid.Should().Be("test-message-id");
        }

        [Fact]
        public void Header_ShouldAllowNullAssignment()
        {
            // Arrange
            var message = new WrappedMessage();
            var header = new MessageHeader { MsgGuid = "test" };
            message.Header = header;

            // Act
            message.Header = null;

            // Assert
            message.Header.Should().BeNull();
        }

        [Fact]
        public void Body_ShouldGetAndSetCorrectly()
        {
            // Arrange
            var message = new WrappedMessage();
            var bodyContent = @"{""eventId"": 123, ""marketId"": 456}";

            // Act
            message.Body = bodyContent;

            // Assert
            message.Body.Should().Be(bodyContent);
        }

        [Fact]
        public void Body_ShouldAllowNullAssignment()
        {
            // Arrange
            var message = new WrappedMessage();
            message.Body = "some content";

            // Act
            message.Body = null;

            // Assert
            message.Body.Should().BeNull();
        }

        [Fact]
        public void Body_ShouldAllowEmptyStringAssignment()
        {
            // Arrange
            var message = new WrappedMessage();

            // Act
            message.Body = string.Empty;

            // Assert
            message.Body.Should().BeEmpty();
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("simple text")]
        [InlineData(@"{""json"": ""content""}")]
        [InlineData("very long content with special characters: áéíóú ñ ¡¿")]
        [InlineData("Content with line breaks\n\rand tabs\t")]
        public void Body_ShouldHandleVariousStringTypes(string bodyContent)
        {
            // Arrange
            var message = new WrappedMessage();

            // Act
            message.Body = bodyContent;

            // Assert
            message.Body.Should().Be(bodyContent);
        }

        [Fact]
        public void Properties_ShouldBeIndependent()
        {
            // Arrange
            var message = new WrappedMessage();
            var header = new MessageHeader { MsgGuid = "test-id" };
            var body = "test body content";

            // Act
            message.Header = header;
            message.Body = body;

            // Assert
            message.Header.Should().BeSameAs(header);
            message.Body.Should().Be(body);
            
            // Modify one property, other should remain unchanged
            message.Header = null;
            message.Body.Should().Be(body);
            
            message.Body = null;
            message.Header.Should().BeNull();
        }

        [Fact]
        public void CompleteMessage_ShouldWorkWithBothProperties()
        {
            // Arrange
            var header = new MessageHeader
            {
                MsgGuid = "msg-123",
                MessageTimestamp = new DateTime(2024, 1, 1, 12, 0, 0, DateTimeKind.Utc),
                Type = 456
            };
            var body = @"{""eventType"": ""MarketUpdate"", ""data"": {""marketId"": 789}}";

            // Act
            var message = new WrappedMessage
            {
                Header = header,
                Body = body
            };

            // Assert
            message.Header.Should().NotBeNull();
            message.Header.MsgGuid.Should().Be("msg-123");
            message.Header.MessageTimestamp.Should().Be(new DateTime(2024, 1, 1, 12, 0, 0, DateTimeKind.Utc));
            message.Body.Should().Be(body);
        }

        [Fact]
        public void Message_ShouldSupportObjectInitializer()
        {
            // Arrange & Act
            var message = new WrappedMessage
            {
                Header = new MessageHeader { MsgGuid = "init-test" },
                Body = "initialized body"
            };

            // Assert
            message.Header.Should().NotBeNull();
            message.Header.MsgGuid.Should().Be("init-test");
            message.Body.Should().Be("initialized body");
        }

        [Fact]
        public void Message_ShouldHandleLargeBodyContent()
        {
            // Arrange
            var largeBody = new string('x', 10000); // 10KB of content
            var message = new WrappedMessage();

            // Act
            message.Body = largeBody;

            // Assert
            message.Body.Should().Be(largeBody);
            message.Body.Length.Should().Be(10000);
        }

        [Fact]
        public void Message_ShouldHandleSpecialCharactersInBody()
        {
            // Arrange
            var specialBody = "Special chars: \n\r\t\"\\\0\u0001\u001F\u007F\u0080\u009F";
            var message = new WrappedMessage();

            // Act
            message.Body = specialBody;

            // Assert
            message.Body.Should().Be(specialBody);
        }

        [Fact]
        public void Message_ShouldAllowHeaderModificationAfterCreation()
        {
            // Arrange
            var message = new WrappedMessage();
            var originalHeader = new MessageHeader { MsgGuid = "original" };
            var newHeader = new MessageHeader { MsgGuid = "modified" };

            // Act
            message.Header = originalHeader;
            message.Header = newHeader;

            // Assert
            message.Header.Should().BeSameAs(newHeader);
            message.Header.MsgGuid.Should().Be("modified");
        }

        [Fact]
        public void Message_ShouldSupportMultipleInstances()
        {
            // Arrange & Act
            var message1 = new WrappedMessage { Body = "message 1" };
            var message2 = new WrappedMessage { Body = "message 2" };

            // Assert
            message1.Body.Should().Be("message 1");
            message2.Body.Should().Be("message 2");
            message1.Should().NotBeSameAs(message2);
        }

        [Fact]
        public void Message_ShouldHandleNullAndValidHeaderTogether()
        {
            // Arrange
            var message = new WrappedMessage();
            var validHeader = new MessageHeader 
            { 
                MsgGuid = "valid-id",
                Type = 123
            };

            // Act & Assert - Start with null
            message.Header.Should().BeNull();
            
            // Set valid header
            message.Header = validHeader;
            message.Header.Should().NotBeNull();
            message.Header.MsgGuid.Should().Be("valid-id");
            
            // Back to null
            message.Header = null;
            message.Header.Should().BeNull();
        }

        [Fact]
        public void Type_ShouldHaveCorrectProperties()
        {
            // Act
            var type = typeof(WrappedMessage);
            var properties = type.GetProperties();

            // Assert
            type.Should().NotBeNull();
            type.Name.Should().Be("WrappedMessage");
            type.Namespace.Should().Be("Trade360SDK.Common.Models");
            
            properties.Should().HaveCount(2);
            properties.Should().Contain(p => p.Name == "Header");
            properties.Should().Contain(p => p.Name == "Body");
            
            var headerProperty = properties.First(p => p.Name == "Header");
            var bodyProperty = properties.First(p => p.Name == "Body");
            
            headerProperty.PropertyType.Should().Be(typeof(MessageHeader));
            bodyProperty.PropertyType.Should().Be(typeof(string));
            
            headerProperty.CanRead.Should().BeTrue();
            headerProperty.CanWrite.Should().BeTrue();
            bodyProperty.CanRead.Should().BeTrue();
            bodyProperty.CanWrite.Should().BeTrue();
        }

        [Fact]
        public void Message_ShouldSupportReflectionAccess()
        {
            // Arrange
            var message = new WrappedMessage();
            var type = typeof(WrappedMessage);
            var headerProperty = type.GetProperty("Header");
            var bodyProperty = type.GetProperty("Body");

            // Act
            headerProperty!.SetValue(message, new MessageHeader { MsgGuid = "reflection-test" });
            bodyProperty!.SetValue(message, "reflection body");

            // Assert
            var headerValue = (MessageHeader?)headerProperty.GetValue(message);
            var bodyValue = (string?)bodyProperty.GetValue(message);

            headerValue.Should().NotBeNull();
            headerValue!.MsgGuid.Should().Be("reflection-test");
            bodyValue.Should().Be("reflection body");
        }
    }
} 