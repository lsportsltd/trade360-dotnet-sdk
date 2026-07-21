using FluentAssertions;
using Moq;
using Trade360SDK.Common.Configuration;
using Trade360SDK.CustomersApi.Http;

namespace Trade360SDK.CustomersApi.Tests;

public class BaseHttpClientValidationTests
{
    [Fact]
    public void Constructor_WithNullHttpClientFactory_ShouldThrowArgumentNullException()
    {
        var credentials = new PackageCredentials { Username = "test", Password = "test", PackageId = 123 };

        var act = () => new MetadataHttpClient(null!, "https://api.test.com", credentials);

        act.Should().Throw<ArgumentNullException>()
           .WithParameterName("httpClientFactory");
    }

    [Fact]
    public void Constructor_WithNullBaseUrl_ShouldThrowArgumentException()
    {
        var mockFactory = new Mock<IHttpClientFactory>();
        var credentials = new PackageCredentials { Username = "test", Password = "test", PackageId = 123 };

        var act = () => new MetadataHttpClient(mockFactory.Object, null!, credentials);

        act.Should().Throw<ArgumentException>()
           .WithParameterName("baseUrl");
    }

    [Fact]
    public void Constructor_WithEmptyBaseUrl_ShouldThrowArgumentException()
    {
        var mockFactory = new Mock<IHttpClientFactory>();
        var credentials = new PackageCredentials { Username = "test", Password = "test", PackageId = 123 };

        var act = () => new MetadataHttpClient(mockFactory.Object, "", credentials);

        act.Should().Throw<ArgumentException>()
           .WithParameterName("baseUrl");
    }

    [Fact]
    public void Constructor_WithWhitespaceBaseUrl_ShouldThrowArgumentException()
    {
        var mockFactory = new Mock<IHttpClientFactory>();
        var credentials = new PackageCredentials { Username = "test", Password = "test", PackageId = 123 };

        var act = () => new MetadataHttpClient(mockFactory.Object, "   ", credentials);

        act.Should().Throw<ArgumentException>()
           .WithParameterName("baseUrl");
    }

    [Fact]
    public void Constructor_WithNullCredentials_ShouldThrowArgumentNullException()
    {
        var mockFactory = new Mock<IHttpClientFactory>();
        mockFactory.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(new HttpClient());

        var act = () => new MetadataHttpClient(mockFactory.Object, "https://api.test.com", null!);

        act.Should().Throw<ArgumentNullException>()
           .WithParameterName("settings");
    }
}
