using FluentAssertions;
using Trade360SDK.Common.Attributes;
using Xunit;

namespace Trade360SDK.Common.Entities.Tests.Attributes
{
    public class Trade360EntityAttributeComprehensiveTests
    {
        [Fact]
        public void Constructor_WithValidEntityKey_ShouldSetPropertyCorrectly()
        {
            // Arrange
            const int entityKey = 123;

            // Act
            var attribute = new Trade360EntityAttribute(entityKey);

            // Assert
            attribute.EntityKey.Should().Be(entityKey);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(100)]
        [InlineData(999)]
        [InlineData(int.MaxValue)]
        public void Constructor_WithVariousEntityKeys_ShouldStoreCorrectly(int entityKey)
        {
            // Act
            var attribute = new Trade360EntityAttribute(entityKey);

            // Assert
            attribute.EntityKey.Should().Be(entityKey);
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(-100)]
        [InlineData(int.MinValue)]
        public void Constructor_WithNegativeEntityKeys_ShouldAcceptValues(int entityKey)
        {
            // Act
            var attribute = new Trade360EntityAttribute(entityKey);

            // Assert
            attribute.EntityKey.Should().Be(entityKey);
        }

        [Fact]
        public void EntityKey_IsReadOnly_ShouldNotHaveSetter()
        {
            // Arrange
            var attributeType = typeof(Trade360EntityAttribute);
            var property = attributeType.GetProperty(nameof(Trade360EntityAttribute.EntityKey));

            // Act & Assert
            property.Should().NotBeNull();
            property!.CanRead.Should().BeTrue();
            property.CanWrite.Should().BeFalse();
        }

        [Fact]
        public void Attribute_ShouldInheritFromAttribute()
        {
            // Arrange
            var attribute = new Trade360EntityAttribute(1);

            // Act & Assert
            attribute.Should().BeAssignableTo<System.Attribute>();
        }

        [Fact]
        public void Attribute_HasCorrectAttributeUsage()
        {
            // Arrange
            var attributeType = typeof(Trade360EntityAttribute);

            // Act
            var attributeUsage = (System.AttributeUsageAttribute?)System.Attribute.GetCustomAttribute(attributeType, typeof(System.AttributeUsageAttribute));

            // Assert
            attributeUsage.Should().NotBeNull();
            attributeUsage!.ValidOn.Should().Be(System.AttributeTargets.Class);
        }

        [Fact]
        public void MultipleInstances_WithSameEntityKey_ShouldBeEqual()
        {
            // Arrange
            const int entityKey = 42;
            var attribute1 = new Trade360EntityAttribute(entityKey);
            var attribute2 = new Trade360EntityAttribute(entityKey);

            // Act & Assert
            attribute1.EntityKey.Should().Be(attribute2.EntityKey);
        }

        [Fact]
        public void MultipleInstances_WithDifferentEntityKeys_ShouldBeDifferent()
        {
            // Arrange
            var attribute1 = new Trade360EntityAttribute(1);
            var attribute2 = new Trade360EntityAttribute(2);

            // Act & Assert
            attribute1.EntityKey.Should().NotBe(attribute2.EntityKey);
        }

        [Fact]
        public void ToString_ShouldNotThrow()
        {
            // Arrange
            var attribute = new Trade360EntityAttribute(123);

            // Act & Assert
            var act = () => attribute.ToString();
            act.Should().NotThrow();
        }

        [Fact]
        public void GetHashCode_ShouldNotThrow()
        {
            // Arrange
            var attribute = new Trade360EntityAttribute(123);

            // Act & Assert
            var act = () => attribute.GetHashCode();
            act.Should().NotThrow();
        }

        [Fact]
        public void Equals_WithSameReference_ShouldReturnTrue()
        {
            // Arrange
            var attribute = new Trade360EntityAttribute(123);

            // Act & Assert
            attribute.Equals(attribute).Should().BeTrue();
        }

        [Fact]
        public void Equals_WithNull_ShouldReturnFalse()
        {
            // Arrange
            var attribute = new Trade360EntityAttribute(123);

            // Act & Assert
            attribute.Equals(null).Should().BeFalse();
        }
    }
} 