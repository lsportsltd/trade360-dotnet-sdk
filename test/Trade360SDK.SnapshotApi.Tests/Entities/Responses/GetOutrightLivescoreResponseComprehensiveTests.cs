using FluentAssertions;
using Trade360SDK.Common.Entities.OutrightSport;
using Trade360SDK.SnapshotApi.Entities.Responses;

namespace Trade360SDK.SnapshotApi.Tests.Entities.Responses;

public class GetOutrightLivescoreResponseComprehensiveTests
{
    [Fact]
    public void GetOutrightLivescoreResponse_DefaultConstructor_ShouldInitializeWithDefaultValues()
    {
        // Act
        var response = new GetOutrightLivescoreResponse();

        // Assert
        response.Should().NotBeNull();
        response.Id.Should().Be(0);
        response.Name.Should().BeNull();
        response.Type.Should().Be(0);
        response.Events.Should().BeNull();
    }

    [Fact]
    public void GetOutrightLivescoreResponse_PropertyAssignment_ShouldWorkCorrectly()
    {
        // Arrange
        var events = new List<OutrightScoreEventResponse>
        {
            new OutrightScoreEventResponse { FixtureId = 100 },
            new OutrightScoreEventResponse { FixtureId = 200 }
        };

        // Act
        var response = new GetOutrightLivescoreResponse
        {
            Id = 50,
            Name = "Tennis Tournament",
            Type = 3,
            Events = events
        };

        // Assert
        response.Id.Should().Be(50);
        response.Name.Should().Be("Tennis Tournament");
        response.Type.Should().Be(3);
        response.Events.Should().HaveCount(2);
        response.Events!.First().FixtureId.Should().Be(100);
    }

    [Theory]
    [InlineData(-999)]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(int.MaxValue)]
    public void GetOutrightLivescoreResponse_IdProperty_ShouldAcceptVariousValues(int id)
    {
        // Act
        var response = new GetOutrightLivescoreResponse { Id = id };

        // Assert
        response.Id.Should().Be(id);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("Tournament Name")]
    [InlineData("Name with unicode 🏆")]
    [InlineData("Very long tournament name that might exceed normal limits and contains various characters !@#$%^&*()")]
    public void GetOutrightLivescoreResponse_NameProperty_ShouldAcceptVariousValues(string? name)
    {
        // Act
        var response = new GetOutrightLivescoreResponse { Name = name };

        // Assert
        response.Name.Should().Be(name);
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(100)]
    [InlineData(int.MaxValue)]
    public void GetOutrightLivescoreResponse_TypeProperty_ShouldAcceptVariousValues(int type)
    {
        // Act
        var response = new GetOutrightLivescoreResponse { Type = type };

        // Assert
        response.Type.Should().Be(type);
    }

    [Fact]
    public void GetOutrightLivescoreResponse_EventsProperty_ShouldAcceptEmptyCollection()
    {
        // Act
        var response = new GetOutrightLivescoreResponse { Events = new List<OutrightScoreEventResponse>() };

        // Assert
        response.Events.Should().NotBeNull();
        response.Events.Should().BeEmpty();
    }

    [Fact]
    public void GetOutrightLivescoreResponse_ComplexScenario_ShouldMaintainDataIntegrity()
    {
        // Arrange
        var events = new List<OutrightScoreEventResponse>
        {
            new OutrightScoreEventResponse 
            { 
                FixtureId = 1001, 
                OutrightScore = new OutrightLivescoreScore() 
            },
            new OutrightScoreEventResponse 
            { 
                FixtureId = 1002, 
                OutrightScore = new OutrightLivescoreScore() 
            }
        };

        // Act
        var response = new GetOutrightLivescoreResponse
        {
            Id = 777,
            Name = "Premier League Outright",
            Type = 5,
            Events = events
        };

        // Modify original collection to test reference integrity
        events.Add(new OutrightScoreEventResponse { FixtureId = 1003 });

        // Assert
        response.Id.Should().Be(777);
        response.Name.Should().Be("Premier League Outright");
        response.Type.Should().Be(5);
        response.Events.Should().HaveCount(3); // Should reflect the added item
        response.Events!.Should().Contain(e => e.FixtureId == 1003);
    }
}

public class OutrightScoreEventResponseComprehensiveTests
{
    [Fact]
    public void OutrightScoreEventResponse_DefaultConstructor_ShouldInitializeWithDefaultValues()
    {
        // Act
        var response = new OutrightScoreEventResponse();

        // Assert
        response.Should().NotBeNull();
        response.FixtureId.Should().Be(0);
        response.OutrightScore.Should().BeNull();
    }

