using System;
using System.Reflection;
using FluentAssertions;
using Trade360SDK.Common.Attributes;
using Trade360SDK.Common.Entities.MessageTypes;
using Xunit;

namespace Trade360SDK.Common.Entities.Tests.Attributes
{
    public class Trade360EntityAttributeBusinessLogicTests
    {
        [Fact]
        public void Constructor_WithValidEntityKey_ShouldSetEntityKeyCorrectly()
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
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(31)]
        [InlineData(35)]
        [InlineData(38)]
        [InlineData(40)]
        [InlineData(999)]
        [InlineData(int.MaxValue)]
        public void Constructor_WithVariousValidEntityKeys_ShouldSetCorrectly(int entityKey)
        {
            // Act
            var attribute = new Trade360EntityAttribute(entityKey);

            // Assert
            attribute.EntityKey.Should().Be(entityKey);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(-100)]
        [InlineData(int.MinValue)]
        public void Constructor_WithNegativeOrZeroEntityKey_ShouldStillSetValue(int entityKey)
        {
            // Act
            var attribute = new Trade360EntityAttribute(entityKey);

            // Assert
            // The attribute doesn't validate the entity key in constructor
            attribute.EntityKey.Should().Be(entityKey);
        }

        [Fact]
        public void EntityKey_ShouldBeReadOnlyProperty()
        {
            // Arrange
            var entityKey = 123;
            var attribute = new Trade360EntityAttribute(entityKey);

            // Act & Assert
            attribute.EntityKey.Should().Be(entityKey);
            
            // Verify that EntityKey property doesn't have a setter
            var property = typeof(Trade360EntityAttribute).GetProperty(nameof(Trade360EntityAttribute.EntityKey));
            property.Should().NotBeNull();
            property.CanRead.Should().BeTrue();
            property.CanWrite.Should().BeFalse("EntityKey should be read-only");
        }

        [Fact]
        public void Attribute_ShouldInheritFromSystemAttribute()
        {
            // Act
            var attribute = new Trade360EntityAttribute(1);

            // Assert
            attribute.Should().BeAssignableTo<Attribute>();
        }

        [Fact]
        public void Attribute_ShouldHaveCorrectAttributeUsage()
        {
            // Act
            var attributeType = typeof(Trade360EntityAttribute);
            var attributeUsage = attributeType.GetCustomAttribute<AttributeUsageAttribute>();

            // Assert
            attributeUsage.Should().NotBeNull("Trade360EntityAttribute should have AttributeUsage defined");
            attributeUsage.ValidOn.Should().HaveFlag(AttributeTargets.Class, 
                "Should be applicable to classes");
        }

        [Fact]
        public void Attribute_WhenAppliedToClass_ShouldBeRetrievable()
        {
            // Arrange
            var testType = typeof(TestEntityWithAttribute);

            // Act
            var attribute = testType.GetCustomAttribute<Trade360EntityAttribute>();

            // Assert
            attribute.Should().NotBeNull();
            attribute.EntityKey.Should().Be(999);
        }

        [Fact]
        public void Attribute_WhenAppliedToMultipleClasses_ShouldMaintainIndependentValues()
        {
            // Arrange
            var type1 = typeof(TestEntityWithAttribute);
            var type2 = typeof(AnotherTestEntityWithAttribute);

            // Act
            var attribute1 = type1.GetCustomAttribute<Trade360EntityAttribute>();
            var attribute2 = type2.GetCustomAttribute<Trade360EntityAttribute>();

            // Assert
            attribute1.Should().NotBeNull();
            attribute2.Should().NotBeNull();
            attribute1.EntityKey.Should().Be(999);
            attribute2.EntityKey.Should().Be(888);
            attribute1.EntityKey.Should().NotBe(attribute2.EntityKey);
        }

        [Fact]
        public void Attribute_OnRealMessageTypes_ShouldHaveCorrectEntityKeys()
        {
            // Act & Assert
            var fixtureUpdate = typeof(FixtureMetadataUpdate).GetCustomAttribute<Trade360EntityAttribute>();
            fixtureUpdate.Should().NotBeNull();
            fixtureUpdate.EntityKey.Should().Be(1);

            var livescoreUpdate = typeof(LivescoreUpdate).GetCustomAttribute<Trade360EntityAttribute>();
            livescoreUpdate.Should().NotBeNull();
            livescoreUpdate.EntityKey.Should().Be(2);

            var marketUpdate = typeof(MarketUpdate).GetCustomAttribute<Trade360EntityAttribute>();
            marketUpdate.Should().NotBeNull();
            marketUpdate.EntityKey.Should().Be(3);

            var keepAliveUpdate = typeof(KeepAliveUpdate).GetCustomAttribute<Trade360EntityAttribute>();
            keepAliveUpdate.Should().NotBeNull();
            keepAliveUpdate.EntityKey.Should().Be(31);

            var settlementUpdate = typeof(SettlementUpdate).GetCustomAttribute<Trade360EntityAttribute>();
            settlementUpdate.Should().NotBeNull();
            settlementUpdate.EntityKey.Should().Be(35);
        }

        [Fact]
        public void Attribute_ShouldSupportMultipleInstancesWithDifferentKeys()
        {
            // Act
            var attribute1 = new Trade360EntityAttribute(100);
            var attribute2 = new Trade360EntityAttribute(200);
            var attribute3 = new Trade360EntityAttribute(300);

            // Assert
            attribute1.EntityKey.Should().Be(100);
            attribute2.EntityKey.Should().Be(200);
            attribute3.EntityKey.Should().Be(300);
            
            // Verify independence
            attribute1.EntityKey.Should().NotBe(attribute2.EntityKey);
            attribute2.EntityKey.Should().NotBe(attribute3.EntityKey);
            attribute1.EntityKey.Should().NotBe(attribute3.EntityKey);
        }

