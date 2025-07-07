using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Trade360SDK.Common.Attributes;
using Trade360SDK.Common.Entities.Markets;
using Trade360SDK.Common.Entities.MessageTypes;
using Xunit;

namespace Trade360SDK.Common.Tests.Entities.MessageTypes
{
    public class MarketUpdateTests
    {
        [Fact]
        public void Constructor_ShouldCreateInstanceSuccessfully()
        {
            // Act
            var update = new MarketUpdate();

            // Assert
            update.Should().NotBeNull();
            update.Events.Should().BeNull();
        }

        [Fact]
        public void Properties_ShouldGetAndSetValues()
        {
            // Arrange
            var eventsList = new List<MarketEvent> { new MarketEvent() };
            var update = new MarketUpdate();

            // Act
            update.Events = eventsList;

            // Assert
            update.Events.Should().BeSameAs(eventsList);
        }

        [Fact]
        public void Properties_ShouldAllowNullsAndDefaults()
        {
            // Arrange & Act
            var update = new MarketUpdate();

            // Assert
            update.Events.Should().BeNull();
        }

        [Fact]
        public void Events_ShouldSupportEmptyCollection()
        {
            // Arrange
            var update = new MarketUpdate();
            var emptyEvents = new List<MarketEvent>();

            // Act
            update.Events = emptyEvents;

            // Assert
            update.Events.Should().NotBeNull();
            update.Events.Should().BeEmpty();
        }

        [Fact]
        public void Events_ShouldSupportLargeCollections()
        {
            // Arrange
            var update = new MarketUpdate();
            var largeEventsList = Enumerable.Range(1, 1000)
                .Select(i => new MarketEvent { FixtureId = i })
                .ToList();

            // Act
            update.Events = largeEventsList;

            // Assert
            update.Events.Should().NotBeNull();
            update.Events.Should().HaveCount(1000);
            update.Events.First().FixtureId.Should().Be(1);
            update.Events.Last().FixtureId.Should().Be(1000);
        }

        [Fact]
        public void Events_ShouldSupportDifferentCollectionTypes()
        {
            // Arrange
            var update = new MarketUpdate();
            var eventsArray = new MarketEvent[] { new MarketEvent { FixtureId = 1 }, new MarketEvent { FixtureId = 2 } };

            // Act
            update.Events = eventsArray;

            // Assert
            update.Events.Should().NotBeNull();
            update.Events.Should().HaveCount(2);
        }

        [Fact]
        public void Events_ShouldSupportHashSetCollection()
        {
            // Arrange
            var update = new MarketUpdate();
            var eventsHashSet = new HashSet<MarketEvent>
            {
                new MarketEvent { FixtureId = 100 },
                new MarketEvent { FixtureId = 200 }
            };

            // Act
            update.Events = eventsHashSet;

            // Assert
            update.Events.Should().NotBeNull();
            update.Events.Should().HaveCount(2);
        }

        [Fact]
        public void MarketUpdate_ShouldInheritFromMessageUpdate()
        {
            // Arrange & Act
            var update = new MarketUpdate();

            // Assert
            update.Should().BeAssignableTo<MessageUpdate>();
            typeof(MarketUpdate).BaseType.Should().Be(typeof(MessageUpdate));
        }

        [Fact]
        public void MarketUpdate_ShouldHaveTrade360EntityAttribute()
        {
            // Arrange
            var type = typeof(MarketUpdate);

            // Act
            var attributes = type.GetCustomAttributes(typeof(Trade360EntityAttribute), false);

            // Assert
            attributes.Should().HaveCount(1);
            var attribute = (Trade360EntityAttribute)attributes[0];
            attribute.EntityKey.Should().Be(3);
        }

        [Fact]
        public void MarketUpdate_ShouldSupportPolymorphicAssignment()
        {
            // Arrange
            var update = new MarketUpdate
            {
                Events = new List<MarketEvent> { new MarketEvent { FixtureId = 123 } }
            };

            // Act
            MessageUpdate baseUpdate = update;

            // Assert
            baseUpdate.Should().BeOfType<MarketUpdate>();
            ((MarketUpdate)baseUpdate).Events.Should().NotBeNull();
            ((MarketUpdate)baseUpdate).Events!.First().FixtureId.Should().Be(123);
        }

