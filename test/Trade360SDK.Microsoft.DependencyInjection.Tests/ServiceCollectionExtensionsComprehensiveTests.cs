using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Http;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Configuration;
using Moq;
using Trade360SDK.Common.Configuration;
using Trade360SDK.Microsoft.DependencyInjection.Extensions;
using Trade360SDK.CustomersApi.Interfaces;
using Trade360SDK.SnapshotApi.Interfaces;

namespace Trade360SDK.Microsoft.DependencyInjection.Tests;

public class ServiceCollectionExtensionsComprehensiveTests
{
    private readonly IServiceCollection _services;
    private readonly IConfiguration _configuration;
    private readonly Mock<IConfiguration> _mockConfiguration;

    public ServiceCollectionExtensionsComprehensiveTests()
    {
        _services = new ServiceCollection();
        _mockConfiguration = new Mock<IConfiguration>();
        _configuration = _mockConfiguration.Object;
    }

    [Fact]
    public void AddTrade360CustomerApiClient_WithValidConfiguration_ShouldRegisterServices()
    {
        _services.AddTrade360CustomerApiClient(_configuration);

        var serviceProvider = _services.BuildServiceProvider();
        var httpClientFactory = serviceProvider.GetService<IHttpClientFactory>();
        var customersApiFactory = serviceProvider.GetService<ICustomersApiFactory>();

        httpClientFactory.Should().NotBeNull();
        customersApiFactory.Should().NotBeNull();
    }

