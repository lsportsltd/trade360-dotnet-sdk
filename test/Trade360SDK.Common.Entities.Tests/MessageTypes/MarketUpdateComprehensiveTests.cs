using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Trade360SDK.Common.Attributes;
using Trade360SDK.Common.Entities.Markets;
using Trade360SDK.Common.Entities.MessageTypes;
using Xunit;

namespace Trade360SDK.Common.Entities.Tests.MessageTypes;

public class MarketUpdateComprehensiveTests
{
    [Fact]
    public void Constructor_ShouldCreateInstanceSuccessfully()
    {
        // Act
        var marketUpdate = new MarketUpdate();

        // Assert
        marketUpdate.Should().NotBeNull();
        marketUpdate.Should().BeAssignableTo<MessageUpdate>();
    }

    [Fact]
    public void Events_Property_ShouldAllowNullValue()
    {
        // Arrange
        var marketUpdate = new MarketUpdate();

        // Act
        marketUpdate.Events = null;

        // Assert
        marketUpdate.Events.Should().BeNull();
    }

    [Fact]
    public void Events_Property_ShouldAllowEmptyCollection()
    {
        // Arrange
        var marketUpdate = new MarketUpdate();
        var emptyEvents = new List<MarketEvent>();

        // Act
        marketUpdate.Events = emptyEvents;

        // Assert
        marketUpdate.Events.Should().NotBeNull();
        marketUpdate.Events.Should().BeEmpty();
    }

    [Fact]
    public void Events_Property_ShouldAllowCollectionWithMultipleItems()
    {
        // Arrange
        var marketUpdate = new MarketUpdate();
        var events = new List<MarketEvent>
        {
            new MarketEvent { FixtureId = 1 },
            new MarketEvent { FixtureId = 2 },
            new MarketEvent { FixtureId = 3 }
        };

        // Act
        marketUpdate.Events = events;

        // Assert
        marketUpdate.Events.Should().NotBeNull();
        marketUpdate.Events.Should().HaveCount(3);
        marketUpdate.Events!.First().FixtureId.Should().Be(1);
        marketUpdate.Events.Last().FixtureId.Should().Be(3);
    }

    [Fact]
    public void Events_Property_ShouldBeSettableAndGettable()
    {
        // Arrange
        var marketUpdate = new MarketUpdate();
        var marketEvent = new MarketEvent { FixtureId = 42 };
        var events = new List<MarketEvent> { marketEvent };

        // Act
        marketUpdate.Events = events;

        // Assert
        marketUpdate.Events.Should().BeSameAs(events);
        marketUpdate.Events!.Single().Should().BeSameAs(marketEvent);
    }

    [Fact]
    public void Trade360EntityAttribute_ShouldBePresent()
    {
        // Arrange
        var marketUpdateType = typeof(MarketUpdate);

        // Act
        var attribute = Attribute.GetCustomAttribute(marketUpdateType, typeof(Trade360EntityAttribute)) as Trade360EntityAttribute;

        // Assert
        attribute.Should().NotBeNull();
        attribute!.EntityKey.Should().Be(3);
    }

    [Fact]
    public void Trade360EntityAttribute_ShouldBeRetrievableViaReflection()
    {
        // Arrange
        var marketUpdateType = typeof(MarketUpdate);

        // Act
        var attributes = marketUpdateType.GetCustomAttributes(typeof(Trade360EntityAttribute), false);

        // Assert
        attributes.Should().HaveCount(1);
        attributes[0].Should().BeOfType<Trade360EntityAttribute>();
        ((Trade360EntityAttribute)attributes[0]).EntityKey.Should().Be(3);
    }

    [Fact]
    public void Inheritance_ShouldInheritFromMessageUpdate()
    {
        // Arrange & Act
        var marketUpdate = new MarketUpdate();

        // Assert
        marketUpdate.Should().BeAssignableTo<MessageUpdate>();
    }

    [Fact]
    public void Type_ShouldHaveCorrectNamespace()
    {
        // Arrange
        var marketUpdateType = typeof(MarketUpdate);

        // Act & Assert
        marketUpdateType.Namespace.Should().Be("Trade360SDK.Common.Entities.MessageTypes");
        marketUpdateType.Name.Should().Be("MarketUpdate");
    }

    [Fact]
    public void Events_Property_ShouldSupportLinqOperations()
    {
        // Arrange
        var marketUpdate = new MarketUpdate();
        var events = new List<MarketEvent>
        {
            new MarketEvent { FixtureId = 1 },
            new MarketEvent { FixtureId = 2 },
            new MarketEvent { FixtureId = 3 },
            new MarketEvent { FixtureId = 4 }
        };
        marketUpdate.Events = events;

        // Act
        var filteredEvents = marketUpdate.Events!.Where(e => e.FixtureId > 2).ToList();

        // Assert
        filteredEvents.Should().HaveCount(2);
        filteredEvents.Should().Contain(e => e.FixtureId == 3);
        filteredEvents.Should().Contain(e => e.FixtureId == 4);
    }

