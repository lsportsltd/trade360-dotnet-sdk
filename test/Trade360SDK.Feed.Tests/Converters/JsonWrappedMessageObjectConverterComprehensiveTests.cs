using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using FluentAssertions;
using Trade360SDK.Common.Models;
using Trade360SDK.Feed.Converters;
using Xunit;

namespace Trade360SDK.Feed.Tests.Converters
{
    public class JsonWrappedMessageObjectConverterComprehensiveTests
    {
        #region Valid JSON Parsing Tests

        [Fact]
        public void ConvertJsonToMessage_WithCompleteValidJson_ShouldParseAllFields()
        {
            // Arrange
            var json = """
            {
                "Header": {
                    "CreationDate": "2024-01-01T10:30:00Z",
                    "Type": 3,
                    "MsgSeq": 12345,
                    "MsgGuid": "550e8400-e29b-41d4-a716-446655440000",
                    "ServerTimestamp": 1704105000000,
                    "MessageBrokerTimestamp": "2024-01-01T10:30:00Z",
                    "MessageTimestamp": "2024-01-01T10:30:00Z"
                },
                "Body": {
                    "Events": [
                        {
                            "Id": 12345,
                            "Status": 1,
                            "Markets": []
                        }
                    ]
                }
            }
            """;

            // Act
            var result = JsonWrappedMessageObjectConverter.ConvertJsonToMessage(json);

            // Assert
            result.Should().NotBeNull();
            result.Header.Should().NotBeNull();
            result.Header.CreationDate.Should().Be("2024-01-01T10:30:00Z");
            result.Header.Type.Should().Be(3);
            result.Header.MsgSeq.Should().Be(12345);
            result.Header.MsgGuid.Should().Be("550e8400-e29b-41d4-a716-446655440000");
            result.Header.ServerTimestamp.Should().Be(1704105000000);
            result.Body.Should().NotBeNull();
            result.Body.Should().Contain("Events");
            result.Body.Should().Contain("12345");
        }

        [Fact]
        public void ConvertJsonToMessage_WithMinimalHeader_ShouldParseRequiredFields()
        {
            // Arrange
            var json = """
            {
                "Header": {
                    "Type": 1
                }
            }
            """;

            // Act
            var result = JsonWrappedMessageObjectConverter.ConvertJsonToMessage(json);

            // Assert
            result.Should().NotBeNull();
            result.Header.Should().NotBeNull();
            result.Header.Type.Should().Be(1);
            result.Header.CreationDate.Should().BeNull();
            result.Header.MsgSeq.Should().BeNull();
            result.Header.MsgGuid.Should().BeNull();
            result.Header.ServerTimestamp.Should().BeNull();
            result.Body.Should().BeNull();
        }

        [Fact]
        public void ConvertJsonToMessage_WithStringBody_ShouldPreserveStringContent()
        {
            // Arrange
            var json = """
            {
                "Header": {
                    "Type": 2,
                    "CreationDate": "2024-01-01T12:00:00Z"
                },
                "Body": "Simple string body content"
            }
            """;

            // Act
            var result = JsonWrappedMessageObjectConverter.ConvertJsonToMessage(json);

            // Assert
            result.Should().NotBeNull();
            result.Header.Should().NotBeNull();
            result.Header.Type.Should().Be(2);
            result.Header.CreationDate.Should().Be("2024-01-01T12:00:00Z");
            result.Body.Should().Be("\"Simple string body content\"");
        }

        [Fact]
        public void ConvertJsonToMessage_WithComplexNestedBody_ShouldPreserveStructure()
        {
            // Arrange
            var json = """
            {
                "Header": {
                    "Type": 3,
                    "MsgSeq": 999
                },
                "Body": {
                    "Events": [
                        {
                            "Id": 12345,
                            "Status": 1,
                            "Markets": [
                                {
                                    "Id": 67890,
                                    "Name": "Match Winner",
                                    "Bets": [
                                        {
                                            "Id": 111,
                                            "Name": "Team A",
                                            "Price": "2.50"
                                        }
                                    ]
                                }
                            ]
                        }
                    ]
                }
            }
            """;

            // Act
            var result = JsonWrappedMessageObjectConverter.ConvertJsonToMessage(json);

            // Assert
            result.Should().NotBeNull();
            result.Header.Type.Should().Be(3);
            result.Header.MsgSeq.Should().Be(999);
            result.Body.Should().NotBeNull();
            result.Body.Should().Contain("Events");
            result.Body.Should().Contain("Markets");
            result.Body.Should().Contain("Bets");
            result.Body.Should().Contain("12345");
            result.Body.Should().Contain("67890");
            result.Body.Should().Contain("Team A");
            result.Body.Should().Contain("2.50");
        }

