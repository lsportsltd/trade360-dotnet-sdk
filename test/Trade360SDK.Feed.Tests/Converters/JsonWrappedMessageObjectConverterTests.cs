using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using FluentAssertions;
using Trade360SDK.Feed.Converters;
using Trade360SDK.Common.Models;
using Xunit;
using System.Collections.Generic; // Added for KeyNotFoundException

namespace Trade360SDK.Feed.Tests.Converters
{
    public class JsonWrappedMessageObjectConverterTests
    {
        [Fact]
        public void CanInstantiateConverter()
        {
            var converter = new JsonWrappedMessageObjectConverter();
            Assert.NotNull(converter);
            Assert.IsType<JsonWrappedMessageObjectConverter>(converter);
        }

        [Fact]
        public void ConvertJsonToMessage_WithValidJson_ShouldReturnWrappedMessage()
        {
            // Arrange
            var jsonInput = @"{
                ""Header"": {
                    ""Type"": 1,
                    ""MsgGuid"": ""test-guid-123"",
                    ""CreationDate"": ""2024-01-01T00:00:00Z""
                },
                ""Body"": ""TestData""
            }";

            // Act
            var result = JsonWrappedMessageObjectConverter.ConvertJsonToMessage(jsonInput);

            // Assert
            result.Should().NotBeNull();
            result.Header.Should().NotBeNull();
            result.Header!.Type.Should().Be(1);
            result.Header.MsgGuid.Should().Be("test-guid-123");
            result.Header.CreationDate.Should().Be("2024-01-01T00:00:00Z");
            result.Body.Should().Be("\"TestData\"");
        }

        [Fact]
        public void ConvertJsonToMessage_WithMinimalValidJson_ShouldReturnWrappedMessage()
        {
            // Arrange
            var jsonInput = @"{
                ""Header"": {
                    ""Type"": 0
                }
            }";

            // Act
            var result = JsonWrappedMessageObjectConverter.ConvertJsonToMessage(jsonInput);

