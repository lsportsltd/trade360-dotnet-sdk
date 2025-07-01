using FluentAssertions;
using Trade360SDK.SnapshotApi.Entities.Requests;

namespace Trade360SDK.SnapshotApi.Tests.Entities.Requests;

public class BaseOutrightRequestComprehensiveTests
{
    [Fact]
    public void BaseOutrightRequest_DefaultConstructor_ShouldInitializeWithDefaultValues()
    {
        // Act
        var request = new BaseOutrightRequest();

        // Assert
        request.Should().NotBeNull();
        request.PackageId.Should().Be(0);
        request.UserName.Should().BeNull();
        request.Password.Should().BeNull();
        request.Timestamp.Should().BeNull();
        request.FromDate.Should().BeNull();
        request.ToDate.Should().BeNull();
        request.Sports.Should().NotBeNull().And.BeEmpty();
        request.Locations.Should().NotBeNull().And.BeEmpty();
        request.Fixtures.Should().NotBeNull().And.BeEmpty();
        request.Tournaments.Should().NotBeNull().And.BeEmpty();
        request.Markets.Should().NotBeNull().And.BeEmpty();
    }

    [Fact]
    public void BaseOutrightRequest_InheritsFromBaseRequest_ShouldHaveBaseProperties()
    {
        // Act
        var request = new BaseOutrightRequest();

        // Assert
        request.Should().BeAssignableTo<BaseRequest>();
        
        // Test base properties
        request.PackageId = 123;
        request.UserName = "testuser";
        request.Password = "testpass";

        request.PackageId.Should().Be(123);
        request.UserName.Should().Be("testuser");
        request.Password.Should().Be("testpass");
    }

    [Theory]
    [InlineData(null)]
    [InlineData(0L)]
    [InlineData(1640995200000L)] // 2022-01-01 00:00:00 UTC
    [InlineData(long.MaxValue)]
    public void BaseOutrightRequest_TimestampProperty_ShouldAcceptVariousValues(long? timestamp)
    {
        // Act
        var request = new BaseOutrightRequest { Timestamp = timestamp };

        // Assert
        request.Timestamp.Should().Be(timestamp);
    }

    [Theory]
    [InlineData(null)]
    [InlineData(0L)]
    [InlineData(1640995200000L)]
    [InlineData(long.MaxValue)]
    public void BaseOutrightRequest_FromDateProperty_ShouldAcceptVariousValues(long? fromDate)
    {
        // Act
        var request = new BaseOutrightRequest { FromDate = fromDate };

        // Assert
        request.FromDate.Should().Be(fromDate);
    }

    [Theory]
    [InlineData(null)]
    [InlineData(0L)]
    [InlineData(1640995200000L)]
    [InlineData(long.MaxValue)]
    public void BaseOutrightRequest_ToDateProperty_ShouldAcceptVariousValues(long? toDate)
    {
        // Act
        var request = new BaseOutrightRequest { ToDate = toDate };

        // Assert
        request.ToDate.Should().Be(toDate);
    }

    [Fact]
    public void BaseOutrightRequest_SportsProperty_ShouldAcceptCollections()
    {
        // Arrange
        var sports = new[] { 1, 2, 3, 5, 8 };

        // Act
        var request = new BaseOutrightRequest { Sports = sports };

        // Assert
        request.Sports.Should().BeEquivalentTo(sports);
        request.Sports.Should().HaveCount(5);
        request.Sports.Should().Contain(new[] { 1, 2, 3, 5, 8 });
    }

    [Fact]
    public void BaseOutrightRequest_LocationsProperty_ShouldAcceptCollections()
    {
        // Arrange
        var locations = new List<int> { 10, 20, 30 };

        // Act
        var request = new BaseOutrightRequest { Locations = locations };

        // Assert
        request.Locations.Should().BeEquivalentTo(locations);
        request.Locations.Should().HaveCount(3);
    }

    [Fact]
    public void BaseOutrightRequest_FixturesProperty_ShouldAcceptCollections()
    {
        // Arrange
        var fixtures = new[] { 100, 200, 300, 400 };

        // Act
        var request = new BaseOutrightRequest { Fixtures = fixtures };

        // Assert
        request.Fixtures.Should().BeEquivalentTo(fixtures);
        request.Fixtures.Should().HaveCount(4);
    }

    [Fact]
    public void BaseOutrightRequest_TournamentsProperty_ShouldAcceptCollections()
    {
        // Arrange
        var tournaments = new[] { 50, 60, 70 };

        // Act
        var request = new BaseOutrightRequest { Tournaments = tournaments };

        // Assert
        request.Tournaments.Should().BeEquivalentTo(tournaments);
        request.Tournaments.Should().HaveCount(3);
    }

    [Fact]
    public void BaseOutrightRequest_MarketsProperty_ShouldAcceptCollections()
    {
        // Arrange
        var markets = new[] { 1000, 2000, 3000 };

        // Act
        var request = new BaseOutrightRequest { Markets = markets };

        // Assert
        request.Markets.Should().BeEquivalentTo(markets);
        request.Markets.Should().HaveCount(3);
    }