        [Fact]
        public void ConvertJsonToMessage_WithNullValues_ShouldHandleNullsCorrectly()
        {
            // Arrange
            var json = """
            {
                "Header": {
                    "Type": 1,
                    "CreationDate": null,
                    "MsgGuid": null,
                    "ServerTimestamp": null
                },
                "Body": null
            }
            """;

            // Act
            var result = JsonWrappedMessageObjectConverter.ConvertJsonToMessage(json);

            // Assert
            result.Should().NotBeNull();
            result.Header.Should().NotBeNull();
            result.Header.Type.Should().Be(1);
            result.Header.CreationDate.Should().BeNull();
            result.Header.MsgGuid.Should().BeNull();
            result.Header.ServerTimestamp.Should().BeNull();
            result.Body.Should().Be("null");
        }

        [Fact]
        public void ConvertJsonToMessage_WithEmptyBodyObject_ShouldParseEmptyBody()
        {
            // Arrange
            var json = """
            {
                "Header": {
                    "Type": 31
                },
                "Body": {}
            }
            """;

            // Act
            var result = JsonWrappedMessageObjectConverter.ConvertJsonToMessage(json);

            // Assert
            result.Should().NotBeNull();
            result.Header.Type.Should().Be(31);
            result.Body.Should().Be("{}");
        }

        [Fact]
        public void ConvertJsonToMessage_WithSpecialCharacters_ShouldPreserveCharacters()
        {
            // Arrange
            var json = """
            {
                "Header": {
                    "Type": 1
                },
                "Body": {
                    "message": "Special chars: éñ中文 @#$%^&*()",
                    "unicode": "🚀🎉⚽",
                    "escaped": "Line1\nLine2\tTabbed"
                }
            }
            """;

            // Act
            var result = JsonWrappedMessageObjectConverter.ConvertJsonToMessage(json);

            // Assert
            result.Should().NotBeNull();
            result.Body.Should().Contain("éñ中文");
            result.Body.Should().Contain("@#$%^&*()");
            result.Body.Should().Contain("🚀🎉⚽");
            result.Body.Should().Contain("\\n");
            result.Body.Should().Contain("\\t");
        }

        [Fact]
        public void ConvertJsonToMessage_WithArrayBody_ShouldParseArrayCorrectly()
        {
            // Arrange
            var json = """
            {
                "Header": {
                    "Type": 2
                },
                "Body": [
                    {"id": 1, "name": "Item 1"},
                    {"id": 2, "name": "Item 2"},
                    {"id": 3, "name": "Item 3"}
                ]
            }
            """;

            // Act
            var result = JsonWrappedMessageObjectConverter.ConvertJsonToMessage(json);

            // Assert
            result.Should().NotBeNull();
            result.Header.Type.Should().Be(2);
            result.Body.Should().StartWith("[");
            result.Body.Should().EndWith("]");
            result.Body.Should().Contain("Item 1");
            result.Body.Should().Contain("Item 2");
            result.Body.Should().Contain("Item 3");
        }

        [Fact]
        public void ConvertJsonToMessage_WithLargeComplexMessage_ShouldHandleEfficiently()
        {
            // Arrange - Create a large message with multiple events and markets
            var events = new List<string>();
            for (int i = 1; i <= 50; i++)
            {
                events.Add($@"{{
                    ""Id"": {i},
                    ""Status"": 1,
                    ""Markets"": [
                        {{
                            ""Id"": {i * 100},
                            ""Name"": ""Market {i}"",
                            ""Bets"": [
                                {{""Id"": {i * 1000}, ""Name"": ""Bet {i}A"", ""Price"": ""1.{i:D2}""}},
                                {{""Id"": {i * 1000 + 1}, ""Name"": ""Bet {i}B"", ""Price"": ""2.{i:D2}""}}
                            ]
                        }}
                    ]
                }}");
            }
            var eventsJson = string.Join(",", events);