    [Fact]
    public void Events_Property_ShouldHandleLargeCollections()
    {
        // Arrange
        var marketUpdate = new MarketUpdate();
        var largeEventList = Enumerable.Range(1, 1000)
            .Select(i => new MarketEvent { FixtureId = i })
            .ToList();

        // Act
        marketUpdate.Events = largeEventList;

        // Assert
        marketUpdate.Events.Should().HaveCount(1000);
        marketUpdate.Events!.First().FixtureId.Should().Be(1);
        marketUpdate.Events.Last().FixtureId.Should().Be(1000);
    }

    [Fact]
    public void Events_Property_ShouldAllowReassignment()
    {
        // Arrange
        var marketUpdate = new MarketUpdate();
        var firstEvents = new List<MarketEvent> { new MarketEvent { FixtureId = 1 } };
        var secondEvents = new List<MarketEvent> { new MarketEvent { FixtureId = 2 } };

        // Act
        marketUpdate.Events = firstEvents;
        var firstResult = marketUpdate.Events;
        
        marketUpdate.Events = secondEvents;
        var secondResult = marketUpdate.Events;

        // Assert
        firstResult.Should().BeSameAs(firstEvents);
        secondResult.Should().BeSameAs(secondEvents);
        secondResult.Should().NotBeSameAs(firstEvents);
        secondResult!.Single().FixtureId.Should().Be(2);
    }

    [Fact]
    public void ToString_ShouldReturnMeaningfulString()
    {
        // Arrange
        var marketUpdate = new MarketUpdate();

        // Act
        var result = marketUpdate.ToString();

        // Assert
        result.Should().NotBeNullOrEmpty();
        result.Should().Contain("MarketUpdate");
    }

    [Fact]
    public void GetHashCode_ShouldBeConsistent()
    {
        // Arrange
        var marketUpdate = new MarketUpdate();

        // Act
        var hashCode1 = marketUpdate.GetHashCode();
        var hashCode2 = marketUpdate.GetHashCode();

        // Assert
        hashCode1.Should().Be(hashCode2);
    }

    [Fact]
    public void Events_WithNullableMarketEvents_ShouldHandleCorrectly()
    {
        // Arrange
        var marketUpdate = new MarketUpdate();
        var events = new List<MarketEvent>
        {
            new MarketEvent { FixtureId = 1 },
            new MarketEvent { FixtureId = 2 }
        };

        // Act
        marketUpdate.Events = events;

        // Assert
        marketUpdate.Events.Should().NotBeNull();
        marketUpdate.Events.Should().HaveCount(2);
        marketUpdate.Events!.All(e => e != null).Should().BeTrue();
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(5)]
    [InlineData(10)]
    public void Events_WithVariousCollectionSizes_ShouldWorkCorrectly(int eventCount)
    {
        // Arrange
        var marketUpdate = new MarketUpdate();
        var events = Enumerable.Range(1, eventCount)
            .Select(i => new MarketEvent { FixtureId = i })
            .ToList();

        // Act
        marketUpdate.Events = events;

        // Assert
        marketUpdate.Events.Should().HaveCount(eventCount);
        if (eventCount > 0)
        {
            marketUpdate.Events!.First().FixtureId.Should().Be(1);
            marketUpdate.Events.Last().FixtureId.Should().Be(eventCount);
        }
    }

    [Fact]
    public void Constructor_ShouldInitializeWithDefaultValues()
    {
        // Act
        var marketUpdate = new MarketUpdate();

        // Assert
        marketUpdate.Events.Should().BeNull();
    }

    [Fact]
    public void Events_WithMarkets_ShouldHandleNestedCollections()
    {
        // Arrange
        var marketUpdate = new MarketUpdate();
        var marketEvent = new MarketEvent 
        { 
            FixtureId = 100,
            Markets = new List<Market>
            {
                new Market { Id = 1, Name = "Market 1" },
                new Market { Id = 2, Name = "Market 2" }
            }
        };
        var events = new List<MarketEvent> { marketEvent };

        // Act
        marketUpdate.Events = events;

        // Assert
        marketUpdate.Events.Should().HaveCount(1);
        var firstEvent = marketUpdate.Events!.First();
        firstEvent.FixtureId.Should().Be(100);
        firstEvent.Markets.Should().HaveCount(2);
        firstEvent.Markets!.First().Id.Should().Be(1);
        firstEvent.Markets.Last().Id.Should().Be(2);
    }
} 