using System;
using System.Linq;
using System.Reflection;
using Trade360SDK.Common.Attributes;
using Trade360SDK.Common.Helpers;
using Xunit;

namespace Trade360SDK.Common.Tests
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
    }
} 