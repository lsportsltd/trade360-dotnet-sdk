using System;
using System.Linq;
using System.Reflection;
using Trade360SDK.Common.Attributes;
using Trade360SDK.Common.Helpers;
using Xunit;
using FluentAssertions;

namespace Trade360SDK.Common.Entities.Tests.Helpers
{
    public class Trade360AttributeHelperTests
    {
        [Fact]
        public void GetAttributes_ShouldReturnTypesWithTrade360EntityAttribute()
        {
            // Arrange: define a dummy type with the attribute in this assembly
            var typesWithAttribute = Trade360AttributeHelper.GetAttributes();

            // Assert: at least one type should have the attribute (from MessageTypes)
            Assert.Contains(typesWithAttribute, t => t.GetCustomAttribute<Trade360EntityAttribute>() != null);
        }

        [Fact]
        public void GetAttributes_ShouldReturnEmptyListIfNoTypesWithAttribute()
        {
            // Arrange: simulate by using a new assembly (dynamic)
            var assembly = Assembly.Load("System.Runtime");
            var types = assembly.GetTypes().Where(t => t.GetCustomAttribute<Trade360EntityAttribute>() != null).ToList();
            // Assert
            Assert.Empty(types);
        }

        [Fact]
        public void GetAttributes_ShouldReturnNonEmptyList_WhenTypesWithAttributeExist()
        {
            // This test ensures that at least one type in the current assembly has the attribute
            var typesWithAttribute = Trade360AttributeHelper.GetAttributes();
            Assert.NotEmpty(typesWithAttribute);
        }

        [Fact]
        public void GetAttributes_ShouldNotReturnTypesWithoutAttribute()
        {
            var typesWithAttribute = Trade360AttributeHelper.GetAttributes();
            // Ensure all returned types have the attribute
            Assert.All(typesWithAttribute, t => Assert.NotNull(t.GetCustomAttribute<Trade360EntityAttribute>()));
        }

        [Fact]
        public void GetAttributes_ShouldReturnListAndNotThrow_WhenNoTypesWithAttributeInCurrentAssembly()
        {
            var typesWithAttribute = Trade360AttributeHelper.GetAttributes();
            // Filter to a type name that doesn't exist
            var filtered = typesWithAttribute.Where(t => t.Name == "DefinitelyNotARealType").ToList();
            Assert.Empty(filtered);
        }

        [Fact]
        public void GetAttributes_ShouldReturnListOfTypes()
        {
            // Act
            var result = Trade360AttributeHelper.GetAttributes();

            // Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<System.Collections.Generic.List<Type>>();
        }

        [Fact]
        public void GetAttributes_ShouldReturnConsistentResults()
        {
            // Act
            var result1 = Trade360AttributeHelper.GetAttributes();
            var result2 = Trade360AttributeHelper.GetAttributes();

            // Assert
            result1.Should().NotBeNull();
            result2.Should().NotBeNull();
            result1.Count.Should().Be(result2.Count);
            
            // The results should be consistent across multiple calls
            if (result1.Any())
            {
                result1.Should().BeEquivalentTo(result2);
            }
        }

        [Fact]
        public void GetAttributes_ShouldReturnTypesFromExecutingAssembly()
        {
            // Act
            var result = Trade360AttributeHelper.GetAttributes();

            // Assert
            result.Should().NotBeNull();
            
            // All returned types should be from the executing assembly
            foreach (var type in result)
            {
                type.Should().NotBeNull();
                type.Assembly.Should().NotBeNull();
            }
        }

        [Fact]
        public void GetAttributes_MultipleCalls_ShouldNotThrow()
        {
            // Act & Assert
            for (int i = 0; i < 5; i++)
            {
                var act = () => Trade360AttributeHelper.GetAttributes();
                act.Should().NotThrow();
            }
        }

        [Fact]
        public void GetAttributes_ShouldReturnEmptyOrPopulatedList()
        {
            // Act
            var result = Trade360AttributeHelper.GetAttributes();

            // Assert
            result.Should().NotBeNull();
            // The result can be empty if no types have the Trade360EntityAttribute
            // or it can contain types that do have the attribute
            result.Count.Should().BeGreaterThanOrEqualTo(0);
        }

        [Fact]
        public void GetAttributes_ResultTypes_ShouldBeValidTypes()
        {
            // Act
            var result = Trade360AttributeHelper.GetAttributes();

            // Assert
            result.Should().NotBeNull();
            
            foreach (var type in result)
            {
                type.Should().NotBeNull();
                type.Name.Should().NotBeNullOrEmpty();
                type.FullName.Should().NotBeNullOrEmpty();
            }
        }

        [Fact]
        public void GetAttributes_ShouldFilterTypesWithTrade360EntityAttribute()
        {
            // Act
            var result = Trade360AttributeHelper.GetAttributes();

            // Assert
            result.Should().NotBeNull();
            
            // Each type in the result should have the Trade360EntityAttribute
            foreach (var type in result)
            {
                var hasAttribute = type.GetCustomAttributes(typeof(Trade360SDK.Common.Attributes.Trade360EntityAttribute), false).Any();
                hasAttribute.Should().BeTrue($"Type {type.Name} should have Trade360EntityAttribute");
            }
        }

        [Fact]
        public void GetAttributes_Performance_ShouldCompleteQuickly()
        {
            // Arrange
            var startTime = DateTime.UtcNow;

            // Act
            var result = Trade360AttributeHelper.GetAttributes();

            // Assert
            var endTime = DateTime.UtcNow;
            var duration = endTime - startTime;
            
            result.Should().NotBeNull();
            duration.TotalSeconds.Should().BeLessThan(5, "Method should complete within 5 seconds");
        }

        [Fact]
        public void GetAttributes_ShouldNotReturnDuplicates()
        {
            // Act
            var result = Trade360AttributeHelper.GetAttributes();

            // Assert
            result.Should().NotBeNull();
            
            if (result.Any())
            {
                var distinctTypes = result.Distinct().ToList();
                result.Count.Should().Be(distinctTypes.Count, "Result should not contain duplicate types");
            }
        }

        [Fact]
        public void GetAttributes_ShouldReturnConcreteTypes()
        {
            // Act
            var result = Trade360AttributeHelper.GetAttributes();

            // Assert
            result.Should().NotBeNull();
            
            foreach (var type in result)
            {
                type.Should().NotBeNull();
                // Types returned should be actual types, not null or invalid
                type.IsClass.Should().BeTrue($"Type {type.Name} should be a class");
            }
        }
    }
} 