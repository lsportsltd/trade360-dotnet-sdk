using Trade360SDK.Common.Entities.Enums;
using Trade360SDK.Common.Entities.Fixtures;
using Trade360SDK.Common.Entities.Shared;

namespace Trade360SDK.Common.Entities.Tests.Entities.Fixtures;

public class FixtureVenueTests
{
    [Fact]
    public void DefaultConstructor_ShouldInitializeWithNullValues()
    {
        // Act
        var fixtureVenue = new FixtureVenue();

        // Assert
        Assert.Equal(0, fixtureVenue.Id);
        Assert.Null(fixtureVenue.Name);
        Assert.Null(fixtureVenue.Assignment);
    }

    [Fact]
    public void Properties_ShouldBeSettable()
    {
        // Arrange
        var expectedId = 123;
        var expectedName = "Test Stadium";
        var expectedCity = new IdNamePair { Id = 456, Name = "London" };
        var expectedCourtSurface = CourtSurface.Hard;
        var expectedEnvironment = VenueEnvironment.Outdoors;
        var expectedAssignment = VenueAssignment.Home;

        // Act
        var fixtureVenue = new FixtureVenue
        {
            Id = expectedId,
            Name = expectedName,
            City = expectedCity,
            CourtSurfaceType = expectedCourtSurface,
            Environment = expectedEnvironment,
            Assignment = expectedAssignment
        };

        // Assert
        Assert.Equal(expectedId, fixtureVenue.Id);
        Assert.Equal(expectedName, fixtureVenue.Name);
        Assert.Equal(expectedCity, fixtureVenue.City);
        Assert.Equal(expectedCourtSurface, fixtureVenue.CourtSurfaceType);
        Assert.Equal(expectedEnvironment, fixtureVenue.Environment);
        Assert.Equal(expectedAssignment, fixtureVenue.Assignment);
    }

    [Fact]
    public void Name_ShouldAcceptNullValue()
    {
        // Arrange & Act
        var fixtureVenue = new FixtureVenue { Name = null };

        // Assert
        Assert.Null(fixtureVenue.Name);
    }

    [Fact]
    public void Assignment_ShouldAcceptNullValue()
    {
        // Arrange & Act
        var fixtureVenue = new FixtureVenue { Assignment = null };

        // Assert
        Assert.Null(fixtureVenue.Assignment);
    }

    [Theory]
    [InlineData(VenueAssignment.Home)]
    [InlineData(VenueAssignment.Away)]
    [InlineData(VenueAssignment.Neutral)]
    public void Assignment_ShouldAcceptValidValues(VenueAssignment assignment)
    {
        // Act
        var fixtureVenue = new FixtureVenue { Assignment = assignment };

        // Assert
        Assert.Equal(assignment, fixtureVenue.Assignment);
    }

    [Fact]
    public void SetAllProperties_ShouldWorkCorrectly()
    {
        // Arrange
        var expectedId = 789;
        var expectedName = "Complete Stadium";
        var expectedCapacity = 50000;
        var expectedAttendance = 45000;
        var expectedCourtSurface = CourtSurface.Grass;
        var expectedEnvironment = VenueEnvironment.Indoors;
        var expectedAssignment = VenueAssignment.Away;
        var expectedCountry = new IdNamePair { Id = 1, Name = "UK" };
        var expectedState = new IdNamePair { Id = 2, Name = "England" };
        var expectedCity = new IdNamePair { Id = 3, Name = "London" };

        // Act
        var fixtureVenue = new FixtureVenue
        {
            Id = expectedId,
            Name = expectedName,
            Capacity = expectedCapacity,
            Attendance = expectedAttendance,
            CourtSurfaceType = expectedCourtSurface,
            Environment = expectedEnvironment,
            Assignment = expectedAssignment,
            Country = expectedCountry,
            State = expectedState,
            City = expectedCity
        };

        // Assert
        Assert.Equal(expectedId, fixtureVenue.Id);
        Assert.Equal(expectedName, fixtureVenue.Name);
        Assert.Equal(expectedCapacity, fixtureVenue.Capacity);
        Assert.Equal(expectedAttendance, fixtureVenue.Attendance);
        Assert.Equal(expectedCourtSurface, fixtureVenue.CourtSurfaceType);
        Assert.Equal(expectedEnvironment, fixtureVenue.Environment);
        Assert.Equal(expectedAssignment, fixtureVenue.Assignment);
        Assert.Equal(expectedCountry, fixtureVenue.Country);
        Assert.Equal(expectedState, fixtureVenue.State);
        Assert.Equal(expectedCity, fixtureVenue.City);
    }

    [Fact]
    public void Capacity_ShouldAcceptNullValue()
    {
        // Arrange & Act
        var fixtureVenue = new FixtureVenue { Capacity = null };

        // Assert
        Assert.Null(fixtureVenue.Capacity);
    }

    [Fact]
    public void Attendance_ShouldAcceptNullValue()
    {
        // Arrange & Act
        var fixtureVenue = new FixtureVenue { Attendance = null };

        // Assert
        Assert.Null(fixtureVenue.Attendance);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1000)]
    [InlineData(50000)]
    [InlineData(100000)]
    public void Capacity_ShouldAcceptValidValues(int capacity)
    {
        // Act
        var fixtureVenue = new FixtureVenue { Capacity = capacity };

        // Assert
        Assert.Equal(capacity, fixtureVenue.Capacity);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(500)]
    [InlineData(25000)]
    [InlineData(75000)]
    public void Attendance_ShouldAcceptValidValues(int attendance)
    {
        // Act
        var fixtureVenue = new FixtureVenue { Attendance = attendance };

        // Assert
        Assert.Equal(attendance, fixtureVenue.Attendance);
    }
}