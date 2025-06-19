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
        public void Trade360Exception_ConstructorWithErrors_ShouldSetErrors()
        {
            // Arrange
            var errors = new List<string> { "Error 1", "Error 2", "Error 3" };

            // Act
            var exception = new Trade360Exception(errors);

            // Assert
            exception.Should().NotBeNull();
            exception.Errors.Should().BeEquivalentTo(errors);
            exception.Message.Should().Be("Error 1; Error 2; Error 3");
        }

        [Fact]
        public void Trade360Exception_ConstructorWithNullErrors_ShouldSetNullErrors()
        {
            // Act
            var exception = new Trade360Exception((IEnumerable<string?>?)null);

            // Assert
            exception.Should().NotBeNull();
            exception.Errors.Should().BeNull();
            exception.Message.Should().Be("Exception of type 'Trade360SDK.Common.Exceptions.Trade360Exception' was thrown.");
        }

        [Fact]
        public void Trade360Exception_ConstructorWithEmptyErrors_ShouldSetEmptyErrors()
        {
            // Arrange
            var errors = new List<string>();

            // Act
            var exception = new Trade360Exception(errors);

            // Assert
            exception.Should().NotBeNull();
            exception.Errors.Should().BeEmpty();
            exception.Message.Should().Be("Exception of type 'Trade360SDK.Common.Exceptions.Trade360Exception' was thrown.");
        }

        [Fact]
        public void Trade360Exception_ConstructorWithMessageAndErrors_ShouldSetMessageAndErrors()
        {
            // Arrange
            var message = "Custom error message";
            var errors = new List<string> { "Error 1", "Error 2" };

            // Act
            var exception = new Trade360Exception(message, errors);

            // Assert
            exception.Should().NotBeNull();
            exception.Errors.Should().BeEquivalentTo(errors);
            exception.Message.Should().Be("Error 1; Error 2");
        }

        [Fact]
        public void Trade360Exception_ConstructorWithMessageAndNullErrors_ShouldSetMessageAndNullErrors()
        {
            // Arrange
            var message = "Custom error message";

            // Act
            var exception = new Trade360Exception(message, (IEnumerable<string?>?)null);

            // Assert
            exception.Should().NotBeNull();
            exception.Errors.Should().BeNull();
            exception.Message.Should().Be(message);
        }

        [Fact]
        public void Trade360Exception_ConstructorWithMessageAndEmptyErrors_ShouldSetMessageAndEmptyErrors()
        {
            // Arrange
            var message = "Custom error message";
            var errors = new List<string>();

            // Act
            var exception = new Trade360Exception(message, errors);

            // Assert
            exception.Should().NotBeNull();
            exception.Errors.Should().BeEmpty();
            exception.Message.Should().Be(message);
        }

        [Fact]
        public void Trade360Exception_ConstructorWithMessageAndRawErrorResponse_ShouldSetCombinedMessage()
        {
            // Arrange
            var message = "API Error";
            var rawErrorResponse = "Internal Server Error";

            // Act
            var exception = new Trade360Exception(message, rawErrorResponse);

            // Assert
            exception.Should().NotBeNull();
            exception.Message.Should().Be("API Error: Internal Server Error");
            exception.Errors.Should().BeNull();
        }

        [Fact]
        public void Trade360Exception_ConstructorWithMessageAndInnerException_ShouldSetMessageAndInnerException()
        {
            // Arrange
            var message = "Outer exception";
            var innerException = new InvalidOperationException("Inner exception");

            // Act
            var exception = new Trade360Exception(message, innerException);

            // Assert
            exception.Should().NotBeNull();
            exception.Message.Should().Be(message);
            exception.InnerException.Should().Be(innerException);
            exception.Errors.Should().BeNull();
        }

        [Theory]
        [InlineData("")]
        [InlineData("   ")]
        [InlineData("Single error")]
        [InlineData("Error with special characters: !@#$%^&*()")]
        [InlineData("Error with unicode: 测试错误消息")]
        public void Trade360Exception_ConstructorWithSingleError_ShouldSetError(string error)
        {
            // Arrange
            var errors = new List<string> { error };

            // Act
            var exception = new Trade360Exception(errors);

            // Assert
            exception.Should().NotBeNull();
            exception.Errors.Should().Contain(error);
            exception.Message.Should().Be(error);
        }

        [Fact]
        public void Trade360Exception_WithMultipleErrors_ShouldJoinWithSemicolon()
        {
            // Arrange
            var errors = new List<string> { "First error", "Second error", "Third error" };

            // Act
            var exception = new Trade360Exception(errors);

            // Assert
            exception.Should().NotBeNull();
            exception.Message.Should().Be("First error; Second error; Third error");
        }

        [Fact]
        public void Trade360Exception_WithNullErrorsInList_ShouldHandleNulls()
        {
            // Arrange
            var errors = new List<string?> { "Error 1", null, "Error 3", null, "Error 5" };

            // Act
            var exception = new Trade360Exception(errors);

            // Assert
            exception.Should().NotBeNull();
            exception.Errors.Should().BeEquivalentTo(errors);
            exception.Message.Should().Be("Error 1; ; Error 3; ; Error 5");
        }

        [Fact]
        public void Trade360Exception_WithEmptyStringsInList_ShouldHandleEmptyStrings()
        {
            // Arrange
            var errors = new List<string> { "Error 1", "", "Error 3", "   ", "Error 5" };

            // Act
            var exception = new Trade360Exception(errors);

            // Assert
            exception.Should().NotBeNull();
            exception.Errors.Should().BeEquivalentTo(errors);
            exception.Message.Should().Be("Error 1; ; Error 3;    ; Error 5");
        }

        [Fact]
        public void Trade360Exception_MessageProperty_WithErrorsNull_ShouldReturnBaseMessage()
        {
            // Arrange
            var message = "Custom message";
            var exception = new Trade360Exception(message, (IEnumerable<string?>?)null);

            // Act
            var actualMessage = exception.Message;

            // Assert
            actualMessage.Should().Be(message);
        }

        [Fact]
        public void Trade360Exception_MessageProperty_WithErrorsEmpty_ShouldReturnBaseMessage()
        {
            // Arrange
            var message = "Custom message";
            var errors = new List<string>();
            var exception = new Trade360Exception(message, errors);

            // Act
            var actualMessage = exception.Message;

            // Assert
            actualMessage.Should().Be(message);
        }

        [Fact]
        public void Trade360Exception_MessageProperty_WithErrorsPresent_ShouldReturnJoinedErrors()
        {
            // Arrange
            var message = "Custom message";
            var errors = new List<string> { "Error A", "Error B" };
            var exception = new Trade360Exception(message, errors);

            // Act
            var actualMessage = exception.Message;

            // Assert
            actualMessage.Should().Be("Error A; Error B");
            actualMessage.Should().NotBe(message);
        }

        [Fact]
        public void Trade360Exception_ShouldInheritFromException()
        {
            // Arrange & Act
            var exception = new Trade360Exception(new List<string> { "Test error" });

            // Assert
            exception.Should().BeAssignableTo<Exception>();
        }

        [Fact]
        public void Trade360Exception_ToString_ShouldReturnStringRepresentation()
        {
            // Arrange
            var errors = new List<string> { "Test error" };
            var exception = new Trade360Exception(errors);

            // Act
            var result = exception.ToString();

            // Assert
            result.Should().NotBeNullOrEmpty();
            result.Should().Contain("Trade360Exception");
            result.Should().Contain("Test error");
        }

        [Fact]
        public void Trade360Exception_GetHashCode_ShouldReturnConsistentValue()
        {
            // Arrange
            var errors = new List<string> { "Test error" };
            var exception = new Trade360Exception(errors);

            // Act
            var hashCode1 = exception.GetHashCode();
            var hashCode2 = exception.GetHashCode();

            // Assert
            hashCode1.Should().Be(hashCode2);
        }

        [Fact]
        public void Trade360Exception_Type_ShouldBeCorrect()
        {
            // Arrange & Act
            var exception = new Trade360Exception(new List<string> { "Test error" });

            // Assert
            exception.GetType().Should().Be(typeof(Trade360Exception));
            exception.GetType().Name.Should().Be("Trade360Exception");
            exception.GetType().Namespace.Should().Be("Trade360SDK.Common.Exceptions");
        }

        [Fact]
        public void Trade360Exception_WithLargeNumberOfErrors_ShouldHandleCorrectly()
        {
            // Arrange
            var errors = Enumerable.Range(1, 100).Select(i => $"Error {i}").ToList();

            // Act
            var exception = new Trade360Exception(errors);

            // Assert
            exception.Should().NotBeNull();
            exception.Errors.Should().HaveCount(100);
            exception.Message.Should().Contain("Error 1");
            exception.Message.Should().Contain("Error 100");
            exception.Message.Should().Contain(";");
        }

        [Fact]
        public void Trade360Exception_WithVeryLongErrorMessages_ShouldHandleCorrectly()
        {
            // Arrange
            var longError = new string('A', 10000);
            var errors = new List<string> { longError, "Short error" };

            // Act
            var exception = new Trade360Exception(errors);

            // Assert
            exception.Should().NotBeNull();
            exception.Errors.Should().HaveCount(2);
            exception.Message.Should().Contain(longError);
            exception.Message.Should().Contain("Short error");
        }

        [Fact]
        public void Trade360Exception_WithSpecialCharactersInRawResponse_ShouldHandleCorrectly()
        {
            // Arrange
            var message = "API Error";
            var rawErrorResponse = "Error: {\"status\": 500, \"message\": \"Internal\\nServer\\tError\"}";

            // Act
            var exception = new Trade360Exception(message, rawErrorResponse);

            // Assert
            exception.Should().NotBeNull();
            exception.Message.Should().Be($"{message}: {rawErrorResponse}");
        }

        [Fact]
        public void Trade360Exception_WithNullInnerException_ShouldHandleCorrectly()
        {
            // Arrange
            var message = "Test message";

            // Act
            var exception = new Trade360Exception(message, (Exception?)null);

            // Assert
            exception.Should().NotBeNull();
            exception.Message.Should().Be(message);
            exception.InnerException.Should().BeNull();
        }

        [Fact]
        public void Trade360Exception_WithComplexInnerException_ShouldHandleCorrectly()
        {
            // Arrange
            var message = "Outer exception";
            var innerMessage = "Inner exception";
            var innerInnerException = new ArgumentException("Inner inner exception");
            var innerException = new InvalidOperationException(innerMessage, innerInnerException);

            // Act
            var exception = new Trade360Exception(message, innerException);

            // Assert
            exception.Should().NotBeNull();
            exception.Message.Should().Be(message);
            exception.InnerException.Should().Be(innerException);
            exception.InnerException.Message.Should().Be(innerMessage);
            exception.InnerException.InnerException.Should().Be(innerInnerException);
        }
    }
} 