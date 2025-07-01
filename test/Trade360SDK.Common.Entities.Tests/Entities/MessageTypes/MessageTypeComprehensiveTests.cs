using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FluentAssertions;
using Trade360SDK.Common.Attributes;
using Trade360SDK.Common.Entities.MessageTypes;
using Trade360SDK.Common.Entities.Markets;
using Trade360SDK.Common.Entities.Livescore;
using Trade360SDK.Common.Entities.Fixtures;
using Trade360SDK.Common.Entities.OutrightLeague;
using Xunit;

namespace Trade360SDK.Common.Entities.Tests.Entities.MessageTypes
{
    public class MessageTypeComprehensiveTests
    {
        #region FixtureMetadataUpdate Tests

        [Fact]
        public void FixtureMetadataUpdate_DefaultConstructor_ShouldInitializeCorrectly()
        {
            // Act
            var update = new FixtureMetadataUpdate();

            // Assert
            update.Should().NotBeNull();
            update.Events.Should().BeNull();
        }

        [Fact]
        public void FixtureMetadataUpdate_ShouldHaveCorrectEntityAttribute()
        {
            // Act
            var attribute = typeof(FixtureMetadataUpdate).GetCustomAttribute<Trade360EntityAttribute>();

            // Assert
            attribute.Should().NotBeNull();
            attribute.EntityKey.Should().Be(1);
        }

        [Fact]
        public void FixtureMetadataUpdate_ShouldInheritFromMessageUpdate()
        {
            // Act
            var update = new FixtureMetadataUpdate();

            // Assert
            update.Should().BeAssignableTo<MessageUpdate>();
        }

        [Fact]
        public void FixtureMetadataUpdate_EventsProperty_ShouldBeSettableAndGettable()
        {
            // Arrange
            var update = new FixtureMetadataUpdate();
            var events = new List<FixtureEvent>
            {
                new FixtureEvent { FixtureId = 1 },
                new FixtureEvent { FixtureId = 2 }
            };

            // Act
            update.Events = events;

            // Assert
            update.Events.Should().NotBeNull();
            update.Events.Should().HaveCount(2);
            update.Events.Should().Contain(e => e.FixtureId == 1);
            update.Events.Should().Contain(e => e.FixtureId == 2);
        }

        #endregion

        #region LivescoreUpdate Tests

        [Fact]
        public void LivescoreUpdate_DefaultConstructor_ShouldInitializeCorrectly()
        {
            // Act
            var update = new LivescoreUpdate();

            // Assert
            update.Should().NotBeNull();
            update.Events.Should().BeNull();
        }

        [Fact]
        public void LivescoreUpdate_ShouldHaveCorrectEntityAttribute()
        {
            // Act
            var attribute = typeof(LivescoreUpdate).GetCustomAttribute<Trade360EntityAttribute>();

            // Assert
            attribute.Should().NotBeNull();
            attribute.EntityKey.Should().Be(2);
        }

        [Fact]
        public void LivescoreUpdate_ShouldInheritFromMessageUpdate()
        {
            // Act
            var update = new LivescoreUpdate();

            // Assert
            update.Should().BeAssignableTo<MessageUpdate>();
        }

        [Fact]
        public void LivescoreUpdate_EventsProperty_ShouldBeSettableAndGettable()
        {
            // Arrange
            var update = new LivescoreUpdate();
            var events = new List<LivescoreEvent>
            {
                new LivescoreEvent { FixtureId = 100 },
                new LivescoreEvent { FixtureId = 200 }
            };

            // Act
            update.Events = events;

            // Assert
            update.Events.Should().NotBeNull();
            update.Events.Should().HaveCount(2);
            update.Events.Should().Contain(e => e.FixtureId == 100);
            update.Events.Should().Contain(e => e.FixtureId == 200);
        }

        #endregion

        #region MarketUpdate Tests

        [Fact]
        public void MarketUpdate_DefaultConstructor_ShouldInitializeCorrectly()
        {
            // Act
            var update = new MarketUpdate();

            // Assert
            update.Should().NotBeNull();
            update.Events.Should().BeNull();
        }

        [Fact]
        public void MarketUpdate_ShouldHaveCorrectEntityAttribute()
        {
            // Act
            var attribute = typeof(MarketUpdate).GetCustomAttribute<Trade360EntityAttribute>();

            // Assert
            attribute.Should().NotBeNull();
            attribute.EntityKey.Should().Be(3);
        }

