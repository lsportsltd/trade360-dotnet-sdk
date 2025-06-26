using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Trade360SDK.Common.Attributes;
using Trade360SDK.Common.Helpers;
using Xunit;

namespace Trade360SDK.Common.Entities.Tests.Entities.Helpers
{
    public class Trade360AttributeHelperComprehensiveTests
    {
        [Fact]
        public void GetAttributes_ShouldReturnListOfTypes()
        {
            // Act
            var result = Trade360AttributeHelper.GetAttributes();

            // Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<List<Type>>();
        }

        [Fact]
        public void GetAttributes_ShouldReturnTypesWithTrade360EntityAttribute()
        {
            // Act
            var result = Trade360AttributeHelper.GetAttributes();

            // Assert
            result.Should().NotBeEmpty();
            foreach (var type in result)
            {
                var attribute = type.GetCustomAttributes(typeof(Trade360EntityAttribute), false);
                attribute.Should().NotBeEmpty("All returned types should have Trade360EntityAttribute");
            }
        }

        [Fact]
        public void GetAttributes_ShouldReturnConsistentResults()
        {
            // Act
            var result1 = Trade360AttributeHelper.GetAttributes();
            var result2 = Trade360AttributeHelper.GetAttributes();

            // Assert
            result1.Should().HaveCount(result2.Count());
            result1.Should().BeEquivalentTo(result2);
        }

        [Fact]
        public void GetAttributes_ShouldIncludeKnownAttributedTypes()
        {
            // Act
            var result = Trade360AttributeHelper.GetAttributes();
            var typeNames = result.Select(t => t.Name).ToList();

            // Assert - Check for known types that have Trade360EntityAttribute
            typeNames.Should().Contain("MarketUpdate", "Types with Trade360EntityAttribute should be included");
            typeNames.Should().Contain("SettlementUpdate", "Types with Trade360EntityAttribute should be included");
            typeNames.Should().Contain("HeartbeatUpdate", "Types with Trade360EntityAttribute should be included");
        }

        [Fact]
        public void GetAttributes_ShouldOnlyReturnTypesFromCurrentAssembly()
        {
            // Act
            var result = Trade360AttributeHelper.GetAttributes();

            // Assert
            foreach (var type in result)
            {
                type.Assembly.Should().BeSameAs(typeof(Trade360AttributeHelper).Assembly, 
                    "Should only return types from the executing assembly");
            }
        }

        [Fact]
        public void GetAttributes_ShouldHandleEmptyResultGracefully()
        {
            // Act
            var result = Trade360AttributeHelper.GetAttributes();

            // Assert
            result.Should().NotBeNull("Should never return null");
        }

        [Fact]
        public void GetAttributes_TypesShouldHaveValidEntityKeys()
        {
            // Act
            var result = Trade360AttributeHelper.GetAttributes();

            // Assert
            foreach (var type in result)
            {
                var attribute = type.GetCustomAttributes(typeof(Trade360EntityAttribute), false)
                    .Cast<Trade360EntityAttribute>()
                    .FirstOrDefault();
                
                attribute.Should().NotBeNull();
                attribute.EntityKey.Should().BeGreaterThan(0, "EntityKey should be positive");
            }
        }

        [Fact]
        public void GetAttributes_ShouldReturnUniqueTypes()
        {
            // Act
            var result = Trade360AttributeHelper.GetAttributes();

            // Assert
            var distinctTypes = result.Distinct().ToList();
            distinctTypes.Should().HaveCount(result.Count, "All returned types should be unique");
        }

        [Fact]
        public void GetAttributes_ShouldReturnTypesWithUniqueEntityKeys()
        {
            // Act
            var result = Trade360AttributeHelper.GetAttributes();

            // Assert
            var entityKeys = result.Select(type =>
            {
                var attribute = type.GetCustomAttributes(typeof(Trade360EntityAttribute), false)
                    .Cast<Trade360EntityAttribute>()
                    .FirstOrDefault();
                return attribute?.EntityKey ?? 0;
            }).ToList();

            var distinctKeys = entityKeys.Distinct().ToList();
            distinctKeys.Should().HaveCount(entityKeys.Count, "All EntityKeys should be unique");
        }

        [Fact]
        public void GetAttributes_PerformanceTest_ShouldCompleteQuickly()
        {
            // Act & Assert
            var startTime = DateTime.UtcNow;
            var result = Trade360AttributeHelper.GetAttributes();
            var endTime = DateTime.UtcNow;
            
            var duration = endTime - startTime;
            duration.Should().BeLessThan(TimeSpan.FromSeconds(1), "Should complete within reasonable time");
            result.Should().NotBeNull();
        }
    }
} 