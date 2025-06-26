using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Xunit;
using Trade360SDK.Common.Exceptions;

namespace Trade360SDK.Common.Entities.Tests.Exceptions
{
    public class Trade360ExceptionComprehensiveTests
    {
        [Fact]
        public void Constructor_WithErrors_ShouldSetErrorsProperty()
        {
            // Arrange
            var errors = new List<string> { "Error 1", "Error 2", "Error 3" };

            // Act
            var exception = new Trade360Exception(errors);

            // Assert
            exception.Errors.Should().BeEquivalentTo(errors);
            exception.Message.Should().Be("Error 1; Error 2; Error 3");
        }

        [Fact]
        public void Constructor_WithNullErrors_ShouldHandleGracefully()
        {
            // Act
            var exception = new Trade360Exception((IEnumerable<string>)null);

            // Assert
            exception.Errors.Should().BeNull();
            exception.Message.Should().NotBeNull();
        }

        [Fact]
        public void Constructor_WithEmptyErrors_ShouldHandleGracefully()
        {
            // Arrange
            var errors = new List<string>();

            // Act
            var exception = new Trade360Exception(errors);

            // Assert
            exception.Errors.Should().BeEmpty();
            exception.Message.Should().NotBeNull();
        }

        [Fact]
        public void Constructor_WithMessageAndErrors_ShouldSetBothProperties()
        {
            // Arrange
            var message = "Custom message";
            var errors = new List<string> { "Error 1", "Error 2" };

            // Act
            var exception = new Trade360Exception(message, errors);

            // Assert
            exception.Errors.Should().BeEquivalentTo(errors);
            exception.Message.Should().Be("Error 1; Error 2");
        }

        [Fact]
        public void Constructor_WithMessageAndRawError_ShouldCombineMessages()
        {
            // Arrange
            var message = "Custom message";
            var rawError = "Raw error response";

            // Act
            var exception = new Trade360Exception(message, rawError);

            // Assert
            exception.Message.Should().Be("Custom message: Raw error response");
        }

        [Fact]
        public void Constructor_WithMessageAndInnerException_ShouldSetInnerException()
        {
            // Arrange
            var message = "Custom message";
            var innerException = new InvalidOperationException("Inner error");

            // Act
            var exception = new Trade360Exception(message, innerException);

            // Assert
            exception.Message.Should().Be(message);
            exception.InnerException.Should().Be(innerException);
        }

        [Fact]
        public void Message_WithErrors_ShouldReturnJoinedErrors()
        {
            // Arrange
            var errors = new List<string> { "First error", "Second error", "Third error" };
            var exception = new Trade360Exception(errors);

            // Act
            var message = exception.Message;

            // Assert
            message.Should().Be("First error; Second error; Third error");
        }

        [Fact]
        public void Message_WithNullErrors_ShouldReturnBaseMessage()
        {
            // Arrange
            var exception = new Trade360Exception((IEnumerable<string>)null);

            // Act
            var message = exception.Message;

            // Assert
            message.Should().NotBeNull();
            message.Should().NotBeEmpty();
        }

        [Fact]
        public void Message_WithEmptyErrors_ShouldReturnBaseMessage()
        {
            // Arrange
            var errors = new List<string>();
            var exception = new Trade360Exception(errors);

            // Act
            var message = exception.Message;

            // Assert
            message.Should().NotBeNull();
            message.Should().NotBeEmpty();
        }

        [Fact]
        public void Message_WithSingleError_ShouldReturnSingleError()
        {
            // Arrange
            var errors = new List<string> { "Single error" };
            var exception = new Trade360Exception(errors);

            // Act
            var message = exception.Message;

            // Assert
            message.Should().Be("Single error");
        }

        [Fact]
        public void Message_WithNullErrorsInList_ShouldHandleGracefully()
        {
            // Arrange
            var errors = new List<string?> { "Error 1", null, "Error 3" };
            var exception = new Trade360Exception(errors);

            // Act
            var message = exception.Message;

            // Assert
            message.Should().Be("Error 1; ; Error 3");
        }

        [Fact]
        public void Message_WithMessageAndErrors_ShouldPrioritizeErrors()
        {
            // Arrange
            var customMessage = "Custom message";
            var errors = new List<string> { "Error from list" };
            var exception = new Trade360Exception(customMessage, errors);

            // Act
            var message = exception.Message;

            // Assert
            message.Should().Be("Error from list");
            message.Should().NotBe(customMessage);
        }

        [Fact]
        public void Message_WithLargeNumberOfErrors_ShouldJoinAll()
        {
            // Arrange
            var errors = Enumerable.Range(1, 100).Select(i => $"Error {i}").ToList();
            var exception = new Trade360Exception(errors);

            // Act
            var message = exception.Message;

            // Assert
            message.Should().Contain("Error 1");
            message.Should().Contain("Error 100");
            message.Should().Contain("; ");
        }

        [Fact]
        public void Errors_PropertyShouldRetainOriginalCollection()
        {
            // Arrange
            var originalErrors = new List<string> { "Error 1", "Error 2" };
            var exception = new Trade360Exception(originalErrors);

            // Act
            var errors = exception.Errors;

            // Assert
            errors.Should().BeEquivalentTo(originalErrors);
        }

        [Fact]
        public void Exception_ShouldBeInstanceOfException()
        {
            // Arrange & Act
            var exception = new Trade360Exception(new List<string> { "Test error" });

            // Assert
            exception.Should().BeAssignableTo<Exception>();
        }

        [Fact]
        public void Exception_WithComplexErrorMessages_ShouldHandleCorrectly()
        {
            // Arrange
            var errors = new List<string>
            {
                "Error with special characters: !@#$%^&*()",
                "Error with unicode: 測試錯誤",
                "Error with newlines:\nSecond line",
                "Very long error message that might cause issues with string concatenation or display in various UI components and should be handled gracefully by the exception system"
            };

            // Act
            var exception = new Trade360Exception(errors);

            // Assert
            exception.Message.Should().Contain("special characters");
            exception.Message.Should().Contain("unicode");
            exception.Message.Should().Contain("newlines");
            exception.Message.Should().Contain("Very long error message");
        }

        [Fact]
        public void ToString_ShouldIncludeExceptionDetails()
        {
            // Arrange
            var errors = new List<string> { "Test error" };
            var exception = new Trade360Exception(errors);

            // Act
            var toString = exception.ToString();

            // Assert
            toString.Should().Contain("Trade360Exception");
            toString.Should().Contain("Test error");
        }
    }
} 