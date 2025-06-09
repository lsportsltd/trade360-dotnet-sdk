using AutoMapper;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System.Net.Http;
using Trade360SDK.Common.Configuration;
using Trade360SDK.CustomersApi;
using Trade360SDK.CustomersApi.Interfaces;

namespace Trade360SDK.CustomersApi.Tests;

public class CustomersApiFactoryTests
{
    private readonly Mock<IServiceProvider> _mockServiceProvider;

    public CustomersApiFactoryTests()
    {
        _mockServiceProvider = new Mock<IServiceProvider>();
        var mockMapper = new Mock<IMapper>();
        var mockHttpClientFactory = new Mock<IHttpClientFactory>();
        var mockHttpClient = new Mock<HttpClient>();

        _mockServiceProvider.Setup(sp => sp.GetService(typeof(IMapper)))
                           .Returns(mockMapper.Object);
        _mockServiceProvider.Setup(sp => sp.GetService(typeof(IHttpClientFactory)))
                           .Returns(mockHttpClientFactory.Object);

        mockHttpClientFactory.Setup(f => f.CreateClient(It.IsAny<string>()))
                             .Returns(mockHttpClient.Object);
    }

    [Fact]
    public void Constructor_ShouldInitializeCorrectly()
    {
        var factory = new CustomersApiFactory(_mockServiceProvider.Object);

        factory.Should().NotBeNull();
    }

    [Fact]
    public void Constructor_WithNullServiceProvider_ShouldNotThrow()
    {
        var factory = new CustomersApiFactory(null!);

        factory.Should().NotBeNull();
    }

    [Fact]
    public void CreateMetadataHttpClient_ShouldReturnMetadataHttpClient()
    {
        var factory = new CustomersApiFactory(_mockServiceProvider.Object);
        var baseUrl = "https://api.test.com";
        var credentials = new PackageCredentials
        {
            PackageId = 1,
            Username = "user",
            Password = "pass"
        };

        var result = factory.CreateMetadataHttpClient(baseUrl, credentials);

        result.Should().NotBeNull();
        result.Should().BeOfType<MetadataHttpClient>();
        _mockServiceProvider.Verify(sp => sp.GetService(typeof(IMapper)), Times.Once);
        _mockServiceProvider.Verify(sp => sp.GetService(typeof(IHttpClientFactory)), Times.Once);
    }

    [Fact]
    public void CreateMetadataHttpClient_WithNullCredentials_ShouldThrowNullReferenceException()
    {
        var factory = new CustomersApiFactory(_mockServiceProvider.Object);
        var baseUrl = "https://api.test.com";

        Action act = () => factory.CreateMetadataHttpClient(baseUrl, null);

        act.Should().Throw<NullReferenceException>();
    }

    [Fact]
    public void CreatePackageDistributionHttpClient_ShouldReturnPackageDistributionHttpClient()
    {
        var factory = new CustomersApiFactory(_mockServiceProvider.Object);
        var baseUrl = "https://api.test.com";
        var credentials = new PackageCredentials
        {
            PackageId = 1,
            Username = "user",
            Password = "pass"
        };

        var result = factory.CreatePackageDistributionHttpClient(baseUrl, credentials);

        result.Should().NotBeNull();
        result.Should().BeOfType<PackageDistributionHttpClient>();
        _mockServiceProvider.Verify(sp => sp.GetService(typeof(IHttpClientFactory)), Times.Once);
    }

    [Fact]
    public void CreateSubscriptionHttpClient_ShouldReturnSubscriptionHttpClient()
    {
        var factory = new CustomersApiFactory(_mockServiceProvider.Object);
        var baseUrl = "https://api.test.com";
        var credentials = new PackageCredentials
        {
            PackageId = 1,
            Username = "user",
            Password = "pass"
        };

        var result = factory.CreateSubscriptionHttpClient(baseUrl, credentials);

        result.Should().NotBeNull();
        result.Should().BeOfType<SubscriptionHttpClient>();
        _mockServiceProvider.Verify(sp => sp.GetService(typeof(IMapper)), Times.Once);
        _mockServiceProvider.Verify(sp => sp.GetService(typeof(IHttpClientFactory)), Times.Once);
    }

    [Fact]
    public void CreateMetadataHttpClient_WithMissingMapper_ShouldThrowInvalidOperationException()
    {
        _mockServiceProvider.Setup(sp => sp.GetService(typeof(IMapper)))
                           .Returns((IMapper?)null);

        var factory = new CustomersApiFactory(_mockServiceProvider.Object);

        Action act = () => factory.CreateMetadataHttpClient("https://api.test.com", null);

        act.Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void CreateMetadataHttpClient_WithMissingHttpClientFactory_ShouldThrowInvalidOperationException()
    {
        _mockServiceProvider.Setup(sp => sp.GetService(typeof(IHttpClientFactory)))
                           .Returns((IHttpClientFactory?)null);

        var factory = new CustomersApiFactory(_mockServiceProvider.Object);

        Action act = () => factory.CreateMetadataHttpClient("https://api.test.com", null);

        act.Should().Throw<InvalidOperationException>();
    }
}
