using System.Collections.Generic;
using Trade360SDK.CustomersApi.Entities.MetadataApi.Requests;

namespace Trade360SDK.CustomersApi.Tests.Entities.MetadataApi.Requests;

public class GetParticipantsRequestDtoTests
{
    [Fact]
    public void DefaultConstructor_ShouldInitializeWithNullValues()
    {
        // Act
        var dto = new GetParticipantsRequestDto();

        // Assert
        Assert.Null(dto.Filter);
        Assert.Equal(0, dto.Page);
        Assert.Equal(0, dto.PageSize);
    }

    [Fact]
    public void Filter_ShouldBeSettable()
    {
        // Arrange
        var expectedFilter = new ParticipantFilterDto
        {
            Ids = new[] { 1, 2, 3 },
            SportIds = new[] { 6046 },
            LocationIds = new[] { 142 },
            Name = "Team Name",
            Gender = 1,
            AgeCategory = 0,
            Type = 1
        };

        // Act
        var dto = new GetParticipantsRequestDto { Filter = expectedFilter };

        // Assert
        Assert.Equal(expectedFilter, dto.Filter);
        Assert.Equal(new[] { 1, 2, 3 }, dto.Filter.Ids);
        Assert.Equal(new[] { 6046 }, dto.Filter.SportIds);
        Assert.Equal(new[] { 142 }, dto.Filter.LocationIds);
        Assert.Equal("Team Name", dto.Filter.Name);
        Assert.Equal(1, dto.Filter.Gender);
        Assert.Equal(0, dto.Filter.AgeCategory);
        Assert.Equal(1, dto.Filter.Type);
    }

    [Fact]
    public void Filter_ShouldAcceptNullValue()
    {
        // Arrange & Act
        var dto = new GetParticipantsRequestDto { Filter = null };

        // Assert
        Assert.Null(dto.Filter);
    }

    [Fact]
    public void PageAndPageSize_ShouldBeSettable()
    {
        // Arrange & Act
        var dto = new GetParticipantsRequestDto
        {
            Page = 2,
            PageSize = 100
        };

        // Assert
        Assert.Equal(2, dto.Page);
        Assert.Equal(100, dto.PageSize);
    }

    [Fact]
    public void ParticipantFilterDto_DefaultConstructor_ShouldInitializeWithNullValues()
    {
        // Act
        var filter = new ParticipantFilterDto();

        // Assert
        Assert.Null(filter.Ids);
        Assert.Null(filter.SportIds);
        Assert.Null(filter.LocationIds);
        Assert.Null(filter.Name);
        Assert.Null(filter.Gender);
        Assert.Null(filter.AgeCategory);
        Assert.Null(filter.Type);
    }

    [Fact]
    public void ParticipantFilterDto_Properties_ShouldBeSettable()
    {
        // Arrange
        var ids = new[] { 1, 2, 3 };
        var sportIds = new[] { 6046, 6047 };
        var locationIds = new[] { 142, 143 };
        var name = "Manchester United";
        var gender = 1; // Men
        var ageCategory = 0; // Regular
        var type = 1; // Club

        // Act
        var filter = new ParticipantFilterDto
        {
            Ids = ids,
            SportIds = sportIds,
            LocationIds = locationIds,
            Name = name,
            Gender = gender,
            AgeCategory = ageCategory,
            Type = type
        };

        // Assert
        Assert.Equal(ids, filter.Ids);
        Assert.Equal(sportIds, filter.SportIds);
        Assert.Equal(locationIds, filter.LocationIds);
        Assert.Equal(name, filter.Name);
        Assert.Equal(gender, filter.Gender);
        Assert.Equal(ageCategory, filter.AgeCategory);
        Assert.Equal(type, filter.Type);
    }

    [Fact]
    public void ParticipantFilterDto_EnumProperties_ShouldAcceptValidValues()
    {
        // Act
        var filter = new ParticipantFilterDto
        {
            Gender = 2, // Women
            AgeCategory = 1, // Youth
            Type = 3 // Individual
        };

        // Assert
        Assert.Equal(2, filter.Gender);
        Assert.Equal(1, filter.AgeCategory);
        Assert.Equal(3, filter.Type);
    }

    [Fact]
    public void ParticipantFilterDto_Collections_ShouldAcceptEmptyArrays()
    {
        // Act
        var filter = new ParticipantFilterDto
        {
            Ids = new int[0],
            SportIds = new int[0],
            LocationIds = new int[0]
        };

        // Assert
        Assert.Empty(filter.Ids);
        Assert.Empty(filter.SportIds);
        Assert.Empty(filter.LocationIds);
    }
}