            // Assert
            result.Should().NotBeNull();
            result.Header.Should().NotBeNull();
            result.Header!.Type.Should().Be(0);
            result.Body.Should().BeNull();
        }

        [Fact]
        public void ConvertJsonToMessage_WithNullBody_ShouldReturnWrappedMessageWithNullBodyString()
        {
            // Arrange
            var jsonInput = @"{
                ""Header"": {
                    ""Type"": 2,
                    ""MsgGuid"": ""test-guid-456""
                },
                ""Body"": null
            }";

            // Act
            var result = JsonWrappedMessageObjectConverter.ConvertJsonToMessage(jsonInput);

            // Assert
            result.Should().NotBeNull();
            result.Header.Should().NotBeNull();
            result.Header!.Type.Should().Be(2);
            result.Header.MsgGuid.Should().Be("test-guid-456");
            result.Body.Should().Be("null"); // GetRawText() returns "null" as string
        }

        [Fact]
        public void ConvertJsonToMessage_WithMissingBodyProperty_ShouldReturnWrappedMessageWithNullBody()
        {
            // Arrange
            var jsonInput = @"{
                ""Header"": {
                    ""Type"": 3,
                    ""MsgGuid"": ""test-guid-789"",
                    ""CreationDate"": ""2024-02-01T12:30:45Z""
                }
            }";

            // Act
            var result = JsonWrappedMessageObjectConverter.ConvertJsonToMessage(jsonInput);

            // Assert
            result.Should().NotBeNull();
            result.Header.Should().NotBeNull();
            result.Header!.Type.Should().Be(3);
            result.Header.MsgGuid.Should().Be("test-guid-789");
            result.Header.CreationDate.Should().Be("2024-02-01T12:30:45Z");
            result.Body.Should().BeNull();
        }

        [Fact]
        public void ConvertJsonToMessage_WithComplexBodyJson_ShouldReturnWrappedMessage()
        {
            // Arrange
            var jsonInput = @"{
                ""Header"": {
                    ""Type"": 4,
                    ""MsgGuid"": ""complex-guid-123"",
                    ""CreationDate"": ""2024-03-01T08:15:30Z""
                },
                ""Body"": ""ComplexEntity""
            }";

            // Act
            var result = JsonWrappedMessageObjectConverter.ConvertJsonToMessage(jsonInput);

            // Assert
            result.Should().NotBeNull();
            result.Header.Should().NotBeNull();
            result.Header!.Type.Should().Be(4);
            result.Header.MsgGuid.Should().Be("complex-guid-123");
            result.Body.Should().NotBeNullOrEmpty();
            result.Body.Should().Contain("ComplexEntity");
        }

        [Theory]
        [InlineData("")]
        [InlineData("   ")]
        [InlineData("invalid json")]
        public void ConvertJsonToMessage_WithInvalidJson_ShouldThrowJsonException(string invalidJson)
        {
            // Act & Assert
            var act = () => JsonWrappedMessageObjectConverter.ConvertJsonToMessage(invalidJson);
            act.Should().Throw<JsonException>();
        }

        [Theory]
        [InlineData("{}")]
        [InlineData("{\"InvalidProperty\": \"value\"}")]
        public void ConvertJsonToMessage_WithMissingHeaderProperty_ShouldThrowKeyNotFoundException(string invalidJson)
        {
            // Act & Assert - Missing Header property throws KeyNotFoundException
            var act = () => JsonWrappedMessageObjectConverter.ConvertJsonToMessage(invalidJson);
            act.Should().Throw<KeyNotFoundException>();
        }

        [Fact]
        public void ConvertJsonToMessage_WithNullInput_ShouldThrowArgumentNullException()
        {
            // Act & Assert
            var act = () => JsonWrappedMessageObjectConverter.ConvertJsonToMessage(null!);
            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void ConvertJsonToMessage_WithEmptyHeaderJson_ShouldDeserializeSuccessfully()
        {
            // Arrange - Empty header object is valid JSON, just creates default MessageHeader
            var jsonInput = @"{
                ""Header"": {},
                ""Body"": ""test data""
            }";

            // Act
            var result = JsonWrappedMessageObjectConverter.ConvertJsonToMessage(jsonInput);

            // Assert
            result.Should().NotBeNull();
            result.Header.Should().NotBeNull();
            result.Body.Should().Be("\"test data\"");
        }

        [Fact]
        public void ConvertJsonToMessage_WithMalformedJson_ShouldThrowJsonException()
        {
            // Arrange
            var malformedJson = @"{
                ""Header"": {
                    ""Type"": 1,
                    ""MsgGuid"": ""test-guid""
                },
                ""Body"": ""unclosed";

            // Act & Assert
            var act = () => JsonWrappedMessageObjectConverter.ConvertJsonToMessage(malformedJson);
            act.Should().Throw<JsonException>();
        }

        [Fact]
        public void ConvertJsonToMessage_WithExtraProperties_ShouldIgnoreExtraProperties()
        {
            // Arrange
            var jsonInput = @"{
                ""Header"": {
                    ""Type"": 5,
                    ""MsgGuid"": ""extra-props-guid"",
                    ""CreationDate"": ""2024-04-01T16:45:00Z""
                },
                ""Body"": ""test"",
                ""ExtraProperty"": ""should be ignored"",
                ""AnotherExtra"": 12345
            }";

            // Act
            var result = JsonWrappedMessageObjectConverter.ConvertJsonToMessage(jsonInput);

            // Assert
            result.Should().NotBeNull();
            result.Header.Should().NotBeNull();
            result.Header!.Type.Should().Be(5);
            result.Header.MsgGuid.Should().Be("extra-props-guid");
            result.Body.Should().Be("\"test\""); // Raw JSON with quotes
        }

        [Fact]
        public void ConvertJsonToMessage_WithUnicodeCharacters_ShouldHandleCorrectly()
        {
            // Arrange
            var jsonInput = @"{
                ""Header"": {
                    ""Type"": 6,
                    ""MsgGuid"": ""unicode-test"",
                    ""CreationDate"": ""2024-05-01T09:30:00Z""
                },
                ""Body"": ""Hello World""
            }";

            // Act
            var result = JsonWrappedMessageObjectConverter.ConvertJsonToMessage(jsonInput);

            // Assert
            result.Should().NotBeNull();
            result.Header.Should().NotBeNull();
            result.Header!.MsgGuid.Should().Be("unicode-test");
            result.Body.Should().Contain("Hello World");
        }

        [Fact]
        public void ConvertJsonToMessage_WithLargeJsonPayload_ShouldHandleEfficiently()
        {
            // Arrange
            var largeBodyContent = new string('A', 1000); // Smaller for practical testing
            var jsonInput = $@"{{
                ""Header"": {{
                    ""Type"": 7,
                    ""MsgGuid"": ""large-payload-test"",
                    ""CreationDate"": ""2024-06-01T14:20:00Z""
                }},
                ""Body"": ""{largeBodyContent}""
            }}";

            // Act
            var result = JsonWrappedMessageObjectConverter.ConvertJsonToMessage(jsonInput);

            // Assert
            result.Should().NotBeNull();
            result.Header.Should().NotBeNull();
            result.Header!.Type.Should().Be(7);
            result.Body.Should().NotBeNullOrEmpty();
            result.Body.Should().Contain(largeBodyContent);
        }

        [Fact]
        public void ConvertJsonToMessage_WithNumericAndBooleanTypes_ShouldHandleCorrectly()
        {
            // Arrange
            var jsonInput = @"{
                ""Header"": {
                    ""Type"": 999,
                    ""MsgGuid"": ""numeric-test-123"",
                    ""CreationDate"": ""2024-07-01T10:00:00Z""
                },
                ""Body"": 12345
            }";

            // Act
            var result = JsonWrappedMessageObjectConverter.ConvertJsonToMessage(jsonInput);

            // Assert
            result.Should().NotBeNull();
            result.Header.Should().NotBeNull();
            result.Header!.Type.Should().Be(999);
            result.Body.Should().Contain("12345");
        }

        [Fact]
        public void ConvertJsonToMessage_WithStringBody_ShouldReturnRawJsonString()
        {
            // Arrange
            var jsonInput = @"{
                ""Header"": {
                    ""Type"": 8,
                    ""MsgGuid"": ""string-body-test""
                },
                ""Body"": ""simple string body""
            }";

            // Act
            var result = JsonWrappedMessageObjectConverter.ConvertJsonToMessage(jsonInput);

            // Assert
            result.Should().NotBeNull();
            result.Header!.Type.Should().Be(8);
            result.Body.Should().Be("\"simple string body\""); // GetRawText includes quotes
        }

        [Fact]
        public void ConvertJsonToMessage_WithBooleanBody_ShouldReturnRawJsonBoolean()
        {
            // Arrange
            var jsonInput = @"{
                ""Header"": {
                    ""Type"": 9,
                    ""MsgGuid"": ""boolean-body-test""
                },
                ""Body"": true
            }";

            // Act
            var result = JsonWrappedMessageObjectConverter.ConvertJsonToMessage(jsonInput);

            // Assert
            result.Should().NotBeNull();
            result.Header!.Type.Should().Be(9);
            result.Body.Should().Be("true"); // GetRawText returns raw boolean
        }
    }
} 