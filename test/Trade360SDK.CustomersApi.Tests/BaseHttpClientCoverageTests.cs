using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.Protected;
using System.Net;
using System.Text;
using System.Text.Json;
using Trade360SDK.Common.Configuration;
using Trade360SDK.Common.Models;
using Trade360SDK.CustomersApi.Http;
using Trade360SDK.CustomersApi.Entities.MetadataApi.Requests;

namespace Trade360SDK.CustomersApi.Tests;

public class BaseHttpClientCoverageTests
{
    [Fact]
    public void Constructor_WithNullHttpClientFactory_ShouldThrowArgumentNullException()
    {
        var settings = new Trade360Settings { CustomersApiBaseUrl = "https://api.test.com" };
        var credentials = new PackageCredentials { Username = "test", Password = "test", PackageId = 123 };

        var act = () => new TestableBaseHttpClient(null!, "https://api.test.com", credentials);

        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void Constructor_WithNullBaseUrl_ShouldThrowArgumentException()
    {
        var mockFactory = new Mock<IHttpClientFactory>();
        var credentials = new PackageCredentials { Username = "test", Password = "test", PackageId = 123 };

        var act = () => new TestableBaseHttpClient(mockFactory.Object, null!, credentials);

        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void Constructor_WithNullCredentials_ShouldThrowArgumentNullException()
    {
        var mockFactory = new Mock<IHttpClientFactory>();

        var act = () => new TestableBaseHttpClient(mockFactory.Object, "https://api.test.com", null!);

        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void BuildQueryString_WithNullRequest_ShouldReturnEmptyString()
    {
        var mockFactory = new Mock<IHttpClientFactory>();
        var mockHandler = new Mock<HttpMessageHandler>();
        var httpClient = new HttpClient(mockHandler.Object);
        mockFactory.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(httpClient);
        
        var credentials = new PackageCredentials { Username = "test", Password = "test", PackageId = 123 };
        var client = new TestableBaseHttpClient(mockFactory.Object, "https://api.test.com", credentials);

        var result = client.TestBuildQueryString(null);

        result.Should().Be(string.Empty);
    }

    [Fact]
    public void BuildQueryString_WithValidParameters_ShouldBuildCorrectQueryString()
    {
        var mockFactory = new Mock<IHttpClientFactory>();
        var mockHandler = new Mock<HttpMessageHandler>();
        var httpClient = new HttpClient(mockHandler.Object);
        mockFactory.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(httpClient);
        
        var credentials = new PackageCredentials { Username = "test", Password = "test", PackageId = 123 };
        var client = new TestableBaseHttpClient(mockFactory.Object, "https://api.test.com", credentials);

        var parameters = new Dictionary<string, object>
        {
            { "SportIds", new[] { 1, 2 } },
            { "Languages", new[] { "en", "es" } }
        };

        var result = client.TestBuildQueryString(parameters);

        result.Should().NotBeEmpty();
        result.Should().Contain("SportIds");
        result.Should().Contain("Languages");
    }
}
