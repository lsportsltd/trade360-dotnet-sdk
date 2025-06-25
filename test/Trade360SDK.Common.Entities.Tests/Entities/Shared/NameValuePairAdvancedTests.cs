using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Xunit;
using Trade360SDK.Common.Entities.Shared;

namespace Trade360SDK.Common.Entities.Tests.Entities.Shared
{
    public class NameValuePairAdvancedTests
    {
        [Fact]
        public void NameValuePair_Constructor_ShouldInitializeWithNullValues()
        {
            // Act
            var pair = new NameValuePair();

            // Assert
            pair.Should().NotBeNull();
            pair.Name.Should().BeNull();
            pair.Value.Should().BeNull();
        }

        [Fact]
        public void NameValuePair_SetName_ShouldAcceptValidString()
        {
            // Arrange
            var pair = new NameValuePair();
            var testName = "ConfigurationKey";

            // Act
            pair.Name = testName;

            // Assert
            pair.Name.Should().Be(testName);
        }

        [Fact]
        public void NameValuePair_SetValue_ShouldAcceptValidString()
        {
            // Arrange
            var pair = new NameValuePair();
            var testValue = "ConfigurationValue";

            // Act
            pair.Value = testValue;

            // Assert
            pair.Value.Should().Be(testValue);
        }

        [Theory]
        [InlineData("", "")]
        [InlineData("   ", "   ")]
        [InlineData("TestName", "TestValue")]
        [InlineData("123", "456")]
        [InlineData("true", "false")]
        [InlineData("Special!@#$%", "Special&*()")]
        public void NameValuePair_SetBothProperties_ShouldStoreValues(string name, string value)
        {
            // Arrange
            var pair = new NameValuePair();

            // Act
            pair.Name = name;
            pair.Value = value;

            // Assert
            pair.Name.Should().Be(name);
            pair.Value.Should().Be(value);
        }

        [Fact]
        public void NameValuePair_SetNullName_ShouldAcceptNull()
        {
            // Arrange
            var pair = new NameValuePair { Name = "InitialName" };

            // Act
            pair.Name = null;

            // Assert
            pair.Name.Should().BeNull();
        }

        [Fact]
        public void NameValuePair_SetNullValue_ShouldAcceptNull()
        {
            // Arrange
            var pair = new NameValuePair { Value = "InitialValue" };

            // Act
            pair.Value = null;

            // Assert
            pair.Value.Should().BeNull();
        }

        [Fact]
        public void NameValuePair_WithLongStrings_ShouldHandleCorrectly()
        {
            // Arrange
            var pair = new NameValuePair();
            var longName = new string('A', 1000);
            var longValue = new string('B', 1000);

            // Act
            pair.Name = longName;
            pair.Value = longValue;

            // Assert
            pair.Name.Should().Be(longName);
            pair.Value.Should().Be(longValue);
            pair.Name.Length.Should().Be(1000);
            pair.Value.Length.Should().Be(1000);
        }

        [Fact]
        public void NameValuePair_WithSpecialCharacters_ShouldPreserveCharacters()
        {
            // Arrange
            var pair = new NameValuePair();
            var nameWithSpecial = "Test\nName\tWith\rSpecial";
            var valueWithSpecial = "Test\nValue\tWith\rSpecial";

            // Act
            pair.Name = nameWithSpecial;
            pair.Value = valueWithSpecial;

            // Assert
            pair.Name.Should().Be(nameWithSpecial);
            pair.Value.Should().Be(valueWithSpecial);
        }

        [Fact]
        public void NameValuePair_WithUnicodeCharacters_ShouldPreserveUnicode()
        {
            // Arrange
            var pair = new NameValuePair();
            var unicodeName = "测试名称";
            var unicodeValue = "测试值";

            // Act
            pair.Name = unicodeName;
            pair.Value = unicodeValue;

            // Assert
            pair.Name.Should().Be(unicodeName);
            pair.Value.Should().Be(unicodeValue);
        }

