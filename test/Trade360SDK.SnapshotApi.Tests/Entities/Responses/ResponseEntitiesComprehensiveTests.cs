using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Trade360SDK.SnapshotApi.Entities.Responses;
using Xunit;

namespace Trade360SDK.SnapshotApi.Tests.Entities.Responses
{
    public class ResponseEntitiesComprehensiveTests
    {
        #region GetOutrightLeaguesMarketsResponse Tests

        [Fact]
        public void GetOutrightLeaguesMarketsResponse_DefaultConstructor_ShouldInitializeCorrectly()
        {
            // Act
            var response = new GetOutrightLeaguesMarketsResponse();

            // Assert
            response.Should().NotBeNull();
            response.Id.Should().Be(0);
            response.Name.Should().BeNull();
            response.Type.Should().Be(0);
            response.Competitions.Should().BeNull();
        }

        [Fact]
        public void GetOutrightLeaguesMarketsResponse_Id_CanBeSetAndRetrieved()
        {
            // Arrange
            var response = new GetOutrightLeaguesMarketsResponse();
            const int expectedId = 12345;

            // Act
            response.Id = expectedId;

            // Assert
            response.Id.Should().Be(expectedId);
        }

        [Fact]
        public void GetOutrightLeaguesMarketsResponse_Name_CanBeSetAndRetrieved()
        {
            // Arrange
            var response = new GetOutrightLeaguesMarketsResponse();
            const string expectedName = "Premier League";

            // Act
            response.Name = expectedName;

            // Assert
            response.Name.Should().Be(expectedName);
        }

        [Fact]
        public void GetOutrightLeaguesMarketsResponse_Type_CanBeSetAndRetrieved()
        {
            // Arrange
            var response = new GetOutrightLeaguesMarketsResponse();
            const int expectedType = 1;

            // Act
            response.Type = expectedType;

            // Assert
            response.Type.Should().Be(expectedType);
        }

        [Fact]
        public void GetOutrightLeaguesMarketsResponse_Competitions_CanBeSetAndRetrieved()
        {
            // Arrange
            var response = new GetOutrightLeaguesMarketsResponse();
            var expectedCompetitions = new[]
            {
                new SnapshotOutrightEventsResponse { Id = 1, Name = "Competition 1" },
                new SnapshotOutrightEventsResponse { Id = 2, Name = "Competition 2" }
            };

            // Act
            response.Competitions = expectedCompetitions;

            // Assert
            response.Competitions.Should().Equal(expectedCompetitions);
            response.Competitions.Should().HaveCount(2);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(-1)]
        [InlineData(999999)]
        [InlineData(int.MaxValue)]
        [InlineData(int.MinValue)]
        public void GetOutrightLeaguesMarketsResponse_Id_ShouldAcceptVariousValues(int id)
        {
            // Arrange
            var response = new GetOutrightLeaguesMarketsResponse();

            // Act
            response.Id = id;

            // Assert
            response.Id.Should().Be(id);
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("League Name")]
        [InlineData("Very Long League Name With Special Characters !@#$%")]
        [InlineData("联赛名称")]
        public void GetOutrightLeaguesMarketsResponse_Name_ShouldAcceptVariousStrings(string name)
        {
            // Arrange
            var response = new GetOutrightLeaguesMarketsResponse();

            // Act
            response.Name = name;

            // Assert
            response.Name.Should().Be(name);
        }

        [Fact]
        public void GetOutrightLeaguesMarketsResponse_Name_CanBeSetToNull()
        {
            // Arrange
            var response = new GetOutrightLeaguesMarketsResponse { Name = "initial" };

            // Act
            response.Name = null;

            // Assert
            response.Name.Should().BeNull();
        }

        [Fact]
        public void GetOutrightLeaguesMarketsResponse_Competitions_CanBeSetToNull()
        {
            // Arrange
            var response = new GetOutrightLeaguesMarketsResponse 
            { 
                Competitions = new[] { new SnapshotOutrightEventsResponse() } 
            };

            // Act
            response.Competitions = null;

            // Assert
            response.Competitions.Should().BeNull();
        }

        [Fact]
        public void GetOutrightLeaguesMarketsResponse_Competitions_CanBeEmpty()
        {
            // Arrange
            var response = new GetOutrightLeaguesMarketsResponse();

            // Act
            response.Competitions = Enumerable.Empty<SnapshotOutrightEventsResponse>();

            // Assert
            response.Competitions.Should().NotBeNull().And.BeEmpty();
        }

