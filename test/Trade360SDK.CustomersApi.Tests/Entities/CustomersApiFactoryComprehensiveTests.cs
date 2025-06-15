using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using Trade360SDK.Common.Configuration;
using Trade360SDK.CustomersApi;
using Trade360SDK.CustomersApi.Interfaces;
using AutoMapper;

namespace Trade360SDK.CustomersApi.Tests;

public class CustomersApiFactoryComprehensiveTests
{
    private readonly Mock<IHttpClientFactory> _mockHttpClientFactory;
    private readonly CustomersApiFactory _factory;

    public CustomersApiFactoryComprehensiveTests()
    {
        var mockServiceProvider = new Mock<IServiceProvider>();
        _mockHttpClientFactory = new Mock<IHttpClientFactory>();

        mockServiceProvider.Setup(x => x.GetService(typeof(IHttpClientFactory))).Returns(_mockHttpClientFactory.Object);
        var mockMapper = new Mock<IMapper>();
        mockServiceProvider.Setup(x => x.GetService(typeof(IMapper))).Returns(mockMapper.Object);

        _factory = new CustomersApiFactory(mockServiceProvider.Object);
    }

    [Fact]
    public void Constructor_WithValidServiceProvider_ShouldInitializeCorrectly()
    {
        _factory.Should().NotBeNull();
    }

    [Fact]
    public void Constructor_WithNullServiceProvider_ShouldNotThrow()
    {
        var factory = new CustomersApiFactory(null);

        factory.Should().NotBeNull();
    }

    [Fact]
    public void CreateMetadataHttpClient_WithValidParameters_ShouldReturnClient()
    {
        var baseUrl = "https://api.example.com/";
        var credentials = new PackageCredentials
        {
            Username = "testuser",
            Password = "testpass",
            PackageId = 123
        };

        var httpClient = new HttpClient();
        _mockHttpClientFactory.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(httpClient);

        var result = _factory.CreateMetadataHttpClient(baseUrl, credentials);

        result.Should().NotBeNull();
        result.Should().BeOfType<MetadataHttpClient>();
    }

    [Fact]
    public void CreatePackageDistributionHttpClient_WithValidParameters_ShouldReturnClient()
    {
        var baseUrl = "https://api.example.com/";
        var credentials = new PackageCredentials
        {
            Username = "testuser",
            Password = "testpass",
            PackageId = 123
        };

        var httpClient = new HttpClient();
        _mockHttpClientFactory.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(httpClient);

        var result = _factory.CreatePackageDistributionHttpClient(baseUrl, credentials);

        result.Should().NotBeNull();
        result.Should().BeOfType<PackageDistributionHttpClient>();
    }

    [Fact]
    public void CreateSubscriptionHttpClient_WithValidParameters_ShouldReturnClient()
    {
        var baseUrl = "https://api.example.com/";
        var credentials = new PackageCredentials
        {
            Username = "testuser",
            Password = "testpass",
            PackageId = 123
        };

        var httpClient = new HttpClient();
        _mockHttpClientFactory.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(httpClient);

        var result = _factory.CreateSubscriptionHttpClient(baseUrl, credentials);

        result.Should().NotBeNull();
        result.Should().BeOfType<SubscriptionHttpClient>();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void CreateMetadataHttpClient_WithInvalidBaseUrl_ShouldThrowArgumentException(string invalidBaseUrl)
    {
        var credentials = new PackageCredentials
        {
            Username = "testuser",
            Password = "testpass",
            PackageId = 123
        };

        var act = () => _factory.CreateMetadataHttpClient(invalidBaseUrl, credentials);

        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void CreateMetadataHttpClient_WithNullCredentials_ShouldThrowArgumentNullException()
    {
        var baseUrl = "https://api.example.com/";

        var act = () => _factory.CreateMetadataHttpClient(baseUrl, null);

        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void CreatePackageDistributionHttpClient_WithNullCredentials_ShouldThrowArgumentNullException()
    {
        var baseUrl = "https://api.example.com/";

        var act = () => _factory.CreatePackageDistributionHttpClient(baseUrl, null);

        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void CreateSubscriptionHttpClient_WithNullCredentials_ShouldThrowArgumentNullException()
    {
        var baseUrl = "https://api.example.com/";

        var act = () => _factory.CreateSubscriptionHttpClient(baseUrl, null);

        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void CreateMetadataHttpClient_MultipleCalls_ShouldReturnDifferentInstances()
    {
        var baseUrl = "https://api.example.com/";
        var credentials = new PackageCredentials
        {
            Username = "testuser",
            Password = "testpass",
            PackageId = 123
        };

        var httpClient = new HttpClient();
        _mockHttpClientFactory.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(httpClient);

        var result1 = _factory.CreateMetadataHttpClient(baseUrl, credentials);
        var result2 = _factory.CreateMetadataHttpClient(baseUrl, credentials);

        result1.Should().NotBeSameAs(result2);
    }

    [Fact]
    public void CreatePackageDistributionHttpClient_MultipleCalls_ShouldReturnDifferentInstances()
    {
        var baseUrl = "https://api.example.com/";
        var credentials = new PackageCredentials
        {
            Username = "testuser",
            Password = "testpass",
            PackageId = 123
        };

        var httpClient = new HttpClient();
        _mockHttpClientFactory.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(httpClient);

        var result1 = _factory.CreatePackageDistributionHttpClient(baseUrl, credentials);
        var result2 = _factory.CreatePackageDistributionHttpClient(baseUrl, credentials);

        result1.Should().NotBeSameAs(result2);
    }

    [Fact]
    public void CreateSubscriptionHttpClient_MultipleCalls_ShouldReturnDifferentInstances()
    {
        var baseUrl = "https://api.example.com/";
        var credentials = new PackageCredentials
        {
            Username = "testuser",
            Password = "testpass",
            PackageId = 123
        };

        var httpClient = new HttpClient();
        _mockHttpClientFactory.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(httpClient);

        var result1 = _factory.CreateSubscriptionHttpClient(baseUrl, credentials);
        var result2 = _factory.CreateSubscriptionHttpClient(baseUrl, credentials);

        result1.Should().NotBeSameAs(result2);
    }
}