    [Fact]
    public void OutrightScoreEventResponse_PropertyAssignment_ShouldWorkCorrectly()
    {
        // Arrange
        var outrightScore = new OutrightLivescoreScore();

        // Act
        var response = new OutrightScoreEventResponse
        {
            FixtureId = 12345,
            OutrightScore = outrightScore
        };

        // Assert
        response.FixtureId.Should().Be(12345);
        response.OutrightScore.Should().Be(outrightScore);
        response.OutrightScore.Should().NotBeNull();
    }

    [Theory]
    [InlineData(-999999)]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(999999)]
    [InlineData(int.MaxValue)]
    public void OutrightScoreEventResponse_FixtureIdProperty_ShouldAcceptVariousValues(int fixtureId)
    {
        // Act
        var response = new OutrightScoreEventResponse { FixtureId = fixtureId };

        // Assert
        response.FixtureId.Should().Be(fixtureId);
    }

    [Fact]
    public void OutrightScoreEventResponse_OutrightScoreProperty_ShouldAcceptNull()
    {
        // Act
        var response = new OutrightScoreEventResponse { OutrightScore = null };

        // Assert
        response.OutrightScore.Should().BeNull();
    }

    [Fact]
    public void OutrightScoreEventResponse_OutrightScoreProperty_ShouldAcceptValidInstance()
    {
        // Arrange
        var outrightScore = new OutrightLivescoreScore();

        // Act
        var response = new OutrightScoreEventResponse { OutrightScore = outrightScore };

        // Assert
        response.OutrightScore.Should().NotBeNull();
        response.OutrightScore.Should().Be(outrightScore);
    }

    [Fact]
    public void OutrightScoreEventResponse_PropertyInitialization_ShouldSupportObjectInitializer()
    {
        // Act
        var response = new OutrightScoreEventResponse
        {
            FixtureId = 88888,
            OutrightScore = new OutrightLivescoreScore()
        };

        // Assert
        response.FixtureId.Should().Be(88888);
        response.OutrightScore.Should().NotBeNull();
    }

    [Fact]
    public void OutrightScoreEventResponse_MultipleInstances_ShouldMaintainIndependence()
    {
        // Arrange
        var score1 = new OutrightLivescoreScore();
        var score2 = new OutrightLivescoreScore();

        // Act
        var response1 = new OutrightScoreEventResponse { FixtureId = 1, OutrightScore = score1 };
        var response2 = new OutrightScoreEventResponse { FixtureId = 2, OutrightScore = score2 };

        // Assert
        response1.FixtureId.Should().NotBe(response2.FixtureId);
        response1.OutrightScore.Should().NotBe(response2.OutrightScore);
        response1.OutrightScore.Should().Be(score1);
        response2.OutrightScore.Should().Be(score2);
    }

    [Fact]
    public void OutrightScoreEventResponse_ReferenceIntegrity_ShouldMaintainCorrectReferences()
    {
        // Arrange
        var outrightScore = new OutrightLivescoreScore();
        var response1 = new OutrightScoreEventResponse { OutrightScore = outrightScore };
        var response2 = new OutrightScoreEventResponse { OutrightScore = outrightScore };

        // Act & Assert
        response1.OutrightScore.Should().Be(response2.OutrightScore);
        response1.OutrightScore.Should().BeSameAs(response2.OutrightScore);
    }

    [Fact]
    public void OutrightScoreEventResponse_EdgeCaseValues_ShouldHandleCorrectly()
    {
        // Act
        var response = new OutrightScoreEventResponse
        {
            FixtureId = int.MinValue,
            OutrightScore = null
        };

        // Assert
        response.FixtureId.Should().Be(int.MinValue);
        response.OutrightScore.Should().BeNull();
    }

    [Fact]
    public void OutrightScoreEventResponse_CollectionScenario_ShouldWorkInCollections()
    {
        // Arrange
        var responses = new List<OutrightScoreEventResponse>
        {
            new OutrightScoreEventResponse { FixtureId = 1, OutrightScore = new OutrightLivescoreScore() },
            new OutrightScoreEventResponse { FixtureId = 2, OutrightScore = null },
            new OutrightScoreEventResponse { FixtureId = 3, OutrightScore = new OutrightLivescoreScore() }
        };

        // Act & Assert
        responses.Should().HaveCount(3);
        responses.Should().Contain(r => r.FixtureId == 1 && r.OutrightScore != null);
        responses.Should().Contain(r => r.FixtureId == 2 && r.OutrightScore == null);
        responses.Should().Contain(r => r.FixtureId == 3 && r.OutrightScore != null);
    }
} 