        [Fact]
        public void MarketUpdate_ShouldInheritFromMessageUpdate()
        {
            // Act
            var update = new MarketUpdate();

            // Assert
            update.Should().BeAssignableTo<MessageUpdate>();
        }

        [Fact]
        public void MarketUpdate_EventsProperty_ShouldBeSettableAndGettable()
        {
            // Arrange
            var update = new MarketUpdate();
            var events = new List<MarketEvent>
            {
                new MarketEvent { FixtureId = 300 },
                new MarketEvent { FixtureId = 400 }
            };

            // Act
            update.Events = events;

            // Assert
            update.Events.Should().NotBeNull();
            update.Events.Should().HaveCount(2);
            update.Events.Should().Contain(e => e.FixtureId == 300);
            update.Events.Should().Contain(e => e.FixtureId == 400);
        }

        #endregion

        #region KeepAliveUpdate Tests

        [Fact]
        public void KeepAliveUpdate_DefaultConstructor_ShouldInitializeCorrectly()
        {
            // Act
            var update = new KeepAliveUpdate();

            // Assert
            update.Should().NotBeNull();
            update.KeepAlive.Should().BeNull();
        }

        [Fact]
        public void KeepAliveUpdate_ShouldHaveCorrectEntityAttribute()
        {
            // Act
            var attribute = typeof(KeepAliveUpdate).GetCustomAttribute<Trade360EntityAttribute>();

            // Assert
            attribute.Should().NotBeNull();
            attribute.EntityKey.Should().Be(31);
        }

        [Fact]
        public void KeepAliveUpdate_ShouldInheritFromMessageUpdate()
        {
            // Act
            var update = new KeepAliveUpdate();

            // Assert
            update.Should().BeAssignableTo<MessageUpdate>();
        }

        [Fact]
        public void KeepAliveUpdate_KeepAliveProperty_ShouldBeSettableAndGettable()
        {
            // Arrange
            var update = new KeepAliveUpdate();
            var keepAlive = new Trade360SDK.Common.Entities.KeepAlive.KeepAlive();

            // Act
            update.KeepAlive = keepAlive;

            // Assert
            update.KeepAlive.Should().NotBeNull();
            update.KeepAlive.Should().BeSameAs(keepAlive);
        }

        #endregion

        #region HeartbeatUpdate Tests

        [Fact]
        public void HeartbeatUpdate_DefaultConstructor_ShouldInitializeCorrectly()
        {
            // Act
            var update = new HeartbeatUpdate();

            // Assert
            update.Should().NotBeNull();
        }

        [Fact]
        public void HeartbeatUpdate_ShouldHaveCorrectEntityAttribute()
        {
            // Act
            var attribute = typeof(HeartbeatUpdate).GetCustomAttribute<Trade360EntityAttribute>();

            // Assert
            attribute.Should().NotBeNull();
            attribute.EntityKey.Should().Be(32);
        }

        [Fact]
        public void HeartbeatUpdate_ShouldInheritFromMessageUpdate()
        {
            // Act
            var update = new HeartbeatUpdate();

            // Assert
            update.Should().BeAssignableTo<MessageUpdate>();
        }

        #endregion

        #region SettlementUpdate Tests

        [Fact]
        public void SettlementUpdate_DefaultConstructor_ShouldInitializeCorrectly()
        {
            // Act
            var update = new SettlementUpdate();

            // Assert
            update.Should().NotBeNull();
            update.Events.Should().BeNull();
        }

        [Fact]
        public void SettlementUpdate_ShouldHaveCorrectEntityAttribute()
        {
            // Act
            var attribute = typeof(SettlementUpdate).GetCustomAttribute<Trade360EntityAttribute>();

            // Assert
            attribute.Should().NotBeNull();
            attribute.EntityKey.Should().Be(35);
        }

        [Fact]
        public void SettlementUpdate_ShouldInheritFromMessageUpdate()
        {
            // Act
            var update = new SettlementUpdate();

            // Assert
            update.Should().BeAssignableTo<MessageUpdate>();
        }

        [Fact]
        public void SettlementUpdate_EventsProperty_ShouldBeSettableAndGettable()
        {
            // Arrange
            var update = new SettlementUpdate();
            var events = new List<MarketEvent>
            {
                new MarketEvent { FixtureId = 500 },
                new MarketEvent { FixtureId = 600 }
            };

            // Act
            update.Events = events;

            // Assert
            update.Events.Should().NotBeNull();
            update.Events.Should().HaveCount(2);
            update.Events.Should().Contain(e => e.FixtureId == 500);
            update.Events.Should().Contain(e => e.FixtureId == 600);
        }

