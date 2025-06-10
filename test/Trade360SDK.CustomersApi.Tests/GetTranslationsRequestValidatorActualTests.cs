using FluentAssertions;
using Trade360SDK.CustomersApi.Entities.MetadataApi.Requests;
using Trade360SDK.CustomersApi.Validators;

namespace Trade360SDK.CustomersApi.Tests;

public class GetTranslationsRequestValidatorActualTests
{
    [Fact]
    public void Validate_WithNullLanguages_ShouldExecuteActualValidation()
    {
        var request = new GetTranslationsRequest
        {
            Languages = null,
            SportIds = new[] { 1 }
        };

        var act = () => GetTranslationsRequestValidator.Validate(request);

        act.Should().Throw<ArgumentException>()
           .WithMessage("Languages must be filled.");
    }

    [Fact]
    public void Validate_WithEmptyLanguages_ShouldExecuteActualValidation()
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
    public void Validate_WithNullLanguageValue_ShouldExecuteActualValidation()
    {
        var request = new GetTranslationsRequest
        {
            Languages = new string[] { "en", null!, "es" },
            SportIds = new[] { 1 }
        };

        var act = () => GetTranslationsRequestValidator.Validate(request);

        act.Should().Throw<ArgumentException>()
           .WithMessage("*Languages cannot contain null, empty, or whitespace values*");
    }

    [Fact]
    public void Validate_WithWhitespaceLanguageValue_ShouldExecuteActualValidation()
    {
        var request = new GetTranslationsRequest
        {
            Languages = new string[] { "en", " ", "es" },
            SportIds = new[] { 1 }
        };

        var act = () => GetTranslationsRequestValidator.Validate(request);

        act.Should().Throw<ArgumentException>()
           .WithMessage("*Languages cannot contain null, empty, or whitespace values*");
    }

    [Fact]
    public void Validate_WithAllFieldsEmpty_ShouldExecuteActualValidation()
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
           .WithMessage("*At least one of SportIds, LocationIds, LeagueIds, MarketIds, or ParticipantIds must be filled*");
    }

    [Fact]
    public void Validate_WithValidRequest_ShouldExecuteActualValidation()
    {
        var request = new GetTranslationsRequest
        {
            Languages = new[] { "en", "es" },
            SportIds = new[] { 1, 2 }
        };

        var act = () => GetTranslationsRequestValidator.Validate(request);

        act.Should().NotThrow();
    }
}
