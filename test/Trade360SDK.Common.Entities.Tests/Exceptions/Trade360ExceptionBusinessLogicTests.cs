using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Trade360SDK.Common.Exceptions;
using Xunit;

namespace Trade360SDK.Common.Entities.Tests.Exceptions
{
    public class Trade360ExceptionBusinessLogicTests
    {
        [Fact]
        public void Constructor_WithMultipleErrors_ShouldAggregateErrorsInMessage()
        {
            // Arrange
            var errors = new List<string>
            {
                "First validation error",
                "Second validation error",
                "Third validation error"
            };

            // Act
            var exception = new Trade360Exception(errors);

            // Assert
            exception.Message.Should().Be("First validation error; Second validation error; Third validation error");
            exception.Errors.Should().BeEquivalentTo(errors);
        }

        [Fact]
        public void Constructor_WithMessageAndErrors_ShouldPrioritizeErrorsInMessage()
        {
            // Arrange
            var message = "Custom error message";
            var errors = new List<string>
            {
                "API validation failed",
                "Invalid request format"
            };

            // Act
            var exception = new Trade360Exception(message, errors);

            // Assert
            exception.Message.Should().Be("API validation failed; Invalid request format");
            exception.Errors.Should().BeEquivalentTo(errors);
        }

        [Fact]
        public void Constructor_WithMessageAndNoErrors_ShouldUseCustomMessage()
        {
            // Arrange
            var message = "Custom error message";
            var errors = new List<string>();

            // Act
            var exception = new Trade360Exception(message, errors);

            // Assert
            exception.Message.Should().Be(message);
            exception.Errors.Should().BeEmpty();
        }

        [Fact]
        public void Constructor_WithMessageAndNullErrors_ShouldUseCustomMessage()
        {
            // Arrange
            var message = "Custom error message";

            // Act
            var exception = new Trade360Exception(message, (IEnumerable<string>)null);

            // Assert
            exception.Message.Should().Be(message);
            exception.Errors.Should().BeNull();
        }

        [Fact]
        public void Constructor_WithEmptyErrorsList_ShouldUseDefaultMessage()
        {
            // Arrange
            var errors = new List<string>();

            // Act
            var exception = new Trade360Exception(errors);

            // Assert
            exception.Message.Should().Be("Exception of type 'Trade360SDK.Common.Exceptions.Trade360Exception' was thrown.");
            exception.Errors.Should().BeEmpty();
        }

        [Fact]
        public void Constructor_WithNullErrorsList_ShouldHaveDefaultMessage()
        {
            // Act
            var exception = new Trade360Exception((IEnumerable<string>)null);

            // Assert
            exception.Message.Should().Be("Exception of type 'Trade360SDK.Common.Exceptions.Trade360Exception' was thrown.");
            exception.Errors.Should().BeNull();
        }

        [Fact]
        public void Constructor_WithMessageAndRawError_ShouldCombineMessages()
        {
            // Arrange
            var message = "API Error";
            var rawError = "Detailed server response";

            // Act
            var exception = new Trade360Exception(message, rawError);

            // Assert
            exception.Message.Should().Be("API Error: Detailed server response");
            exception.Errors.Should().BeNull();
        }

        [Fact]
        public void Constructor_WithMessageAndInnerException_ShouldSetInnerException()
        {
            // Arrange
            var message = "Outer exception";
            var innerException = new InvalidOperationException("Inner exception message");

            // Act
            var exception = new Trade360Exception(message, innerException);

            // Assert
            exception.Message.Should().Be(message);
            exception.InnerException.Should().Be(innerException);
            exception.Errors.Should().BeNull();
        }

        [Fact]
        public void Constructor_WithErrorsContainingNullValues_ShouldHandleNullsGracefully()
        {
            // Arrange
            var errors = new List<string>
            {
                "Valid error",
                null,
                "Another valid error",
                null
            };

            // Act
            var exception = new Trade360Exception(errors);

            // Assert
            exception.Message.Should().Be("Valid error; ; Another valid error; ");
            exception.Errors.Should().BeEquivalentTo(errors);
        }

        [Fact]
        public void Constructor_WithSingleError_ShouldNotAddSemicolon()
        {
            // Arrange
            var errors = new List<string> { "Single error message" };

            // Act
            var exception = new Trade360Exception(errors);

            // Assert
            exception.Message.Should().Be("Single error message");
            exception.Errors.Should().BeEquivalentTo(errors);
        }

