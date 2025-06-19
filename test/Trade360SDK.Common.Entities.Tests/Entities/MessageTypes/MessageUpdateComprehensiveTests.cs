using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Xunit;
using Trade360SDK.Common.Entities.MessageTypes;
using Trade360SDK.Common.Entities.Markets;
using Trade360SDK.Common.Entities.Livescore;
using Trade360SDK.Common.Attributes;

namespace Trade360SDK.Common.Entities.Tests.Entities.MessageTypes
{
    public class MessageUpdateComprehensiveTests
    {
        [Fact]
        public void MessageUpdate_DefaultConstructor_ShouldCreateInstance()
        {
            // Act
            var messageUpdate = new MessageUpdate();

            // Assert
            messageUpdate.Should().NotBeNull();
            messageUpdate.GetType().Should().Be(typeof(MessageUpdate));
        }

        [Fact]
        public void MessageUpdate_Type_ShouldBeCorrect()
        {
            // Arrange & Act
            var messageUpdate = new MessageUpdate();

            // Assert
            messageUpdate.GetType().Should().Be(typeof(MessageUpdate));
            messageUpdate.GetType().Name.Should().Be("MessageUpdate");
            messageUpdate.GetType().Namespace.Should().Be("Trade360SDK.Common.Entities.MessageTypes");
        }

        [Fact]
        public void MessageUpdate_ToString_ShouldReturnStringRepresentation()
        {
            // Arrange
            var messageUpdate = new MessageUpdate();

            // Act
            var result = messageUpdate.ToString();

            // Assert
            result.Should().NotBeNullOrEmpty();
            result.Should().Contain("MessageUpdate");
        }

        [Fact]
        public void MessageUpdate_GetHashCode_ShouldReturnConsistentValue()
        {
            // Arrange
            var messageUpdate = new MessageUpdate();

            // Act
            var hashCode1 = messageUpdate.GetHashCode();
            var hashCode2 = messageUpdate.GetHashCode();

            // Assert
            hashCode1.Should().Be(hashCode2);
        }

        [Fact]
        public void MessageUpdate_MultipleInstances_ShouldBeIndependent()
        {
            // Arrange & Act
            var messageUpdate1 = new MessageUpdate();
            var messageUpdate2 = new MessageUpdate();

            // Assert
            messageUpdate1.Should().NotBeSameAs(messageUpdate2);
        }
    }

    public class MarketUpdateComprehensiveTests
    {
        [Fact]
        public void MarketUpdate_DefaultConstructor_ShouldCreateInstanceWithDefaultValues()
        {
            // Act
            var marketUpdate = new MarketUpdate();

            // Assert
            marketUpdate.Should().NotBeNull();
            marketUpdate.Events.Should().BeNull();
        }

        [Fact]
        public void MarketUpdate_ShouldInheritFromMessageUpdate()
        {
            // Arrange & Act
            var marketUpdate = new MarketUpdate();

            // Assert
            marketUpdate.Should().BeAssignableTo<MessageUpdate>();
        }

        [Fact]
        public void MarketUpdate_ShouldHaveTrade360EntityAttribute()
        {
            // Arrange
            var type = typeof(MarketUpdate);

            // Act
            var attribute = Attribute.GetCustomAttribute(type, typeof(Trade360EntityAttribute)) as Trade360EntityAttribute;

            // Assert
            attribute.Should().NotBeNull();
            attribute!.EntityKey.Should().Be(3);
        }

        [Fact]
        public void MarketUpdate_SetEvents_ShouldSetValue()
        {
            // Arrange
            var marketUpdate = new MarketUpdate();
            var events = new List<MarketEvent> { new MarketEvent() };

            // Act
            marketUpdate.Events = events;

            // Assert
            marketUpdate.Events.Should().BeSameAs(events);
        }

        [Fact]
        public void MarketUpdate_SetNullEvents_ShouldSetNull()
        {
            // Arrange
            var marketUpdate = new MarketUpdate();

            // Act
            marketUpdate.Events = null;

            // Assert
            marketUpdate.Events.Should().BeNull();
        }

