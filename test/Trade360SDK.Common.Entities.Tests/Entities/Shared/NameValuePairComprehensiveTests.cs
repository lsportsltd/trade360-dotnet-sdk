using FluentAssertions;
using Trade360SDK.Common.Entities.Shared;
using Xunit;

namespace Trade360SDK.Common.Entities.Tests.Entities.Shared
{
    public class NameValuePairComprehensiveTests
    {
        [Fact]
        public void Constructor_ShouldInitializeWithDefaultValues()
        {
            // Act
            var nameValuePair = new NameValuePair();

            // Assert
            nameValuePair.Name.Should().BeNull();
            nameValuePair.Value.Should().BeNull();
        }

        [Fact]
        public void Name_WithValidString_ShouldStoreCorrectly()
        {
            // Arrange
            var nameValuePair = new NameValuePair();
            const string expectedName = "TestName";

            // Act
            nameValuePair.Name = expectedName;

            // Assert
            nameValuePair.Name.Should().Be(expectedName);
        }

        [Fact]
        public void Value_WithValidString_ShouldStoreCorrectly()
        {
            // Arrange
            var nameValuePair = new NameValuePair();
            const string expectedValue = "TestValue";

            // Act
            nameValuePair.Value = expectedValue;

            // Assert
            nameValuePair.Value.Should().Be(expectedValue);
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("test")]
        [InlineData("Test Name")]
        [InlineData("Special@#$%Characters")]
        [InlineData("1234567890")]
        [InlineData("VeryLongNameThatContainsLotsOfCharactersAndShouldStillBeStoredCorrectly")]
        public void Name_WithVariousStrings_ShouldStoreCorrectly(string name)
        {
            // Arrange
            var nameValuePair = new NameValuePair();

            // Act
            nameValuePair.Name = name;

            // Assert
            nameValuePair.Name.Should().Be(name);
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("value")]
        [InlineData("Test Value")]
        [InlineData("JSON:{\"key\":\"value\"}")]
        [InlineData("9876543210")]
        [InlineData("VeryLongValueThatContainsLotsOfCharactersAndShouldStillBeStoredCorrectly")]
        public void Value_WithVariousStrings_ShouldStoreCorrectly(string value)
        {
            // Arrange
            var nameValuePair = new NameValuePair();

            // Act
            nameValuePair.Value = value;

            // Assert
            nameValuePair.Value.Should().Be(value);
        }

        [Fact]
        public void Name_WithNull_ShouldAcceptValue()
        {
            // Arrange
            var nameValuePair = new NameValuePair();

            // Act
            nameValuePair.Name = null;

            // Assert
            nameValuePair.Name.Should().BeNull();
        }

        [Fact]
        public void Value_WithNull_ShouldAcceptValue()
        {
            // Arrange
            var nameValuePair = new NameValuePair();

            // Act
            nameValuePair.Value = null;

            // Assert
            nameValuePair.Value.Should().BeNull();
        }

        [Fact]
        public void Properties_CanBeSetIndependently()
        {
            // Arrange
            var nameValuePair = new NameValuePair();
            const string expectedName = "PropertyName";
            const string expectedValue = "PropertyValue";

            // Act
            nameValuePair.Name = expectedName;
            nameValuePair.Value = expectedValue;

            // Assert
            nameValuePair.Name.Should().Be(expectedName);
            nameValuePair.Value.Should().Be(expectedValue);
        }

        [Fact]
        public void Properties_CanBeOverwritten()
        {
            // Arrange
            var nameValuePair = new NameValuePair
            {
                Name = "InitialName",
                Value = "InitialValue"
            };

            const string newName = "UpdatedName";
            const string newValue = "UpdatedValue";

            // Act
            nameValuePair.Name = newName;
            nameValuePair.Value = newValue;

            // Assert
            nameValuePair.Name.Should().Be(newName);
            nameValuePair.Value.Should().Be(newValue);
        }

