using FluentAssertions;
using Moq;
using Trade360SDK.Common.Configuration;
using Trade360SDK.CustomersApi.Http;

namespace Trade360SDK.CustomersApi.Tests;

public class BaseHttpClientCoverageTests
{
    [Fact]
    public void Constructor_WithNullHttpClientFactory_ShouldExecuteValidation()
    {
        var credentials = new PackageCredentials { Username = "test", Password = "test", PackageId = 123 };
        var mockMapper = new Mock<AutoMapper.IMapper>();

        var act = () => new MetadataHttpClient(null!, "https://api.test.com", credentials, mockMapper.Object);

        act.Should().Throw<ArgumentNullException>()
           .WithParameterName("httpClientFactory");
    }

    [Fact]
    public void Constructor_WithNullBaseUrl_ShouldExecuteValidation()
    {
        var mockFactory = new Mock<IHttpClientFactory>();
        var credentials = new PackageCredentials { Username = "test", Password = "test", PackageId = 123 };
        var mockMapper = new Mock<AutoMapper.IMapper>();

        var act = () => new MetadataHttpClient(mockFactory.Object, null!, credentials, mockMapper.Object);

        act.Should().Throw<ArgumentException>()
           .WithParameterName("baseUrl");
    }

    [Fact]
    public void Constructor_WithNullCredentials_ShouldExecuteValidation()
    {
        var mockFactory = new Mock<IHttpClientFactory>();
        var mockMapper = new Mock<AutoMapper.IMapper>();

        var act = () => new MetadataHttpClient(mockFactory.Object, "https://api.test.com", null!, mockMapper.Object);

        act.Should().Throw<ArgumentNullException>()
           .WithParameterName("settings");
    }

    [Fact]
    public void Constructor_WithEmptyBaseUrl_ShouldExecuteValidation()
    {
        var mockFactory = new Mock<IHttpClientFactory>();
        var credentials = new PackageCredentials { Username = "test", Password = "test", PackageId = 123 };
        var mockMapper = new Mock<AutoMapper.IMapper>();

        var act = () => new MetadataHttpClient(mockFactory.Object, "", credentials, mockMapper.Object);

        act.Should().Throw<ArgumentException>()
           .WithParameterName("baseUrl");
    }

    [Fact]
    public void Constructor_WithNullMapper_ShouldExecuteValidation()
    {
        var mockFactory = new Mock<IHttpClientFactory>();
        var credentials = new PackageCredentials { Username = "test", Password = "test", PackageId = 123 };

        var act = () => new MetadataHttpClient(mockFactory.Object, "https://api.test.com", credentials, null!);

        act.Should().Throw<ArgumentNullException>()
           .WithParameterName("mapper");
    }
}