        [Fact]
        public void MarketUpdate_SetEmptyEvents_ShouldSetEmptyCollection()
        {
            // Arrange
            var marketUpdate = new MarketUpdate();
            var emptyEvents = new List<MarketEvent>();

            // Act
            marketUpdate.Events = emptyEvents;

            // Assert
            marketUpdate.Events.Should().BeEmpty();
        }

        [Fact]
        public void MarketUpdate_Type_ShouldBeCorrect()
        {
            // Arrange & Act
            var marketUpdate = new MarketUpdate();

            // Assert
            marketUpdate.GetType().Should().Be(typeof(MarketUpdate));
            marketUpdate.GetType().Name.Should().Be("MarketUpdate");
            marketUpdate.GetType().Namespace.Should().Be("Trade360SDK.Common.Entities.MessageTypes");
        }
    }

    public class LivescoreUpdateComprehensiveTests
    {
        [Fact]
        public void LivescoreUpdate_DefaultConstructor_ShouldCreateInstanceWithDefaultValues()
        {
            // Act
            var livescoreUpdate = new LivescoreUpdate();

            // Assert
            livescoreUpdate.Should().NotBeNull();
            livescoreUpdate.Events.Should().BeNull();
        }

        [Fact]
        public void LivescoreUpdate_ShouldInheritFromMessageUpdate()
        {
            // Arrange & Act
            var livescoreUpdate = new LivescoreUpdate();

            // Assert
            livescoreUpdate.Should().BeAssignableTo<MessageUpdate>();
        }

        [Fact]
        public void LivescoreUpdate_ShouldHaveTrade360EntityAttribute()
        {
            // Arrange
            var type = typeof(LivescoreUpdate);

            // Act
            var attribute = Attribute.GetCustomAttribute(type, typeof(Trade360EntityAttribute)) as Trade360EntityAttribute;

            // Assert
            attribute.Should().NotBeNull();
            attribute!.EntityKey.Should().Be(2);
        }

        [Fact]
        public void LivescoreUpdate_SetEvents_ShouldSetValue()
        {
            // Arrange
            var livescoreUpdate = new LivescoreUpdate();
            var events = new List<LivescoreEvent> { new LivescoreEvent() };

            // Act
            livescoreUpdate.Events = events;

            // Assert
            livescoreUpdate.Events.Should().BeSameAs(events);
        }

        [Fact]
        public void LivescoreUpdate_Type_ShouldBeCorrect()
        {
            // Arrange & Act
            var livescoreUpdate = new LivescoreUpdate();

            // Assert
            livescoreUpdate.GetType().Should().Be(typeof(LivescoreUpdate));
            livescoreUpdate.GetType().Name.Should().Be("LivescoreUpdate");
        }
    }

    public class SettlementUpdateComprehensiveTests
    {
        [Fact]
        public void SettlementUpdate_DefaultConstructor_ShouldCreateInstanceWithDefaultValues()
        {
            // Act
            var settlementUpdate = new SettlementUpdate();

            // Assert
            settlementUpdate.Should().NotBeNull();
            settlementUpdate.Events.Should().BeNull();
        }

        [Fact]
        public void SettlementUpdate_ShouldInheritFromMessageUpdate()
        {
            // Arrange & Act
            var settlementUpdate = new SettlementUpdate();

            // Assert
            settlementUpdate.Should().BeAssignableTo<MessageUpdate>();
        }

        [Fact]
        public void SettlementUpdate_ShouldHaveTrade360EntityAttribute()
        {
            // Arrange
            var type = typeof(SettlementUpdate);

            // Act
            var attribute = Attribute.GetCustomAttribute(type, typeof(Trade360EntityAttribute)) as Trade360EntityAttribute;

            // Assert
            attribute.Should().NotBeNull();
            attribute!.EntityKey.Should().Be(35);
        }