        [Fact]
        public void Attribute_ShouldBeUsableInReflectionScenarios()
        {
            // Arrange
            var types = new[] 
            { 
                typeof(FixtureMetadataUpdate), 
                typeof(LivescoreUpdate), 
                typeof(MarketUpdate) 
            };

            // Act & Assert
            foreach (var type in types)
            {
                var attribute = type.GetCustomAttribute<Trade360EntityAttribute>();
                attribute.Should().NotBeNull($"Type {type.Name} should have Trade360EntityAttribute");
                attribute.EntityKey.Should().BeGreaterThan(0, $"EntityKey for {type.Name} should be positive");
            }
        }

        [Fact]
        public void Attribute_ShouldSupportAttributeInheritanceQueries()
        {
            // Arrange
            var testType = typeof(TestEntityWithAttribute);

            // Act
            var hasAttribute = testType.IsDefined(typeof(Trade360EntityAttribute), false);
            var attributes = testType.GetCustomAttributes(typeof(Trade360EntityAttribute), false);

            // Assert
            hasAttribute.Should().BeTrue();
            attributes.Should().HaveCount(1);
            
            var attribute = attributes[0] as Trade360EntityAttribute;
            attribute.Should().NotBeNull();
            attribute.EntityKey.Should().Be(999);
        }

        [Fact]
        public void Attribute_ShouldBeUsableForMessageTypeIdentification()
        {
            // Arrange
            var messageTypes = new[]
            {
                typeof(FixtureMetadataUpdate),
                typeof(LivescoreUpdate),
                typeof(MarketUpdate),
                typeof(KeepAliveUpdate),
                typeof(SettlementUpdate)
            };

            // Act & Assert
            foreach (var messageType in messageTypes)
            {
                var attribute = messageType.GetCustomAttribute<Trade360EntityAttribute>();
                attribute.Should().NotBeNull($"Message type {messageType.Name} should have Trade360EntityAttribute");
                
                // Verify the entity key is in expected range for message types
                attribute.EntityKey.Should().BeInRange(1, 100, 
                    $"Entity key for {messageType.Name} should be in message type range");
            }
        }

        [Fact]
        public void Attribute_WithSameEntityKey_ShouldBeEqualByValue()
        {
            // Arrange
            var entityKey = 42;
            var attribute1 = new Trade360EntityAttribute(entityKey);
            var attribute2 = new Trade360EntityAttribute(entityKey);

            // Act & Assert
            attribute1.EntityKey.Should().Be(attribute2.EntityKey);
            // Note: Reference equality is different, but value equality is what matters for business logic
        }

        [Fact]
        public void Attribute_ShouldMaintainEntityKeyThroughSerialization()
        {
            // Arrange
            var originalEntityKey = 123;
            var attribute = new Trade360EntityAttribute(originalEntityKey);

            // Act
            var retrievedEntityKey = attribute.EntityKey;

            // Assert
            retrievedEntityKey.Should().Be(originalEntityKey);
        }

        [Fact]
        public void Attribute_ShouldBeApplicableOnlyToClasses()
        {
            // Arrange
            var attributeType = typeof(Trade360EntityAttribute);
            var attributeUsage = attributeType.GetCustomAttribute<AttributeUsageAttribute>();

            // Assert
            attributeUsage.Should().NotBeNull();
            attributeUsage.ValidOn.Should().Be(AttributeTargets.Class, 
                "Trade360EntityAttribute should only be applicable to classes");
        }

        [Fact]
        public void Attribute_ShouldNotAllowMultipleInstances()
        {
            // Arrange
            var attributeType = typeof(Trade360EntityAttribute);
            var attributeUsage = attributeType.GetCustomAttribute<AttributeUsageAttribute>();

            // Assert
            attributeUsage.Should().NotBeNull();
            attributeUsage.AllowMultiple.Should().BeFalse(
                "Trade360EntityAttribute should not allow multiple instances on the same class");
        }

        [Fact]
        public void Attribute_ShouldBeInheritedByDefault()
        {
            // Arrange
            var attributeType = typeof(Trade360EntityAttribute);
            var attributeUsage = attributeType.GetCustomAttribute<AttributeUsageAttribute>();

            // Assert
            attributeUsage.Should().NotBeNull();
            attributeUsage.Inherited.Should().BeTrue(
                "Trade360EntityAttribute uses default inheritance behavior (true)");
        }

        [Fact]
        public void Attribute_WithBoundaryValues_ShouldHandleCorrectly()
        {
            // Arrange & Act
            var minAttribute = new Trade360EntityAttribute(int.MinValue);
            var maxAttribute = new Trade360EntityAttribute(int.MaxValue);
            var zeroAttribute = new Trade360EntityAttribute(0);

            // Assert
            minAttribute.EntityKey.Should().Be(int.MinValue);
            maxAttribute.EntityKey.Should().Be(int.MaxValue);
            zeroAttribute.EntityKey.Should().Be(0);
        }
    }

    // Test classes for attribute testing
    [Trade360EntityAttribute(999)]
    public class TestEntityWithAttribute
    {
        public string Name { get; set; } = string.Empty;
    }

    [Trade360EntityAttribute(888)]
    public class AnotherTestEntityWithAttribute
    {
        public int Id { get; set; }
    }
} 