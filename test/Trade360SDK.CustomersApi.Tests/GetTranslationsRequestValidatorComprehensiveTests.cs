using FluentAssertions;
using Trade360SDK.CustomersApi.Entities.MetadataApi.Requests;
using Trade360SDK.CustomersApi.Validators;

namespace Trade360SDK.CustomersApi.Tests;

public class GetTranslationsRequestValidatorComprehensiveTests
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
    public void Validate_WithNullLanguages_ShouldThrowArgumentException()
    {
        var request = new GetTranslationsRequest
        {
            Languages = null,
            SportIds = new List<int> { 1 }
        };

        var act = () => GetTranslationsRequestValidator.Validate(request);

        act.Should().Throw<ArgumentException>();
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

        act.Should().Throw<ArgumentException>();
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public void Validate_WithInvalidLanguageValues_ShouldThrowArgumentException(string invalidLanguage)
    {
        var request = new GetTranslationsRequest
        {
            Languages = new List<string> { "en", invalidLanguage },
            SportIds = new List<int> { 1 }
        };

        var act = () => GetTranslationsRequestValidator.Validate(request);

        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void Validate_WithAllEntityTypesProvided_ShouldNotThrow()
    {
        var request = new GetTranslationsRequest
        {
            Languages = new List<string> { "en" },
            SportIds = new List<int> { 1 },
            LocationIds = new List<int> { 1 },
            LeagueIds = new List<int> { 1 },
            MarketIds = new List<int> { 1 },
            ParticipantIds = new List<int> { 1 }
        };

        var act = () => GetTranslationsRequestValidator.Validate(request);

        act.Should().NotThrow();
    }

    [Fact]
    public void Validate_WithNoEntityIds_ShouldThrowArgumentException()
    {
        var request = new GetTranslationsRequest
        {
            Languages = new List<string> { "en" }
        };

        var act = () => GetTranslationsRequestValidator.Validate(request);

        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void Validate_WithEmptyEntityIds_ShouldThrowArgumentException()
    {
        var request = new GetTranslationsRequest
        {
            Languages = new List<string> { "en" },
            SportIds = new List<int>(),
            LocationIds = new List<int>(),
            LeagueIds = new List<int>(),
            MarketIds = new List<int>(),
            ParticipantIds = new List<int>()
        };

        var act = () => GetTranslationsRequestValidator.Validate(request);

        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void Validate_WithOnlyLocationIds_ShouldNotThrow()
    {
        var request = new GetTranslationsRequest
        {
            Languages = new List<string> { "en" },
            LocationIds = new List<int> { 1, 2, 3 }
        };

        var act = () => GetTranslationsRequestValidator.Validate(request);

        act.Should().NotThrow();
    }

    [Fact]
    public void Validate_WithOnlyLeagueIds_ShouldNotThrow()
    {
        var request = new GetTranslationsRequest
        {
            Languages = new List<string> { "en" },
            LeagueIds = new List<int> { 1, 2, 3 }
        };

        var act = () => GetTranslationsRequestValidator.Validate(request);

        act.Should().NotThrow();
    }

    [Fact]
    public void Validate_WithOnlyMarketIds_ShouldNotThrow()
    {
        var request = new GetTranslationsRequest
        {
            Languages = new List<string> { "en" },
            MarketIds = new List<int> { 1, 2, 3 }
        };

        var act = () => GetTranslationsRequestValidator.Validate(request);

        act.Should().NotThrow();
    }

    [Fact]
    public void Validate_WithOnlyParticipantIds_ShouldNotThrow()
    {
        var request = new GetTranslationsRequest
        {
            Languages = new List<string> { "en" },
            ParticipantIds = new List<int> { 1, 2, 3 }
        };

        var act = () => GetTranslationsRequestValidator.Validate(request);

        act.Should().NotThrow();
    }

    [Fact]
    public void Validate_WithMultipleLanguages_ShouldNotThrow()
    {
        var request = new GetTranslationsRequest
        {
            Languages = new List<string> { "en", "es", "fr", "de", "it" },
            SportIds = new List<int> { 1 }
        };

        var act = () => GetTranslationsRequestValidator.Validate(request);

        act.Should().NotThrow();
    }

    [Fact]
    public void Validate_WithLargeEntityIdLists_ShouldNotThrow()
    {
        var request = new GetTranslationsRequest
        {
            Languages = new List<string> { "en" },
            SportIds = Enumerable.Range(1, 100).ToList(),
            LocationIds = Enumerable.Range(1, 50).ToList(),
            LeagueIds = Enumerable.Range(1, 200).ToList()
        };

        var act = () => GetTranslationsRequestValidator.Validate(request);

        act.Should().NotThrow();
    }

    [Fact]
    public void Validate_WithNegativeEntityIds_ShouldNotThrow()
    {
        var request = new GetTranslationsRequest
        {
            Languages = new List<string> { "en" },
            SportIds = new List<int> { -1, -2, -3 }
        };

        var act = () => GetTranslationsRequestValidator.Validate(request);

        act.Should().NotThrow();
    }

    [Fact]
    public void Validate_WithZeroEntityIds_ShouldNotThrow()
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
    public void Validate_WithMixedValidAndInvalidLanguages_ShouldThrowArgumentException()
    {
        var request = new GetTranslationsRequest
        {
            Languages = new List<string> { "en", "", "es" },
            SportIds = new List<int> { 1 }
        };

        var act = () => GetTranslationsRequestValidator.Validate(request);

        act.Should().Throw<ArgumentException>();
    }
}