        [Fact]
        public void NameValuePair_WithEmptyStrings_ShouldAcceptEmpty()
        {
            // Arrange
            var pair = new NameValuePair();

            // Act
            pair.Name = string.Empty;
            pair.Value = string.Empty;

            // Assert
            pair.Name.Should().Be(string.Empty);
            pair.Value.Should().Be(string.Empty);
        }

        [Fact]
        public void NameValuePair_WithWhitespaceStrings_ShouldPreserveWhitespace()
        {
            // Arrange
            var pair = new NameValuePair();
            var whitespaceName = "   ";
            var whitespaceValue = "\t\t";

            // Act
            pair.Name = whitespaceName;
            pair.Value = whitespaceValue;

            // Assert
            pair.Name.Should().Be(whitespaceName);
            pair.Value.Should().Be(whitespaceValue);
        }

        [Fact]
        public void NameValuePair_PropertyAccessMultipleTimes_ShouldReturnSameValue()
        {
            // Arrange
            var pair = new NameValuePair
            {
                Name = "ConsistentName",
                Value = "ConsistentValue"
            };

            // Act & Assert
            pair.Name.Should().Be("ConsistentName");
            pair.Name.Should().Be("ConsistentName");
            pair.Value.Should().Be("ConsistentValue");
            pair.Value.Should().Be("ConsistentValue");
        }



        [Fact]
        public void NameValuePair_UsedInCollection_ShouldWork()
        {
            // Arrange
            var pairs = new List<NameValuePair>
            {
                new NameValuePair { Name = "Key1", Value = "Value1" },
                new NameValuePair { Name = "Key2", Value = "Value2" },
                new NameValuePair { Name = "Key3", Value = "Value3" }
            };

            // Act & Assert
            pairs.Should().HaveCount(3);
            pairs.First().Name.Should().Be("Key1");
            pairs.First().Value.Should().Be("Value1");
            pairs.Last().Name.Should().Be("Key3");
            pairs.Last().Value.Should().Be("Value3");
        }

        [Fact]
        public void NameValuePair_WithConfigurationScenario_ShouldWorkCorrectly()
        {
            // Arrange
            var connectionString = new NameValuePair
            {
                Name = "ConnectionString",
                Value = "Server=localhost;Database=TestDB;Trusted_Connection=true;"
            };

            var apiKey = new NameValuePair
            {
                Name = "ApiKey",
                Value = "abc123def456ghi789"
            };

            var timeout = new NameValuePair
            {
                Name = "Timeout",
                Value = "30"
            };

            // Act
            var config = new List<NameValuePair> { connectionString, apiKey, timeout };

            // Assert
            config.Should().HaveCount(3);
            config.Should().Contain(p => p.Name == "ConnectionString");
            config.Should().Contain(p => p.Name == "ApiKey");
            config.Should().Contain(p => p.Name == "Timeout");
        }

        [Fact]
        public void NameValuePair_WithBusinessLogicScenario_ShouldSupportVariousDataTypes()
        {
            // Arrange & Act
            var settings = new List<NameValuePair>
            {
                new NameValuePair { Name = "MaxRetries", Value = "3" },
                new NameValuePair { Name = "EnableLogging", Value = "true" },
                new NameValuePair { Name = "ApiBaseUrl", Value = "https://api.trade360.com" },
                new NameValuePair { Name = "Version", Value = "1.0.0" },
                new NameValuePair { Name = "LastModified", Value = DateTime.UtcNow.ToString() }
            };

            // Assert
            settings.Should().HaveCount(5);
            settings.Should().Contain(s => s.Name == "MaxRetries" && s.Value == "3");
            settings.Should().Contain(s => s.Name == "EnableLogging" && s.Value == "true");
            settings.Should().Contain(s => s.Name == "ApiBaseUrl" && s.Value == "https://api.trade360.com");
        }
    }
} 