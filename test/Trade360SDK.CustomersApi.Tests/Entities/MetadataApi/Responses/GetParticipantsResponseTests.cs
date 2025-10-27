using System.Collections.Generic;
using System.Linq;
using Trade360SDK.Common.Entities.Enums;
using Trade360SDK.CustomersApi.Entities.MetadataApi.Responses;

namespace Trade360SDK.CustomersApi.Tests.Entities.MetadataApi.Responses;

public class GetParticipantsResponseTests
{
    [Fact]
    public void DefaultConstructor_ShouldInitializeWithDefaultValues()
    {
        // Act
        var response = new GetParticipantsResponse();

        // Assert
        Assert.Null(response.Data);
        Assert.Equal(0, response.TotalItems);
    }

    [Fact]
    public void Data_ShouldBeSettable()
    {
        // Arrange
        var participants = new[]
        {
            new ParticipantInfo { Id = 1, Name = "Team A" },
            new ParticipantInfo { Id = 2, Name = "Team B" }
        };

        // Act
        var response = new GetParticipantsResponse { Data = participants };

        // Assert
        Assert.NotNull(response.Data);
        Assert.Equal(2, response.Data.Count());
        Assert.Equal("Team A", response.Data.First().Name);
    }

    [Fact]
    public void Data_ShouldAcceptNullValue()
    {
        // Act
        var response = new GetParticipantsResponse { Data = null };

        // Assert
        Assert.Null(response.Data);
    }

    [Fact]
    public void Data_ShouldAcceptEmptyCollection()
    {
        // Act
        var response = new GetParticipantsResponse { Data = new ParticipantInfo[0] };

        // Assert
        Assert.NotNull(response.Data);
        Assert.Empty(response.Data);
    }

    [Fact]
    public void TotalItems_ShouldBeSettable()
    {
        // Act
        var response = new GetParticipantsResponse { TotalItems = 150 };

        // Assert
        Assert.Equal(150, response.TotalItems);
    }

    [Fact]
    public void TotalItems_ShouldAcceptZero()
    {
        // Act
        var response = new GetParticipantsResponse { TotalItems = 0 };

        // Assert
        Assert.Equal(0, response.TotalItems);
    }

    [Fact]
    public void Response_WithAllProperties_ShouldBeSettable()
    {
        // Arrange
        var participants = new[]
        {
            new ParticipantInfo 
            { 
                Id = 1, 
                SportId = 6046, 
                LocationId = 142, 
                Name = "Manchester United",
                Gender = Gender.Men,
                AgeCategory = AgeCategory.Regular,
                Type = ParticipantType.Club
            }
        };

        // Act
        var response = new GetParticipantsResponse
        {
            Data = participants,
            TotalItems = 500
        };

        // Assert
        Assert.NotNull(response.Data);
        Assert.Equal(500, response.TotalItems);
        Assert.Single(response.Data);
        Assert.Equal(1, response.Data.First().Id);
        Assert.Equal(6046, response.Data.First().SportId);
        Assert.Equal(142, response.Data.First().LocationId);
        Assert.Equal("Manchester United", response.Data.First().Name);
        Assert.Equal(Gender.Men, response.Data.First().Gender);
        Assert.Equal(AgeCategory.Regular, response.Data.First().AgeCategory);
        Assert.Equal(ParticipantType.Club, response.Data.First().Type);
    }
}

public class ParticipantInfoTests
{
    [Fact]
    public void DefaultConstructor_ShouldInitializeWithDefaultValues()
    {
        // Act
        var participant = new ParticipantInfo();

        // Assert
        Assert.Equal(0, participant.Id);
        Assert.Equal(0, participant.SportId);
        Assert.Equal(0, participant.LocationId);
        Assert.Null(participant.Name);
        Assert.Null(participant.Gender);
        Assert.Null(participant.AgeCategory);
        Assert.Null(participant.Type);
    }

    [Fact]
    public void Id_ShouldBeSettable()
    {
        // Act
        var participant = new ParticipantInfo { Id = 12345 };

        // Assert
        Assert.Equal(12345, participant.Id);
    }

    [Fact]
    public void SportId_ShouldBeSettable()
    {
        // Act
        var participant = new ParticipantInfo { SportId = 6046 };

        // Assert
        Assert.Equal(6046, participant.SportId);
    }

    [Fact]
    public void LocationId_ShouldBeSettable()
    {
        // Act
        var participant = new ParticipantInfo { LocationId = 142 };

        // Assert
        Assert.Equal(142, participant.LocationId);
    }

    [Fact]
    public void Name_ShouldBeSettable()
    {
        // Arrange
        var name = "Chelsea FC";

        // Act
        var participant = new ParticipantInfo { Name = name };

        // Assert
        Assert.Equal(name, participant.Name);
    }