            var json = $@"{{
                ""Header"": {{
                    ""Type"": 3,
                    ""CreationDate"": ""2024-01-01T10:30:00Z"",
                    ""MsgSeq"": 12345
                }},
                ""Body"": {{
                    ""Events"": [{eventsJson}]
                }}
            }}";

            // Act
            var result = JsonWrappedMessageObjectConverter.ConvertJsonToMessage(json);

            // Assert
            result.Should().NotBeNull();
            result.Header.Type.Should().Be(3);
            result.Body.Should().NotBeNull();
            result.Body.Should().Contain("Events");
            result.Body.Should().Contain("Market 1");
            result.Body.Should().Contain("Market 50");
            result.Body.Should().Contain("Bet 25A");
        }

        #endregion

        #region Error Handling Tests

        [Fact]
        public void ConvertJsonToMessage_WithInvalidJson_ShouldThrowJsonException()
        {
            // Arrange
            var invalidJson = "{ invalid json structure without closing brace";

            // Act & Assert
            var act = () => JsonWrappedMessageObjectConverter.ConvertJsonToMessage(invalidJson);
            act.Should().Throw<JsonException>();
        }

        [Fact]
        public void ConvertJsonToMessage_WithMissingHeader_ShouldThrowKeyNotFoundException()
        {
            // Arrange
            var json = """
            {
                "Body": {
                    "message": "test"
                }
            }
            """;

            // Act & Assert
            var act = () => JsonWrappedMessageObjectConverter.ConvertJsonToMessage(json);
            act.Should().Throw<KeyNotFoundException>();
        }

        [Fact]
        public void ConvertJsonToMessage_WithEmptyString_ShouldThrowJsonException()
        {
            // Act & Assert
            var act = () => JsonWrappedMessageObjectConverter.ConvertJsonToMessage("");
            act.Should().Throw<JsonException>();
        }

        [Fact]
        public void ConvertJsonToMessage_WithNullString_ShouldThrowArgumentNullException()
        {
            // Act & Assert
            var act = () => JsonWrappedMessageObjectConverter.ConvertJsonToMessage(null!);
            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void ConvertJsonToMessage_WithWhitespaceOnly_ShouldThrowJsonException()
        {
            // Act & Assert
            var act = () => JsonWrappedMessageObjectConverter.ConvertJsonToMessage("   \t\n  ");
            act.Should().Throw<JsonException>();
        }

        [Fact]
        public void ConvertJsonToMessage_WithHeaderAsArray_ShouldThrowJsonException()
        {
            // Arrange
            var json = """
            {
                "Header": [1, 2, 3],
                "Body": {}
            }
            """;

            // Act & Assert
            var act = () => JsonWrappedMessageObjectConverter.ConvertJsonToMessage(json);
            act.Should().Throw<JsonException>();
        }

        [Fact]
        public void ConvertJsonToMessage_WithMalformedJson_ShouldThrowJsonException()
        {
            // Arrange
            var malformedJson = """
            {
                "Header": {
                    "Type": 1,
                    "CreationDate": "2024-01-01T10:30:00Z"
                },
                "Body": {
                    "Events": [
                        {
                            "Id": 12345,
                            "Status": 1,
                            "Markets": [
                                {
                                    "Id": 67890,
                                    "Name": "Match Winner",
                                    "Bets": [
                                        {
                                            "Id": 111,
                                            "Name": "Team A",
                                            "Price": "2.50"
                                        }
                                    ]
                                }
                            ]
                        }
                    ]
                }
            """;

            // Act & Assert
            var act = () => JsonWrappedMessageObjectConverter.ConvertJsonToMessage(malformedJson);
            act.Should().Throw<JsonException>();
        }

        [Fact]
        public void ConvertJsonToMessage_WithInvalidHeaderStructure_ShouldThrowJsonException()
        {
            // Arrange
            var json = """
            {
                "Header": "invalid header structure",
                "Body": {}
            }
            """;

            // Act & Assert
            var act = () => JsonWrappedMessageObjectConverter.ConvertJsonToMessage(json);
            act.Should().Throw<JsonException>();
        }

        #endregion

        #region Data Type Tests

        [Fact]
        public void ConvertJsonToMessage_WithDateTimeFormats_ShouldPreserveDateTimeStrings()
        {
            // Arrange
            var json = """
            {
                "Header": {
                    "Type": 1,
                    "CreationDate": "2024-12-25T15:30:45.123Z",
                    "MessageBrokerTimestamp": "2024-12-25T15:30:45.123Z",
                    "MessageTimestamp": "2024-12-25T15:30:45.123Z"
                },
                "Body": {
                    "timestamp": "2024-12-25T15:30:45.123Z",
                    "lastUpdate": "2024-12-25T15:30:45.123Z"
                }
            }
            """;

            // Act
            var result = JsonWrappedMessageObjectConverter.ConvertJsonToMessage(json);

            // Assert
            result.Should().NotBeNull();
            result.Header.CreationDate.Should().Be("2024-12-25T15:30:45.123Z");
            result.Body.Should().Contain("2024-12-25T15:30:45.123Z");
        }

        [Fact]
        public void ConvertJsonToMessage_WithNumericValues_ShouldPreserveNumericTypes()
        {
            // Arrange
            var json = """
            {
                "Header": {
                    "Type": 42,
                    "MsgSeq": 999999,
                    "ServerTimestamp": 1704105000000
                },
                "Body": {
                    "intValue": 12345,
                    "floatValue": 123.45,
                    "negativeValue": -67890,
                    "zeroValue": 0,
                    "scientificNotation": 1.23e10
                }
            }
            """;

            // Act
            var result = JsonWrappedMessageObjectConverter.ConvertJsonToMessage(json);

            // Assert
            result.Should().NotBeNull();
            result.Header.Type.Should().Be(42);
            result.Header.MsgSeq.Should().Be(999999);
            result.Header.ServerTimestamp.Should().Be(1704105000000);
            result.Body.Should().Contain("12345");
            result.Body.Should().Contain("123.45");
            result.Body.Should().Contain("-67890");
            result.Body.Should().Contain("1.23e10");
        }

        [Fact]
        public void ConvertJsonToMessage_WithBooleanValues_ShouldPreserveBooleans()
        {
            // Arrange
            var json = """
            {
                "Header": {
                    "Type": 1
                },
                "Body": {
                    "isActive": true,
                    "isComplete": false,
                    "settings": {
                        "autoAck": true,
                        "retryEnabled": false
                    }
                }
            }
            """;

            // Act
            var result = JsonWrappedMessageObjectConverter.ConvertJsonToMessage(json);

            // Assert
            result.Should().NotBeNull();
            result.Body.Should().Contain("true");
            result.Body.Should().Contain("false");
            result.Body.Should().Contain("autoAck");
            result.Body.Should().Contain("retryEnabled");
        }

        [Fact]
        public void ConvertJsonToMessage_WithBoundaryValues_ShouldHandleCorrectly()
        {
            // Arrange
            var json = $@"{{
                ""Header"": {{
                    ""Type"": {int.MaxValue},
                    ""MsgSeq"": {int.MinValue},
                    ""ServerTimestamp"": {long.MaxValue}
                }},
                ""Body"": {{
                    ""maxInt"": {int.MaxValue},
                    ""minInt"": {int.MinValue},
                    ""maxLong"": {long.MaxValue},
                    ""minLong"": {long.MinValue}
                }}
            }}";

            // Act
            var result = JsonWrappedMessageObjectConverter.ConvertJsonToMessage(json);

            // Assert
            result.Should().NotBeNull();
            result.Header.Type.Should().Be(int.MaxValue);
            result.Header.MsgSeq.Should().Be(int.MinValue);
            result.Header.ServerTimestamp.Should().Be(long.MaxValue);
            result.Body.Should().Contain(int.MaxValue.ToString());
            result.Body.Should().Contain(int.MinValue.ToString());
            result.Body.Should().Contain(long.MaxValue.ToString());
            result.Body.Should().Contain(long.MinValue.ToString());
        }

        #endregion

        #region Edge Cases and Performance Tests

        [Fact]
        public void ConvertJsonToMessage_WithVeryLargeStringValues_ShouldHandleCorrectly()
        {
            // Arrange
            var largeString = new string('A', 10000);
            var json = $@"{{
                ""Header"": {{
                    ""Type"": 1,
                    ""CreationDate"": ""{largeString}""
                }},
                ""Body"": {{
                    ""largeValue"": ""{largeString}""
                }}
            }}";

            // Act
            var result = JsonWrappedMessageObjectConverter.ConvertJsonToMessage(json);

            // Assert
            result.Should().NotBeNull();
            result.Header.CreationDate.Should().Be(largeString);
            result.Body.Should().Contain(largeString);
        }

        [Fact]
        public void ConvertJsonToMessage_WithNestedArraysAndObjects_ShouldPreserveStructure()
        {
            // Arrange
            var json = """
            {
                "Header": {
                    "Type": 3
                },
                "Body": {
                    "level1": {
                        "level2": {
                            "level3": {
                                "arrays": [
                                    [1, 2, 3],
                                    [4, 5, 6],
                                    [7, 8, 9]
                                ],
                                "objects": [
                                    {"a": 1, "b": {"c": 2}},
                                    {"d": 3, "e": {"f": 4}}
                                ]
                            }
                        }
                    }
                }
            }
            """;

            // Act
            var result = JsonWrappedMessageObjectConverter.ConvertJsonToMessage(json);

            // Assert
            result.Should().NotBeNull();
            result.Body.Should().Contain("level1");
            result.Body.Should().Contain("level2");
            result.Body.Should().Contain("level3");
            result.Body.Should().Contain("arrays");
            result.Body.Should().Contain("objects");
            result.Body.Should().Contain("1").And.Contain("2").And.Contain("3");
            result.Body.Should().Contain("\"a\"").And.Contain("1");
            result.Body.Should().Contain("\"c\"").And.Contain("2");
        }

        [Fact]
        public void ConvertJsonToMessage_WithRepeatedCalls_ShouldReturnConsistentResults()
        {
            // Arrange
            var json = """
            {
                "Header": {
                    "Type": 1,
                    "MsgSeq": 12345
                },
                "Body": {
                    "test": "value"
                }
            }
            """;

            // Act
            var result1 = JsonWrappedMessageObjectConverter.ConvertJsonToMessage(json);
            var result2 = JsonWrappedMessageObjectConverter.ConvertJsonToMessage(json);
            var result3 = JsonWrappedMessageObjectConverter.ConvertJsonToMessage(json);

            // Assert
            result1.Should().NotBeNull();
            result2.Should().NotBeNull();
            result3.Should().NotBeNull();
            
            result1.Header.Type.Should().Be(result2.Header.Type);
            result2.Header.Type.Should().Be(result3.Header.Type);
            result1.Header.MsgSeq.Should().Be(result2.Header.MsgSeq);
            result2.Header.MsgSeq.Should().Be(result3.Header.MsgSeq);
            result1.Body.Should().Be(result2.Body);
            result2.Body.Should().Be(result3.Body);
        }

        [Fact]
        public void ConvertJsonToMessage_WithMultipleMessageTypes_ShouldHandleAllTypes()
        {
            // Arrange
            var messageTypes = new[] { 1, 2, 3, 31, 32, 35, 38, 40 };
            
            foreach (var messageType in messageTypes)
            {
                var json = $@"{{
                    ""Header"": {{
                        ""Type"": {messageType},
                        ""MsgSeq"": {messageType * 100}
                    }},
                    ""Body"": {{
                        ""messageType"": {messageType},
                        ""data"": ""test data for type {messageType}""
                    }}
                }}";

                // Act
                var result = JsonWrappedMessageObjectConverter.ConvertJsonToMessage(json);

                // Assert
                result.Should().NotBeNull($"Message type {messageType} should be parsed");
                result.Header.Type.Should().Be(messageType);
                result.Header.MsgSeq.Should().Be(messageType * 100);
                result.Body.Should().Contain($"test data for type {messageType}");
            }
        }

        [Fact]
        public void ConvertJsonToMessage_WithMissingBodyProperty_ShouldReturnNullBody()
        {
            // Arrange
            var json = """
            {
                "Header": {
                    "Type": 1,
                    "MsgSeq": 12345
                }
            }
            """;

            // Act
            var result = JsonWrappedMessageObjectConverter.ConvertJsonToMessage(json);

            // Assert
            result.Should().NotBeNull();
            result.Header.Should().NotBeNull();
            result.Header.Type.Should().Be(1);
            result.Header.MsgSeq.Should().Be(12345);
            result.Body.Should().BeNull();
        }

        [Fact]
        public void ConvertJsonToMessage_WithEmptyHeaderProperties_ShouldHandleGracefully()
        {
            // Arrange
            var json = """
            {
                "Header": {
                    "Type": 0,
                    "CreationDate": "",
                    "MsgGuid": "",
                    "MsgSeq": 0,
                    "ServerTimestamp": 0
                },
                "Body": ""
            }
            """;

            // Act
            var result = JsonWrappedMessageObjectConverter.ConvertJsonToMessage(json);

            // Assert
            result.Should().NotBeNull();
            result.Header.Should().NotBeNull();
            result.Header.Type.Should().Be(0);
            result.Header.CreationDate.Should().Be("");
            result.Header.MsgGuid.Should().Be("");
            result.Header.MsgSeq.Should().Be(0);
            result.Header.ServerTimestamp.Should().Be(0);
            result.Body.Should().Be("\"\"");
        }

        #endregion
    }
} 