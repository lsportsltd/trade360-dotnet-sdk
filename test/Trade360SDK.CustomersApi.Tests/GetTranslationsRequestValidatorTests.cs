using FluentAssertions;
using Trade360SDK.CustomersApi.Entities.MetadataApi.Requests;
using Trade360SDK.CustomersApi.Validators;

namespace Trade360SDK.CustomersApi.Tests;

public class GetTranslationsRequestValidatorTests
{
    [Fact]
    public void Validate_WithValidLanguages_ShouldNotThrowException()
    {
        var request = new GetTranslationsRequest
        {
            Languages = new[] { "en", "es", "fr" },
            SportIds = new[] { 1 }
        };

        var act = () => GetTranslationsRequestValidator.Validate(request);

        act.Should().NotThrow();
    }

    [Fact]
    public void Validate_WithNullLanguages_ShouldThrowArgumentException()
    {
        var request = new GetTranslationsRequest
        {
            Languages = null
        };

        var act = () => GetTranslationsRequestValidator.Validate(request);

        act.Should().Throw<ArgumentException>()
           .WithMessage("Languages must be filled.");
    }

    [Fact]
    public void Validate_WithEmptyLanguages_ShouldThrowArgumentException()
    {
        var request = new GetTranslationsRequest
        {
            Languages = Array.Empty<string>(),
            SportIds = new[] { 1 }
        };

        var act = () => GetTranslationsRequestValidator.Validate(request);

        act.Should().Throw<ArgumentException>()
           .WithMessage("Languages must be filled.");
    }

    [Fact]
    public void Validate_WithSingleLanguage_ShouldNotThrowException()
    {
        var request = new GetTranslationsRequest
        {
            Languages = new[] { "en" },
            SportIds = new[] { 1 }
        };

        var act = () => GetTranslationsRequestValidator.Validate(request);

        act.Should().NotThrow();
    }

    [Fact]
    public void Validate_WithMultipleLanguages_ShouldNotThrowException()
    {
        var request = new GetTranslationsRequest
        {
            Languages = new[] { "en", "es", "fr", "de", "it" },
            SportIds = new[] { 1 }
        };

        var act = () => GetTranslationsRequestValidator.Validate(request);

        act.Should().NotThrow();
    }

    [Fact]
    public void Validate_WithValidSportIds_ShouldNotThrowException()
    {
        var request = new GetTranslationsRequest
        {
            Languages = new[] { "en" },
            SportIds = new[] { 1, 2, 3 }
        };

        var act = () => GetTranslationsRequestValidator.Validate(request);

        act.Should().NotThrow();
    }

    [Fact]
    public void Validate_WithValidLocationIds_ShouldNotThrowException()
    {
        var request = new GetTranslationsRequest
        {
            Languages = new[] { "en" },
            LocationIds = new[] { 1, 2, 3 }
        };

        var act = () => GetTranslationsRequestValidator.Validate(request);

        act.Should().NotThrow();
    }

    [Fact]
    public void Validate_WithValidLeagueIds_ShouldNotThrowException()
    {
        var request = new GetTranslationsRequest
        {
            Languages = new[] { "en" },
            LeagueIds = new[] { 10, 20, 30 }
        };

        var act = () => GetTranslationsRequestValidator.Validate(request);

        act.Should().NotThrow();
    }

    [Fact]
    public void Validate_WithValidMarketIds_ShouldNotThrowException()
    {
        var request = new GetTranslationsRequest
        {
            Languages = new[] { "en" },
            MarketIds = new[] { 100, 200, 300 }
        };

        var act = () => GetTranslationsRequestValidator.Validate(request);

        act.Should().NotThrow();
    }

    [Fact]
    public void Validate_WithValidParticipantIds_ShouldNotThrowException()
    {
        var request = new GetTranslationsRequest
        {
            Languages = new[] { "en" },
            ParticipantIds = new[] { 1000, 2000, 3000 }
        };

        var act = () => GetTranslationsRequestValidator.Validate(request);

        act.Should().NotThrow();
    }

    [Fact]
    public void Validate_WithAllValidParameters_ShouldNotThrowException()
    {
        var request = new GetTranslationsRequest
        {
            Languages = new[] { "en", "es" },
            SportIds = new[] { 1, 2 },
            LocationIds = new[] { 1, 2 },
            LeagueIds = new[] { 10, 20 },
            MarketIds = new[] { 100, 200 },
            ParticipantIds = new[] { 1000, 2000 }
        };

        var act = () => GetTranslationsRequestValidator.Validate(request);

        act.Should().NotThrow();
    }

