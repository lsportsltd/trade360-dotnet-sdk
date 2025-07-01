using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using Trade360SDK.Common.Entities.Enums;
using Trade360SDK.Common.Entities.Fixtures;
using Trade360SDK.Common.Entities.Shared;
using Trade360SDK.SnapshotApi.Entities.Responses;
using Xunit;

namespace Trade360SDK.SnapshotApi.Tests.Entities.Responses
{
    public class GetOutrightFixtureResponseTests
    {
        [Fact]
        public void GetOutrightFixtureResponse_DefaultConstructor_ShouldInitializeWithDefaultValues()
        {
            // Act
            var response = new GetOutrightFixtureResponse();

            // Assert
            response.Id.Should().Be(0);
            response.Name.Should().BeNull();
            response.Type.Should().Be(0);
            response.Events.Should().BeNull();
        }

        [Theory]
        [InlineData(1)]
        [InlineData(999)]
        [InlineData(-1)]
        [InlineData(int.MaxValue)]
        public void GetOutrightFixtureResponse_SetId_ShouldRetainValue(int id)
        {
            // Arrange
            var response = new GetOutrightFixtureResponse();

            // Act
            response.Id = id;

            // Assert
            response.Id.Should().Be(id);
        }

        [Theory]
        [InlineData("Premier League")]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("Champions League Final")]
        public void GetOutrightFixtureResponse_SetName_ShouldRetainValue(string name)
        {
            // Arrange
            var response = new GetOutrightFixtureResponse();

            // Act
            response.Name = name;

            // Assert
            response.Name.Should().Be(name);
        }

        [Fact]
        public void GetOutrightFixtureResponse_SetEvents_ShouldRetainCollection()
        {
            // Arrange
            var response = new GetOutrightFixtureResponse();
            var events = new List<SnapshotOutrightFixtureEvent>
            {
                new SnapshotOutrightFixtureEvent { FixtureId = 1 },
                new SnapshotOutrightFixtureEvent { FixtureId = 2 }
            };

            // Act
            response.Events = events;

            // Assert
            response.Events.Should().NotBeNull();
            response.Events.Should().HaveCount(2);
            response.Events.First().FixtureId.Should().Be(1);
            response.Events.Last().FixtureId.Should().Be(2);
        }

        [Fact]
        public void GetOutrightFixtureResponse_CompleteObjectInitialization_ShouldSetAllProperties()
        {
            // Arrange
            var events = new List<SnapshotOutrightFixtureEvent>
            {
                new SnapshotOutrightFixtureEvent { FixtureId = 123 }
            };

            // Act
            var response = new GetOutrightFixtureResponse
            {
                Id = 456,
                Name = "World Cup 2024",
                Type = 10,
                Events = events
            };

            // Assert
            response.Id.Should().Be(456);
            response.Name.Should().Be("World Cup 2024");
            response.Type.Should().Be(10);
            response.Events.Should().HaveCount(1);
        }
    }

    public class SnapshotOutrightFixtureEventTests
    {
        [Fact]
        public void SnapshotOutrightFixtureEvent_DefaultConstructor_ShouldInitializeWithDefaultValues()
        {
            // Act
            var eventItem = new SnapshotOutrightFixtureEvent();

            // Assert
            eventItem.FixtureId.Should().Be(0);
            eventItem.OutrightFixture.Should().BeNull();
        }

        [Fact]
        public void SnapshotOutrightFixtureEvent_SetOutrightFixture_ShouldRetainValue()
        {
            // Arrange
            var eventItem = new SnapshotOutrightFixtureEvent();
            var fixture = new OutrightFixtureSnapshotResponse
            {
                StartDate = DateTime.UtcNow,
                Status = FixtureStatus.NSY
            };

            // Act
            eventItem.OutrightFixture = fixture;

            // Assert
            eventItem.OutrightFixture.Should().NotBeNull();
            eventItem.OutrightFixture.Status.Should().Be(FixtureStatus.NSY);
        }
    }

    public class OutrightFixtureSnapshotResponseTests
    {
        [Fact]
        public void OutrightFixtureSnapshotResponse_DefaultConstructor_ShouldInitializeWithDefaultValues()
        {
            // Act
            var response = new OutrightFixtureSnapshotResponse();

            // Assert
            response.Sport.Should().BeNull();
            response.Location.Should().BeNull();
            response.StartDate.Should().BeNull();
            response.Status.Should().Be(default);
            response.Participants.Should().BeNull();
            response.ExtraData.Should().BeNull();
            response.Subscription.Should().BeNull();
        }

        [Theory]
        [InlineData(FixtureStatus.NSY)]
        [InlineData(FixtureStatus.InProgress)]
        [InlineData(FixtureStatus.Finished)]
        public void OutrightFixtureSnapshotResponse_SetStatus_ShouldRetainValue(FixtureStatus status)
        {
            // Arrange
            var response = new OutrightFixtureSnapshotResponse();

            // Act
            response.Status = status;

            // Assert
            response.Status.Should().Be(status);
        }

        [Fact]
        public void OutrightFixtureSnapshotResponse_SetParticipants_ShouldRetainCollection()
        {
            // Arrange
            var response = new OutrightFixtureSnapshotResponse();
            var participants = new List<Participant>
            {
                new Participant { Id = 1, Name = "Team A" },
                new Participant { Id = 2, Name = "Team B" }
            };

            // Act
            response.Participants = participants;

            // Assert
            response.Participants.Should().NotBeNull();
            response.Participants.Should().HaveCount(2);
        }

        [Fact]
        public void OutrightFixtureSnapshotResponse_CompleteObjectInitialization_ShouldSetAllProperties()
        {
            // Arrange
            var sport = new Sport { Id = 5, Name = "Basketball" };
            var startDate = DateTime.UtcNow.AddHours(2);
            var participants = new List<Participant>
            {
                new Participant { Id = 10, Name = "Lakers" }
            };

            // Act
            var response = new OutrightFixtureSnapshotResponse
            {
                Sport = sport,
                StartDate = startDate,
                Status = FixtureStatus.InProgress,
                Participants = participants
            };

            // Assert
            response.Sport.Should().BeSameAs(sport);
            response.StartDate.Should().Be(startDate);
            response.Status.Should().Be(FixtureStatus.InProgress);
            response.Participants.Should().HaveCount(1);
        }
    }
} 