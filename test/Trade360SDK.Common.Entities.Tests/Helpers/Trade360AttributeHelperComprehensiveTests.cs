using System;
using System.Linq;
using FluentAssertions;
using Trade360SDK.Common.Attributes;
using Trade360SDK.Common.Helpers;
using Xunit;

namespace Trade360SDK.Common.Entities.Tests.Helpers;

public class Trade360AttributeHelperComprehensiveTests
{
    [Fact]
    public void GetAttributes_ShouldReturnNonEmptyList()
    {
        // Act
        var result = Trade360AttributeHelper.GetAttributes();

        // Assert
        result.Should().NotBeNull();
        result.Should().NotBeEmpty();
    }

    [Fact]
    public void GetAttributes_ShouldReturnOnlyTypesWithTrade360EntityAttribute()
    {
        // Act
        var result = Trade360AttributeHelper.GetAttributes();

        // Assert
        result.Should().NotBeNull();
        foreach (var type in result)
        {
            // Each returned type should have the Trade360EntityAttribute
            var hasAttribute = type.GetCustomAttributes(typeof(Trade360EntityAttribute), false).Any();
            hasAttribute.Should().BeTrue($"Type {type.Name} should have Trade360EntityAttribute");
        }
    }

    [Fact]
    public void GetAttributes_ShouldIncludeKnownMessageTypes()
    {
        // Act
        var result = Trade360AttributeHelper.GetAttributes();
        var typeNames = result.Select(t => t.Name).ToList();

        // Assert - Check for some known message types that should have the attribute
        typeNames.Should().Contain("FixtureMetadataUpdate");
        typeNames.Should().Contain("LivescoreUpdate");
        typeNames.Should().Contain("MarketUpdate");
        typeNames.Should().Contain("HeartbeatUpdate");
        typeNames.Should().Contain("KeepAliveUpdate");
    }

    [Fact]
    public void GetAttributes_ResultTypes_ShouldHaveValidEntityKeys()
    {
        // Act
        var result = Trade360AttributeHelper.GetAttributes();

        // Assert
        result.Should().NotBeEmpty();
        
        foreach (var type in result)
        {
            var attribute = Attribute.GetCustomAttribute(type, typeof(Trade360EntityAttribute)) as Trade360EntityAttribute;
            attribute.Should().NotBeNull($"Type {type.Name} should have Trade360EntityAttribute");
            
            // Verify the entity key is accessible
            var entityKey = attribute!.EntityKey;
            entityKey.Should().BeGreaterThanOrEqualTo(0, $"Type {type.Name} should have a valid entity key");
        }
    }

    [Fact]
    public void GetAttributes_ShouldReturnTypesFromCurrentAssembly()
    {
        // Act
        var result = Trade360AttributeHelper.GetAttributes();

        // Assert
        result.Should().NotBeEmpty();
        
        foreach (var type in result)
        {
            // All returned types should be from the Trade360SDK.Common assembly
            type.Assembly.GetName().Name.Should().Be("Trade360SDK.Common");
        }
    }

    [Fact]
    public void GetAttributes_ShouldHandleMultipleCalls()
    {
        // Act
        var result1 = Trade360AttributeHelper.GetAttributes();
        var result2 = Trade360AttributeHelper.GetAttributes();

        // Assert
        result1.Should().NotBeNull();
        result2.Should().NotBeNull();
        result1.Count.Should().Be(result2.Count);
        
        // Both calls should return the same types (order might differ)
        var typeNames1 = result1.Select(t => t.FullName).OrderBy(n => n).ToList();
        var typeNames2 = result2.Select(t => t.FullName).OrderBy(n => n).ToList();
        typeNames1.Should().BeEquivalentTo(typeNames2);
    }

    [Fact]
    public void GetAttributes_ShouldReturnUniqueTypes()
    {
        // Act
        var result = Trade360AttributeHelper.GetAttributes();

        // Assert
        result.Should().NotBeNull();
        var uniqueTypeNames = result.Select(t => t.FullName).Distinct().ToList();
        result.Count.Should().Be(uniqueTypeNames.Count, "All returned types should be unique");
    }

