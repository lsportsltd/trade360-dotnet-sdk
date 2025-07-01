using FluentAssertions;
using Trade360SDK.Common.Entities.OutrightSport;
using Trade360SDK.SnapshotApi.Entities.Responses;

namespace Trade360SDK.SnapshotApi.Tests.Entities.Responses;

public class GetOutrightEventsResponseComprehensiveTests
{
    [Fact]
    public void GetOutrightEventsResponse_DefaultConstructor_ShouldInitializeWithDefaultValues()
    {
        // Act
        var response = new GetOutrightEventsResponse();

        // Assert
        response.Should().NotBeNull();
        response.Id.Should().Be(0);
        response.Name.Should().BeNull();
        response.Type.Should().Be(0);
        response.Events.Should().BeNull();
    }

    [Fact]
    public void GetOutrightEventsResponse_PropertyAssignment_ShouldWorkCorrectly()
    {
        // Arrange
        var events = new List<OutrightEventResponse>
        {
            new OutrightEventResponse { FixtureId = 123 },
            new OutrightEventResponse { FixtureId = 456 }
        };

        // Act
        var response = new GetOutrightEventsResponse
        {
            Id = 100,
            Name = "Test Event",
            Type = 5,
            Events = events
        };

        // Assert
        response.Id.Should().Be(100);
        response.Name.Should().Be("Test Event");
        response.Type.Should().Be(5);
        response.Events.Should().HaveCount(2);
        response.Events!.First().FixtureId.Should().Be(123);
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(int.MaxValue)]
    public void GetOutrightEventsResponse_IdProperty_ShouldAcceptVariousValues(int id)
    {
        // Act
        var response = new GetOutrightEventsResponse { Id = id };

        // Assert
        response.Id.Should().Be(id);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("Normal Name")]
    [InlineData("Name with special chars !@#$%")]
    public void GetOutrightEventsResponse_NameProperty_ShouldAcceptVariousValues(string? name)
    {
        // Act
        var response = new GetOutrightEventsResponse { Name = name };

        // Assert
        response.Name.Should().Be(name);
    }

    [Fact]
    public void GetOutrightEventsResponse_EventsProperty_ShouldAcceptEmptyCollection()
    {
        // Act
        var response = new GetOutrightEventsResponse { Events = new List<OutrightEventResponse>() };

        // Assert
        response.Events.Should().NotBeNull();
        response.Events.Should().BeEmpty();
    }
}

public class OutrightEventResponseComprehensiveTests
{
    [Fact]
    public void OutrightEventResponse_DefaultConstructor_ShouldInitializeWithDefaultValues()
    {
        // Act
        var response = new OutrightEventResponse();

        // Assert
        response.Should().NotBeNull();
        response.FixtureId.Should().Be(0);
        response.OutrightFixture.Should().BeNull();
        response.OutrightScore.Should().BeNull();
        response.Markets.Should().BeNull();
    }

    [Fact]
    public void OutrightEventResponse_PropertyAssignment_ShouldWorkCorrectly()
    {
        // Arrange
        var outrightFixture = new OutrightFixtureSnapshotResponse();
        var outrightScore = new OutrightLivescoreScore();
        var markets = new List<OutrightMarketResponse>
        {
            new OutrightMarketResponse { Id = 1, Name = "Market 1" }
        };

        // Act
        var response = new OutrightEventResponse
        {
            FixtureId = 789,
            OutrightFixture = outrightFixture,
            OutrightScore = outrightScore,
            Markets = markets
        };

        // Assert
        response.FixtureId.Should().Be(789);
        response.OutrightFixture.Should().Be(outrightFixture);
        response.OutrightScore.Should().Be(outrightScore);
        response.Markets.Should().HaveCount(1);
        response.Markets!.First().Id.Should().Be(1);
    }

    [Theory]
    [InlineData(-100)]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(999999)]
    public void OutrightEventResponse_FixtureIdProperty_ShouldAcceptVariousValues(int fixtureId)
    {
        // Act
        var response = new OutrightEventResponse { FixtureId = fixtureId };

        // Assert
        response.FixtureId.Should().Be(fixtureId);
    }

    [Fact]
    public void OutrightEventResponse_OutrightFixtureProperty_ShouldAcceptNull()
    {
        // Act
        var response = new OutrightEventResponse { OutrightFixture = null };

        // Assert
        response.OutrightFixture.Should().BeNull();
    }

    [Fact]
    public void OutrightEventResponse_OutrightScoreProperty_ShouldAcceptNull()
    {
        // Act
        var response = new OutrightEventResponse { OutrightScore = null };

        // Assert
        response.OutrightScore.Should().BeNull();
    }

    [Fact]
    public void OutrightEventResponse_MarketsProperty_ShouldAcceptEmptyCollection()
    {
        // Act
        var response = new OutrightEventResponse { Markets = new List<OutrightMarketResponse>() };

        // Assert
        response.Markets.Should().NotBeNull();
        response.Markets.Should().BeEmpty();
    }

    [Fact]
    public void OutrightEventResponse_ComplexScenario_ShouldMaintainDataIntegrity()
    {
        // Arrange
        var fixture = new OutrightFixtureSnapshotResponse();
        var score = new OutrightLivescoreScore();
        var markets = new List<OutrightMarketResponse>
        {
            new OutrightMarketResponse { Id = 1, Name = "Win" },
            new OutrightMarketResponse { Id = 2, Name = "Place" }
        };

        // Act
        var response = new OutrightEventResponse
        {
            FixtureId = 12345,
            OutrightFixture = fixture,
            OutrightScore = score,
            Markets = markets
        };

        // Modify original collections to test reference integrity
        markets.Add(new OutrightMarketResponse { Id = 3, Name = "Show" });

        // Assert
        response.FixtureId.Should().Be(12345);
        response.OutrightFixture.Should().NotBeNull();
        response.OutrightScore.Should().NotBeNull();
        response.Markets.Should().HaveCount(3); // Should reflect the added item
        response.Markets!.Should().Contain(m => m.Name == "Show");
    }

    [Fact]
    public void OutrightEventResponse_PropertyInitialization_ShouldSupportObjectInitializer()
    {
        // Act
        var response = new OutrightEventResponse
        {
            FixtureId = 555,
            OutrightFixture = new OutrightFixtureSnapshotResponse(),
            OutrightScore = new OutrightLivescoreScore(),
            Markets = new[]
            {
                new OutrightMarketResponse { Id = 10, Name = "First Market" },
                new OutrightMarketResponse { Id = 20, Name = "Second Market" }
            }
        };

        // Assert
        response.FixtureId.Should().Be(555);
        response.OutrightFixture.Should().NotBeNull();
        response.OutrightScore.Should().NotBeNull();
        response.Markets.Should().HaveCount(2);
        response.Markets!.Should().Contain(m => m.Name == "First Market");
        response.Markets!.Should().Contain(m => m.Name == "Second Market");
    }
} 