using Trade360SDK.Common.Attributes;
using Xunit;

namespace Trade360SDK.Common.Entities.Tests.Attributes
{
    public class Trade360EntityAttributeTests
    {
        [Fact]
        public void Constructor_SetsEntityKey()
        {
            // Arrange
            int expectedKey = 42;

            // Act
            var attribute = new Trade360EntityAttribute(expectedKey);

            // Assert
            Assert.Equal(expectedKey, attribute.EntityKey);
        }
    }
} 