    [Fact]
    public void BaseOutrightRequest_EmptyCollections_ShouldWorkCorrectly()
    {
        // Act
        var request = new BaseOutrightRequest
        {
            Sports = new int[0],
            Locations = new List<int>(),
            Fixtures = Enumerable.Empty<int>(),
            Tournaments = new int[0],
            Markets = new List<int>()
        };

        // Assert
        request.Sports.Should().NotBeNull().And.BeEmpty();
        request.Locations.Should().NotBeNull().And.BeEmpty();
        request.Fixtures.Should().NotBeNull().And.BeEmpty();
        request.Tournaments.Should().NotBeNull().And.BeEmpty();
        request.Markets.Should().NotBeNull().And.BeEmpty();
    }

    [Fact]
    public void BaseOutrightRequest_ComplexScenario_ShouldMaintainAllProperties()
    {
        // Arrange
        var timestamp = 1640995200000L;
        var fromDate = 1640908800000L;
        var toDate = 1641081600000L;
        var sports = new[] { 1, 4, 6 };
        var locations = new[] { 10, 20 };
        var fixtures = new[] { 100, 200, 300 };
        var tournaments = new[] { 50, 60 };
        var markets = new[] { 1000, 2000 };

        // Act
        var request = new BaseOutrightRequest
        {
            PackageId = 123,
            UserName = "testuser",
            Password = "testpass",
            Timestamp = timestamp,
            FromDate = fromDate,
            ToDate = toDate,
            Sports = sports,
            Locations = locations,
            Fixtures = fixtures,
            Tournaments = tournaments,
            Markets = markets
        };

        // Assert
        request.PackageId.Should().Be(123);
        request.UserName.Should().Be("testuser");
        request.Password.Should().Be("testpass");
        request.Timestamp.Should().Be(timestamp);
        request.FromDate.Should().Be(fromDate);
        request.ToDate.Should().Be(toDate);
        request.Sports.Should().BeEquivalentTo(sports);
        request.Locations.Should().BeEquivalentTo(locations);
        request.Fixtures.Should().BeEquivalentTo(fixtures);
        request.Tournaments.Should().BeEquivalentTo(tournaments);
        request.Markets.Should().BeEquivalentTo(markets);
    }

    [Fact]
    public void BaseOutrightRequest_CollectionModification_ShouldMaintainReferenceIntegrity()
    {
        // Arrange
        var sports = new List<int> { 1, 2, 3 };
        var request = new BaseOutrightRequest { Sports = sports };

        // Act
        sports.Add(4);

        // Assert
        request.Sports.Should().Contain(4);
        request.Sports.Should().HaveCount(4);
    }

    [Fact]
    public void BaseOutrightRequest_DifferenceFromBaseStandardRequest_ShouldHaveTournamentsInsteadOfLeagues()
    {
        // This test verifies the key difference between BaseOutrightRequest and BaseStandardRequest
        // BaseOutrightRequest has Tournaments, BaseStandardRequest has Leagues

        // Act
        var request = new BaseOutrightRequest();

        // Assert
        request.Tournaments.Should().NotBeNull();
        
        // Verify it doesn't have Leagues property (would cause compilation error if it did)
        // This is a compile-time check, but we can verify the type
        typeof(BaseOutrightRequest).GetProperty("Leagues").Should().BeNull();
        typeof(BaseOutrightRequest).GetProperty("Tournaments").Should().NotBeNull();
    }

    [Theory]
    [InlineData(new int[0])]
    [InlineData(new[] { 1 })]
    [InlineData(new[] { 1, 2, 3, 4, 5 })]
    [InlineData(new[] { -1, 0, 1 })]
    public void BaseOutrightRequest_TournamentsProperty_ShouldAcceptVariousArraySizes(int[] tournaments)
    {
        // Act
        var request = new BaseOutrightRequest { Tournaments = tournaments };

        // Assert
        request.Tournaments.Should().BeEquivalentTo(tournaments);
        request.Tournaments.Should().HaveCount(tournaments.Length);
    }

    [Fact]
    public void BaseOutrightRequest_PropertyInitialization_ShouldSupportObjectInitializer()
    {
        // Act
        var request = new BaseOutrightRequest
        {
            PackageId = 999,
            UserName = "user123",
            Password = "pass456",
            Timestamp = 1640995200000L,
            FromDate = 1640908800000L,
            ToDate = 1641081600000L,
            Sports = new[] { 1, 2 },
            Locations = new[] { 10 },
            Fixtures = new[] { 100, 200 },
            Tournaments = new[] { 50 },
            Markets = new[] { 1000, 2000, 3000 }
        };

        // Assert
        request.PackageId.Should().Be(999);
        request.UserName.Should().Be("user123");
        request.Password.Should().Be("pass456");
        request.Timestamp.Should().Be(1640995200000L);
        request.FromDate.Should().Be(1640908800000L);
        request.ToDate.Should().Be(1641081600000L);
        request.Sports.Should().HaveCount(2);
        request.Locations.Should().HaveCount(1);
        request.Fixtures.Should().HaveCount(2);
        request.Tournaments.Should().HaveCount(1);
        request.Markets.Should().HaveCount(3);
    }
} 