    [Fact]
    public void Validate_WithEmptyArraysForOptionalParameters_ShouldThrowArgumentException()
    {
        var request = new GetTranslationsRequest
        {
            Languages = new[] { "en" },
            SportIds = Array.Empty<int>(),
            LocationIds = Array.Empty<int>(),
            LeagueIds = Array.Empty<int>(),
            MarketIds = Array.Empty<int>(),
            ParticipantIds = Array.Empty<int>()
        };

        var act = () => GetTranslationsRequestValidator.Validate(request);

        act.Should().Throw<ArgumentException>()
           .WithMessage("At least one of SportIds, LocationIds, LeagueIds, MarketIds, or ParticipantIds must be filled.");
    }

    [Fact]
    public void Validate_WithNullOptionalParameters_ShouldThrowArgumentException()
    {
        var request = new GetTranslationsRequest
        {
            Languages = new[] { "en" },
            SportIds = null,
            LocationIds = null,
            LeagueIds = null,
            MarketIds = null,
            ParticipantIds = null
        };

        var act = () => GetTranslationsRequestValidator.Validate(request);

        act.Should().Throw<ArgumentException>()
           .WithMessage("At least one of SportIds, LocationIds, LeagueIds, MarketIds, or ParticipantIds must be filled.");
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    [InlineData(4)]
    [InlineData(5)]
    [InlineData(6)]
    [InlineData(7)]
    [InlineData(8)]
    public void Validate_WithSingleValidLanguageCode_ShouldNotThrowException(int languageCode)
    {
        var request = new GetTranslationsRequest
        {
            Languages = new[] { languageCode.ToString() },
            SportIds = new[] { 1 }
        };

        var act = () => GetTranslationsRequestValidator.Validate(request);

        act.Should().NotThrow();
    }

    [Fact]
    public void Validate_WithDuplicateLanguages_ShouldNotThrowException()
    {
        var request = new GetTranslationsRequest
        {
            Languages = new[] { "en", "en", "es", "es" },
            SportIds = new[] { 1 }
        };

        var act = () => GetTranslationsRequestValidator.Validate(request);

        act.Should().NotThrow();
    }

    [Fact]
    public void Validate_WithLargeNumberOfLanguages_ShouldNotThrowException()
    {
        var languages = Enumerable.Range(1, 100).Select(i => $"lang{i}").ToArray();
        var request = new GetTranslationsRequest
        {
            Languages = languages,
            SportIds = new[] { 1 }
        };

        var act = () => GetTranslationsRequestValidator.Validate(request);

        act.Should().NotThrow();
    }

    [Fact]
    public void Validate_WithLargeNumberOfIds_ShouldNotThrowException()
    {
        var largeIdArray = Enumerable.Range(1, 1000).ToArray();
        var request = new GetTranslationsRequest
        {
            Languages = new[] { "en" },
            SportIds = largeIdArray,
            LocationIds = largeIdArray,
            LeagueIds = largeIdArray,
            MarketIds = largeIdArray,
            ParticipantIds = largeIdArray
        };

        var act = () => GetTranslationsRequestValidator.Validate(request);

        act.Should().NotThrow();
    }

    [Fact]
    public void Validate_WithNegativeIds_ShouldNotThrowException()
    {
        var request = new GetTranslationsRequest
        {
            Languages = new[] { "en" },
            SportIds = new[] { -1, -2, -3 },
            LocationIds = new[] { -10, -20 },
            LeagueIds = new[] { -100 },
            MarketIds = new[] { -1000, -2000 },
            ParticipantIds = new[] { -10000 }
        };

        var act = () => GetTranslationsRequestValidator.Validate(request);

        act.Should().NotThrow();
    }

    [Fact]
    public void Validate_WithZeroIds_ShouldNotThrowException()
    {
        var request = new GetTranslationsRequest
        {
            Languages = new[] { "en" },
            SportIds = new[] { 0 },
            LocationIds = new[] { 0 },
            LeagueIds = new[] { 0 },
            MarketIds = new[] { 0 },
            ParticipantIds = new[] { 0 }
        };

        var act = () => GetTranslationsRequestValidator.Validate(request);

        act.Should().NotThrow();
    }

    [Fact]
    public void Validate_WithMaxIntIds_ShouldNotThrowException()
    {
        var request = new GetTranslationsRequest
        {
            Languages = new[] { "en" },
            SportIds = new[] { int.MaxValue },
            LocationIds = new[] { int.MaxValue },
            LeagueIds = new[] { int.MaxValue },
            MarketIds = new[] { int.MaxValue },
            ParticipantIds = new[] { int.MaxValue }
        };

        var act = () => GetTranslationsRequestValidator.Validate(request);

        act.Should().NotThrow();
    }
}