        #endregion

        #region OutrightLeagueMarketUpdate Tests

        [Fact]
        public void OutrightLeagueMarketUpdate_DefaultConstructor_ShouldInitializeCorrectly()
        {
            // Act
            var update = new OutrightLeagueMarketUpdate();

            // Assert
            update.Should().NotBeNull();
            update.Competition.Should().BeNull();
        }

        [Fact]
        public void OutrightLeagueMarketUpdate_ShouldHaveCorrectEntityAttribute()
        {
            // Act
            var attribute = typeof(OutrightLeagueMarketUpdate).GetCustomAttribute<Trade360EntityAttribute>();

            // Assert
            attribute.Should().NotBeNull();
            attribute.EntityKey.Should().Be(40);
        }

        [Fact]
        public void OutrightLeagueMarketUpdate_ShouldInheritFromMessageUpdate()
        {
            // Act
            var update = new OutrightLeagueMarketUpdate();

            // Assert
            update.Should().BeAssignableTo<MessageUpdate>();
        }

        [Fact]
        public void OutrightLeagueMarketUpdate_CompetitionProperty_ShouldBeSettableAndGettable()
        {
            // Arrange
            var update = new OutrightLeagueMarketUpdate();
            var competition = new OutrightLeagueCompetitionWrapper<OutrightLeagueMarketEvent>();

            // Act
            update.Competition = competition;

            // Assert
            update.Competition.Should().NotBeNull();
            update.Competition.Should().BeSameAs(competition);
        }

        #endregion

        #region Cross-Type Tests

        [Fact]
        public void AllMessageTypes_ShouldHaveUniqueEntityKeys()
        {
            // Arrange
            var messageTypes = new[]
            {
                typeof(FixtureMetadataUpdate),
                typeof(LivescoreUpdate),
                typeof(MarketUpdate),
                typeof(KeepAliveUpdate),
                typeof(HeartbeatUpdate),
                typeof(SettlementUpdate),
                typeof(OutrightLeagueMarketUpdate)
            };

            // Act
            var entityKeys = messageTypes.Select(t => t.GetCustomAttribute<Trade360EntityAttribute>()?.EntityKey).ToList();

            // Assert
            entityKeys.Should().OnlyHaveUniqueItems("All message types should have unique entity keys");
            entityKeys.Should().NotContain((int?)null, "All message types should have entity attributes");
        }

        [Fact]
        public void AllMessageTypes_ShouldInheritFromMessageUpdate()
        {
            // Arrange
            var messageTypes = new[]
            {
                typeof(FixtureMetadataUpdate),
                typeof(LivescoreUpdate),
                typeof(MarketUpdate),
                typeof(KeepAliveUpdate),
                typeof(HeartbeatUpdate),
                typeof(SettlementUpdate),
                typeof(OutrightLeagueMarketUpdate)
            };

            // Act & Assert
            foreach (var messageType in messageTypes)
            {
                messageType.Should().BeAssignableTo<MessageUpdate>($"{messageType.Name} should inherit from MessageUpdate");
            }
        }

        [Fact]
        public void AllMessageTypes_ShouldHaveParameterlessConstructors()
        {
            // Arrange
            var messageTypes = new[]
            {
                typeof(FixtureMetadataUpdate),
                typeof(LivescoreUpdate),
                typeof(MarketUpdate),
                typeof(KeepAliveUpdate),
                typeof(HeartbeatUpdate),
                typeof(SettlementUpdate),
                typeof(OutrightLeagueMarketUpdate)
            };

            // Act & Assert
            foreach (var messageType in messageTypes)
            {
                var constructor = messageType.GetConstructor(Type.EmptyTypes);
                constructor.Should().NotBeNull($"{messageType.Name} should have a parameterless constructor");
                
                // Verify we can create an instance
                var instance = Activator.CreateInstance(messageType);
                instance.Should().NotBeNull($"Should be able to create instance of {messageType.Name}");
                instance.Should().BeOfType(messageType);
            }
        }

        [Fact]
        public void AllMessageTypes_ShouldBeInCorrectNamespace()
        {
            // Arrange
            var messageTypes = new[]
            {
                typeof(FixtureMetadataUpdate),
                typeof(LivescoreUpdate),
                typeof(MarketUpdate),
                typeof(KeepAliveUpdate),
                typeof(HeartbeatUpdate),
                typeof(SettlementUpdate),
                typeof(OutrightLeagueMarketUpdate)
            };

            // Act & Assert
            foreach (var messageType in messageTypes)
            {
                messageType.Namespace.Should().Be("Trade360SDK.Common.Entities.MessageTypes",
                    $"{messageType.Name} should be in the correct namespace");
            }
        }

