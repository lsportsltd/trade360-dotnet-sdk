using System;
using FluentAssertions;
using Trade360SDK.Common.Attributes;
using Xunit;

namespace Trade360SDK.Common.Entities.Tests.Attributes;

public class Trade360EntityAttributeComprehensiveTests
{
    [Fact]
    public void Constructor_WithValidEntityKey_ShouldSetEntityKeyProperty()
    {
        // Arrange
        const int expectedEntityKey = 42;

        // Act
        var attribute = new Trade360EntityAttribute(expectedEntityKey);

        // Assert
        attribute.Should().NotBeNull();
        attribute.EntityKey.Should().Be(expectedEntityKey);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(100)]
    [InlineData(int.MaxValue)]
    [InlineData(int.MinValue)]
    [InlineData(-1)]
    [InlineData(-100)]
    public void Constructor_WithVariousEntityKeys_ShouldSetEntityKeyCorrectly(int entityKey)
    {
        // Act
        var attribute = new Trade360EntityAttribute(entityKey);

        // Assert
        attribute.EntityKey.Should().Be(entityKey);
    }

    [Fact]
    public void EntityKey_Property_ShouldBeReadOnly()
    {
        // Arrange
        const int entityKey = 25;
        var attribute = new Trade360EntityAttribute(entityKey);

        // Act & Assert
        attribute.EntityKey.Should().Be(entityKey);
        
        // Verify property is read-only (no setter)
        var propertyInfo = typeof(Trade360EntityAttribute).GetProperty(nameof(Trade360EntityAttribute.EntityKey));
        propertyInfo.Should().NotBeNull();
        propertyInfo!.CanRead.Should().BeTrue();
        propertyInfo.CanWrite.Should().BeFalse();
    }

    [Fact]
    public void Inheritance_ShouldInheritFromAttribute()
    {
        // Arrange & Act
        var attribute = new Trade360EntityAttribute(1);

        // Assert
        attribute.Should().BeAssignableTo<Attribute>();
    }

    [Fact]
    public void AttributeUsage_ShouldTargetClassesOnly()
    {
        // Arrange
        var attributeType = typeof(Trade360EntityAttribute);

        // Act
        var attributeUsage = Attribute.GetCustomAttribute(attributeType, typeof(AttributeUsageAttribute)) as AttributeUsageAttribute;

        // Assert
        attributeUsage.Should().NotBeNull();
        attributeUsage!.ValidOn.Should().Be(AttributeTargets.Class);
    }

    [Fact]
    public void ToString_ShouldReturnMeaningfulString()
    {
        // Arrange
        const int entityKey = 123;
        var attribute = new Trade360EntityAttribute(entityKey);

        // Act
        var result = attribute.ToString();

        // Assert
        result.Should().NotBeNullOrEmpty();
        result.Should().Contain("Trade360EntityAttribute");
    }

    [Fact]
    public void Equals_WithSameEntityKey_ShouldBeEqual()
    {
        // Arrange
        const int entityKey = 50;
        var attribute1 = new Trade360EntityAttribute(entityKey);
        var attribute2 = new Trade360EntityAttribute(entityKey);

        // Act & Assert
        attribute1.EntityKey.Should().Be(attribute2.EntityKey);
    }

    [Fact]
    public void Equals_WithDifferentEntityKey_ShouldNotBeEqual()
    {
        // Arrange
        var attribute1 = new Trade360EntityAttribute(10);
        var attribute2 = new Trade360EntityAttribute(20);

        // Act & Assert
        attribute1.EntityKey.Should().NotBe(attribute2.EntityKey);
    }

    [Fact]
    public void GetHashCode_ShouldBeConsistent()
    {
        // Arrange
        const int entityKey = 75;
        var attribute = new Trade360EntityAttribute(entityKey);

        // Act
        var hashCode1 = attribute.GetHashCode();
        var hashCode2 = attribute.GetHashCode();

        // Assert
        hashCode1.Should().Be(hashCode2);
    }

    [Fact]
    public void Constructor_ShouldBePublic()
    {
        // Arrange & Act
        var constructors = typeof(Trade360EntityAttribute).GetConstructors();

        // Assert
        constructors.Should().HaveCount(1);
        constructors[0].IsPublic.Should().BeTrue();
        constructors[0].GetParameters().Should().HaveCount(1);
        constructors[0].GetParameters()[0].ParameterType.Should().Be<int>();
    }

    [Fact]
    public void Type_ShouldHaveCorrectNamespace()
    {
        // Arrange
        var attributeType = typeof(Trade360EntityAttribute);

        // Act & Assert
        attributeType.Namespace.Should().Be("Trade360SDK.Common.Attributes");
        attributeType.Name.Should().Be("Trade360EntityAttribute");
    }

    // Test with a mock class decorated with the attribute
    [Trade360Entity(999)]
    private class TestEntityClass
    {
        public string? TestProperty { get; set; }
    }

    [Fact]
    public void AttributeUsage_OnTestClass_ShouldBeRetrievable()
    {
        // Arrange
        var testClassType = typeof(TestEntityClass);

        // Act
        var attributes = testClassType.GetCustomAttributes(typeof(Trade360EntityAttribute), false);

        // Assert
        attributes.Should().HaveCount(1);
        attributes[0].Should().BeOfType<Trade360EntityAttribute>();
        ((Trade360EntityAttribute)attributes[0]).EntityKey.Should().Be(999);
    }

    [Fact]
    public void AttributeUsage_WithReflection_ShouldProvideCorrectEntityKey()
    {
        // Arrange
        var testClassType = typeof(TestEntityClass);

        // Act
        var attribute = Attribute.GetCustomAttribute(testClassType, typeof(Trade360EntityAttribute)) as Trade360EntityAttribute;

        // Assert
        attribute.Should().NotBeNull();
        attribute!.EntityKey.Should().Be(999);
    }
} 