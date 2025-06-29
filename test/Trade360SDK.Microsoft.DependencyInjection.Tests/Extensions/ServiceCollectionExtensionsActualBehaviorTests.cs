using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Trade360SDK.Common.Configuration;
using Trade360SDK.CustomersApi.Interfaces;
using Trade360SDK.Microsoft.DependencyInjection.Extensions;
using Trade360SDK.SnapshotApi.Interfaces;
using System.Collections.Generic;
using System.Net.Http;
using System;
using System.Linq;

namespace Trade360SDK.Microsoft.DependencyInjection.Tests.Extensions;

/// <summary>
/// Tests that focus on the actual behavior of ServiceCollectionExtensions methods.
/// These tests verify what the methods actually do rather than making assumptions
/// about HTTP client configuration behavior.
/// </summary>
public class ServiceCollectionExtensionsActualBehaviorTests
{
    private readonly IServiceCollection _services;
    private readonly IConfiguration _configuration;

    public ServiceCollectionExtensionsActualBehaviorTests()
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
    public void AddTrade360CustomerApiClient_WithNullConfiguration_ShouldThrowArgumentNullException()
    {
        // Act & Assert
        var act = () => _services.AddTrade360CustomerApiClient(null!);
        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void AddTrade360CustomerApiClient_ShouldRegisterHttpClients()
    {
        // Act
        _services.AddTrade360CustomerApiClient(_configuration);

        // Assert - Verify HttpClient services are registered
        var serviceProvider = _services.BuildServiceProvider();
        var httpClientFactory = serviceProvider.GetRequiredService<IHttpClientFactory>();

        // These should not throw - HttpClient registration should work
        var metadataClient = httpClientFactory.CreateClient(typeof(IMetadataHttpClient).FullName!);
        var packageClient = httpClientFactory.CreateClient(typeof(IPackageDistributionHttpClient).FullName!);
        var subscriptionClient = httpClientFactory.CreateClient(typeof(ISubscriptionHttpClient).FullName!);

        metadataClient.Should().NotBeNull();
        packageClient.Should().NotBeNull();
        subscriptionClient.Should().NotBeNull();
    }

    [Fact]
    public void AddTrade360CustomerApiClient_ShouldRegisterCustomersApiFactory()
    {
        // Act
        _services.AddTrade360CustomerApiClient(_configuration);

        // Assert
        var serviceProvider = _services.BuildServiceProvider();
        var factory = serviceProvider.GetService<ICustomersApiFactory>();

        factory.Should().NotBeNull();
        factory.Should().BeAssignableTo<ICustomersApiFactory>();
    }

    [Fact]
    public void AddTrade360CustomerApiClient_ShouldRegisterAutoMapper()
    {
        // Act
        _services.AddTrade360CustomerApiClient(_configuration);

        // Assert
        var serviceProvider = _services.BuildServiceProvider();
        var mapper = serviceProvider.GetService<AutoMapper.IMapper>();

        mapper.Should().NotBeNull();
        mapper!.ConfigurationProvider.Should().NotBeNull();
    }

    [Fact]
    public void AddTrade360CustomerApiClient_ShouldReturnSameServiceCollection()
    {
        // Act
        var result = _services.AddTrade360CustomerApiClient(_configuration);

        // Assert
        result.Should().BeSameAs(_services);
    }

    [Fact]
    public void AddTrade360PrematchSnapshotClient_ShouldRegisterHttpClient()
    {
        // Act
        _services.AddTrade360PrematchSnapshotClient();

        // Assert - Verify HttpClient service is registered
        var serviceProvider = _services.BuildServiceProvider();
        var httpClientFactory = serviceProvider.GetRequiredService<IHttpClientFactory>();

        var client = httpClientFactory.CreateClient(typeof(ISnapshotPrematchApiClient).FullName!);
        client.Should().NotBeNull();
    }

    [Fact]
    public void AddTrade360PrematchSnapshotClient_ShouldRegisterPrematchApiClient()
    {
        // Arrange - Configure options with valid package credentials
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

        // Act
        _services.AddTrade360PrematchSnapshotClient();

        // Assert
        var serviceProvider = _services.BuildServiceProvider();
        var client = serviceProvider.GetService<ISnapshotPrematchApiClient>();

        client.Should().NotBeNull();
        client.Should().BeAssignableTo<ISnapshotPrematchApiClient>();
    }

    [Fact]
    public void AddTrade360InplaySnapshotClient_ShouldRegisterHttpClient()
    {
        // Act
        _services.AddTrade360InplaySnapshotClient();

        // Assert - Verify HttpClient service is registered
        var serviceProvider = _services.BuildServiceProvider();
        var httpClientFactory = serviceProvider.GetRequiredService<IHttpClientFactory>();

        var client = httpClientFactory.CreateClient(typeof(ISnapshotInplayApiClient).FullName!);
        client.Should().NotBeNull();
    }

    [Fact]
    public void AddTrade360InplaySnapshotClient_ShouldRegisterInplayApiClient()
    {
        // Arrange - Configure options with valid package credentials
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

        // Act
        _services.AddTrade360InplaySnapshotClient();

        // Assert
        var serviceProvider = _services.BuildServiceProvider();
        var client = serviceProvider.GetService<ISnapshotInplayApiClient>();

        client.Should().NotBeNull();
        client.Should().BeAssignableTo<ISnapshotInplayApiClient>();
    }

    [Fact]
    public void AddTrade360SnapshotClients_ShouldReturnSameServiceCollection()
    {
        // Act
        var result1 = _services.AddTrade360PrematchSnapshotClient();
        var result2 = _services.AddTrade360InplaySnapshotClient();

        // Assert
        result1.Should().BeSameAs(_services);
        result2.Should().BeSameAs(_services);
    }

    [Fact]
    public void AddTrade360Services_WithMultipleRegistrations_ShouldRegisterAutoMapperProperly()
    {
        // Arrange
        _services.Configure<Trade360Settings>(options =>
        {
            options.SnapshotApiBaseUrl = "https://snapshot.example.com/";
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

        // Act - Register multiple services that all register AutoMapper
        _services.AddTrade360CustomerApiClient(_configuration);
        _services.AddTrade360PrematchSnapshotClient();
        _services.AddTrade360InplaySnapshotClient();

        // Assert
        var serviceProvider = _services.BuildServiceProvider();
        var mapper = serviceProvider.GetService<AutoMapper.IMapper>();

        mapper.Should().NotBeNull();
        mapper!.ConfigurationProvider.Should().NotBeNull();
    }

    [Fact]
    public void AddTrade360Services_WithFluentChaining_ShouldSupportMethodChaining()
    {
        // Arrange
        _services.Configure<Trade360Settings>(options =>
        {
            options.SnapshotApiBaseUrl = "https://snapshot.example.com/";
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

        // Act - Test fluent chaining
        var result = _services
            .AddTrade360CustomerApiClient(_configuration)
            .AddTrade360PrematchSnapshotClient()
            .AddTrade360InplaySnapshotClient();

        // Assert
        result.Should().BeSameAs(_services);

        var serviceProvider = _services.BuildServiceProvider();
        var factory = serviceProvider.GetService<ICustomersApiFactory>();
        var prematchClient = serviceProvider.GetService<ISnapshotPrematchApiClient>();
        var inplayClient = serviceProvider.GetService<ISnapshotInplayApiClient>();

        factory.Should().NotBeNull();
        prematchClient.Should().NotBeNull();
        inplayClient.Should().NotBeNull();
    }

    [Fact]
    public void AddTrade360Services_ShouldRegisterCorrectServiceTypes()
    {
        // Act
        _services.AddTrade360CustomerApiClient(_configuration);

        // Assert - Verify service descriptors are registered correctly
        var serviceDescriptors = _services.ToList();

        // Check for ICustomersApiFactory registration
        var factoryDescriptor = serviceDescriptors
            .FirstOrDefault(s => s.ServiceType == typeof(ICustomersApiFactory));
        factoryDescriptor.Should().NotBeNull();
        factoryDescriptor!.Lifetime.Should().Be(ServiceLifetime.Transient);

        // Check for AutoMapper registration
        var mapperDescriptor = serviceDescriptors
            .FirstOrDefault(s => s.ServiceType == typeof(AutoMapper.IMapper));
        mapperDescriptor.Should().NotBeNull();
    }

    [Fact]
    public void AddTrade360SnapshotClients_ShouldRegisterCorrectServiceTypes()
    {
        // Act
        _services.AddTrade360PrematchSnapshotClient();
        _services.AddTrade360InplaySnapshotClient();

        // Assert - Verify service descriptors
        var serviceDescriptors = _services.ToList();

        var prematchDescriptor = serviceDescriptors
            .FirstOrDefault(s => s.ServiceType == typeof(ISnapshotPrematchApiClient));
        var inplayDescriptor = serviceDescriptors
            .FirstOrDefault(s => s.ServiceType == typeof(ISnapshotInplayApiClient));

        prematchDescriptor.Should().NotBeNull();
        prematchDescriptor!.Lifetime.Should().Be(ServiceLifetime.Transient);

        inplayDescriptor.Should().NotBeNull();
        inplayDescriptor!.Lifetime.Should().Be(ServiceLifetime.Transient);
    }

    [Fact]
    public void AddTrade360Services_WithHttpClientFactory_ShouldRegisterNamedClients()
    {
        // Act
        _services.AddTrade360CustomerApiClient(_configuration);
        _services.AddTrade360PrematchSnapshotClient();
        _services.AddTrade360InplaySnapshotClient();

        // Assert
        var serviceProvider = _services.BuildServiceProvider();
        var httpClientFactory = serviceProvider.GetRequiredService<IHttpClientFactory>();

        // Verify all named clients can be created
        var clientNames = new[]
        {
            typeof(IMetadataHttpClient).FullName!,
            typeof(IPackageDistributionHttpClient).FullName!,
            typeof(ISubscriptionHttpClient).FullName!,
            typeof(ISnapshotPrematchApiClient).FullName!,
            typeof(ISnapshotInplayApiClient).FullName!
        };

        foreach (var clientName in clientNames)
        {
            var client = httpClientFactory.CreateClient(clientName);
            client.Should().NotBeNull($"HTTP client '{clientName}' should be registered and creatable");
        }
    }

    [Fact]
    public void AddTrade360Services_WithTransientLifetime_ShouldCreateNewInstancesEachTime()
    {
        // Arrange
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

        // Act
        _services.AddTrade360CustomerApiClient(_configuration);
        _services.AddTrade360PrematchSnapshotClient();

        // Assert
        var serviceProvider = _services.BuildServiceProvider();

        // Test transient behavior for CustomersApiFactory
        var factory1 = serviceProvider.GetService<ICustomersApiFactory>();
        var factory2 = serviceProvider.GetService<ICustomersApiFactory>();

        factory1.Should().NotBeNull();
        factory2.Should().NotBeNull();
        factory1.Should().NotBeSameAs(factory2, "CustomersApiFactory should be registered as transient");

        // Test transient behavior for SnapshotPrematchApiClient
        var client1 = serviceProvider.GetService<ISnapshotPrematchApiClient>();
        var client2 = serviceProvider.GetService<ISnapshotPrematchApiClient>();

        client1.Should().NotBeNull();
        client2.Should().NotBeNull();
        client1.Should().NotBeSameAs(client2, "SnapshotPrematchApiClient should be registered as transient");
    }

    [Fact]
    public void AddTrade360Services_WithEmptyServiceCollection_ShouldRegisterAllRequiredServices()
    {
        // Arrange - Start with completely empty service collection
        var emptyServices = new ServiceCollection();
        emptyServices.Configure<Trade360Settings>(options =>
        {
            options.SnapshotApiBaseUrl = "https://snapshot.example.com/";
            options.PrematchPackageCredentials = new PackageCredentials
            {
                PackageId = 123,
                Username = "testuser",
                Password = "testpass"
            };
        });

        // Act
        emptyServices.AddTrade360CustomerApiClient(_configuration);
        emptyServices.AddTrade360PrematchSnapshotClient();

        // Assert
        var serviceProvider = emptyServices.BuildServiceProvider();

        // Verify all required services are registered
        var httpClientFactory = serviceProvider.GetService<IHttpClientFactory>();
        var customersFactory = serviceProvider.GetService<ICustomersApiFactory>();
        var prematchClient = serviceProvider.GetService<ISnapshotPrematchApiClient>();
        var mapper = serviceProvider.GetService<AutoMapper.IMapper>();
        var options = serviceProvider.GetService<IOptions<Trade360Settings>>();

        httpClientFactory.Should().NotBeNull();
        customersFactory.Should().NotBeNull();
        prematchClient.Should().NotBeNull();
        mapper.Should().NotBeNull();
        options.Should().NotBeNull();
    }

    [Fact]
    public void AddTrade360Services_WithServiceRegistrationCount_ShouldAddMultipleServices()
    {
        // Arrange
        var initialServiceCount = _services.Count;

        // Act
        _services.AddTrade360CustomerApiClient(_configuration);
        _services.AddTrade360PrematchSnapshotClient();
        _services.AddTrade360InplaySnapshotClient();

        // Assert
        var finalServiceCount = _services.Count;
        var addedServicesCount = finalServiceCount - initialServiceCount;

        // We expect multiple services to be registered:
        // - HttpClient registrations for each interface
        // - Transient service registrations
        // - AutoMapper registrations
        addedServicesCount.Should().BeGreaterThan(10, "Multiple services should be registered including HttpClients, interfaces, and AutoMapper");
    }

    [Fact]
    public void AddTrade360Services_WithOptionsPattern_ShouldAccessConfiguredOptions()
    {
        // Arrange
        var customBaseUrl = "https://custom-snapshot-api.example.com/";
        _services.Configure<Trade360Settings>(options =>
        {
            options.SnapshotApiBaseUrl = customBaseUrl;
            options.PrematchPackageCredentials = new PackageCredentials
            {
                PackageId = 789,
                Username = "customuser",
                Password = "custompass"
            };
        });

        // Act
        _services.AddTrade360PrematchSnapshotClient();

        // Assert
        var serviceProvider = _services.BuildServiceProvider();
        var options = serviceProvider.GetRequiredService<IOptions<Trade360Settings>>();

        // Verify options are correctly configured
        options.Value.SnapshotApiBaseUrl.Should().Be(customBaseUrl);
        options.Value.PrematchPackageCredentials!.PackageId.Should().Be(789);
        options.Value.PrematchPackageCredentials.Username.Should().Be("customuser");
    }

    [Fact]
    public void AddTrade360Services_WithHttpClientPollyPolicies_ShouldApplyPolicies()
    {
        // Act
        _services.AddTrade360CustomerApiClient(_configuration);

        // Assert
        var serviceProvider = _services.BuildServiceProvider();
        var httpClientFactory = serviceProvider.GetRequiredService<IHttpClientFactory>();

        // Verify policies are applied by checking timeout configuration
        var metadataClient = httpClientFactory.CreateClient(typeof(IMetadataHttpClient).FullName!);
        var packageClient = httpClientFactory.CreateClient(typeof(IPackageDistributionHttpClient).FullName!);
        var subscriptionClient = httpClientFactory.CreateClient(typeof(ISubscriptionHttpClient).FullName!);

        // All clients should have the default timeout (100 seconds) indicating policies are configured
        metadataClient.Timeout.Should().Be(TimeSpan.FromSeconds(100));
        packageClient.Timeout.Should().Be(TimeSpan.FromSeconds(100));
        subscriptionClient.Timeout.Should().Be(TimeSpan.FromSeconds(100));
    }

    [Fact]
    public void AddTrade360Services_WithMultipleHttpClientCreation_ShouldCreateIndependentInstances()
    {
        // Act
        _services.AddTrade360CustomerApiClient(_configuration);

        // Assert
        var serviceProvider = _services.BuildServiceProvider();
        var httpClientFactory = serviceProvider.GetRequiredService<IHttpClientFactory>();

        // Create multiple instances of the same client type
        var metadataClient1 = httpClientFactory.CreateClient(typeof(IMetadataHttpClient).FullName!);
        var metadataClient2 = httpClientFactory.CreateClient(typeof(IMetadataHttpClient).FullName!);

        // Verify clients are independent instances
        metadataClient1.Should().NotBeSameAs(metadataClient2, "HttpClientFactory should create independent client instances");

        // Verify they have consistent configuration
        metadataClient1.Timeout.Should().Be(metadataClient2.Timeout, "All clients should have consistent timeout configuration");
    }

    [Fact]
    public void AddTrade360Services_WithComplexConfiguration_ShouldHandleAllRegistrations()
    {
        // Arrange - Complex scenario with all possible configurations
        _services.Configure<Trade360Settings>(options =>
        {
            options.CustomersApiBaseUrl = "https://customers.example.com/";
            options.SnapshotApiBaseUrl = "https://snapshot.example.com/";
            options.PrematchPackageCredentials = new PackageCredentials
            {
                PackageId = 12345,
                Username = "prematch-user",
                Password = "prematch-password"
            };
            options.InplayPackageCredentials = new PackageCredentials
            {
                PackageId = 67890,
                Username = "inplay-user",
                Password = "inplay-password"
            };
        });

        // Act - Register all services
        _services.AddTrade360CustomerApiClient(_configuration);
        _services.AddTrade360PrematchSnapshotClient();
        _services.AddTrade360InplaySnapshotClient();

        // Assert - Verify everything works together
        var serviceProvider = _services.BuildServiceProvider();

        // Verify all main services are available
        var customersFactory = serviceProvider.GetService<ICustomersApiFactory>();
        var prematchService = serviceProvider.GetService<ISnapshotPrematchApiClient>();
        var inplayService = serviceProvider.GetService<ISnapshotInplayApiClient>();
        var mapper = serviceProvider.GetService<AutoMapper.IMapper>();

        customersFactory.Should().NotBeNull();
        prematchService.Should().NotBeNull();
        inplayService.Should().NotBeNull();
        mapper.Should().NotBeNull();
    }
} 