using FluentAssertions;
using Trade360SDK.CustomersApi.Entities.MetadataApi.Requests;
using Trade360SDK.CustomersApi.Validators;

namespace Trade360SDK.CustomersApi.Tests;

public class GetTranslationsRequestValidatorAdvancedTests
{
    [Fact]
    public void Validate_WithValidRequest_ShouldNotThrow()
    {
        var request = new GetTranslationsRequest
        {
            Languages = new List<int> { 1, 2 },
            SportIds = new List<int> { 1, 2 }
        };

        var act = () => GetTranslationsRequestValidator.Validate(request);

        act.Should().NotThrow();
    }

    [Fact]
    public void Validate_WithNullRequest_ShouldThrowNullReferenceException()
    {
        var act = () => GetTranslationsRequestValidator.Validate(null!);

        act.Should().Throw<NullReferenceException>();
    }

    [Fact]
    public void Validate_WithNullLanguages_ShouldThrowArgumentException()
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
    public void Validate_WithEmptyLanguages_ShouldThrowArgumentException()
    {
        var request = new GetTranslationsRequest
        {
            Languages = new List<int>(),
            SportIds = new List<int> { 1 }
        };

        var act = () => GetTranslationsRequestValidator.Validate(request);

        act.Should().Throw<ArgumentException>().WithMessage("Languages must be filled.");
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    [InlineData(4)]
    [InlineData(5)]
    [InlineData(6)]
    [InlineData(7)]
    public void Validate_WithInvalidLanguageCode_ShouldThrowArgumentException(int invalidLanguage)
    {
        var request = new GetTranslationsRequest
        {
            Languages = new List<int> { invalidLanguage },
            SportIds = new List<int> { 1 }
        };

        var act = () => GetTranslationsRequestValidator.Validate(request);

        act.Should().Throw<ArgumentException>().WithMessage("Languages cannot contain null, empty, or whitespace values.");
    }

    [Fact]
    public void Validate_WithValidLanguagesOnly_ShouldThrowArgumentException()
    {
        var request = new GetTranslationsRequest
        {
            Languages = new List<int> { 1, 2, 3 }
        };

        var act = () => GetTranslationsRequestValidator.Validate(request);

        act.Should().Throw<ArgumentException>().WithMessage("At least one of SportIds, LocationIds, LeagueIds, MarketIds, or ParticipantIds must be filled.");
    }

    [Fact]
    public void Validate_WithSportIdsOnly_ShouldThrowArgumentException()
    {
        var request = new GetTranslationsRequest
        {
            SportIds = new List<int> { 1, 2, 3 }
        };

        var act = () => GetTranslationsRequestValidator.Validate(request);

        act.Should().Throw<ArgumentException>().WithMessage("Languages must be filled.");
    }

    [Fact]
    public void Validate_WithAllValidParameters_ShouldNotThrow()
    {
        var request = new GetTranslationsRequest
        {
            Languages = new List<int> { 1, 2 },
            SportIds = new List<int> { 1, 2 },
            LocationIds = new List<int> { 10, 20 },
            LeagueIds = new List<int> { 100, 200 },
            MarketIds = new List<int> { 1000, 2000 },
            ParticipantIds = new List<int> { 10000, 20000 }
        };

        var act = () => GetTranslationsRequestValidator.Validate(request);

        act.Should().NotThrow();
    }

    [Fact]
    public void Validate_WithDuplicateLanguages_ShouldNotThrow()
    {
        var request = new GetTranslationsRequest
        {
            Languages = new List<int> { 1, 1, 2 },
            SportIds = new List<int> { 1 }
        };

        var act = () => GetTranslationsRequestValidator.Validate(request);

        act.Should().NotThrow();
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    [InlineData(4)]
    [InlineData(5)]
    [InlineData(6)]
    [InlineData(7)]
    public void Validate_WithValidLanguageCodes_ShouldNotThrow(int language)
    {
        var request = new GetTranslationsRequest
        {
            Languages = new List<int> { language },
            SportIds = new List<int> { 1 }
        };

        var act = () => GetTranslationsRequestValidator.Validate(request);

        act.Should().NotThrow();
    }

    [Fact]
    public void Validate_WithEmptyCollections_ShouldThrowArgumentException()
    {
        var request = new GetTranslationsRequest
        {
            Languages = new List<int>(),
            SportIds = new List<int>(),
            LocationIds = new List<int>(),
            LeagueIds = new List<int>(),
            MarketIds = new List<int>(),
            ParticipantIds = new List<int>()
        };

        var act = () => GetTranslationsRequestValidator.Validate(request);

        act.Should().Throw<ArgumentException>().WithMessage("Languages must be filled.");
    }

    [Fact]
    public void Validate_WithNegativeIds_ShouldNotThrow()
    {
        var request = new GetTranslationsRequest
        {
            Languages = new List<int> { 1 },
            SportIds = new List<int> { -1, -2 }
        };

        var act = () => GetTranslationsRequestValidator.Validate(request);

        act.Should().NotThrow();
    }

    [Fact]
    public void Validate_WithZeroIds_ShouldNotThrow()
    {
        var request = new GetTranslationsRequest
        {
            Languages = new List<int> { 0 },
            SportIds = new List<int> { 0 }
        };

        var act = () => GetTranslationsRequestValidator.Validate(request);

        act.Should().NotThrow();
    }

    [Fact]
    public void Validate_WithLargeIds_ShouldNotThrow()
    {
        var request = new GetTranslationsRequest
        {
            Languages = new List<int> { int.MaxValue },
            SportIds = new List<int> { int.MaxValue }
        };

        var act = () => GetTranslationsRequestValidator.Validate(request);

        act.Should().NotThrow();
    }
}
