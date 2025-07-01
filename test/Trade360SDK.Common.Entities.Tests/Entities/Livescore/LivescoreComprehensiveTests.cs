using FluentAssertions;
using Trade360SDK.Common.Entities.Livescore;
using Trade360SDK.Common.Entities.Shared;
using Xunit;
using System.Collections.Generic;
using System.Linq;

namespace Trade360SDK.Common.Entities.Tests.Entities.Livescore
{
    public class LivescoreComprehensiveTests
    {
        [Fact]
        public void Livescore_DefaultConstructor_ShouldInitializeWithDefaults()
        {
            // Arrange & Act
            var livescore = new Trade360SDK.Common.Entities.Livescore.Livescore();

            // Assert
            livescore.Scoreboard.Should().BeNull();
            livescore.Periods.Should().BeNull();
            livescore.Statistics.Should().BeNull();
            livescore.LivescoreExtraData.Should().BeNull();
            livescore.CurrentIncident.Should().BeNull();
            livescore.DangerTriggers.Should().BeNull();
        }

        [Fact]
        public void Scoreboard_WhenSet_ShouldReturnExpectedValue()
        {
            // Arrange
            var livescore = new Trade360SDK.Common.Entities.Livescore.Livescore();
            var scoreboard = new Scoreboard();

            // Act
            livescore.Scoreboard = scoreboard;

            // Assert
            livescore.Scoreboard.Should().Be(scoreboard);
        }

        [Fact]
        public void Periods_WhenSetToCollection_ShouldReturnExpectedValue()
        {
            // Arrange
            var livescore = new Trade360SDK.Common.Entities.Livescore.Livescore();
            var periods = new List<Period>
            {
                new Period { Type = 1, IsFinished = true },
                new Period { Type = 2, IsFinished = false }
            };

            // Act
            livescore.Periods = periods;

            // Assert
            livescore.Periods.Should().NotBeNull();
            livescore.Periods.Should().HaveCount(2);
            livescore.Periods.Should().ContainEquivalentOf(periods[0]);
            livescore.Periods.Should().ContainEquivalentOf(periods[1]);
        }

        [Fact]
        public void Statistics_WhenSetToCollection_ShouldReturnExpectedValue()
        {
            // Arrange
            var livescore = new Trade360SDK.Common.Entities.Livescore.Livescore();
            var statistics = new List<Statistic>
            {
                new Statistic { Type = 1 },
                new Statistic { Type = 2 }
            };

            // Act
            livescore.Statistics = statistics;

            // Assert
            livescore.Statistics.Should().NotBeNull();
            livescore.Statistics.Should().HaveCount(2);
            livescore.Statistics.Should().ContainEquivalentOf(statistics[0]);
            livescore.Statistics.Should().ContainEquivalentOf(statistics[1]);
        }

        [Fact]
        public void LivescoreExtraData_WhenSetToCollection_ShouldReturnExpectedValue()
        {
            // Arrange
            var livescore = new Trade360SDK.Common.Entities.Livescore.Livescore();
            var extraData = new List<NameValuePair>
            {
                new NameValuePair { Name = "key1", Value = "value1" },
                new NameValuePair { Name = "key2", Value = "value2" }
            };

            // Act
            livescore.LivescoreExtraData = extraData;

            // Assert
            livescore.LivescoreExtraData.Should().NotBeNull();
            livescore.LivescoreExtraData.Should().HaveCount(2);
            livescore.LivescoreExtraData.Should().ContainEquivalentOf(extraData[0]);
            livescore.LivescoreExtraData.Should().ContainEquivalentOf(extraData[1]);
        }

        [Fact]
        public void CurrentIncident_WhenSet_ShouldReturnExpectedValue()
        {
            // Arrange
            var livescore = new Trade360SDK.Common.Entities.Livescore.Livescore();
            var currentIncident = new CurrentIncident
            {
                Id = 123,
                Name = "Test Incident",
                LastUpdate = DateTime.UtcNow
            };

            // Act
            livescore.CurrentIncident = currentIncident;

            // Assert
            livescore.CurrentIncident.Should().Be(currentIncident);
            livescore.CurrentIncident.Id.Should().Be(123);
            livescore.CurrentIncident.Name.Should().Be("Test Incident");
        }

