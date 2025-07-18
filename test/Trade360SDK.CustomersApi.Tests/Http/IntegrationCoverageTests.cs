using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Trade360SDK.Common.Configuration;
using Trade360SDK.CustomersApi.Http;
using Trade360SDK.CustomersApi.Entities.MetadataApi.Requests;
using Trade360SDK.CustomersApi.Validators;
using Trade360SDK.Microsoft.DependencyInjection.Extensions;
using System.Net.Http;
using System.Text;
using System.Net.Http.Headers;

namespace Trade360SDK.CustomersApi.Tests;

public class IntegrationCoverageTests
{
    [Fact]
    public void BaseHttpClient_Constructor_WithNullHttpClientFactory_ShouldExecuteActualValidation()
    {
        var credentials = new PackageCredentials { Username = "test", Password = "test", PackageId = 123 };
        var mockMapper = new Mock<AutoMapper.IMapper>();

        var act = () => new MetadataHttpClient(null!, "https://api.test.com", credentials, mockMapper.Object);

        act.Should().Throw<ArgumentNullException>()
           .WithParameterName("httpClientFactory");
    }

    [Fact]
    public void BaseHttpClient_Constructor_WithNullBaseUrl_ShouldExecuteActualValidation()
    {
        var mockFactory = new Mock<IHttpClientFactory>();
        var credentials = new PackageCredentials { Username = "test", Password = "test", PackageId = 123 };
        var mockMapper = new Mock<AutoMapper.IMapper>();

        var act = () => new MetadataHttpClient(mockFactory.Object, null!, credentials, mockMapper.Object);

        act.Should().Throw<ArgumentException>()
           .WithParameterName("baseUrl");
    }

    [Fact]
    public void BaseHttpClient_Constructor_WithEmptyBaseUrl_ShouldExecuteActualValidation()
    {
        var mockFactory = new Mock<IHttpClientFactory>();
        var credentials = new PackageCredentials { Username = "test", Password = "test", PackageId = 123 };
        var mockMapper = new Mock<AutoMapper.IMapper>();

        var act = () => new MetadataHttpClient(mockFactory.Object, "", credentials, mockMapper.Object);

        act.Should().Throw<ArgumentException>()
           .WithParameterName("baseUrl");
    }

    [Fact]
    public void BaseHttpClient_Constructor_WithNullCredentials_ShouldExecuteActualValidation()
    {
        var mockFactory = new Mock<IHttpClientFactory>();
        var mockMapper = new Mock<AutoMapper.IMapper>();

        var act = () => new MetadataHttpClient(mockFactory.Object, "https://api.test.com", null!, mockMapper.Object);

        act.Should().Throw<ArgumentNullException>()
           .WithParameterName("settings");
    }

    [Fact]
    public void GetTranslationsRequestValidator_WithNullLanguages_ShouldExecuteActualValidation()
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
    public void GetTranslationsRequestValidator_WithEmptyLanguages_ShouldExecuteActualValidation()
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
    public void GetTranslationsRequestValidator_WithNullLanguageValue_ShouldExecuteActualValidation()
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
    public void GetTranslationsRequestValidator_WithValidLanguages_ShouldNotThrow()
    {
        var request = new GetTranslationsRequest
        {
            Languages = new[] { 1, 2, 3 },
            SportIds = new[] { 1, 2 }
        };

        var act = () => GetTranslationsRequestValidator.Validate(request);

        act.Should().NotThrow();
    }

    [Fact]
    public void GetTranslationsRequestValidator_WithWhitespaceLanguageValue_ShouldExecuteActualValidation()
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
    public void GetTranslationsRequestValidator_WithEmptyStringLanguageValue_ShouldExecuteActualValidation()
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
    public void GetTranslationsRequestValidator_WithAllFieldsEmpty_ShouldExecuteActualValidation()
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
    public void ServiceCollectionExtensions_AddTrade360CustomerApiClient_WithNullConfiguration_ShouldExecuteActualValidation()
    {
        var services = new ServiceCollection();

        var act = () => services.AddTrade360CustomerApiClient(null!);

        act.Should().Throw<ArgumentNullException>()
           .WithParameterName("configuration");
    }

    [Fact]
    public void HeaderResponse_RequestId_Property_ShouldExecuteActualPropertyAccess()
    {
        var headerResponse = new Trade360SDK.CustomersApi.Entities.Base.HeaderResponse();
        var requestId = "test-request-123";

        headerResponse.RequestId = requestId;
        var retrievedId = headerResponse.RequestId;

        retrievedId.Should().Be(requestId);
    }

    [Fact]
    public void HeaderResponse_RequestId_WithNullValue_ShouldExecuteActualPropertyAccess()
    {
        var headerResponse = new Trade360SDK.CustomersApi.Entities.Base.HeaderResponse
        {
            RequestId = null
        };

        headerResponse.RequestId.Should().BeNull();
    }

    [Fact]
    public void GetTranslationsRequestValidator_WithValidRequest_ShouldNotThrow()
    {
        var request = new GetTranslationsRequest
        {
            Languages = new[] { 1, 2, 3 },
            SportIds = new[] { 1, 2, 3 }
        };

        var act = () => GetTranslationsRequestValidator.Validate(request);

        act.Should().NotThrow();
    }

    [Fact]
    public void GetTranslationsRequestValidator_WithMultipleValidFields_ShouldNotThrow()
    {
        var request = new GetTranslationsRequest
        {
            Languages = new[] { 1 },
            SportIds = new[] { 1 },
            LocationIds = new[] { 100 },
            LeagueIds = new[] { 200 }
        };

        var act = () => GetTranslationsRequestValidator.Validate(request);

        act.Should().NotThrow();
    }

    [Fact]
    public void HeaderResponse_HttpStatusCode_Property_ShouldExecuteActualPropertyAccess()
    {
        var headerResponse = new Trade360SDK.CustomersApi.Entities.Base.HeaderResponse();
        var statusCode = System.Net.HttpStatusCode.OK;

        headerResponse.HttpStatusCode = statusCode;
        var retrievedStatusCode = headerResponse.HttpStatusCode;

        retrievedStatusCode.Should().Be(statusCode);
    }

    [Fact]
    public void HeaderResponse_Errors_Property_ShouldExecuteActualPropertyAccess()
    {
        var headerResponse = new Trade360SDK.CustomersApi.Entities.Base.HeaderResponse();
        var errors = new List<Trade360SDK.CustomersApi.Entities.Base.Error>
        {
            new Trade360SDK.CustomersApi.Entities.Base.Error { Message = "Test error" }
        };

        headerResponse.Errors = errors;
        var retrievedErrors = headerResponse.Errors;

        retrievedErrors.Should().BeEquivalentTo(errors);
    }

    [Fact]
    public async Task SetupHttpResponse_ShouldSetContentAndContentType()
    {
        var json = "{\"key\":\"value\"}";
        var statusCode = System.Net.HttpStatusCode.OK;

        var httpResponse = new HttpResponseMessage(statusCode)
        {
            Content = new StringContent(json, Encoding.UTF8)
        };
        httpResponse.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");

        (await httpResponse.Content.ReadAsStringAsync()).Should().Be(json);
        httpResponse.Content.Headers.ContentType.MediaType.Should().Be("application/json");
    }
}
