using System;
using FluentAssertions;
using Xunit;
using Trade360SDK.Common.Entities.Shared;

namespace Trade360SDK.Common.Entities.Tests.Entities.Shared
{
    public class NameValuePairComprehensiveTests
    {
        [Fact]
        public void NameValuePair_DefaultConstructor_ShouldCreateInstanceWithNullProperties()
        {
            // Act
            var pair = new NameValuePair();

            // Assert
            pair.Should().NotBeNull();
            pair.Name.Should().BeNull();
            pair.Value.Should().BeNull();
        }

        [Fact]
        public void NameValuePair_SetName_ShouldSetValue()
        {
            // Arrange
            var pair = new NameValuePair();
            var name = "TestName";

            // Act
            pair.Name = name;

            // Assert
            pair.Name.Should().Be(name);
        }

        [Fact]
        public void NameValuePair_SetValue_ShouldSetValue()
        {
            // Arrange
            var pair = new NameValuePair();
            var value = "TestValue";

            // Act
            pair.Value = value;

            // Assert
            pair.Value.Should().Be(value);
        }

        [Fact]
        public void NameValuePair_SetBothProperties_ShouldSetBothValues()
        {
            // Arrange
            var pair = new NameValuePair();
            var name = "Configuration";
            var value = "Production";

            // Act
            pair.Name = name;
            pair.Value = value;

            // Assert
            pair.Name.Should().Be(name);
            pair.Value.Should().Be(value);
        }

        [Theory]
        [InlineData("", "")]
        [InlineData("   ", "   ")]
        [InlineData("Name", "Value")]
        [InlineData("VeryLongNameWithSpecialCharacters!@#$%^&*()", "VeryLongValueWithSpecialCharacters!@#$%^&*()")]
        [InlineData("123", "456")]
        [InlineData("true", "false")]
        public void NameValuePair_SetVariousValues_ShouldSetValues(string name, string value)
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
        public void NameValuePair_SetNullValues_ShouldSetNulls()
        {
            // Arrange
            var pair = new NameValuePair();

            // Act
            pair.Name = null;
            pair.Value = null;

            // Assert
            pair.Name.Should().BeNull();
            pair.Value.Should().BeNull();
        }

        [Fact]
        public void NameValuePair_PropertySettersGetters_ShouldWorkCorrectly()
        {
            // Arrange
            var pair = new NameValuePair();

            // Act & Assert - Test that we can set and get each property multiple times
            pair.Name = "Name1";
            pair.Name.Should().Be("Name1");
            pair.Name = "Name2";
            pair.Name.Should().Be("Name2");
            pair.Name = null;
            pair.Name.Should().BeNull();

            pair.Value = "Value1";
            pair.Value.Should().Be("Value1");
            pair.Value = "Value2";
            pair.Value.Should().Be("Value2");
            pair.Value = null;
            pair.Value.Should().BeNull();
        }

        [Fact]
        public void NameValuePair_WithConfigurationData_ShouldStoreCorrectly()
        {
            // Arrange
            var pair = new NameValuePair();

            // Act
            pair.Name = "DatabaseConnectionString";
            pair.Value = "Server=localhost;Database=Test;Trusted_Connection=true;";

            // Assert
            pair.Name.Should().Be("DatabaseConnectionString");
            pair.Value.Should().Be("Server=localhost;Database=Test;Trusted_Connection=true;");
        }

        [Fact]
        public void NameValuePair_WithJsonLikeData_ShouldStoreCorrectly()
        {
            // Arrange
            var pair = new NameValuePair();

            // Act
            pair.Name = "JsonConfig";
            pair.Value = "{\"key\": \"value\", \"number\": 42}";

            // Assert
            pair.Name.Should().Be("JsonConfig");
            pair.Value.Should().Be("{\"key\": \"value\", \"number\": 42}");
        }

        [Fact]
        public void NameValuePair_WithXmlLikeData_ShouldStoreCorrectly()
        {
            // Arrange
            var pair = new NameValuePair();

            // Act
            pair.Name = "XmlConfig";
            pair.Value = "<config><setting>value</setting></config>";

            // Assert
            pair.Name.Should().Be("XmlConfig");
            pair.Value.Should().Be("<config><setting>value</setting></config>");
        }

        [Fact]
        public void NameValuePair_WithUnicodeCharacters_ShouldStoreCorrectly()
        {
            // Arrange
            var pair = new NameValuePair();

            // Act
            pair.Name = "Unicode测试";
            pair.Value = "Value with émojis 🚀🎉";

            // Assert
            pair.Name.Should().Be("Unicode测试");
            pair.Value.Should().Be("Value with émojis 🚀🎉");
        }

        [Fact]
        public void NameValuePair_WithNewlineCharacters_ShouldStoreCorrectly()
        {
            // Arrange
            var pair = new NameValuePair();

            // Act
            pair.Name = "MultiLine\nName";
            pair.Value = "Line1\nLine2\rLine3\r\nLine4";

            // Assert
            pair.Name.Should().Be("MultiLine\nName");
            pair.Value.Should().Be("Line1\nLine2\rLine3\r\nLine4");
        }

        [Fact]
        public void NameValuePair_WithTabCharacters_ShouldStoreCorrectly()
        {
            // Arrange
            var pair = new NameValuePair();

            // Act
            pair.Name = "Name\twith\ttabs";
            pair.Value = "Value\twith\ttabs";

            // Assert
            pair.Name.Should().Be("Name\twith\ttabs");
            pair.Value.Should().Be("Value\twith\ttabs");
        }

        [Theory]
        [InlineData("Temperature", "25.5")]
        [InlineData("Humidity", "60%")]
        [InlineData("Status", "Active")]
        [InlineData("LastUpdate", "2023-12-01T10:30:00Z")]
        [InlineData("Count", "1000")]
        public void NameValuePair_WithCommonConfigurationPairs_ShouldStoreCorrectly(string name, string value)
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
        public void NameValuePair_ToString_ShouldReturnStringRepresentation()
        {
            // Arrange
            var pair = new NameValuePair
            {
                Name = "TestName",
                Value = "TestValue"
            };

            // Act
            var result = pair.ToString();

            // Assert
            result.Should().NotBeNullOrEmpty();
            // ToString should return some representation of the object
            result.Should().Contain("NameValuePair");
        }

        [Fact]
        public void NameValuePair_GetHashCode_ShouldReturnConsistentValue()
        {
            // Arrange
            var pair = new NameValuePair
            {
                Name = "TestName",
                Value = "TestValue"
            };

            // Act
            var hashCode1 = pair.GetHashCode();
            var hashCode2 = pair.GetHashCode();

            // Assert
            hashCode1.Should().Be(hashCode2);
        }

        [Fact]
        public void NameValuePair_Type_ShouldBeCorrect()
        {
            // Arrange & Act
            var pair = new NameValuePair();

            // Assert
            pair.GetType().Should().Be(typeof(NameValuePair));
            pair.GetType().Name.Should().Be("NameValuePair");
        }
    }
} 