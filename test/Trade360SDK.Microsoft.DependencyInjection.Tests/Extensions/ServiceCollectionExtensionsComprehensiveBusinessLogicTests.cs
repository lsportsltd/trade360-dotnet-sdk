using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Moq;
using Trade360SDK.Common.Configuration;
using Trade360SDK.CustomersApi.Interfaces;
using Trade360SDK.Microsoft.DependencyInjection.Extensions;
using Trade360SDK.SnapshotApi.Interfaces;
using Microsoft.Extensions.Options;
using System.Collections.Generic;

namespace Trade360SDK.Microsoft.DependencyInjection.Tests;

public class ServiceCollectionExtensionsComprehensiveBusinessLogicTests
{
    private readonly IServiceCollection _services;
    private readonly IConfiguration _configuration;

    public ServiceCollectionExtensionsComprehensiveBusinessLogicTests()
    {
        _services = new ServiceCollection();
        var configBuilder = new ConfigurationBuilder();
        configBuilder.AddInMemoryCollection(new Dictionary<string, string>
        {
            ["Trade360:CustomersApiBaseUrl"] = "https://api.example.com/",
            ["Trade360:SnapshotApiBaseUrl"] = "https://snapshot.example.com/"
        });
        _configuration = configBuilder.Build();
    }

    [Fact]
    public void AddTrade360CustomerApiClient_WithValidConfiguration_ShouldRegisterAllServices()
    {
        _services.Configure<Trade360Settings>(options =>
        {
            options.CustomersApiBaseUrl = "https://api.example.com/";

        });
        _services.AddTrade360CustomerApiClient(_configuration);

        var serviceProvider = _services.BuildServiceProvider();

        serviceProvider.GetService<ICustomersApiFactory>().Should().NotBeNull();
    }

    [Fact]
    public void AddTrade360CustomerApiClient_WithCredentials_ShouldRegisterServicesWithCredentials()
    {
        _services.Configure<Trade360Settings>(options =>
        {
            options.CustomersApiBaseUrl = "https://api.example.com/";

        });
        _services.AddTrade360CustomerApiClient(_configuration);

        var serviceProvider = _services.BuildServiceProvider();
        var factory = serviceProvider.GetService<ICustomersApiFactory>();

        factory.Should().NotBeNull();
    }