        [Fact]
        public void SettlementUpdate_SetEvents_ShouldSetValue()
        {
            // Arrange
            var settlementUpdate = new SettlementUpdate();
            var events = new List<MarketEvent> { new MarketEvent() };

            // Act
            settlementUpdate.Events = events;

            // Assert
            settlementUpdate.Events.Should().BeSameAs(events);
        }

        [Fact]
        public void SettlementUpdate_Type_ShouldBeCorrect()
        {
            // Arrange & Act
            var settlementUpdate = new SettlementUpdate();

            // Assert
            settlementUpdate.GetType().Should().Be(typeof(SettlementUpdate));
            settlementUpdate.GetType().Name.Should().Be("SettlementUpdate");
        }
    }

    public class KeepAliveUpdateComprehensiveTests
    {
        [Fact]
        public void KeepAliveUpdate_DefaultConstructor_ShouldCreateInstanceWithDefaultValues()
        {
            // Act
            var keepAliveUpdate = new KeepAliveUpdate();

            // Assert
            keepAliveUpdate.Should().NotBeNull();
            keepAliveUpdate.KeepAlive.Should().BeNull();
        }

        [Fact]
        public void KeepAliveUpdate_ShouldInheritFromMessageUpdate()
        {
            // Arrange & Act
            var keepAliveUpdate = new KeepAliveUpdate();

            // Assert
            keepAliveUpdate.Should().BeAssignableTo<MessageUpdate>();
        }

        [Fact]
        public void KeepAliveUpdate_ShouldHaveTrade360EntityAttribute()
        {
            // Arrange
            var type = typeof(KeepAliveUpdate);

            // Act
            var attribute = Attribute.GetCustomAttribute(type, typeof(Trade360EntityAttribute)) as Trade360EntityAttribute;

            // Assert
            attribute.Should().NotBeNull();
            attribute!.EntityKey.Should().Be(31);
        }

        [Fact]
        public void KeepAliveUpdate_SetKeepAlive_ShouldSetValue()
        {
            // Arrange
            var keepAliveUpdate = new KeepAliveUpdate();
            var keepAlive = new Trade360SDK.Common.Entities.KeepAlive.KeepAlive();

            // Act
            keepAliveUpdate.KeepAlive = keepAlive;

            // Assert
            keepAliveUpdate.KeepAlive.Should().BeSameAs(keepAlive);
        }

        [Fact]
        public void KeepAliveUpdate_Type_ShouldBeCorrect()
        {
            // Arrange & Act
            var keepAliveUpdate = new KeepAliveUpdate();

            // Assert
            keepAliveUpdate.GetType().Should().Be(typeof(KeepAliveUpdate));
            keepAliveUpdate.GetType().Name.Should().Be("KeepAliveUpdate");
        }
    }

    public class HeartbeatUpdateComprehensiveTests
    {
        [Fact]
        public void HeartbeatUpdate_DefaultConstructor_ShouldCreateInstance()
        {
            // Act
            var heartbeatUpdate = new HeartbeatUpdate();

            // Assert
            heartbeatUpdate.Should().NotBeNull();
        }

        [Fact]
        public void HeartbeatUpdate_ShouldInheritFromMessageUpdate()
        {
            // Arrange & Act
            var heartbeatUpdate = new HeartbeatUpdate();

            // Assert
            heartbeatUpdate.Should().BeAssignableTo<MessageUpdate>();
        }

        [Fact]
        public void HeartbeatUpdate_ShouldHaveTrade360EntityAttribute()
        {
            // Arrange
            var type = typeof(HeartbeatUpdate);

            // Act
            var attribute = Attribute.GetCustomAttribute(type, typeof(Trade360EntityAttribute)) as Trade360EntityAttribute;

            // Assert
            attribute.Should().NotBeNull();
            attribute!.EntityKey.Should().Be(32);
        }

