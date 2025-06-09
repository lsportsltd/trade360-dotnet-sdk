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
            Languages = new List<string> { "en", "es" },
            SportIds = new List<int> { 1, 2 }
        };

        var act = () => GetTranslationsRequestValidator.Validate(request);

        act.Should().NotThrow();
    }

    [Fact]
    public void Validate_WithNullRequest_ShouldThrowNullReferenceException()
    {
        var act = () => GetTranslationsRequestValidator.Validate(null);

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
            Languages = new List<string>(),
            SportIds = new List<int> { 1 }
        };

        var act = () => GetTranslationsRequestValidator.Validate(request);

        act.Should().Throw<ArgumentException>().WithMessage("Languages must be filled.");
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("\t")]
    [InlineData("\n")]
    public void Validate_WithInvalidLanguageCode_ShouldNotThrow(string invalidLanguage)
    {
        var request = new GetTranslationsRequest
        {
            Languages = new List<string> { invalidLanguage },
            SportIds = new List<int> { 1 }
        };

        var act = () => GetTranslationsRequestValidator.Validate(request);

        act.Should().NotThrow();
    }

    [Fact]
    public void Validate_WithValidLanguagesOnly_ShouldThrowArgumentException()
    {
        var request = new GetTranslationsRequest
        {
            Languages = new List<string> { "en", "es", "fr" }
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
            Languages = new List<string> { "en", "es" },
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
            Languages = new List<string> { "en", "en", "es" },
            SportIds = new List<int> { 1 }
        };

        var act = () => GetTranslationsRequestValidator.Validate(request);

        act.Should().NotThrow();
    }

    [Theory]
    [InlineData("en")]
    [InlineData("EN")]
    [InlineData("es")]
    [InlineData("fr")]
    [InlineData("de")]
    [InlineData("zh")]
    [InlineData("ja")]
    public void Validate_WithValidLanguageCodes_ShouldNotThrow(string language)
    {
        var request = new GetTranslationsRequest
        {
            Languages = new List<string> { language },
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
            Languages = new List<string>(),
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
            Languages = new List<string> { "en" },
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
            Languages = new List<string> { "en" },
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
            Languages = new List<string> { "en" },
            SportIds = new List<int> { int.MaxValue }
        };

        var act = () => GetTranslationsRequestValidator.Validate(request);

        act.Should().NotThrow();
    }
}