    [Fact]
    public void Name_ShouldAcceptNullValue()
    {
        // Act
        var participant = new ParticipantInfo { Name = null };

        // Assert
        Assert.Null(participant.Name);
    }

    [Fact]
    public void Gender_ShouldBeSettable()
    {
        // Act
        var participant = new ParticipantInfo { Gender = Gender.Women };

        // Assert
        Assert.Equal(Gender.Women, participant.Gender);
    }

    [Fact]
    public void Gender_ShouldAcceptNullValue()
    {
        // Act
        var participant = new ParticipantInfo { Gender = null };

        // Assert
        Assert.Null(participant.Gender);
    }

    [Fact]
    public void Gender_ShouldAcceptAllEnumValues()
    {
        // Arrange & Act
        var menParticipant = new ParticipantInfo { Gender = Gender.Men };
        var womenParticipant = new ParticipantInfo { Gender = Gender.Women };
        var mixParticipant = new ParticipantInfo { Gender = Gender.Mix };

        // Assert
        Assert.Equal(Gender.Men, menParticipant.Gender);
        Assert.Equal(Gender.Women, womenParticipant.Gender);
        Assert.Equal(Gender.Mix, mixParticipant.Gender);
    }

    [Fact]
    public void AgeCategory_ShouldBeSettable()
    {
        // Act
        var participant = new ParticipantInfo { AgeCategory = AgeCategory.Youth };

        // Assert
        Assert.Equal(AgeCategory.Youth, participant.AgeCategory);
    }

    [Fact]
    public void AgeCategory_ShouldAcceptNullValue()
    {
        // Act
        var participant = new ParticipantInfo { AgeCategory = null };

        // Assert
        Assert.Null(participant.AgeCategory);
    }

    [Fact]
    public void AgeCategory_ShouldAcceptAllEnumValues()
    {
        // Arrange & Act
        var regular = new ParticipantInfo { AgeCategory = AgeCategory.Regular };
        var youth = new ParticipantInfo { AgeCategory = AgeCategory.Youth };
        var reserves = new ParticipantInfo { AgeCategory = AgeCategory.Reserves };

        // Assert
        Assert.Equal(AgeCategory.Regular, regular.AgeCategory);
        Assert.Equal(AgeCategory.Youth, youth.AgeCategory);
        Assert.Equal(AgeCategory.Reserves, reserves.AgeCategory);
    }

    [Fact]
    public void Type_ShouldBeSettable()
    {
        // Act
        var participant = new ParticipantInfo { Type = ParticipantType.National };

        // Assert
        Assert.Equal(ParticipantType.National, participant.Type);
    }

    [Fact]
    public void Type_ShouldAcceptNullValue()
    {
        // Act
        var participant = new ParticipantInfo { Type = null };

        // Assert
        Assert.Null(participant.Type);
    }

    [Fact]
    public void Type_ShouldAcceptMultipleEnumValues()
    {
        // Arrange & Act
        var club = new ParticipantInfo { Type = ParticipantType.Club };
        var national = new ParticipantInfo { Type = ParticipantType.National };
        var individual = new ParticipantInfo { Type = ParticipantType.Individual };

        // Assert
        Assert.Equal(ParticipantType.Club, club.Type);
        Assert.Equal(ParticipantType.National, national.Type);
        Assert.Equal(ParticipantType.Individual, individual.Type);
    }

    [Fact]
    public void AllProperties_ShouldBeSettable()
    {
        // Arrange & Act
        var participant = new ParticipantInfo
        {
            Id = 999,
            SportId = 6046,
            LocationId = 142,
            Name = "Liverpool FC",
            Gender = Gender.Men,
            AgeCategory = AgeCategory.Regular,
            Type = ParticipantType.Club
        };

        // Assert
        Assert.Equal(999, participant.Id);
        Assert.Equal(6046, participant.SportId);
        Assert.Equal(142, participant.LocationId);
        Assert.Equal("Liverpool FC", participant.Name);
        Assert.Equal(Gender.Men, participant.Gender);
        Assert.Equal(AgeCategory.Regular, participant.AgeCategory);
        Assert.Equal(ParticipantType.Club, participant.Type);
    }

    [Fact]
    public void Properties_ShouldBeMutableAfterCreation()
    {
        // Arrange
        var participant = new ParticipantInfo
        {
            Id = 1,
            Name = "Initial Name",
            Gender = Gender.Men
        };

        // Act
        participant.Id = 2;
        participant.Name = "Updated Name";
        participant.Gender = Gender.Women;

        // Assert
        Assert.Equal(2, participant.Id);
        Assert.Equal("Updated Name", participant.Name);
        Assert.Equal(Gender.Women, participant.Gender);
    }
}

