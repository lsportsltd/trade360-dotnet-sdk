using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FluentAssertions;
using Trade360SDK.Common.Attributes;
using Trade360SDK.Common.Helpers;
using Trade360SDK.Common.Entities.MessageTypes;
using Xunit;

namespace Trade360SDK.Common.Entities.Tests.Helpers
{
    public class Trade360AttributeHelperBusinessLogicTests
    {
        [Fact]
        public void GetAttributes_ShouldReturnAllMessageTypesWithTrade360EntityAttribute()
        {
            // Act
            var result = Trade360AttributeHelper.GetAttributes();

            // Assert
            result.Should().NotBeNull();
            result.Should().NotBeEmpty();
            
            // Verify that all known message types are included
            result.Should().Contain(t => t.Name == "FixtureMetadataUpdate");
            result.Should().Contain(t => t.Name == "LivescoreUpdate");
            result.Should().Contain(t => t.Name == "MarketUpdate");
            result.Should().Contain(t => t.Name == "KeepAliveUpdate");
            result.Should().Contain(t => t.Name == "SettlementUpdate");
        }

        [Fact]
        public void GetAttributes_ShouldReturnTypesWithUniqueEntityKeys()
        {
            // Act
            var result = Trade360AttributeHelper.GetAttributes();

            // Assert
            result.Should().NotBeNull();
            
            var entityKeys = result.Select(t => t.GetCustomAttribute<Trade360EntityAttribute>()?.EntityKey).ToList();
            var uniqueKeys = entityKeys.Distinct().ToList();
            
            entityKeys.Count.Should().Be(uniqueKeys.Count, "All entity keys should be unique");
        }

        [Fact]
        public void GetAttributes_ShouldReturnValidTrade360EntityAttributes()
        {
            // Act
            var result = Trade360AttributeHelper.GetAttributes();

            // Assert
            result.Should().NotBeNull();
            
            foreach (var type in result)
            {
                var attribute = type.GetCustomAttribute<Trade360EntityAttribute>();
                attribute.Should().NotBeNull($"Type {type.Name} should have Trade360EntityAttribute");
                attribute.EntityKey.Should().BeGreaterThan(0, $"EntityKey for {type.Name} should be positive");
            }
        }

        [Fact]
        public void GetAttributes_ShouldIncludeKnownMessageTypesWithCorrectEntityKeys()
        {
            // Act
            var result = Trade360AttributeHelper.GetAttributes();

            // Assert
            result.Should().NotBeNull();
            
            // Create a dictionary for easy lookup
            var typesByEntityKey = result.ToDictionary(
                t => t.GetCustomAttribute<Trade360EntityAttribute>().EntityKey,
                t => t
            );

            // Verify specific known entity keys
            typesByEntityKey.Should().ContainKey(1); // FixtureMetadataUpdate
            typesByEntityKey.Should().ContainKey(2); // LivescoreUpdate
            typesByEntityKey.Should().ContainKey(3); // MarketUpdate
            typesByEntityKey.Should().ContainKey(31); // KeepAliveUpdate
            typesByEntityKey.Should().ContainKey(35); // SettlementUpdate

            // Verify the types match expected names
            typesByEntityKey[1].Name.Should().Be("FixtureMetadataUpdate");
            typesByEntityKey[2].Name.Should().Be("LivescoreUpdate");
            typesByEntityKey[3].Name.Should().Be("MarketUpdate");
            typesByEntityKey[31].Name.Should().Be("KeepAliveUpdate");
            typesByEntityKey[35].Name.Should().Be("SettlementUpdate");
        }

        [Fact]
        public void GetAttributes_ShouldReturnOnlyConcreteClasses()
        {
            // Act
            var result = Trade360AttributeHelper.GetAttributes();

            // Assert
            result.Should().NotBeNull();
            
            foreach (var type in result)
            {
                type.IsClass.Should().BeTrue($"Type {type.Name} should be a class");
                type.IsAbstract.Should().BeFalse($"Type {type.Name} should not be abstract");
                type.IsInterface.Should().BeFalse($"Type {type.Name} should not be an interface");
            }
        }

        [Fact]
        public void GetAttributes_ShouldReturnTypesFromCorrectNamespace()
        {
            // Act
            var result = Trade360AttributeHelper.GetAttributes();

            // Assert
            result.Should().NotBeNull();
            
            foreach (var type in result)
            {
                type.Namespace.Should().StartWith("Trade360SDK.Common.Entities", 
                    $"Type {type.Name} should be in Trade360SDK.Common.Entities namespace or its sub-namespaces");
            }
        }

        [Fact]
        public void GetAttributes_ShouldReturnTypesInheritingFromMessageUpdate()
        {
            // Act
            var result = Trade360AttributeHelper.GetAttributes();

            // Assert
            result.Should().NotBeNull();
            
            // Most types should inherit from MessageUpdate (though this is not strictly required by the attribute)
            var messageUpdateTypes = result.Where(t => t.BaseType?.Name == "MessageUpdate").ToList();
            messageUpdateTypes.Should().NotBeEmpty("At least some types should inherit from MessageUpdate");
            
            // Verify specific known types inherit from MessageUpdate
            messageUpdateTypes.Should().Contain(t => t.Name == "FixtureMetadataUpdate");
            messageUpdateTypes.Should().Contain(t => t.Name == "LivescoreUpdate");
            messageUpdateTypes.Should().Contain(t => t.Name == "MarketUpdate");
        }