        [Fact]
        public void Events_ShouldAllowReassignment()
        {
            // Arrange
            var update = new MarketUpdate();
            var firstEvents = new List<MarketEvent> { new MarketEvent { FixtureId = 1 } };
            var secondEvents = new List<MarketEvent> { new MarketEvent { FixtureId = 2 } };

            // Act
            update.Events = firstEvents;
            update.Events = secondEvents;

            // Assert
            update.Events.Should().BeSameAs(secondEvents);
            update.Events.First().FixtureId.Should().Be(2);
        }

        [Fact]
        public void Events_ShouldAllowNullAfterAssignment()
        {
            // Arrange
            var update = new MarketUpdate();
            var events = new List<MarketEvent> { new MarketEvent() };
            update.Events = events;

            // Act
            update.Events = null;

            // Assert
            update.Events.Should().BeNull();
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(5)]
        [InlineData(100)]
        public void Events_ShouldHandleVariousCollectionSizes(int eventCount)
        {
            // Arrange
            var update = new MarketUpdate();
            var events = Enumerable.Range(1, eventCount)
                .Select(i => new MarketEvent { FixtureId = i })
                .ToList();

            // Act
            update.Events = events;

            // Assert
            if (eventCount == 0)
            {
                update.Events.Should().BeEmpty();
            }
            else
            {
                update.Events.Should().HaveCount(eventCount);
                update.Events.First().FixtureId.Should().Be(1);
            }
        }

        [Fact]
        public void MarketUpdate_ShouldSupportObjectInitializer()
        {
            // Arrange & Act
            var update = new MarketUpdate
            {
                Events = new List<MarketEvent>
                {
                    new MarketEvent { FixtureId = 42 },
                    new MarketEvent { FixtureId = 84 }
                }
            };

            // Assert
            update.Events.Should().NotBeNull();
            update.Events.Should().HaveCount(2);
            update.Events.ElementAt(0).FixtureId.Should().Be(42);
            update.Events.ElementAt(1).FixtureId.Should().Be(84);
        }

        [Fact]
        public void MarketUpdate_ShouldSupportReflectionAccess()
        {
            // Arrange
            var update = new MarketUpdate();
            var type = typeof(MarketUpdate);
            var eventsProperty = type.GetProperty("Events");

            // Act
            eventsProperty!.SetValue(update, new List<MarketEvent> { new MarketEvent { FixtureId = 999 } });

            // Assert
            var eventsValue = (IEnumerable<MarketEvent>?)eventsProperty.GetValue(update);
            eventsValue.Should().NotBeNull();
            eventsValue!.First().FixtureId.Should().Be(999);
        }

        [Fact]
        public void MarketUpdate_ShouldBeInCorrectNamespace()
        {
            // Arrange
            var type = typeof(MarketUpdate);

            // Act & Assert
            type.Namespace.Should().Be("Trade360SDK.Common.Entities.MessageTypes");
            type.FullName.Should().Be("Trade360SDK.Common.Entities.MessageTypes.MarketUpdate");
        }

        [Fact]
        public void Events_WithComplexMarketEvents_ShouldMaintainIntegrity()
        {
            // Arrange
            var update = new MarketUpdate();
            var marketEvent = new MarketEvent
            {
                FixtureId = 12345,
                Markets = new List<Market>
                {
                    new Market { Id = 1, Name = "Test Market" }
                }
            };

            // Act
            update.Events = new List<MarketEvent> { marketEvent };

            // Assert
            update.Events.Should().NotBeNull();
            update.Events.First().FixtureId.Should().Be(12345);
            update.Events.First().Markets.Should().NotBeNull();
            update.Events.First().Markets!.First().Name.Should().Be("Test Market");
        }

        [Fact]
        public void Events_ShouldHandleDuplicateFixtureIds()
        {
            // Arrange
            var update = new MarketUpdate();
            var events = new List<MarketEvent>
            {
                new MarketEvent { FixtureId = 100 },
                new MarketEvent { FixtureId = 100 },
                new MarketEvent { FixtureId = 200 }
            };

            // Act
            update.Events = events;

            // Assert
            update.Events.Should().HaveCount(3);
            update.Events.Count(e => e.FixtureId == 100).Should().Be(2);
            update.Events.Count(e => e.FixtureId == 200).Should().Be(1);
        }
    }
} 