        [Fact]
        public void GetOutrightLeaguesMarketsResponse_CompleteInitialization_ShouldWork()
        {
            // Arrange & Act
            var response = new GetOutrightLeaguesMarketsResponse
            {
                Id = 123,
                Name = "Test League",
                Type = 1,
                Competitions = new[]
                {
                    new SnapshotOutrightEventsResponse { Id = 1, Name = "Competition 1" }
                }
            };

            // Assert
            response.Id.Should().Be(123);
            response.Name.Should().Be("Test League");
            response.Type.Should().Be(1);
            response.Competitions.Should().HaveCount(1);
        }

        #endregion

        #region SnapshotOutrightEventsResponse Tests

        [Fact]
        public void SnapshotOutrightEventsResponse_DefaultConstructor_ShouldInitializeCorrectly()
        {
            // Act
            var response = new SnapshotOutrightEventsResponse();

            // Assert
            response.Should().NotBeNull();
            response.Id.Should().Be(0);
            response.Name.Should().BeNull();
            response.Type.Should().Be(0);
            response.Events.Should().BeNull();
        }

        [Fact]
        public void SnapshotOutrightEventsResponse_Id_CanBeSetAndRetrieved()
        {
            // Arrange
            var response = new SnapshotOutrightEventsResponse();
            const int expectedId = 54321;

            // Act
            response.Id = expectedId;

            // Assert
            response.Id.Should().Be(expectedId);
        }

        [Fact]
        public void SnapshotOutrightEventsResponse_Name_CanBeSetAndRetrieved()
        {
            // Arrange
            var response = new SnapshotOutrightEventsResponse();
            const string expectedName = "Championship";

            // Act
            response.Name = expectedName;

            // Assert
            response.Name.Should().Be(expectedName);
        }

        [Fact]
        public void SnapshotOutrightEventsResponse_Type_CanBeSetAndRetrieved()
        {
            // Arrange
            var response = new SnapshotOutrightEventsResponse();
            const int expectedType = 2;

            // Act
            response.Type = expectedType;

            // Assert
            response.Type.Should().Be(expectedType);
        }

        [Fact]
        public void SnapshotOutrightEventsResponse_Events_CanBeSetAndRetrieved()
        {
            // Arrange
            var response = new SnapshotOutrightEventsResponse();
            var expectedEvents = new[]
            {
                new OutrightMarketsResponse(),
                new OutrightMarketsResponse()
            };

            // Act
            response.Events = expectedEvents;

            // Assert
            response.Events.Should().Equal(expectedEvents);
            response.Events.Should().HaveCount(2);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(-1)]
        [InlineData(999999)]
        [InlineData(int.MaxValue)]
        [InlineData(int.MinValue)]
        public void SnapshotOutrightEventsResponse_Id_ShouldAcceptVariousValues(int id)
        {
            // Arrange
            var response = new SnapshotOutrightEventsResponse();

            // Act
            response.Id = id;

            // Assert
            response.Id.Should().Be(id);
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("Event Name")]
        [InlineData("Very Long Event Name With Special Characters !@#$%")]
        [InlineData("赛事名称")]
        public void SnapshotOutrightEventsResponse_Name_ShouldAcceptVariousStrings(string name)
        {
            // Arrange
            var response = new SnapshotOutrightEventsResponse();

            // Act
            response.Name = name;

            // Assert
            response.Name.Should().Be(name);
        }

        [Fact]
        public void SnapshotOutrightEventsResponse_Name_CanBeSetToNull()
        {
            // Arrange
            var response = new SnapshotOutrightEventsResponse { Name = "initial" };

            // Act
            response.Name = null;

            // Assert
            response.Name.Should().BeNull();
        }

        [Fact]
        public void SnapshotOutrightEventsResponse_Events_CanBeSetToNull()
        {
            // Arrange
            var response = new SnapshotOutrightEventsResponse 
            { 
                Events = new[] { new OutrightMarketsResponse() } 
            };

            // Act
            response.Events = null;

            // Assert
            response.Events.Should().BeNull();
        }

        [Fact]
        public void SnapshotOutrightEventsResponse_Events_CanBeEmpty()
        {
            // Arrange
            var response = new SnapshotOutrightEventsResponse();

            // Act
            response.Events = Enumerable.Empty<OutrightMarketsResponse>();

            // Assert
            response.Events.Should().NotBeNull().And.BeEmpty();
        }

        [Fact]
        public void SnapshotOutrightEventsResponse_CompleteInitialization_ShouldWork()
        {
            // Arrange & Act
            var response = new SnapshotOutrightEventsResponse
            {
                Id = 456,
                Name = "Test Event",
                Type = 2,
                Events = new[]
                {
                    new OutrightMarketsResponse()
                }
            };

            // Assert
            response.Id.Should().Be(456);
            response.Name.Should().Be("Test Event");
            response.Type.Should().Be(2);
            response.Events.Should().HaveCount(1);
        }

        #endregion

        #region Integration and Edge Case Tests