        [Fact]
        public void GetAttributes_ShouldReturnConsistentResultsAcrossMultipleCalls()
        {
            // Act
            var result1 = Trade360AttributeHelper.GetAttributes();
            var result2 = Trade360AttributeHelper.GetAttributes();
            var result3 = Trade360AttributeHelper.GetAttributes();

            // Assert
            result1.Should().NotBeNull();
            result2.Should().NotBeNull();
            result3.Should().NotBeNull();
            
            result1.Count.Should().Be(result2.Count);
            result2.Count.Should().Be(result3.Count);
            
            // Results should contain the same types (order might differ)
            result1.Should().BeEquivalentTo(result2);
            result2.Should().BeEquivalentTo(result3);
        }

        [Fact]
        public void GetAttributes_ShouldReturnTypesWithPublicConstructors()
        {
            // Act
            var result = Trade360AttributeHelper.GetAttributes();

            // Assert
            result.Should().NotBeNull();
            
            foreach (var type in result)
            {
                var constructors = type.GetConstructors(BindingFlags.Public | BindingFlags.Instance);
                constructors.Should().NotBeEmpty($"Type {type.Name} should have at least one public constructor");
            }
        }

        [Fact]
        public void GetAttributes_ShouldReturnTypesUsableForMessageProcessing()
        {
            // Act
            var result = Trade360AttributeHelper.GetAttributes();

            // Assert
            result.Should().NotBeNull();
            
            // Verify that we can create instances of these types (basic instantiation test)
            foreach (var type in result)
            {
                try
                {
                    var instance = Activator.CreateInstance(type);
                    instance.Should().NotBeNull($"Should be able to create instance of {type.Name}");
                    instance.Should().BeOfType(type);
                }
                catch (Exception ex)
                {
                    Assert.True(false, $"Failed to create instance of {type.Name}: {ex.Message}");
                }
            }
        }

        [Fact]
        public void GetAttributes_ShouldReturnTypesWithEntityKeysInValidRange()
        {
            // Act
            var result = Trade360AttributeHelper.GetAttributes();

            // Assert
            result.Should().NotBeNull();
            
            foreach (var type in result)
            {
                var attribute = type.GetCustomAttribute<Trade360EntityAttribute>();
                attribute.EntityKey.Should().BeInRange(1, 1000, 
                    $"EntityKey for {type.Name} should be in reasonable range (1-1000)");
            }
        }

        [Fact]
        public void GetAttributes_ShouldReturnExpectedMinimumNumberOfTypes()
        {
            // Act
            var result = Trade360AttributeHelper.GetAttributes();

            // Assert
            result.Should().NotBeNull();
            result.Count.Should().BeGreaterThan(5, "Should return at least 6 message types with attributes");
        }

        [Fact]
        public void GetAttributes_ShouldReturnTypesUsableInDictionary()
        {
            // Act
            var result = Trade360AttributeHelper.GetAttributes();

            // Assert
            result.Should().NotBeNull();
            
            // This simulates how the types are used in MessageConsumer
            var messageTypesDictionary = new Dictionary<int, Type>();
            
            foreach (var type in result)
            {
                var attribute = type.GetCustomAttribute<Trade360EntityAttribute>();
                var entityKey = attribute.EntityKey;
                
                // Should be able to add to dictionary without conflicts
                messageTypesDictionary.Should().NotContainKey(entityKey, 
                    $"EntityKey {entityKey} should be unique (conflict with {type.Name})");
                
                messageTypesDictionary.Add(entityKey, type);
            }
            
            messageTypesDictionary.Count.Should().Be(result.Count);
        }

        [Fact]
        public void GetAttributes_ShouldReturnTypesWithCorrectAttributeInheritance()
        {
            // Act
            var result = Trade360AttributeHelper.GetAttributes();

            // Assert
            result.Should().NotBeNull();
            
            foreach (var type in result)
            {
                // Verify the attribute is directly applied, not inherited
                var attributes = type.GetCustomAttributes(typeof(Trade360EntityAttribute), false);
                attributes.Should().HaveCount(1, $"Type {type.Name} should have exactly one Trade360EntityAttribute");
                
                var attribute = attributes[0] as Trade360EntityAttribute;
                attribute.Should().NotBeNull();
                attribute.EntityKey.Should().BeGreaterThan(0);
            }
        }

        [Fact]
        public void GetAttributes_ShouldReturnTypesFromCurrentAssembly()
        {
            // Act
            var result = Trade360AttributeHelper.GetAttributes();

            // Assert
            result.Should().NotBeNull();
            
            var expectedAssembly = Assembly.GetAssembly(typeof(Trade360AttributeHelper));
            
            foreach (var type in result)
            {
                type.Assembly.Should().BeSameAs(expectedAssembly, 
                    $"Type {type.Name} should be from the same assembly as Trade360AttributeHelper");
            }
        }

        [Fact]
        public void GetAttributes_PerformanceTest_ShouldCompleteQuickly()
        {
            // Arrange
            var startTime = DateTime.UtcNow;

            // Act
            var result = Trade360AttributeHelper.GetAttributes();

            // Assert
            var duration = DateTime.UtcNow - startTime;
            
            result.Should().NotBeNull();
            duration.TotalMilliseconds.Should().BeLessThan(1000, 
                "GetAttributes should complete within 1 second");
        }

        [Fact]
        public void GetAttributes_ShouldNotReturnDuplicateTypes()
        {
            // Act
            var result = Trade360AttributeHelper.GetAttributes();

            // Assert
            result.Should().NotBeNull();
            
            var distinctTypes = result.Distinct().ToList();
            result.Count.Should().Be(distinctTypes.Count, "Result should not contain duplicate types");
        }
    }
} 