        [Fact]
        public void HeartbeatUpdate_Type_ShouldBeCorrect()
        {
            // Arrange & Act
            var heartbeatUpdate = new HeartbeatUpdate();

            // Assert
            heartbeatUpdate.GetType().Should().Be(typeof(HeartbeatUpdate));
            heartbeatUpdate.GetType().Name.Should().Be("HeartbeatUpdate");
        }

        [Fact]
        public void HeartbeatUpdate_ToString_ShouldReturnStringRepresentation()
        {
            // Arrange
            var heartbeatUpdate = new HeartbeatUpdate();

            // Act
            var result = heartbeatUpdate.ToString();

            // Assert
            result.Should().NotBeNullOrEmpty();
            result.Should().Contain("HeartbeatUpdate");
        }

        [Fact]
        public void HeartbeatUpdate_GetHashCode_ShouldReturnConsistentValue()
        {
            // Arrange
            var heartbeatUpdate = new HeartbeatUpdate();

            // Act
            var hashCode1 = heartbeatUpdate.GetHashCode();
            var hashCode2 = heartbeatUpdate.GetHashCode();

            // Assert
            hashCode1.Should().Be(hashCode2);
        }

        [Fact]
        public void HeartbeatUpdate_MultipleInstances_ShouldBeIndependent()
        {
            // Arrange & Act
            var heartbeatUpdate1 = new HeartbeatUpdate();
            var heartbeatUpdate2 = new HeartbeatUpdate();

            // Assert
            heartbeatUpdate1.Should().NotBeSameAs(heartbeatUpdate2);
        }
    }

    public class MessageUpdatePolymorphismTests
    {
        [Fact]
        public void MessageUpdate_DerivedClasses_ShouldBeAssignableToBase()
        {
            // Arrange & Act
            MessageUpdate marketUpdate = new MarketUpdate();
            MessageUpdate livescoreUpdate = new LivescoreUpdate();
            MessageUpdate settlementUpdate = new SettlementUpdate();
            MessageUpdate keepAliveUpdate = new KeepAliveUpdate();
            MessageUpdate heartbeatUpdate = new HeartbeatUpdate();

            // Assert
            marketUpdate.Should().BeAssignableTo<MessageUpdate>();
            livescoreUpdate.Should().BeAssignableTo<MessageUpdate>();
            settlementUpdate.Should().BeAssignableTo<MessageUpdate>();
            keepAliveUpdate.Should().BeAssignableTo<MessageUpdate>();
            heartbeatUpdate.Should().BeAssignableTo<MessageUpdate>();
        }

        [Fact]
        public void MessageUpdate_DerivedClasses_ShouldHaveUniqueEntityKeys()
        {
            // Arrange
            var types = new[]
            {
                typeof(MarketUpdate),
                typeof(LivescoreUpdate),
                typeof(SettlementUpdate),
                typeof(KeepAliveUpdate),
                typeof(HeartbeatUpdate)
            };

            // Act
            var entityKeys = types
                .Select(t => Attribute.GetCustomAttribute(t, typeof(Trade360EntityAttribute)) as Trade360EntityAttribute)
                .Where(attr => attr != null)
                .Select(attr => attr!.EntityKey)
                .ToList();

            // Assert
            entityKeys.Should().OnlyHaveUniqueItems();
            entityKeys.Should().HaveCount(5);
            entityKeys.Should().Contain(new[] { 2, 3, 31, 32, 35 });
        }

        [Fact]
        public void MessageUpdate_Collection_ShouldSupportPolymorphism()
        {
            // Arrange & Act
            var messageUpdates = new List<MessageUpdate>
            {
                new MarketUpdate(),
                new LivescoreUpdate(),
                new SettlementUpdate(),
                new KeepAliveUpdate(),
                new HeartbeatUpdate()
            };

            // Assert
            messageUpdates.Should().HaveCount(5);
            messageUpdates.Should().AllBeAssignableTo<MessageUpdate>();
            messageUpdates.Select(m => m.GetType()).Should().OnlyHaveUniqueItems();
        }
    }
} 