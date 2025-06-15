using FluentAssertions;
using Trade360SDK.CustomersApi.Entities.MetadataApi.Requests;
using Trade360SDK.CustomersApi.Validators;

namespace Trade360SDK.CustomersApi.Tests;

public class GetTranslationsRequestValidatorCoverageTests
{
    [Fact]
    public void Validate_WithLanguagesContainingWhitespace_ShouldExecuteValidationLogic()
    {
        var request = new GetTranslationsRequest
        {
            Languages = new[] { 1, 0, 2 },
            SportIds = new[] { 1 }
        };

        var act = () => GetTranslationsRequestValidator.Validate(request);

        act.Should().Throw<ArgumentException>()
           .WithMessage("*Languages cannot contain null, empty, or whitespace values*");
    }

    [Fact]
    public void Validate_WithLanguagesContainingEmpty_ShouldExecuteValidationLogic()
    {
        var request = new GetTranslationsRequest
        {
            Languages = new[] { 1, 0, 2 },
            SportIds = new[] { 1 }
        };

        var act = () => GetTranslationsRequestValidator.Validate(request);

        act.Should().Throw<ArgumentException>()
           .WithMessage("*Languages cannot contain null, empty, or whitespace values*");
    }

    [Fact]
    public void Validate_WithValidLanguages_ShouldExecuteValidationLogic()
    {
        var request = new GetTranslationsRequest
        {
            Languages = new[] { 1, 2, 3 },
            SportIds = new[] { 1 }
        };

        var act = () => GetTranslationsRequestValidator.Validate(request);

        act.Should().NotThrow();
    }
}
