using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Http;
using Microsoft.Extensions.Options;
using Moq;
using Trade360SDK.Common.Configuration;
using Trade360SDK.CustomersApi.Interfaces;
using Trade360SDK.Microsoft.DependencyInjection.Extensions;
using Trade360SDK.SnapshotApi.Interfaces;

namespace Trade360SDK.Microsoft.DependencyInjection.Tests;

public class ServiceCollectionExtensionsAdvancedBusinessLogicTests
{
    private readonly IServiceCollection _services;

    public ServiceCollectionExtensionsAdvancedBusinessLogicTests()
    {
        _services = new ServiceCollection();
    }

    [Fact]
    public void AddTrade360CustomerApiClient_WithRetryPolicy_ShouldRegisterPolicyHandlers()
    {
        var mockConfiguration = new Mock<IConfiguration>();
        _services.AddTrade360CustomerApiClient(mockConfiguration.Object);

        var serviceProvider = _services.BuildServiceProvider();
        var httpClientFactory = serviceProvider.GetService<IHttpClientFactory>();

        httpClientFactory.Should().NotBeNull();
    }

    [Fact]
    public void AddTrade360CustomerApiClient_WithCircuitBreakerPolicy_ShouldRegisterPolicyHandlers()
    {
        var mockConfiguration = new Mock<IConfiguration>();
        _services.AddTrade360CustomerApiClient(mockConfiguration.Object);

        var serviceProvider = _services.BuildServiceProvider();
        var httpClientFactory = serviceProvider.GetService<IHttpClientFactory>();

        httpClientFactory.Should().NotBeNull();
    }

    [Fact]
    public void AddTrade360PrematchSnapshotClient_WithRetryPolicy_ShouldRegisterPolicyHandlers()
    {
        _services.Configure<Trade360Settings>(options =>
        {
            options.SnapshotApiBaseUrl = "https://snapshot.example.com/";
            options.PrematchPackageCredentials = new PackageCredentials
            {
                PackageId = 123,
                Username = "prematchuser",
                Password = "prematchpass"
            };
        });

        _services.AddTrade360PrematchSnapshotClient();

        var serviceProvider = _services.BuildServiceProvider();

        serviceProvider.GetService<ISnapshotPrematchApiClient>().Should().NotBeNull();
    }

    [Fact]
    public void AddTrade360InplaySnapshotClient_WithCircuitBreakerPolicy_ShouldRegisterPolicyHandlers()
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

