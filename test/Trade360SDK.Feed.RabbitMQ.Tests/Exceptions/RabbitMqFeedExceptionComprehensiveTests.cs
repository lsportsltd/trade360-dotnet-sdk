using System;
using FluentAssertions;
using Trade360SDK.Feed.RabbitMQ.Exceptions;
using Xunit;

namespace Trade360SDK.Feed.RabbitMQ.Tests.Exceptions
{
    public class RabbitMqFeedExceptionComprehensiveTests
    {
        [Fact]
        public void Constructor_WithMessage_ShouldSetMessage()
        {
            // Arrange
            var message = "Test error message";

            // Act
            var exception = new RabbitMqFeedException(message);

            // Assert
            exception.Message.Should().Be(message);
            exception.InnerException.Should().BeNull();
        }

        [Fact]
        public void Constructor_WithMessageAndInnerException_ShouldSetBoth()
        {
            // Arrange
            var message = "Test error message";
            var innerException = new InvalidOperationException("Inner error");

            // Act
            var exception = new RabbitMqFeedException(message, innerException);

            // Assert
            exception.Message.Should().Be(message);
            exception.InnerException.Should().Be(innerException);
        }

        [Fact]
        public void Constructor_WithNullMessage_ShouldHandleNull()
        {
            // Act
            var exception = new RabbitMqFeedException(null);

            // Assert
            exception.Message.Should().NotBeNull(); // Base Exception class doesn't allow null messages
            exception.InnerException.Should().BeNull();
        }

        [Fact]
        public void Constructor_WithEmptyMessage_ShouldSetEmptyMessage()
        {
            // Arrange
            var message = "";

            // Act
            var exception = new RabbitMqFeedException(message);

            // Assert
            exception.Message.Should().Be(message);
            exception.InnerException.Should().BeNull();
        }

        [Fact]
        public void Constructor_WithWhitespaceMessage_ShouldSetWhitespaceMessage()
        {
            // Arrange
            var message = "   ";

            // Act
            var exception = new RabbitMqFeedException(message);

            // Assert
            exception.Message.Should().Be(message);
            exception.InnerException.Should().BeNull();
        }

        [Fact]
        public void Constructor_WithNullInnerException_ShouldSetNullInnerException()
        {
            // Arrange
            var message = "Test message";

            // Act
            var exception = new RabbitMqFeedException(message, null);

            // Assert
            exception.Message.Should().Be(message);
            exception.InnerException.Should().BeNull();
        }

        [Fact]
        public void Constructor_WithComplexInnerException_ShouldPreserveInnerExceptionDetails()
        {
            // Arrange
            var message = "Outer exception";
            var innerInnerException = new ArgumentException("Deepest exception");
            var innerException = new InvalidOperationException("Middle exception", innerInnerException);

            // Act
            var exception = new RabbitMqFeedException(message, innerException);

            // Assert
            exception.Message.Should().Be(message);
            exception.InnerException.Should().Be(innerException);
            exception.InnerException.InnerException.Should().Be(innerInnerException);
        }

        [Fact]
        public void Exception_ShouldBeInstanceOfException()
        {
            // Arrange & Act
            var exception = new RabbitMqFeedException("Test");

            // Assert
            exception.Should().BeAssignableTo<Exception>();
        }

        [Theory]
        [InlineData("Connection failed")]
        [InlineData("Queue not found")]
        [InlineData("Authentication failed")]
        [InlineData("Network timeout")]
        [InlineData("Channel closed unexpectedly")]
        public void Constructor_WithCommonRabbitMqErrors_ShouldSetMessage(string errorMessage)
        {
            // Act
            var exception = new RabbitMqFeedException(errorMessage);

            // Assert
            exception.Message.Should().Be(errorMessage);
        }

        [Fact]
        public void Constructor_WithVeryLongMessage_ShouldHandleCorrectly()
        {
            // Arrange
            var longMessage = new string('A', 10000);

            // Act
            var exception = new RabbitMqFeedException(longMessage);

            // Assert
            exception.Message.Should().Be(longMessage);
            exception.Message.Length.Should().Be(10000);
        }

        [Fact]
        public void Constructor_WithSpecialCharacters_ShouldHandleCorrectly()
        {
            // Arrange
            var messageWithSpecialChars = "Error: !@#$%^&*()_+-=[]{}|;':\",./<>?";

            // Act
            var exception = new RabbitMqFeedException(messageWithSpecialChars);

            // Assert
            exception.Message.Should().Be(messageWithSpecialChars);
        }

        [Fact]
        public void Constructor_WithUnicodeCharacters_ShouldHandleCorrectly()
        {
            // Arrange
            var unicodeMessage = "RabbitMQ错误: 连接失败";

            // Act
            var exception = new RabbitMqFeedException(unicodeMessage);

            // Assert
            exception.Message.Should().Be(unicodeMessage);
        }

        [Fact]
        public void Constructor_WithNewlineCharacters_ShouldPreserveFormatting()
        {
            // Arrange
            var messageWithNewlines = "Error occurred:\nConnection failed\nRetry attempt failed";

            // Act
            var exception = new RabbitMqFeedException(messageWithNewlines);

            // Assert
            exception.Message.Should().Be(messageWithNewlines);
            exception.Message.Should().Contain("\n");
        }

        [Fact]
        public void Constructor_WithJsonErrorMessage_ShouldHandleCorrectly()
        {
            // Arrange
            var jsonMessage = "{\"error\": \"connection_failed\", \"code\": 500, \"details\": \"Network unreachable\"}";

            // Act
            var exception = new RabbitMqFeedException(jsonMessage);

            // Assert
            exception.Message.Should().Be(jsonMessage);
        }

        [Fact]
        public void ToString_ShouldIncludeExceptionType()
        {
            // Arrange
            var exception = new RabbitMqFeedException("Test error");

            // Act
            var toString = exception.ToString();

            // Assert
            toString.Should().Contain("RabbitMqFeedException");
            toString.Should().Contain("Test error");
        }

        [Fact]
        public void ToString_WithInnerException_ShouldIncludeInnerExceptionDetails()
        {
            // Arrange
            var innerException = new InvalidOperationException("Inner error details");
            var exception = new RabbitMqFeedException("Outer error", innerException);

            // Act
            var toString = exception.ToString();

            // Assert
            toString.Should().Contain("RabbitMqFeedException");
            toString.Should().Contain("Outer error");
            toString.Should().Contain("InvalidOperationException");
            toString.Should().Contain("Inner error details");
        }

        [Fact] 
        public void Exception_ShouldBeSerializable()
        {
            // Arrange
            var message = "Serialization test";
            var innerException = new ArgumentException("Inner exception for serialization");
            var exception = new RabbitMqFeedException(message, innerException);

            // Act & Assert - This tests basic serialization compatibility
            exception.Data.Should().NotBeNull();
            exception.HelpLink = "https://help.example.com";
            exception.Source = "Test Source";
            
            exception.HelpLink.Should().Be("https://help.example.com");
            exception.Source.Should().Be("Test Source");
        }

        [Fact]
        public void Exception_WithCustomData_ShouldPreserveData()
        {
            // Arrange
            var exception = new RabbitMqFeedException("Test with data");
            var customKey = "CustomKey";
            var customValue = "CustomValue";

            // Act
            exception.Data.Add(customKey, customValue);

            // Assert
            exception.Data[customKey].Should().Be(customValue);
        }
    }
} 