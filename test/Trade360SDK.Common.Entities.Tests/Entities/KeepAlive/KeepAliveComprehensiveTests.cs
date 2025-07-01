using FluentAssertions;
using Trade360SDK.Common.Entities.KeepAlive;
using Trade360SDK.Common.Entities.Shared;
using Xunit;
using System.Collections.Generic;
using System.Linq;

namespace Trade360SDK.Common.Entities.Tests.Entities.KeepAlive
{
    public class KeepAliveComprehensiveTests
    {
        [Fact]
        public void KeepAlive_DefaultConstructor_ShouldInitializeWithDefaults()
        {
            // Arrange & Act
            var keepAlive = new Trade360SDK.Common.Entities.KeepAlive.KeepAlive();

            // Assert
            keepAlive.ActiveEvents.Should().BeNull();
            keepAlive.ExtraData.Should().BeNull();
            keepAlive.ProviderId.Should().BeNull();
        }

        [Fact]
        public void ProviderId_WhenSet_ShouldReturnExpectedValue()
        {
            // Arrange
            var keepAlive = new Trade360SDK.Common.Entities.KeepAlive.KeepAlive();
            const int expectedProviderId = 12345;

            // Act
            keepAlive.ProviderId = expectedProviderId;

            // Assert
            keepAlive.ProviderId.Should().Be(expectedProviderId);
        }

        [Theory]
        [InlineData(null)]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(-1)]
        [InlineData(int.MaxValue)]
        [InlineData(int.MinValue)]
        public void ProviderId_WithVariousValues_ShouldStoreCorrectly(int? providerId)
        {
            // Arrange
            var keepAlive = new Trade360SDK.Common.Entities.KeepAlive.KeepAlive();

            // Act
            keepAlive.ProviderId = providerId;

            // Assert
            keepAlive.ProviderId.Should().Be(providerId);
        }

        [Fact]
        public void ActiveEvents_WhenSetToCollection_ShouldReturnExpectedValue()
        {
            // Arrange
            var keepAlive = new Trade360SDK.Common.Entities.KeepAlive.KeepAlive();
            var activeEvents = new List<int> { 1001, 1002, 1003 };

            // Act
            keepAlive.ActiveEvents = activeEvents;

            // Assert
            keepAlive.ActiveEvents.Should().NotBeNull();
            keepAlive.ActiveEvents.Should().HaveCount(3);
            keepAlive.ActiveEvents.Should().BeEquivalentTo(activeEvents);
        }

        [Fact]
        public void ActiveEvents_WhenSetToEmptyCollection_ShouldHandleGracefully()
        {
            // Arrange
            var keepAlive = new Trade360SDK.Common.Entities.KeepAlive.KeepAlive();
            var emptyEvents = new List<int>();

            // Act
            keepAlive.ActiveEvents = emptyEvents;

            // Assert
            keepAlive.ActiveEvents.Should().NotBeNull().And.BeEmpty();
        }

        [Fact]
        public void ExtraData_WhenSetToCollection_ShouldReturnExpectedValue()
        {
            // Arrange
            var keepAlive = new Trade360SDK.Common.Entities.KeepAlive.KeepAlive();
            var extraData = new List<NameValuePair>
            {
                new NameValuePair { Name = "key1", Value = "value1" },
                new NameValuePair { Name = "key2", Value = "value2" }
            };

            // Act
            keepAlive.ExtraData = extraData;

            // Assert
            keepAlive.ExtraData.Should().NotBeNull();
            keepAlive.ExtraData.Should().HaveCount(2);
            keepAlive.ExtraData.Should().ContainEquivalentOf(extraData[0]);
            keepAlive.ExtraData.Should().ContainEquivalentOf(extraData[1]);
        }

        [Fact]
        public void ExtraData_WhenSetToEmptyCollection_ShouldHandleGracefully()
        {
            // Arrange
            var keepAlive = new Trade360SDK.Common.Entities.KeepAlive.KeepAlive();
            var emptyExtraData = new List<NameValuePair>();

            // Act
            keepAlive.ExtraData = emptyExtraData;

            // Assert
            keepAlive.ExtraData.Should().NotBeNull().And.BeEmpty();
        }