        [Fact]
        public void AllMessageTypes_ShouldBePublic()
        {
            // Arrange
            var messageTypes = new[]
            {
                typeof(FixtureMetadataUpdate),
                typeof(LivescoreUpdate),
                typeof(MarketUpdate),
                typeof(KeepAliveUpdate),
                typeof(HeartbeatUpdate),
                typeof(SettlementUpdate),
                typeof(OutrightLeagueMarketUpdate)
            };

            // Act & Assert
            foreach (var messageType in messageTypes)
            {
                messageType.IsPublic.Should().BeTrue($"{messageType.Name} should be public");
                messageType.IsClass.Should().BeTrue($"{messageType.Name} should be a class");
                messageType.IsAbstract.Should().BeFalse($"{messageType.Name} should not be abstract");
            }
        }

        [Theory]
        [InlineData(typeof(FixtureMetadataUpdate), 1)]
        [InlineData(typeof(LivescoreUpdate), 2)]
        [InlineData(typeof(MarketUpdate), 3)]
        [InlineData(typeof(KeepAliveUpdate), 31)]
        [InlineData(typeof(HeartbeatUpdate), 32)]
        [InlineData(typeof(SettlementUpdate), 35)]
        [InlineData(typeof(OutrightLeagueMarketUpdate), 40)]
        public void MessageType_ShouldHaveCorrectEntityKey(Type messageType, int expectedEntityKey)
        {
            // Act
            var attribute = messageType.GetCustomAttribute<Trade360EntityAttribute>();

            // Assert
            attribute.Should().NotBeNull($"{messageType.Name} should have Trade360EntityAttribute");
            attribute.EntityKey.Should().Be(expectedEntityKey, $"{messageType.Name} should have entity key {expectedEntityKey}");
        }

        [Fact]
        public void MessageTypes_WithEventsProperty_ShouldAllowNullAndEmptyCollections()
        {
            // Arrange
            var marketUpdate = new MarketUpdate();
            var fixtureUpdate = new FixtureMetadataUpdate();
            var livescoreUpdate = new LivescoreUpdate();
            var settlementUpdate = new SettlementUpdate();

            // Act & Assert - Null collections
            marketUpdate.Events = null;
            fixtureUpdate.Events = null;
            livescoreUpdate.Events = null;
            settlementUpdate.Events = null;

            marketUpdate.Events.Should().BeNull();
            fixtureUpdate.Events.Should().BeNull();
            livescoreUpdate.Events.Should().BeNull();
            settlementUpdate.Events.Should().BeNull();

            // Act & Assert - Empty collections
            marketUpdate.Events = new List<MarketEvent>();
            fixtureUpdate.Events = new List<FixtureEvent>();
            livescoreUpdate.Events = new List<LivescoreEvent>();
            settlementUpdate.Events = new List<MarketEvent>();

            marketUpdate.Events.Should().NotBeNull().And.BeEmpty();
            fixtureUpdate.Events.Should().NotBeNull().And.BeEmpty();
            livescoreUpdate.Events.Should().NotBeNull().And.BeEmpty();
            settlementUpdate.Events.Should().NotBeNull().And.BeEmpty();
        }

        [Fact]
        public void MessageTypes_ShouldSupportPolymorphicUsage()
        {
            // Arrange
            var messageUpdates = new List<MessageUpdate>
            {
                new FixtureMetadataUpdate(),
                new LivescoreUpdate(),
                new MarketUpdate(),
                new KeepAliveUpdate(),
                new HeartbeatUpdate(),
                new SettlementUpdate(),
                new OutrightLeagueMarketUpdate()
            };

            // Act & Assert
            messageUpdates.Should().HaveCount(7);
            messageUpdates.Should().AllBeAssignableTo<MessageUpdate>();
            
            // Verify we can access them polymorphically
            foreach (var update in messageUpdates)
            {
                update.Should().NotBeNull();
                update.Should().BeAssignableTo<MessageUpdate>();
                
                // Get the entity attribute through reflection
                var attribute = update.GetType().GetCustomAttribute<Trade360EntityAttribute>();
                attribute.Should().NotBeNull();
                attribute.EntityKey.Should().BeGreaterThan(0);
            }
        }

        #endregion
    }
} 