        [Fact]
        public void DangerTriggers_WhenSetToCollection_ShouldReturnExpectedValue()
        {
            // Arrange
            var livescore = new Trade360SDK.Common.Entities.Livescore.Livescore();
            var dangerTriggers = new List<DangerIndicator>
            {
                new DangerIndicator { Type = Common.Entities.Enums.DangerIndicatorType.Cards },
                new DangerIndicator { Type = Common.Entities.Enums.DangerIndicatorType.General }
            };

            // Act
            livescore.DangerTriggers = dangerTriggers;

            // Assert
            livescore.DangerTriggers.Should().NotBeNull();
            livescore.DangerTriggers.Should().HaveCount(2);
            livescore.DangerTriggers.Should().ContainEquivalentOf(dangerTriggers[0]);
            livescore.DangerTriggers.Should().ContainEquivalentOf(dangerTriggers[1]);
        }

        [Fact]
        public void Livescore_WithEmptyCollections_ShouldHandleGracefully()
        {
            // Arrange
            var livescore = new Trade360SDK.Common.Entities.Livescore.Livescore
            {
                Periods = new List<Period>(),
                Statistics = new List<Statistic>(),
                LivescoreExtraData = new List<NameValuePair>(),
                DangerTriggers = new List<DangerIndicator>()
            };

            // Act & Assert
            livescore.Periods.Should().NotBeNull().And.BeEmpty();
            livescore.Statistics.Should().NotBeNull().And.BeEmpty();
            livescore.LivescoreExtraData.Should().NotBeNull().And.BeEmpty();
            livescore.DangerTriggers.Should().NotBeNull().And.BeEmpty();
        }

        [Fact]
        public void Livescore_WithAllPropertiesSet_ShouldRetainAllValues()
        {
            // Arrange
            var scoreboard = new Scoreboard();
            var periods = new List<Period> { new Period { Type = 1 } };
            var statistics = new List<Statistic> { new Statistic { Type = 1 } };
            var extraData = new List<NameValuePair> { new NameValuePair { Name = "test" } };
            var currentIncident = new CurrentIncident { Id = 456 };
            var dangerTriggers = new List<DangerIndicator> { new DangerIndicator() };

            var livescore = new Trade360SDK.Common.Entities.Livescore.Livescore
            {
                Scoreboard = scoreboard,
                Periods = periods,
                Statistics = statistics,
                LivescoreExtraData = extraData,
                CurrentIncident = currentIncident,
                DangerTriggers = dangerTriggers
            };

            // Act & Assert
            livescore.Scoreboard.Should().Be(scoreboard);
            livescore.Periods.Should().BeEquivalentTo(periods);
            livescore.Statistics.Should().BeEquivalentTo(statistics);
            livescore.LivescoreExtraData.Should().BeEquivalentTo(extraData);
            livescore.CurrentIncident.Should().Be(currentIncident);
            livescore.DangerTriggers.Should().BeEquivalentTo(dangerTriggers);
        }

        [Fact]
        public void Livescore_Properties_ShouldAllowNullAssignment()
        {
            // Arrange
            var livescore = new Trade360SDK.Common.Entities.Livescore.Livescore
            {
                Scoreboard = new Scoreboard(),
                CurrentIncident = new CurrentIncident()
            };

            // Act
            livescore.Scoreboard = null;
            livescore.Periods = null;
            livescore.Statistics = null;
            livescore.LivescoreExtraData = null;
            livescore.CurrentIncident = null;
            livescore.DangerTriggers = null;

            // Assert
            livescore.Scoreboard.Should().BeNull();
            livescore.Periods.Should().BeNull();
            livescore.Statistics.Should().BeNull();
            livescore.LivescoreExtraData.Should().BeNull();
            livescore.CurrentIncident.Should().BeNull();
            livescore.DangerTriggers.Should().BeNull();
        }
    }
} 