using FluentAssertions;
using Moq;
using Trade360SDK.Common.Configuration;
using Trade360SDK.CustomersApi.Http;
using AutoMapper;

namespace Trade360SDK.CustomersApi.Tests;

public class BaseHttpClientValidationTests
{
    [Fact]
    public void Constructor_WithNullHttpClientFactory_ShouldThrowArgumentNullException()
    {
        var credentials = new PackageCredentials { Username = "test", Password = "test", PackageId = 123 };
        var mockMapper = new Mock<IMapper>();

        var act = () => new MetadataHttpClient(null!, "https://api.test.com", credentials, mockMapper.Object);

        act.Should().Throw<ArgumentNullException>()
           .WithParameterName("httpClientFactory");
    }

    [Fact]
    public void Constructor_WithNullBaseUrl_ShouldThrowArgumentException()
    {
        var mockFactory = new Mock<IHttpClientFactory>();
        var credentials = new PackageCredentials { Username = "test", Password = "test", PackageId = 123 };
        var mockMapper = new Mock<IMapper>();

        var act = () => new MetadataHttpClient(mockFactory.Object, null!, credentials, mockMapper.Object);

        act.Should().Throw<ArgumentException>()
           .WithParameterName("baseUrl");
    }

    [Fact]
    public void Constructor_WithEmptyBaseUrl_ShouldThrowArgumentException()
    {
        var mockFactory = new Mock<IHttpClientFactory>();
        var credentials = new PackageCredentials { Username = "test", Password = "test", PackageId = 123 };
        var mockMapper = new Mock<IMapper>();

        var act = () => new MetadataHttpClient(mockFactory.Object, "", credentials, mockMapper.Object);

        act.Should().Throw<ArgumentException>()
           .WithParameterName("baseUrl");
    }

    [Fact]
    public void Constructor_WithWhitespaceBaseUrl_ShouldThrowArgumentException()
    {
        var mockFactory = new Mock<IHttpClientFactory>();
        var credentials = new PackageCredentials { Username = "test", Password = "test", PackageId = 123 };
        var mockMapper = new Mock<IMapper>();

        var act = () => new MetadataHttpClient(mockFactory.Object, "   ", credentials, mockMapper.Object);

        act.Should().Throw<ArgumentException>()
           .WithParameterName("baseUrl");
    }

    [Fact]
    public void Constructor_WithNullCredentials_ShouldThrowArgumentNullException()
    {
        var mockFactory = new Mock<IHttpClientFactory>();
        mockFactory.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(new HttpClient());
        var mockMapper = new Mock<IMapper>();

        var act = () => new MetadataHttpClient(mockFactory.Object, "https://api.test.com", null!, mockMapper.Object);

        act.Should().Throw<ArgumentNullException>()
           .WithParameterName("settings");
    }

    [Fact]
    public void Constructor_WithNullMapper_ShouldThrowArgumentNullException()
    {
        var mockFactory = new Mock<IHttpClientFactory>();
        mockFactory.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(new HttpClient());
        var credentials = new PackageCredentials { Username = "test", Password = "test", PackageId = 123 };

        var act = () => new MetadataHttpClient(mockFactory.Object, "https://api.test.com", credentials, null!);

        act.Should().Throw<ArgumentNullException>()
           .WithParameterName("mapper");
    }
}