    [Fact]
    public void AddTrade360CustomerApiClient_WithNullConfiguration_ShouldThrowArgumentNullException()
    {
        var act = () => _services.AddTrade360CustomerApiClient(null);

        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void AddTrade360PrematchSnapshotClient_ShouldRegisterServices()
    {
        _services.Configure<Trade360Settings>(options =>
        {
            options.SnapshotApiBaseUrl = "https://snapshot.example.com/";
            options.PrematchPackageCredentials = new PackageCredentials
            {
                PackageId = 123,
                Username = "testuser",
                Password = "testpass"
            };
        });

        _services.AddTrade360PrematchSnapshotClient();

        var serviceProvider = _services.BuildServiceProvider();
        var snapshotClient = serviceProvider.GetService<ISnapshotPrematchApiClient>();

        snapshotClient.Should().NotBeNull();
    }

    [Fact]
    public void AddTrade360InplaySnapshotClient_ShouldRegisterServices()
    {
        _services.Configure<Trade360Settings>(options =>
        {
            options.SnapshotApiBaseUrl = "https://snapshot.example.com/";
            options.InplayPackageCredentials = new PackageCredentials
            {
                PackageId = 456,
                Username = "inplayuser",
                Password = "inplaypass"
            };
        });

        _services.AddTrade360InplaySnapshotClient();

        var serviceProvider = _services.BuildServiceProvider();
        var snapshotClient = serviceProvider.GetService<ISnapshotInplayApiClient>();

        snapshotClient.Should().NotBeNull();
    }

    [Fact]
    public void AddTrade360CustomerApiClient_ShouldRegisterHttpClientsWithPolicies()
    {
        _services.AddTrade360CustomerApiClient(_configuration);

        var serviceProvider = _services.BuildServiceProvider();
        var httpClientFactory = serviceProvider.GetService<IHttpClientFactory>();

        httpClientFactory.Should().NotBeNull();
    }

    [Fact]
    public void AddTrade360CustomerApiClient_ShouldRegisterAutoMapper()
    {
        _services.AddTrade360CustomerApiClient(_configuration);

        var serviceProvider = _services.BuildServiceProvider();
        var mapper = serviceProvider.GetService<AutoMapper.IMapper>();

        mapper.Should().NotBeNull();
    }

    [Fact]
    public void AddTrade360PrematchSnapshotClient_WithNullSnapshotApiBaseUrl_ShouldThrowInvalidOperationException()
    {
        _services.Configure<Trade360Settings>(options =>
        {
            options.SnapshotApiBaseUrl = null;
            options.PrematchPackageCredentials = new PackageCredentials
            {
                PackageId = 123,
                Username = "testuser",
                Password = "testpass"
            };
        });

        var act = () =>
        {
            _services.AddTrade360PrematchSnapshotClient();
            var serviceProvider = _services.BuildServiceProvider();
            var snapshotClient = serviceProvider.GetService<ISnapshotPrematchApiClient>();
        };

        act.Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void AddTrade360InplaySnapshotClient_WithNullSnapshotApiBaseUrl_ShouldThrowInvalidOperationException()
    {
        _services.Configure<Trade360Settings>(options =>
        {
            options.SnapshotApiBaseUrl = null;
            options.InplayPackageCredentials = new PackageCredentials
            {
                PackageId = 456,
                Username = "inplayuser",
                Password = "inplaypass"
            };
        });

        var act = () =>
        {
            _services.AddTrade360InplaySnapshotClient();
            var serviceProvider = _services.BuildServiceProvider();
            var snapshotClient = serviceProvider.GetService<ISnapshotInplayApiClient>();
        };

        act.Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void AddTrade360PrematchSnapshotClient_ShouldRegisterAutoMapper()
    {
        _services.Configure<Trade360Settings>(options =>
        {
            options.SnapshotApiBaseUrl = "https://snapshot.example.com/";
            options.PrematchPackageCredentials = new PackageCredentials
            {
                PackageId = 123,
                Username = "testuser",
                Password = "testpass"
            };
        });

        _services.AddTrade360PrematchSnapshotClient();

        var serviceProvider = _services.BuildServiceProvider();
        var mapper = serviceProvider.GetService<AutoMapper.IMapper>();

        mapper.Should().NotBeNull();
    }

    [Fact]
    public void AddTrade360InplaySnapshotClient_ShouldRegisterAutoMapper()
    {
        _services.Configure<Trade360Settings>(options =>
        {
            options.SnapshotApiBaseUrl = "https://snapshot.example.com/";
            options.InplayPackageCredentials = new PackageCredentials
            {
                PackageId = 456,
                Username = "inplayuser",
                Password = "inplaypass"
            };
        });

        _services.AddTrade360InplaySnapshotClient();

        var serviceProvider = _services.BuildServiceProvider();
        var mapper = serviceProvider.GetService<AutoMapper.IMapper>();

        mapper.Should().NotBeNull();
    }

    [Fact]
    public void AddTrade360CustomerApiClient_ShouldRegisterMultipleHttpClients()
    {
        _services.AddTrade360CustomerApiClient(_configuration);

        var serviceProvider = _services.BuildServiceProvider();
        var httpClientFactory = serviceProvider.GetService<IHttpClientFactory>();

        httpClientFactory.Should().NotBeNull();
        
        var metadataClient = httpClientFactory.CreateClient("Trade360SDK.CustomersApi.MetadataHttpClient");
        var packageDistributionClient = httpClientFactory.CreateClient("Trade360SDK.CustomersApi.PackageDistributionHttpClient");
        var subscriptionClient = httpClientFactory.CreateClient("Trade360SDK.CustomersApi.SubscriptionHttpClient");

        metadataClient.Should().NotBeNull();
        packageDistributionClient.Should().NotBeNull();
        subscriptionClient.Should().NotBeNull();
    }

    [Fact]
    public void AddTrade360PrematchSnapshotClient_WithValidSettings_ShouldConfigureHttpClient()
    {
        _services.Configure<Trade360Settings>(options =>
        {
            options.SnapshotApiBaseUrl = "https://snapshot.example.com/";
            options.PrematchPackageCredentials = new PackageCredentials
            {
                PackageId = 123,
                Username = "testuser",
                Password = "testpass"
            };
        });

        _services.AddTrade360PrematchSnapshotClient();

        var serviceProvider = _services.BuildServiceProvider();
        var snapshotClient = serviceProvider.GetService<ISnapshotPrematchApiClient>();

        snapshotClient.Should().NotBeNull();
    }

    [Fact]
    public void AddTrade360InplaySnapshotClient_WithValidSettings_ShouldConfigureHttpClient()
    {
        _services.Configure<Trade360Settings>(options =>
        {
            options.SnapshotApiBaseUrl = "https://snapshot.example.com/";
            options.InplayPackageCredentials = new PackageCredentials
            {
                PackageId = 456,
                Username = "inplayuser",
                Password = "inplaypass"
            };
        });

        _services.AddTrade360InplaySnapshotClient();

        var serviceProvider = _services.BuildServiceProvider();
        var snapshotClient = serviceProvider.GetService<ISnapshotInplayApiClient>();

        snapshotClient.Should().NotBeNull();
    }

    [Fact]
    public void AddTrade360CustomerApiClient_ShouldRegisterTransientServices()
    {
        _services.AddTrade360CustomerApiClient(_configuration);

        var serviceProvider = _services.BuildServiceProvider();
        
        var factory1 = serviceProvider.GetService<ICustomersApiFactory>();
        var factory2 = serviceProvider.GetService<ICustomersApiFactory>();

        factory1.Should().NotBeNull();
        factory2.Should().NotBeNull();
        factory1.Should().NotBeSameAs(factory2);
    }

    [Fact]
    public void AddTrade360PrematchSnapshotClient_ShouldRegisterTransientServices()
    {
        _services.Configure<Trade360Settings>(options =>
        {
            options.SnapshotApiBaseUrl = "https://snapshot.example.com/";
            options.PrematchPackageCredentials = new PackageCredentials
            {
                PackageId = 123,
                Username = "testuser",
                Password = "testpass"
            };
        });

        _services.AddTrade360PrematchSnapshotClient();

        var serviceProvider = _services.BuildServiceProvider();
        
        var client1 = serviceProvider.GetService<ISnapshotPrematchApiClient>();
        var client2 = serviceProvider.GetService<ISnapshotPrematchApiClient>();

        client1.Should().NotBeNull();
        client2.Should().NotBeNull();
        client1.Should().NotBeSameAs(client2);
    }

    [Fact]
    public void AddTrade360InplaySnapshotClient_ShouldRegisterTransientServices()
    {
        _services.Configure<Trade360Settings>(options =>
        {
            options.SnapshotApiBaseUrl = "https://snapshot.example.com/";
            options.InplayPackageCredentials = new PackageCredentials
            {
                PackageId = 456,
                Username = "inplayuser",
                Password = "inplaypass"
            };
        });

        _services.AddTrade360InplaySnapshotClient();

        var serviceProvider = _services.BuildServiceProvider();
        
        var client1 = serviceProvider.GetService<ISnapshotInplayApiClient>();
        var client2 = serviceProvider.GetService<ISnapshotInplayApiClient>();

        client1.Should().NotBeNull();
        client2.Should().NotBeNull();
        client1.Should().NotBeSameAs(client2);
    }
}