        [Fact]
        public void Constructor_WithErrorsContainingEmptyStrings_ShouldIncludeEmptyStrings()
        {
            // Arrange
            var errors = new List<string>
            {
                "First error",
                "",
                "Third error"
            };

            // Act
            var exception = new Trade360Exception(errors);

            // Assert
            exception.Message.Should().Be("First error; ; Third error");
            exception.Errors.Should().BeEquivalentTo(errors);
        }

        [Fact]
        public void Constructor_WithLargeNumberOfErrors_ShouldHandleAllErrors()
        {
            // Arrange
            var errors = Enumerable.Range(1, 100)
                .Select(i => $"Error number {i}")
                .ToList();

            // Act
            var exception = new Trade360Exception(errors);

            // Assert
            exception.Errors.Should().HaveCount(100);
            exception.Message.Should().Contain("Error number 1");
            exception.Message.Should().Contain("Error number 100");
            exception.Message.Split(';').Should().HaveCount(100);
        }

        [Fact]
        public void Constructor_WithVeryLongErrorMessages_ShouldConcatenateCorrectly()
        {
            // Arrange
            var longError1 = new string('A', 1000);
            var longError2 = new string('B', 1000);
            var errors = new List<string> { longError1, longError2 };

            // Act
            var exception = new Trade360Exception(errors);

            // Assert
            exception.Message.Should().StartWith(longError1);
            exception.Message.Should().Contain("; ");
            exception.Message.Should().EndWith(longError2);
            exception.Message.Length.Should().Be(2002); // 1000 + 2 + 1000 for semicolon and space
        }

        [Fact]
        public void Constructor_WithSpecialCharactersInErrors_ShouldPreserveCharacters()
        {
            // Arrange
            var errors = new List<string>
            {
                "Error with unicode: é, ñ, 中文",
                "Error with symbols: @#$%^&*()",
                "Error with line breaks:\nNew line content"
            };

            // Act
            var exception = new Trade360Exception(errors);

            // Assert
            exception.Message.Should().Contain("é, ñ, 中文");
            exception.Message.Should().Contain("@#$%^&*()");
            exception.Message.Should().Contain("\nNew line content");
        }

        [Theory]
        [InlineData("")]
        [InlineData("   ")]
        [InlineData("\t")]
        [InlineData("\n")]
        public void Constructor_WithWhitespaceOnlyMessages_ShouldPreserveWhitespace(string whitespaceMessage)
        {
            // Arrange
            var errors = new List<string> { "Valid error", whitespaceMessage, "Another error" };

            // Act
            var exception = new Trade360Exception(errors);

            // Assert
            exception.Message.Should().Be($"Valid error; {whitespaceMessage}; Another error");
        }

        [Fact]
        public void Message_Property_ShouldBeConsistentAcrossMultipleCalls()
        {
            // Arrange
            var errors = new List<string>
            {
                "Consistent error 1",
                "Consistent error 2"
            };
            var exception = new Trade360Exception(errors);

            // Act
            var message1 = exception.Message;
            var message2 = exception.Message;
            var message3 = exception.Message;

            // Assert
            message1.Should().Be(message2);
            message2.Should().Be(message3);
            message1.Should().Be("Consistent error 1; Consistent error 2");
        }

        [Fact]
        public void Errors_Property_ShouldReturnSameReferenceAsConstructor()
        {
            // Arrange
            var errors = new List<string> { "Test error" };

            // Act
            var exception = new Trade360Exception(errors);

            // Assert
            exception.Errors.Should().BeSameAs(errors);
        }

        [Fact]
        public void Constructor_WithMessageAndEmptyRawError_ShouldHandleEmptyRawError()
        {
            // Arrange
            var message = "API Error";
            var rawError = "";

            // Act
            var exception = new Trade360Exception(message, rawError);

            // Assert
            exception.Message.Should().Be("API Error: ");
        }

        [Fact]
        public void Constructor_WithEmptyMessageAndRawError_ShouldHandleEmptyMessage()
        {
            // Arrange
            var message = "";
            var rawError = "Raw error details";

            // Act
            var exception = new Trade360Exception(message, rawError);

            // Assert
            exception.Message.Should().Be(": Raw error details");
        }
    }
} 