        [Fact]
        public void KeepAlive_WithAllPropertiesSet_ShouldRetainAllValues()
        {
            // Arrange
            const int expectedProviderId = 999;
            var expectedActiveEvents = new List<int> { 100, 200, 300 };
            var expectedExtraData = new List<NameValuePair>
            {
                new NameValuePair { Name = "timeout", Value = "30" },
                new NameValuePair { Name = "region", Value = "EU" }
            };

            var keepAlive = new Trade360SDK.Common.Entities.KeepAlive.KeepAlive
            {
                ProviderId = expectedProviderId,
                ActiveEvents = expectedActiveEvents,
                ExtraData = expectedExtraData
            };

            // Act & Assert
            keepAlive.ProviderId.Should().Be(expectedProviderId);
            keepAlive.ActiveEvents.Should().BeEquivalentTo(expectedActiveEvents);
            keepAlive.ExtraData.Should().BeEquivalentTo(expectedExtraData);
        }

        [Fact]
        public void KeepAlive_PropertiesCanBeOverwritten()
        {
            // Arrange
            var keepAlive = new Trade360SDK.Common.Entities.KeepAlive.KeepAlive
            {
                ProviderId = 123,
                ActiveEvents = new List<int> { 1, 2, 3 },
                ExtraData = new List<NameValuePair> { new NameValuePair { Name = "old" } }
            };

            const int newProviderId = 456;
            var newActiveEvents = new List<int> { 4, 5, 6 };
            var newExtraData = new List<NameValuePair> { new NameValuePair { Name = "new" } };

            // Act
            keepAlive.ProviderId = newProviderId;
            keepAlive.ActiveEvents = newActiveEvents;
            keepAlive.ExtraData = newExtraData;

            // Assert
            keepAlive.ProviderId.Should().Be(newProviderId);
            keepAlive.ActiveEvents.Should().BeEquivalentTo(newActiveEvents);
            keepAlive.ExtraData.Should().BeEquivalentTo(newExtraData);
        }

        [Fact]
        public void KeepAlive_CollectionProperties_CanBeSetToNull()
        {
            // Arrange
            var keepAlive = new Trade360SDK.Common.Entities.KeepAlive.KeepAlive
            {
                ActiveEvents = new List<int> { 1, 2, 3 },
                ExtraData = new List<NameValuePair> { new NameValuePair() }
            };

            // Act
            keepAlive.ActiveEvents = null;
            keepAlive.ExtraData = null;

            // Assert
            keepAlive.ActiveEvents.Should().BeNull();
            keepAlive.ExtraData.Should().BeNull();
        }

        [Fact]
        public void KeepAlive_ProviderId_CanBeSetToNull()
        {
            // Arrange
            var keepAlive = new Trade360SDK.Common.Entities.KeepAlive.KeepAlive
            {
                ProviderId = 999
            };

            // Act
            keepAlive.ProviderId = null;

            // Assert
            keepAlive.ProviderId.Should().BeNull();
        }

        [Fact]
        public void ActiveEvents_WithDuplicateValues_ShouldStoreAllValues()
        {
            // Arrange
            var keepAlive = new Trade360SDK.Common.Entities.KeepAlive.KeepAlive();
            var eventsWithDuplicates = new List<int> { 100, 200, 100, 300, 200 };

            // Act
            keepAlive.ActiveEvents = eventsWithDuplicates;

            // Assert
            keepAlive.ActiveEvents.Should().NotBeNull();
            keepAlive.ActiveEvents.Should().HaveCount(5);
            keepAlive.ActiveEvents.Should().BeEquivalentTo(eventsWithDuplicates);
        }

        [Fact]
        public void ActiveEvents_WithNegativeValues_ShouldStoreCorrectly()
        {
            // Arrange
            var keepAlive = new Trade360SDK.Common.Entities.KeepAlive.KeepAlive();
            var eventsWithNegatives = new List<int> { -1, 0, 1, -999, int.MinValue, int.MaxValue };

            // Act
            keepAlive.ActiveEvents = eventsWithNegatives;

            // Assert
            keepAlive.ActiveEvents.Should().NotBeNull();
            keepAlive.ActiveEvents.Should().HaveCount(6);
            keepAlive.ActiveEvents.Should().BeEquivalentTo(eventsWithNegatives);
        }
    }
} 