        [Fact]
        public void ResponseEntities_PropertyTypes_ShouldBeCorrect()
        {
            // Arrange
            var leagueResponse = new GetOutrightLeaguesMarketsResponse();
            var eventResponse = new SnapshotOutrightEventsResponse();

            // Act & Assert - GetOutrightLeaguesMarketsResponse
            leagueResponse.Id.Should().BeOfType(typeof(int));
            leagueResponse.Type.Should().BeOfType(typeof(int));
            
            // Act & Assert - SnapshotOutrightEventsResponse
            eventResponse.Id.Should().BeOfType(typeof(int));
            eventResponse.Type.Should().BeOfType(typeof(int));
        }

        [Fact]
        public void ResponseEntities_NestedStructure_ShouldWork()
        {
            // Arrange & Act
            var response = new GetOutrightLeaguesMarketsResponse
            {
                Id = 1,
                Name = "Main League",
                Type = 1,
                Competitions = new[]
                {
                    new SnapshotOutrightEventsResponse
                    {
                        Id = 10,
                        Name = "Competition A",
                        Type = 2,
                        Events = new[]
                        {
                            new OutrightMarketsResponse(),
                            new OutrightMarketsResponse()
                        }
                    },
                    new SnapshotOutrightEventsResponse
                    {
                        Id = 20,
                        Name = "Competition B",
                        Type = 3,
                        Events = new[]
                        {
                            new OutrightMarketsResponse()
                        }
                    }
                }
            };

            // Assert
            response.Should().NotBeNull();
            response.Competitions.Should().HaveCount(2);
            response.Competitions!.First().Events.Should().HaveCount(2);
            response.Competitions!.Last().Events.Should().HaveCount(1);
        }

        [Fact]
        public void ResponseEntities_EmptyCollections_ShouldBeHandledCorrectly()
        {
            // Arrange & Act
            var response = new GetOutrightLeaguesMarketsResponse
            {
                Id = 1,
                Name = "Empty League",
                Type = 1,
                Competitions = Enumerable.Empty<SnapshotOutrightEventsResponse>()
            };

            // Assert
            response.Competitions.Should().NotBeNull().And.BeEmpty();
        }

        [Fact]
        public void ResponseEntities_NullCollections_ShouldBeHandledCorrectly()
        {
            // Arrange & Act
            var response = new GetOutrightLeaguesMarketsResponse
            {
                Id = 1,
                Name = "Null League",
                Type = 1,
                Competitions = null
            };

            // Assert
            response.Competitions.Should().BeNull();
        }

        [Fact]
        public void ResponseEntities_LargeCollections_ShouldWork()
        {
            // Arrange
            var largeCompetitionsList = Enumerable.Range(1, 1000)
                .Select(i => new SnapshotOutrightEventsResponse 
                { 
                    Id = i, 
                    Name = $"Competition {i}",
                    Type = i % 3
                })
                .ToArray();

            // Act
            var response = new GetOutrightLeaguesMarketsResponse
            {
                Id = 1,
                Name = "Large League",
                Type = 1,
                Competitions = largeCompetitionsList
            };

            // Assert
            response.Competitions.Should().HaveCount(1000);
            response.Competitions!.First().Id.Should().Be(1);
            response.Competitions!.Last().Id.Should().Be(1000);
        }

        [Fact]
        public void ResponseEntities_SpecialCharactersInNames_ShouldBeHandled()
        {
            // Arrange & Act
            var response = new GetOutrightLeaguesMarketsResponse
            {
                Id = 1,
                Name = "League with special chars: !@#$%^&*()_+-=[]{}|;':\",./<>?",
                Type = 1,
                Competitions = new[]
                {
                    new SnapshotOutrightEventsResponse
                    {
                        Id = 1,
                        Name = "Event with unicode: 🏆⚽🎯",
                        Type = 1
                    }
                }
            };

            // Assert
            response.Name.Should().Contain("!@#$%^&*()_+-=[]{}|;':\",./<>?");
            response.Competitions!.First().Name.Should().Contain("🏆⚽🎯");
        }

        [Fact]
        public void ResponseEntities_DefaultValues_ShouldBeConsistent()
        {
            // Arrange & Act
            var leagueResponse = new GetOutrightLeaguesMarketsResponse();
            var eventResponse = new SnapshotOutrightEventsResponse();

            // Assert
            leagueResponse.Id.Should().Be(0);
            leagueResponse.Type.Should().Be(0);
            leagueResponse.Name.Should().BeNull();
            leagueResponse.Competitions.Should().BeNull();

            eventResponse.Id.Should().Be(0);
            eventResponse.Type.Should().Be(0);
            eventResponse.Name.Should().BeNull();
            eventResponse.Events.Should().BeNull();
        }

        #endregion
    }
} 