    [Fact]
    public void GetAttributes_ShouldHandleReflectionCorrectly()
    {
        // Act
        var result = Trade360AttributeHelper.GetAttributes();

        // Assert
        result.Should().NotBeNull();
        
        // Verify that all types in the result actually exist and are loadable
        foreach (var type in result)
        {
            type.Should().NotBeNull();
            type.FullName.Should().NotBeNullOrEmpty();
            
            // Verify the type can be instantiated (if it has a parameterless constructor)
            var constructors = type.GetConstructors();
            if (constructors.Any(c => c.GetParameters().Length == 0))
            {
                var instance = Activator.CreateInstance(type);
                instance.Should().NotBeNull();
            }
        }
    }

    [Fact]
    public void GetAttributes_ShouldIncludeCorrectEntityKeys()
    {
        // Act
        var result = Trade360AttributeHelper.GetAttributes();

        // Assert
        result.Should().NotBeEmpty();
        
        // Check for specific known entity keys
        var typeEntityKeys = result.ToDictionary(
            type => type.Name,
            type => ((Trade360EntityAttribute)Attribute.GetCustomAttribute(type, typeof(Trade360EntityAttribute))!).EntityKey
        );

        // Verify some known entity keys
        if (typeEntityKeys.ContainsKey("FixtureMetadataUpdate"))
            typeEntityKeys["FixtureMetadataUpdate"].Should().Be(1);
            
        if (typeEntityKeys.ContainsKey("LivescoreUpdate"))
            typeEntityKeys["LivescoreUpdate"].Should().Be(2);
            
        if (typeEntityKeys.ContainsKey("MarketUpdate"))
            typeEntityKeys["MarketUpdate"].Should().Be(3);
    }

    [Fact]
    public void GetAttributes_Performance_ShouldCompleteReasonablyFast()
    {
        // Act & Assert
        var stopwatch = System.Diagnostics.Stopwatch.StartNew();
        var result = Trade360AttributeHelper.GetAttributes();
        stopwatch.Stop();

        // Should complete within reasonable time (2 seconds is generous for reflection)
        stopwatch.ElapsedMilliseconds.Should().BeLessThan(2000);
        result.Should().NotBeEmpty();
    }

    [Fact]
    public void GetAttributes_ShouldNotIncludeAbstractTypes()
    {
        // Act
        var result = Trade360AttributeHelper.GetAttributes();

        // Assert
        foreach (var type in result)
        {
            // Most concrete message types should not be abstract
            if (type.Name.EndsWith("Update"))
            {
                type.IsAbstract.Should().BeFalse($"Message type {type.Name} should not be abstract");
            }
        }
    }

    [Fact]
    public void GetAttributes_ShouldReturnClassTypes()
    {
        // Act
        var result = Trade360AttributeHelper.GetAttributes();

        // Assert
        result.Should().NotBeEmpty();
        
        foreach (var type in result)
        {
            type.IsClass.Should().BeTrue($"Type {type.Name} should be a class");
            type.IsInterface.Should().BeFalse($"Type {type.Name} should not be an interface");
            type.IsEnum.Should().BeFalse($"Type {type.Name} should not be an enum");
        }
    }

    [Fact]
    public void GetAttributes_ShouldFilterCorrectly()
    {
        // Arrange - Get all types from assembly first
        var allTypes = typeof(Trade360AttributeHelper).Assembly.GetTypes();
        var typesWithAttribute = allTypes.Where(t => t.GetCustomAttributes(typeof(Trade360EntityAttribute), false).Any()).ToList();

        // Act
        var result = Trade360AttributeHelper.GetAttributes();

        // Assert
        result.Count.Should().Be(typesWithAttribute.Count);
        result.Should().BeEquivalentTo(typesWithAttribute);
    }
} 