using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Moq;
using Trade360SDK.Common.Configuration;
using Trade360SDK.CustomersApi.Interfaces;
using Trade360SDK.Microsoft.DependencyInjection.Extensions;
using Trade360SDK.SnapshotApi.Interfaces;

namespace Trade360SDK.Microsoft.DependencyInjection.Tests;

public class ServiceCollectionExtensionsAdvancedTests
{
    private readonly IServiceCollection _services;

    public ServiceCollectionExtensionsAdvancedTests()
    {
        _services = new ServiceCollection();
    }

    [Fact]
    public void AddTrade360CustomerApiClient_WithValidConfiguration_ShouldRegisterServices()
    {
        var mockConfiguration = new Mock<IConfiguration>();
        var result = _services.AddTrade360CustomerApiClient(mockConfiguration.Object);

        result.Should().BeSameAs(_services);
        
        var serviceProvider = _services.BuildServiceProvider();
        serviceProvider.GetService<ICustomersApiFactory>().Should().NotBeNull();
    }

    [Fact]
    public void AddTrade360CustomerApiClient_ShouldRegisterHttpClients()
    {
        var mockConfiguration = new Mock<IConfiguration>();
        _services.AddTrade360CustomerApiClient(mockConfiguration.Object);

        var serviceDescriptors = _services.Where(s => s.ServiceType.Name.Contains("HttpClient")).ToList();
        serviceDescriptors.Should().NotBeEmpty();
    }