        [Fact]
        public void ObjectInitializer_ShouldSetAllProperties()
        {
            // Arrange
            const string expectedName = "InitializerName";
            const string expectedValue = "InitializerValue";

            // Act
            var nameValuePair = new NameValuePair
            {
                Name = expectedName,
                Value = expectedValue
            };

            // Assert
            nameValuePair.Name.Should().Be(expectedName);
            nameValuePair.Value.Should().Be(expectedValue);
        }

        [Fact]
        public void ObjectInitializer_WithNullValues_ShouldAcceptValues()
        {
            // Act
            var nameValuePair = new NameValuePair
            {
                Name = null,
                Value = null
            };

            // Assert
            nameValuePair.Name.Should().BeNull();
            nameValuePair.Value.Should().BeNull();
        }

        [Fact]
        public void Properties_WithEmptyStrings_ShouldStoreCorrectly()
        {
            // Arrange
            var nameValuePair = new NameValuePair();

            // Act
            nameValuePair.Name = string.Empty;
            nameValuePair.Value = string.Empty;

            // Assert
            nameValuePair.Name.Should().Be(string.Empty);
            nameValuePair.Value.Should().Be(string.Empty);
        }

        [Fact]
        public void Properties_WithWhitespace_ShouldStoreCorrectly()
        {
            // Arrange
            var nameValuePair = new NameValuePair();
            const string whitespaceName = "   ";
            const string whitespaceValue = "\t\n\r";

            // Act
            nameValuePair.Name = whitespaceName;
            nameValuePair.Value = whitespaceValue;

            // Assert
            nameValuePair.Name.Should().Be(whitespaceName);
            nameValuePair.Value.Should().Be(whitespaceValue);
        }

        [Fact]
        public void Properties_WithSpecialCharacters_ShouldStoreCorrectly()
        {
            // Arrange
            var nameValuePair = new NameValuePair();
            const string specialName = "!@#$%^&*()_+-=[]{}|;:,.<>?";
            const string specialValue = "«»€£¥™®©±÷×¿¡";

            // Act
            nameValuePair.Name = specialName;
            nameValuePair.Value = specialValue;

            // Assert
            nameValuePair.Name.Should().Be(specialName);
            nameValuePair.Value.Should().Be(specialValue);
        }

        [Fact]
        public void Properties_WithUnicode_ShouldStoreCorrectly()
        {
            // Arrange
            var nameValuePair = new NameValuePair();
            const string unicodeName = "测试名称";
            const string unicodeValue = "テスト値";

            // Act
            nameValuePair.Name = unicodeName;
            nameValuePair.Value = unicodeValue;

            // Assert
            nameValuePair.Name.Should().Be(unicodeName);
            nameValuePair.Value.Should().Be(unicodeValue);
        }

        [Fact]
        public void ToString_ShouldNotThrow()
        {
            // Arrange
            var nameValuePair = new NameValuePair
            {
                Name = "TestName",
                Value = "TestValue"
            };

            // Act & Assert
            var act = () => nameValuePair.ToString();
            act.Should().NotThrow();
        }

        [Fact]
        public void GetHashCode_ShouldNotThrow()
        {
            // Arrange
            var nameValuePair = new NameValuePair
            {
                Name = "HashName",
                Value = "HashValue"
            };

            // Act & Assert
            var act = () => nameValuePair.GetHashCode();
            act.Should().NotThrow();
        }

        [Fact]
        public void Equals_WithSameReference_ShouldReturnTrue()
        {
            // Arrange
            var nameValuePair = new NameValuePair
            {
                Name = "SameName",
                Value = "SameValue"
            };

            // Act & Assert
            nameValuePair.Equals(nameValuePair).Should().BeTrue();
        }

        [Fact]
        public void Equals_WithNull_ShouldReturnFalse()
        {
            // Arrange
            var nameValuePair = new NameValuePair
            {
                Name = "NotNullName",
                Value = "NotNullValue"
            };

            // Act & Assert
            nameValuePair.Equals(null).Should().BeFalse();
        }
    }
} 