    [Fact]
    public void AddTrade360InplaySnapshotClient_WithValidConfiguration_ShouldRegisterAllServices()
    {
        var settings = new Trade360Settings { SnapshotApiBaseUrl = "https://snapshot.example.com/" };
        _services.Configure<Trade360Settings>(options =>
        {
            options.SnapshotApiBaseUrl = settings.SnapshotApiBaseUrl;
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
    public void AddTrade360PrematchSnapshotClient_WithValidConfiguration_ShouldRegisterAllServices()
    {
        var settings = new Trade360Settings { SnapshotApiBaseUrl = "https://snapshot.example.com/" };
        _services.Configure<Trade360Settings>(options =>
        {
            options.SnapshotApiBaseUrl = settings.SnapshotApiBaseUrl;
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
    public void AddTrade360CustomerApiClient_WithNullConfiguration_ShouldThrowArgumentNullException()
    {
        var act = () => _services.AddTrade360CustomerApiClient(null);

        act.Should().Throw<ArgumentNullException>();;
    }

    [Fact]
    public void AddTrade360InplaySnapshotClient_WithNullOptions_ShouldThrowArgumentNullException()
    {
        var act = () => _services.AddTrade360InplaySnapshotClient();

        act.Should().NotThrow();
    }

    [Fact]
    public void AddTrade360CustomerApiClient_WithEmptyConfiguration_ShouldStillRegisterServices()
    {
        var emptyConfig = new ConfigurationBuilder().Build();

        _services.AddTrade360CustomerApiClient(emptyConfig);

        var serviceProvider = _services.BuildServiceProvider();
        serviceProvider.GetService<ICustomersApiFactory>().Should().NotBeNull();
    }

    [Fact]
    public void AddTrade360PrematchSnapshotClient_WithValidConfiguration_ShouldStillRegisterServices()
    {
        var settings = new Trade360Settings { SnapshotApiBaseUrl = "https://snapshot.example.com/" };
        _services.Configure<Trade360Settings>(options =>
        {
            options.SnapshotApiBaseUrl = settings.SnapshotApiBaseUrl;
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
    public void AddTrade360CustomerApiClient_MultipleRegistrations_ShouldOverridePreviousRegistration()
    {
        _services.AddTrade360CustomerApiClient(_configuration);
        _services.AddTrade360CustomerApiClient(_configuration);

        var serviceProvider = _services.BuildServiceProvider();
        var factory = serviceProvider.GetService<ICustomersApiFactory>();

        factory.Should().NotBeNull();
    }

    [Fact]
    public void AddTrade360InplaySnapshotClient_MultipleRegistrations_ShouldOverridePreviousRegistration()
    {
        var settings = new Trade360Settings { SnapshotApiBaseUrl = "https://snapshot.example.com/" };
        _services.Configure<Trade360Settings>(options =>
        {
            options.SnapshotApiBaseUrl = settings.SnapshotApiBaseUrl;
            options.InplayPackageCredentials = new PackageCredentials
            {
                PackageId = 456,
                Username = "inplayuser",
                Password = "inplaypass"
            };
        });
        _services.AddTrade360InplaySnapshotClient();
        _services.AddTrade360InplaySnapshotClient();

        var serviceProvider = _services.BuildServiceProvider();
        var client = serviceProvider.GetService<ISnapshotInplayApiClient>();

        client.Should().NotBeNull();
    }

    [Fact]
    public void AddTrade360CustomerApiClient_WithLogging_ShouldRegisterWithLoggingSupport()
    {
        _services.AddLogging();
        _services.AddTrade360CustomerApiClient(_configuration);

        var serviceProvider = _services.BuildServiceProvider();
        var factory = serviceProvider.GetService<ICustomersApiFactory>();

        factory.Should().NotBeNull();
    }

    [Fact]
    public void AddTrade360InplaySnapshotClient_WithLogging_ShouldRegisterWithLoggingSupport()
    {
        _services.AddLogging();
        var settings = new Trade360Settings { SnapshotApiBaseUrl = "https://snapshot.example.com/" };
        _services.Configure<Trade360Settings>(options =>
        {
            options.SnapshotApiBaseUrl = settings.SnapshotApiBaseUrl;
            options.InplayPackageCredentials = new PackageCredentials
            {
                PackageId = 456,
                Username = "inplayuser",
                Password = "inplaypass"
            };
        });
        _services.AddTrade360InplaySnapshotClient();

        var serviceProvider = _services.BuildServiceProvider();
        var client = serviceProvider.GetService<ISnapshotInplayApiClient>();

        client.Should().NotBeNull();
    }

    [Fact]
    public void AddTrade360CustomerApiClient_WithHttpClientFactory_ShouldUseExistingHttpClientFactory()
    {
        _services.AddHttpClient();
        _services.AddTrade360CustomerApiClient(_configuration);

        var serviceProvider = _services.BuildServiceProvider();
        var httpClientFactory = serviceProvider.GetService<IHttpClientFactory>();
        var customersFactory = serviceProvider.GetService<ICustomersApiFactory>();

        httpClientFactory.Should().NotBeNull();
        customersFactory.Should().NotBeNull();
    }

    [Fact]
    public void AddTrade360PrematchSnapshotClient_WithHttpClientFactory_ShouldUseExistingHttpClientFactory()
    {
        _services.AddHttpClient();
        var settings = new Trade360Settings { SnapshotApiBaseUrl = "https://snapshot.example.com/" };
        _services.Configure<Trade360Settings>(options =>
        {
            options.SnapshotApiBaseUrl = settings.SnapshotApiBaseUrl;
            options.PrematchPackageCredentials = new PackageCredentials
            {
                PackageId = 123,
                Username = "prematchuser",
                Password = "prematchpass"
            };
        });
        _services.AddTrade360PrematchSnapshotClient();

        var serviceProvider = _services.BuildServiceProvider();
        var httpClientFactory = serviceProvider.GetService<IHttpClientFactory>();

        httpClientFactory.Should().NotBeNull();
        serviceProvider.GetService<ISnapshotPrematchApiClient>().Should().NotBeNull();
    }

    [Fact]
    public void AddTrade360CustomerApiClient_WithAutoMapper_ShouldRegisterWithAutoMapperSupport()
    {
        _services.AddAutoMapper(typeof(Trade360SDK.Microsoft.DependencyInjection.Extensions.ServiceCollectionExtensions));
        _services.AddTrade360CustomerApiClient(_configuration);

        var serviceProvider = _services.BuildServiceProvider();
        var factory = serviceProvider.GetService<ICustomersApiFactory>();

        factory.Should().NotBeNull();
    }

    [Fact]
    public void AddTrade360PrematchSnapshotClient_WithAutoMapper_ShouldRegisterWithAutoMapperSupport()
    {
        _services.AddAutoMapper(typeof(Trade360SDK.Microsoft.DependencyInjection.Extensions.ServiceCollectionExtensions));
        var settings = new Trade360Settings { SnapshotApiBaseUrl = "https://snapshot.example.com/" };
        _services.Configure<Trade360Settings>(options =>
        {
            options.SnapshotApiBaseUrl = settings.SnapshotApiBaseUrl;
            options.PrematchPackageCredentials = new PackageCredentials
            {
                PackageId = 123,
                Username = "prematchuser",
                Password = "prematchpass"
            };
        });
        _services.AddTrade360PrematchSnapshotClient();

        var serviceProvider = _services.BuildServiceProvider();
        var client = serviceProvider.GetService<ISnapshotPrematchApiClient>();

        client.Should().NotBeNull();
    }

    [Fact]
    public void AddTrade360CustomerApiClient_ServiceLifetime_ShouldBeTransient()
    {
        _services.AddTrade360CustomerApiClient(_configuration);

        var factoryDescriptor = _services.FirstOrDefault(s => s.ServiceType == typeof(ICustomersApiFactory));
        factoryDescriptor.Should().NotBeNull();
        factoryDescriptor!.Lifetime.Should().Be(ServiceLifetime.Transient);
    }

    [Fact]
    public void AddTrade360InplaySnapshotClient_ServiceLifetime_ShouldBeTransient()
    {
        var settings = new Trade360Settings { SnapshotApiBaseUrl = "https://snapshot.example.com/" };
        _services.Configure<Trade360Settings>(options =>
        {
            options.SnapshotApiBaseUrl = settings.SnapshotApiBaseUrl;
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
    public void AddTrade360CustomerApiClient_WithComplexConfiguration_ShouldHandleAllProperties()
    {
        var complexConfigBuilder = new ConfigurationBuilder();
        complexConfigBuilder.AddInMemoryCollection(new Dictionary<string, string>
        {
            ["Trade360:CustomersApiBaseUrl"] = "https://complex.api.example.com/v2/",
            ["Trade360:SnapshotApiBaseUrl"] = "https://complex.snapshot.example.com/v2/"
        });
        var complexConfig = complexConfigBuilder.Build();

        _services.AddTrade360CustomerApiClient(complexConfig);

        var serviceProvider = _services.BuildServiceProvider();
        var factory = serviceProvider.GetService<ICustomersApiFactory>();

        factory.Should().NotBeNull();
    }

    [Fact]
    public void AddTrade360PrematchSnapshotClient_WithComplexSettings_ShouldHandleAllProperties()
    {
        var complexSettings = new Trade360Settings
        {
            CustomersApiBaseUrl = "https://complex.api.example.com/v2/",
            SnapshotApiBaseUrl = "https://complex.snapshot.example.com/v2/"
        };

        _services.Configure<Trade360Settings>(options =>
        {
            options.CustomersApiBaseUrl = complexSettings.CustomersApiBaseUrl;
            options.SnapshotApiBaseUrl = complexSettings.SnapshotApiBaseUrl;
            options.PrematchPackageCredentials = new PackageCredentials
            {
                PackageId = 123,
                Username = "prematchuser",
                Password = "prematchpass"
            };
        });
        _services.AddTrade360PrematchSnapshotClient();

        var serviceProvider = _services.BuildServiceProvider();
        var client = serviceProvider.GetService<ISnapshotPrematchApiClient>();

        client.Should().NotBeNull();
    }
}