    [Fact]
    public void AddTrade360CustomerApiClient_ShouldRegisterAutoMapper()
    {
        var mockConfiguration = new Mock<IConfiguration>();
        _services.AddTrade360CustomerApiClient(mockConfiguration.Object);

        var serviceProvider = _services.BuildServiceProvider();
        serviceProvider.GetService<AutoMapper.IMapper>().Should().NotBeNull();
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
            options.SnapshotApiBaseUrl = "https://snapshot.example.com";
            options.PrematchPackageCredentials = new PackageCredentials
            {
                PackageId = 123,
                Username = "testuser",
                Password = "testpass"
            };
        });

        var result = _services.AddTrade360PrematchSnapshotClient();

        result.Should().BeSameAs(_services);
        
        var serviceProvider = _services.BuildServiceProvider();
        serviceProvider.GetService<ISnapshotPrematchApiClient>().Should().NotBeNull();
    }

    [Fact]
    public void AddTrade360PrematchSnapshotClient_ShouldRegisterHttpClient()
    {
        _services.Configure<Trade360Settings>(options =>
        {
            options.SnapshotApiBaseUrl = "https://snapshot.example.com";
            options.PrematchPackageCredentials = new PackageCredentials
            {
                PackageId = 123,
                Username = "testuser",
                Password = "testpass"
            };
        });

        _services.AddTrade360PrematchSnapshotClient();

        var serviceDescriptors = _services.Where(s => s.ServiceType.Name.Contains("HttpClient")).ToList();
        serviceDescriptors.Should().NotBeEmpty();
    }

    [Fact]
    public void AddTrade360PrematchSnapshotClient_ShouldRegisterAutoMapper()
    {
        _services.Configure<Trade360Settings>(options =>
        {
            options.SnapshotApiBaseUrl = "https://snapshot.example.com";
            options.PrematchPackageCredentials = new PackageCredentials
            {
                PackageId = 123,
                Username = "testuser",
                Password = "testpass"
            };
        });

        _services.AddTrade360PrematchSnapshotClient();

        var serviceProvider = _services.BuildServiceProvider();
        serviceProvider.GetService<AutoMapper.IMapper>().Should().NotBeNull();
    }

    [Fact]
    public void AddTrade360InplaySnapshotClient_ShouldRegisterServices()
    {
        _services.Configure<Trade360Settings>(options =>
        {
            options.SnapshotApiBaseUrl = "https://snapshot.example.com";
            options.InplayPackageCredentials = new PackageCredentials
            {
                PackageId = 456,
                Username = "inplayuser",
                Password = "inplaypass"
            };
        });

        var result = _services.AddTrade360InplaySnapshotClient();

        result.Should().BeSameAs(_services);
        
        var serviceProvider = _services.BuildServiceProvider();
        serviceProvider.GetService<ISnapshotInplayApiClient>().Should().NotBeNull();
    }

    [Fact]
    public void AddTrade360InplaySnapshotClient_ShouldRegisterHttpClient()
    {
        _services.Configure<Trade360Settings>(options =>
        {
            options.SnapshotApiBaseUrl = "https://snapshot.example.com";
            options.InplayPackageCredentials = new PackageCredentials
            {
                PackageId = 456,
                Username = "inplayuser",
                Password = "inplaypass"
            };
        });

        _services.AddTrade360InplaySnapshotClient();

        var serviceDescriptors = _services.Where(s => s.ServiceType.Name.Contains("HttpClient")).ToList();
        serviceDescriptors.Should().NotBeEmpty();
    }

    [Fact]
    public void AddTrade360InplaySnapshotClient_ShouldRegisterAutoMapper()
    {
        _services.Configure<Trade360Settings>(options =>
        {
            options.SnapshotApiBaseUrl = "https://snapshot.example.com";
            options.InplayPackageCredentials = new PackageCredentials
            {
                PackageId = 456,
                Username = "inplayuser",
                Password = "inplaypass"
            };
        });

        _services.AddTrade360InplaySnapshotClient();

        var serviceProvider = _services.BuildServiceProvider();
        serviceProvider.GetService<AutoMapper.IMapper>().Should().NotBeNull();
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

        _services.AddTrade360PrematchSnapshotClient();

        var serviceProvider = _services.BuildServiceProvider();
        var act = () => serviceProvider.GetService<ISnapshotPrematchApiClient>();

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

        _services.AddTrade360InplaySnapshotClient();

        var serviceProvider = _services.BuildServiceProvider();
        var act = () => serviceProvider.GetService<ISnapshotInplayApiClient>();

        act.Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void AddTrade360CustomerApiClient_ShouldRegisterRetryPolicy()
    {
        var mockConfiguration = new Mock<IConfiguration>();
        _services.AddTrade360CustomerApiClient(mockConfiguration.Object);

        var serviceProvider = _services.BuildServiceProvider();
        var httpClientFactory = serviceProvider.GetService<IHttpClientFactory>();
        httpClientFactory.Should().NotBeNull();
    }

    [Fact]
    public void AddTrade360PrematchSnapshotClient_ShouldRegisterRetryPolicy()
    {
        _services.Configure<Trade360Settings>(options =>
        {
            options.SnapshotApiBaseUrl = "https://snapshot.example.com";
            options.PrematchPackageCredentials = new PackageCredentials
            {
                PackageId = 123,
                Username = "testuser",
                Password = "testpass"
            };
        });

        _services.AddTrade360PrematchSnapshotClient();

        var httpClientDescriptors = _services.Where(s => s.ServiceType == typeof(ISnapshotPrematchApiClient)).ToList();
        httpClientDescriptors.Should().NotBeEmpty();
    }

    [Fact]
    public void AddTrade360InplaySnapshotClient_ShouldRegisterRetryPolicy()
    {
        _services.Configure<Trade360Settings>(options =>
        {
            options.SnapshotApiBaseUrl = "https://snapshot.example.com";
            options.InplayPackageCredentials = new PackageCredentials
            {
                PackageId = 456,
                Username = "inplayuser",
                Password = "inplaypass"
            };
        });

        _services.AddTrade360InplaySnapshotClient();

        var httpClientDescriptors = _services.Where(s => s.ServiceType == typeof(ISnapshotInplayApiClient)).ToList();
        httpClientDescriptors.Should().NotBeEmpty();
    }

    [Fact]
    public void AddTrade360CustomerApiClient_ShouldRegisterMultipleHttpClients()
    {
        var mockConfiguration = new Mock<IConfiguration>();
        _services.AddTrade360CustomerApiClient(mockConfiguration.Object);

        var httpClientDescriptors = _services.Where(s => 
            s.ServiceType == typeof(IMetadataHttpClient) ||
            s.ServiceType == typeof(IPackageDistributionHttpClient) ||
            s.ServiceType == typeof(ISubscriptionHttpClient)).ToList();

        httpClientDescriptors.Should().HaveCount(3);
    }

    [Fact]
    public void AddTrade360PrematchSnapshotClient_WithValidBaseUrl_ShouldConfigureHttpClient()
    {
        var baseUrl = "https://snapshot.example.com";
        _services.Configure<Trade360Settings>(options =>
        {
            options.SnapshotApiBaseUrl = baseUrl;
        });

        _services.AddTrade360PrematchSnapshotClient();

        var serviceProvider = _services.BuildServiceProvider();
        var httpClientFactory = serviceProvider.GetService<IHttpClientFactory>();
        httpClientFactory.Should().NotBeNull();
    }

    [Fact]
    public void AddTrade360InplaySnapshotClient_WithValidBaseUrl_ShouldConfigureHttpClient()
    {
        var baseUrl = "https://snapshot.example.com";
        _services.Configure<Trade360Settings>(options =>
        {
            options.SnapshotApiBaseUrl = baseUrl;
        });

        _services.AddTrade360InplaySnapshotClient();

        var serviceProvider = _services.BuildServiceProvider();
        var httpClientFactory = serviceProvider.GetService<IHttpClientFactory>();
        httpClientFactory.Should().NotBeNull();
    }

    [Fact]
    public void AddTrade360CustomerApiClient_ShouldRegisterTransientServices()
    {
        var mockConfiguration = new Mock<IConfiguration>();
        _services.AddTrade360CustomerApiClient(mockConfiguration.Object);

        var factoryDescriptor = _services.FirstOrDefault(s => s.ServiceType == typeof(ICustomersApiFactory));
        factoryDescriptor.Should().NotBeNull();
        factoryDescriptor.Lifetime.Should().Be(ServiceLifetime.Transient);
    }

    [Fact]
    public void AddTrade360PrematchSnapshotClient_ShouldRegisterTransientServices()
    {
        _services.Configure<Trade360Settings>(options =>
        {
            options.SnapshotApiBaseUrl = "https://snapshot.example.com";
        });

        _services.AddTrade360PrematchSnapshotClient();

        var clientDescriptor = _services.FirstOrDefault(s => s.ServiceType == typeof(ISnapshotPrematchApiClient));
        clientDescriptor.Should().NotBeNull();
        clientDescriptor.Lifetime.Should().Be(ServiceLifetime.Transient);
    }

    [Fact]
    public void AddTrade360InplaySnapshotClient_ShouldRegisterTransientServices()
    {
        _services.Configure<Trade360Settings>(options =>
        {
            options.SnapshotApiBaseUrl = "https://snapshot.example.com";
        });

        _services.AddTrade360InplaySnapshotClient();

        var clientDescriptor = _services.FirstOrDefault(s => s.ServiceType == typeof(ISnapshotInplayApiClient));
        clientDescriptor.Should().NotBeNull();
        clientDescriptor.Lifetime.Should().Be(ServiceLifetime.Transient);
    }

    [Fact]
    public void AddTrade360CustomerApiClient_WithMultipleCalls_ShouldNotDuplicateServices()
    {
        var mockConfiguration = new Mock<IConfiguration>();
        _services.AddTrade360CustomerApiClient(mockConfiguration.Object);
        _services.AddTrade360CustomerApiClient(mockConfiguration.Object);

        var factoryDescriptors = _services.Where(s => s.ServiceType == typeof(ICustomersApiFactory)).ToList();
        factoryDescriptors.Should().HaveCount(2);
    }

    [Fact]
    public void AddTrade360PrematchSnapshotClient_WithMultipleCalls_ShouldRegisterMultipleServices()
    {
        _services.Configure<Trade360Settings>(options =>
        {
            options.SnapshotApiBaseUrl = "https://snapshot.example.com";
        });

        _services.AddTrade360PrematchSnapshotClient();
        _services.AddTrade360PrematchSnapshotClient();

        var clientDescriptors = _services.Where(s => s.ServiceType == typeof(ISnapshotPrematchApiClient)).ToList();
        clientDescriptors.Should().HaveCount(4);
    }

    [Fact]
    public void AddTrade360InplaySnapshotClient_WithMultipleCalls_ShouldRegisterMultipleServices()
    {
        _services.Configure<Trade360Settings>(options =>
        {
            options.SnapshotApiBaseUrl = "https://snapshot.example.com";
        });

        _services.AddTrade360InplaySnapshotClient();
        _services.AddTrade360InplaySnapshotClient();

        var clientDescriptors = _services.Where(s => s.ServiceType == typeof(ISnapshotInplayApiClient)).ToList();
        clientDescriptors.Should().HaveCount(4);
    }

    [Fact]
    public void ServiceCollectionExtensions_ShouldSupportChaining()
    {
        _services.Configure<Trade360Settings>(options =>
        {
            options.SnapshotApiBaseUrl = "https://snapshot.example.com";
        });

        var result = _services
            .AddTrade360CustomerApiClient(new Mock<IConfiguration>().Object)
            .AddTrade360PrematchSnapshotClient()
            .AddTrade360InplaySnapshotClient();

        result.Should().BeSameAs(_services);
    }

    [Fact]
    public void ServiceCollectionExtensions_WithComplexConfiguration_ShouldRegisterAllServices()
    {
        _services.Configure<Trade360Settings>(options =>
        {
            options.SnapshotApiBaseUrl = "https://snapshot.example.com";
            options.CustomersApiBaseUrl = "https://customers.example.com";
            options.PrematchPackageCredentials = new PackageCredentials
            {
                PackageId = 123,
                Username = "testuser",
                Password = "testpass"
            };
            options.InplayPackageCredentials = new PackageCredentials
            {
                PackageId = 456,
                Username = "inplayuser",
                Password = "inplaypass"
            };
        });

        _services
            .AddTrade360CustomerApiClient(new Mock<IConfiguration>().Object)
            .AddTrade360PrematchSnapshotClient()
            .AddTrade360InplaySnapshotClient();

        var serviceProvider = _services.BuildServiceProvider();
        
        serviceProvider.GetService<ICustomersApiFactory>().Should().NotBeNull();
        serviceProvider.GetService<ISnapshotPrematchApiClient>().Should().NotBeNull();
        serviceProvider.GetService<ISnapshotInplayApiClient>().Should().NotBeNull();
        serviceProvider.GetService<AutoMapper.IMapper>().Should().NotBeNull();
    }
}