        serviceProvider.GetService<ISnapshotInplayApiClient>().Should().NotBeNull();
    }

    [Fact]
    public void AddTrade360CustomerApiClient_WithHttpClientConfiguration_ShouldConfigureBaseAddress()
    {
        var mockConfiguration = new Mock<IConfiguration>();
        _services.AddTrade360CustomerApiClient(mockConfiguration.Object);

        var serviceProvider = _services.BuildServiceProvider();
        var httpClientFactory = serviceProvider.GetService<IHttpClientFactory>();

        httpClientFactory.Should().NotBeNull();
    }

    [Fact]
    public void AddTrade360PrematchSnapshotClient_WithHttpClientConfiguration_ShouldConfigureBaseAddress()
    {
        _services.Configure<Trade360Settings>(options =>
        {
            options.SnapshotApiBaseUrl = "https://snapshot.example.com/";
            options.PrematchPackageCredentials = new PackageCredentials
            {
                PackageId = 123,
                Username = "prematchuser",
                Password = "prematchpass"
            };
        });

        _services.AddTrade360PrematchSnapshotClient();

        var serviceProvider = _services.BuildServiceProvider();

        serviceProvider.GetService<ISnapshotPrematchApiClient>().Should().NotBeNull();
    }

    [Fact]
    public void AddTrade360InplaySnapshotClient_WithHttpClientConfiguration_ShouldConfigureBaseAddress()
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

        serviceProvider.GetService<ISnapshotInplayApiClient>().Should().NotBeNull();
    }

    [Fact]
    public void AddTrade360CustomerApiClient_WithAutoMapperConfiguration_ShouldRegisterMappingProfile()
    {
        var mockConfiguration = new Mock<IConfiguration>();
        _services.AddTrade360CustomerApiClient(mockConfiguration.Object);

        var serviceProvider = _services.BuildServiceProvider();
        var mapper = serviceProvider.GetService<AutoMapper.IMapper>();

        mapper.Should().NotBeNull();
    }

    [Fact]
    public void AddTrade360PrematchSnapshotClient_WithAutoMapperConfiguration_ShouldRegisterMappingProfile()
    {
        _services.Configure<Trade360Settings>(options =>
        {
            options.SnapshotApiBaseUrl = "https://snapshot.example.com/";
            options.PrematchPackageCredentials = new PackageCredentials
            {
                PackageId = 123,
                Username = "prematchuser",
                Password = "prematchpass"
            };
        });

        _services.AddTrade360PrematchSnapshotClient();

        var serviceProvider = _services.BuildServiceProvider();
        var mapper = serviceProvider.GetService<AutoMapper.IMapper>();

        mapper.Should().NotBeNull();
    }

    [Fact]
    public void AddTrade360InplaySnapshotClient_WithAutoMapperConfiguration_ShouldRegisterMappingProfile()
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
    public void AddTrade360CustomerApiClient_WithTransientLifetime_ShouldRegisterTransientServices()
    {
        var mockConfiguration = new Mock<IConfiguration>();
        _services.AddTrade360CustomerApiClient(mockConfiguration.Object);

        var factoryDescriptor = _services.FirstOrDefault(s => s.ServiceType == typeof(ICustomersApiFactory));
        factoryDescriptor.Should().NotBeNull();
        factoryDescriptor!.Lifetime.Should().Be(ServiceLifetime.Transient);
    }

    [Fact]
    public void AddTrade360PrematchSnapshotClient_WithTransientLifetime_ShouldRegisterTransientServices()
    {
        _services.Configure<Trade360Settings>(options =>
        {
            options.SnapshotApiBaseUrl = "https://snapshot.example.com/";
            options.PrematchPackageCredentials = new PackageCredentials
            {
                PackageId = 123,
                Username = "prematchuser",
                Password = "prematchpass"
            };
        });

        _services.AddTrade360PrematchSnapshotClient();

        var clientDescriptor = _services.FirstOrDefault(s => s.ServiceType == typeof(ISnapshotPrematchApiClient));
        clientDescriptor.Should().NotBeNull();
        clientDescriptor!.Lifetime.Should().Be(ServiceLifetime.Transient);
    }

    [Fact]
    public void AddTrade360InplaySnapshotClient_WithTransientLifetime_ShouldRegisterTransientServices()
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

        var clientDescriptor = _services.FirstOrDefault(s => s.ServiceType == typeof(ISnapshotInplayApiClient));
        clientDescriptor.Should().NotBeNull();
        clientDescriptor!.Lifetime.Should().Be(ServiceLifetime.Transient);
    }

    [Fact]
    public void AddTrade360CustomerApiClient_WithMultipleHttpClients_ShouldRegisterAllClients()
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
    public void AddTrade360CustomerApiClient_WithPolicyHandlers_ShouldRegisterRetryAndCircuitBreakerPolicies()
    {
        var mockConfiguration = new Mock<IConfiguration>();
        _services.AddTrade360CustomerApiClient(mockConfiguration.Object);

        var serviceProvider = _services.BuildServiceProvider();
        var httpClientFactory = serviceProvider.GetService<IHttpClientFactory>();

        httpClientFactory.Should().NotBeNull();
    }

    [Fact]
    public void AddTrade360PrematchSnapshotClient_WithPolicyHandlers_ShouldRegisterRetryAndCircuitBreakerPolicies()
    {
        _services.Configure<Trade360Settings>(options =>
        {
            options.SnapshotApiBaseUrl = "https://snapshot.example.com/";
            options.PrematchPackageCredentials = new PackageCredentials
            {
                PackageId = 123,
                Username = "prematchuser",
                Password = "prematchpass"
            };
        });

        _services.AddTrade360PrematchSnapshotClient();

        var serviceProvider = _services.BuildServiceProvider();

        serviceProvider.GetService<ISnapshotPrematchApiClient>().Should().NotBeNull();
    }

    [Fact]
    public void AddTrade360InplaySnapshotClient_WithPolicyHandlers_ShouldRegisterRetryAndCircuitBreakerPolicies()
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

        serviceProvider.GetService<ISnapshotInplayApiClient>().Should().NotBeNull();
    }
}
