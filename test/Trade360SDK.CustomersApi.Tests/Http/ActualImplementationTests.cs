using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System.Net;
using Trade360SDK.Common.Configuration;
using Trade360SDK.CustomersApi.Http;
using Trade360SDK.CustomersApi.Entities.MetadataApi.Requests;
using Trade360SDK.CustomersApi.Validators;

namespace Trade360SDK.CustomersApi.Tests;

public class ActualImplementationTests
{
    [Fact]
    public void MetadataHttpClient_Constructor_WithNullFactory_ShouldExecuteBaseValidation()
    {
        var credentials = new PackageCredentials { Username = "test", Password = "test", PackageId = 123 };
        var mockMapper = new Mock<AutoMapper.IMapper>();

        var act = () => new MetadataHttpClient(null!, "https://api.test.com", credentials, mockMapper.Object);

        act.Should().Throw<ArgumentNullException>()
           .WithParameterName("httpClientFactory");
    }

    [Fact]
    public void MetadataHttpClient_Constructor_WithEmptyBaseUrl_ShouldExecuteBaseValidation()
    {
        var mockFactory = new Mock<IHttpClientFactory>();
        var credentials = new PackageCredentials { Username = "test", Password = "test", PackageId = 123 };
        var mockMapper = new Mock<AutoMapper.IMapper>();

        var act = () => new MetadataHttpClient(mockFactory.Object, "", credentials, mockMapper.Object);

        act.Should().Throw<ArgumentException>()
           .WithParameterName("baseUrl");
    }

    [Fact]
    public void MetadataHttpClient_Constructor_WithWhitespaceBaseUrl_ShouldExecuteBaseValidation()
    {
        var mockFactory = new Mock<IHttpClientFactory>();
        var credentials = new PackageCredentials { Username = "test", Password = "test", PackageId = 123 };
        var mockMapper = new Mock<AutoMapper.IMapper>();

        var act = () => new MetadataHttpClient(mockFactory.Object, "   ", credentials, mockMapper.Object);

        act.Should().Throw<ArgumentException>()
           .WithParameterName("baseUrl");
    }

    [Fact]
    public void MetadataHttpClient_Constructor_WithNullCredentials_ShouldExecuteBaseValidation()
    {
        var mockFactory = new Mock<IHttpClientFactory>();
        var mockMapper = new Mock<AutoMapper.IMapper>();

        var act = () => new MetadataHttpClient(mockFactory.Object, "https://api.test.com", null!, mockMapper.Object);

        act.Should().Throw<ArgumentNullException>()
           .WithParameterName("settings");
    }

    [Fact]
    public void GetTranslationsRequestValidator_WithNullLanguages_ShouldExecuteValidation()
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
    public void GetTranslationsRequestValidator_WithEmptyLanguages_ShouldExecuteValidation()
    {
        var request = new GetTranslationsRequest
        {
            Languages = Array.Empty<int>(),
            SportIds = new[] { 1 }
        };

        var act = () => GetTranslationsRequestValidator.Validate(request);

        act.Should().Throw<ArgumentException>()
           .WithMessage("Languages must be filled.");
    }

    [Fact]
    public void GetTranslationsRequestValidator_WithNullLanguageValue_ShouldExecuteValidation()
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
    public void GetTranslationsRequestValidator_WithAllFieldsEmpty_ShouldExecuteValidation()
    {
        var request = new GetTranslationsRequest
        {
            Languages = new[] { 1 },
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
    public void GetTranslationsRequestValidator_WithEmptyArrays_ShouldExecuteValidation()
    {
        var request = new GetTranslationsRequest
        {
            Languages = new[] { 1 },
            SportIds = Array.Empty<int>(),
            LocationIds = Array.Empty<int>(),
            LeagueIds = Array.Empty<int>(),
            MarketIds = Array.Empty<int>(),
            ParticipantIds = Array.Empty<int>()
        };

        var act = () => GetTranslationsRequestValidator.Validate(request);

        act.Should().Throw<ArgumentException>()
           .WithMessage("*At least one of SportIds, LocationIds, LeagueIds, MarketIds, or ParticipantIds must be filled*");
    }
}
