using FluentAssertions;
using Trade360SDK.CustomersApi.Entities.MetadataApi.Requests;
using Trade360SDK.CustomersApi.Validators;

namespace Trade360SDK.CustomersApi.Tests;

public class GetTranslationsRequestValidatorBusinessTests
{
    [Fact]
    public void Validate_LanguagesIsNull_ThrowsArgumentException()
    {
        var request = new GetTranslationsRequest
        {
            Languages = null,
            SportIds = new List<int> { 1 }
        };
        var act = () => GetTranslationsRequestValidator.Validate(request);
        act.Should().Throw<ArgumentException>().WithMessage("Languages must be filled.");
    }

    [Fact]
    public void Validate_LanguagesIsEmpty_ThrowsArgumentException()
    {
        var request = new GetTranslationsRequest
        {
            Languages = new List<int>(),
            SportIds = new List<int> { 1 }
        };
        var act = () => GetTranslationsRequestValidator.Validate(request);
        act.Should().Throw<ArgumentException>().WithMessage("Languages must be filled.");
    }

    [Fact]
    public void Validate_LanguagesContainsZero_ThrowsArgumentException()
    {
        var request = new GetTranslationsRequest
        {
            Languages = new List<int> { 1, 0, 2 },
            SportIds = new List<int> { 1 }
        };
        var act = () => GetTranslationsRequestValidator.Validate(request);
        act.Should().Throw<ArgumentException>().WithMessage("Languages cannot contain null, empty, or whitespace values.");
    }

    [Fact]
    public void Validate_LanguagesContainsNegativeNumbers_DoesNotThrow()
    {
        var request = new GetTranslationsRequest
        {
            Languages = new List<int> { -1, -2, 3 },
            SportIds = new List<int> { 1 }
        };
        var act = () => GetTranslationsRequestValidator.Validate(request);
        act.Should().NotThrow();
    }

    [Fact]
    public void Validate_AllOtherFieldsNull_ThrowsArgumentException()
    {
        var request = new GetTranslationsRequest
        {
            Languages = new List<int> { 1 }
            // All other fields are null
        };
        var act = () => GetTranslationsRequestValidator.Validate(request);
        act.Should().Throw<ArgumentException>().WithMessage("At least one of SportIds, LocationIds, LeagueIds, MarketIds, or ParticipantIds must be filled.");
    }

    [Fact]
    public void Validate_AllOtherFieldsEmpty_ThrowsArgumentException()
    {
        var request = new GetTranslationsRequest
        {
            Languages = new List<int> { 1 },
            SportIds = new List<int>(),
            LocationIds = new List<int>(),
            LeagueIds = new List<int>(),
            MarketIds = new List<int>(),
            ParticipantIds = new List<int>()
        };
        var act = () => GetTranslationsRequestValidator.Validate(request);
        act.Should().Throw<ArgumentException>().WithMessage("At least one of SportIds, LocationIds, LeagueIds, MarketIds, or ParticipantIds must be filled.");
    }

    [Fact]
    public void Validate_AtLeastOneOtherFieldFilled_DoesNotThrow()
    {
        var request = new GetTranslationsRequest
        {
            Languages = new List<int> { 1 },
            SportIds = new List<int> { 1 }
        };
        var act = () => GetTranslationsRequestValidator.Validate(request);
        act.Should().NotThrow();
    }

    [Theory]
    [InlineData("SportIds")]
    [InlineData("LocationIds")]
    [InlineData("LeagueIds")]
    [InlineData("MarketIds")]
    [InlineData("ParticipantIds")]
    public void Validate_EachOtherFieldFilledIndividually_DoesNotThrow(string field)
    {
        var request = new GetTranslationsRequest
        {
            Languages = new List<int> { 1 }
        };
        switch (field)
        {
            case "SportIds": request.SportIds = new List<int> { 1 }; break;
            case "LocationIds": request.LocationIds = new List<int> { 1 }; break;
            case "LeagueIds": request.LeagueIds = new List<int> { 1 }; break;
            case "MarketIds": request.MarketIds = new List<int> { 1 }; break;
            case "ParticipantIds": request.ParticipantIds = new List<int> { 1 }; break;
        }
        var act = () => GetTranslationsRequestValidator.Validate(request);
        act.Should().NotThrow();
    }

    [Fact]
    public void Validate_LanguagesContainsDuplicates_DoesNotThrow()
    {
        var request = new GetTranslationsRequest
        {
            Languages = new List<int> { 1, 1, 2 },
            SportIds = new List<int> { 1 }
        };
        var act = () => GetTranslationsRequestValidator.Validate(request);
        act.Should().NotThrow();
    }

    [Fact]
    public void Validate_LanguagesContainsSingleValidValue_DoesNotThrow()
    {
        var request = new GetTranslationsRequest
        {
            Languages = new List<int> { 42 },
            SportIds = new List<int> { 1 }
        };
        var act = () => GetTranslationsRequestValidator.Validate(request);
        act.Should().NotThrow();
    }

    [Fact]
    public void Validate_LanguagesContainsOnlyZero_ThrowsArgumentException()
    {
        var request = new GetTranslationsRequest
        {
            Languages = new List<int> { 0 },
            SportIds = new List<int> { 1 }
        };
        var act = () => GetTranslationsRequestValidator.Validate(request);
        act.Should().Throw<ArgumentException>().WithMessage("Languages cannot contain null, empty, or whitespace values.");
    }

    [Fact]
    public void Validate_LanguagesContainsZeroAndNegative_ThrowsArgumentException()
    {
        var request = new GetTranslationsRequest
        {
            Languages = new List<int> { 0, -1 },
            SportIds = new List<int> { 1 }
        };
        var act = () => GetTranslationsRequestValidator.Validate(request);
        act.Should().Throw<ArgumentException>().WithMessage("Languages cannot contain null, empty, or whitespace values.");
    }

    [Fact]
    public void Validate_AllFieldsFilledWithValidData_DoesNotThrow()
    {
        var request = new GetTranslationsRequest
        {
            Languages = new List<int> { 1, 2, 3 },
            SportIds = new List<int> { 1 },
            LocationIds = new List<int> { 2 },
            LeagueIds = new List<int> { 3 },
            MarketIds = new List<int> { 4 },
            ParticipantIds = new List<int> { 5 }
        };
        var act = () => GetTranslationsRequestValidator.Validate(request);
        act.Should().NotThrow();
    }
} 