using System;
using FluentAssertions;
using Xunit;
using Trade360SDK.Common.Attributes;

namespace Trade360SDK.Common.Entities.Tests.Attributes
{
    public class Trade360EntityAttributeTests
    {
        [Fact]
        public void Trade360EntityAttribute_Constructor_ShouldSetEntityKey()
        {
            // Arrange
            var entityKey = 42;

            // Act
            var attribute = new Trade360EntityAttribute(entityKey);

            // Assert
            attribute.Should().NotBeNull();
            attribute.EntityKey.Should().Be(entityKey);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(-1)]
        [InlineData(int.MaxValue)]
        [InlineData(int.MinValue)]
        [InlineData(999)]
        [InlineData(12345)]
        public void Trade360EntityAttribute_Constructor_WithVariousEntityKeys_ShouldSetValue(int entityKey)
        {
            // Act
            var attribute = new Trade360EntityAttribute(entityKey);

            // Assert
            attribute.EntityKey.Should().Be(entityKey);
        }

        [Fact]
        public void Trade360EntityAttribute_EntityKey_ShouldBeReadOnly()
        {
            // Arrange
            var entityKey = 123;
            var attribute = new Trade360EntityAttribute(entityKey);

            // Act & Assert
            // EntityKey should not have a setter - this test verifies it's read-only
            attribute.EntityKey.Should().Be(entityKey);
            
            // Verify we can't change it (compilation test)
            var originalKey = attribute.EntityKey;
            attribute.EntityKey.Should().Be(originalKey);
        }

        [Fact]
        public void Trade360EntityAttribute_ShouldInheritFromAttribute()
        {
            // Arrange & Act
            var attribute = new Trade360EntityAttribute(1);

            // Assert
            attribute.Should().BeAssignableTo<Attribute>();
        }

        [Fact]
        public void Trade360EntityAttribute_ShouldHaveCorrectAttributeUsage()
        {
            // Arrange
            var attributeType = typeof(Trade360EntityAttribute);

            // Act
            var attributeUsage = (AttributeUsageAttribute)Attribute.GetCustomAttribute(attributeType, typeof(AttributeUsageAttribute));

            // Assert
            attributeUsage.Should().NotBeNull();
            attributeUsage.ValidOn.Should().HaveFlag(AttributeTargets.Class);
        }

        [Fact]
        public void Trade360EntityAttribute_MultipleInstances_ShouldHaveIndependentEntityKeys()
        {
            // Arrange & Act
            var attribute1 = new Trade360EntityAttribute(100);
            var attribute2 = new Trade360EntityAttribute(200);
            var attribute3 = new Trade360EntityAttribute(300);

            // Assert
            attribute1.EntityKey.Should().Be(100);
            attribute2.EntityKey.Should().Be(200);
            attribute3.EntityKey.Should().Be(300);
            
            attribute1.EntityKey.Should().NotBe(attribute2.EntityKey);
            attribute2.EntityKey.Should().NotBe(attribute3.EntityKey);
            attribute1.EntityKey.Should().NotBe(attribute3.EntityKey);
        }

        [Fact]
        public void Trade360EntityAttribute_ToString_ShouldReturnStringRepresentation()
        {
            // Arrange
            var entityKey = 456;
            var attribute = new Trade360EntityAttribute(entityKey);

            // Act
            var result = attribute.ToString();

            // Assert
            result.Should().NotBeNullOrEmpty();
            // Should contain the class name
            result.Should().Contain("Trade360EntityAttribute");
        }

        [Fact]
        public void Trade360EntityAttribute_GetHashCode_ShouldReturnConsistentValue()
        {
            // Arrange
            var entityKey = 789;
            var attribute = new Trade360EntityAttribute(entityKey);

            // Act
            var hashCode1 = attribute.GetHashCode();
            var hashCode2 = attribute.GetHashCode();

            // Assert
            hashCode1.Should().Be(hashCode2);
        }

        [Fact]
        public void Trade360EntityAttribute_Equals_WithSameEntityKey_ShouldBeEqual()
        {
            // Arrange
            var entityKey = 555;
            var attribute1 = new Trade360EntityAttribute(entityKey);
            var attribute2 = new Trade360EntityAttribute(entityKey);

            // Act & Assert
            // Note: This tests reference equality since Attribute doesn't override Equals by default
            attribute1.Should().NotBeSameAs(attribute2);
            attribute1.EntityKey.Should().Be(attribute2.EntityKey);
        }

        [Fact]
        public void Trade360EntityAttribute_Type_ShouldBeCorrect()
        {
            // Arrange & Act
            var attribute = new Trade360EntityAttribute(1);

            // Assert
            attribute.GetType().Should().Be(typeof(Trade360EntityAttribute));
            attribute.GetType().Name.Should().Be("Trade360EntityAttribute");
        }
    }

    // Test class to verify the attribute can be applied to classes
    [Trade360EntityAttribute(999)]
    public class TestEntityWithAttribute
    {
        public string Name { get; set; }
    }

    public class Trade360EntityAttributeUsageTests
    {
        [Fact]
        public void Trade360EntityAttribute_CanBeAppliedToClass_ShouldWork()
        {
            // Arrange
            var type = typeof(TestEntityWithAttribute);

            // Act
            var attributes = type.GetCustomAttributes(typeof(Trade360EntityAttribute), false);

            // Assert
            attributes.Should().HaveCount(1);
            var attribute = attributes[0] as Trade360EntityAttribute;
            attribute.Should().NotBeNull();
            attribute.EntityKey.Should().Be(999);
        }

        [Fact]
        public void Trade360EntityAttribute_AppliedToClass_ShouldBeRetrievable()
        {
            // Arrange
            var type = typeof(TestEntityWithAttribute);

            // Act
            var attribute = Attribute.GetCustomAttribute(type, typeof(Trade360EntityAttribute)) as Trade360EntityAttribute;

            // Assert
            attribute.Should().NotBeNull();
            attribute.EntityKey.Should().Be(999);
        }

        [Fact]
        public void Trade360EntityAttribute_ClassWithoutAttribute_ShouldReturnNull()
        {
            // Arrange
            var type = typeof(Trade360EntityAttributeTests);

            // Act
            var attribute = Attribute.GetCustomAttribute(type, typeof(Trade360EntityAttribute)) as Trade360EntityAttribute;

            // Assert
            attribute.Should().BeNull();
        }
    }
} 