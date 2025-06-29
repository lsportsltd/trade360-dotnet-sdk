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
/// Edge case tests that target specific uncovered code paths 
/// to maximize coverage for ServiceCollectionExtensions.
/// </summary>
public class ServiceCollectionExtensionsEdgeCaseTests
{
    private readonly IServiceCollection _services;
    private readonly IConfiguration _configuration;

    public ServiceCollectionExtensionsEdgeCaseTests()
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
    public void AddTrade360CustomerApiClient_WithMinimalConfiguration_ShouldRegisterBasicServices()
    {
        // Act
        _services.AddTrade360CustomerApiClient(_configuration);

        // Assert
        var serviceProvider = _services.BuildServiceProvider();
        
        // Verify basic service registration without trying to create complex clients
        var factory = serviceProvider.GetService<ICustomersApiFactory>();
        var mapper = serviceProvider.GetService<AutoMapper.IMapper>();
        var httpClientFactory = serviceProvider.GetService<IHttpClientFactory>();

        factory.Should().NotBeNull("CustomersApiFactory should be registered");
        mapper.Should().NotBeNull("AutoMapper should be registered");
        httpClientFactory.Should().NotBeNull("HttpClientFactory should be registered");
    }

    [Fact]
    public void AddTrade360CustomerApiClient_WithEmptyConfiguration_ShouldStillRegisterServices()
    {
        // Arrange
        var emptyConfig = new ConfigurationBuilder().Build();

        // Act
        _services.AddTrade360CustomerApiClient(emptyConfig);

        // Assert
        var serviceProvider = _services.BuildServiceProvider();
        var factory = serviceProvider.GetService<ICustomersApiFactory>();
        
        factory.Should().NotBeNull("CustomersApiFactory should be registered even with empty config");
    }

    [Fact]
    public void AddTrade360PrematchSnapshotClient_WithoutOptions_ShouldRegisterService()
    {
        // Act - Don't configure options, just register the service
        _services.AddTrade360PrematchSnapshotClient();

        // Assert
        var serviceProvider = _services.BuildServiceProvider();
        var httpClientFactory = serviceProvider.GetService<IHttpClientFactory>();

        httpClientFactory.Should().NotBeNull("HttpClientFactory should be registered");
        
        // Verify service descriptor registration
        var serviceDescriptors = _services.ToList();
        var prematchDescriptor = serviceDescriptors
            .FirstOrDefault(s => s.ServiceType == typeof(ISnapshotPrematchApiClient));
        
        prematchDescriptor.Should().NotBeNull("ISnapshotPrematchApiClient should be registered");
        prematchDescriptor!.Lifetime.Should().Be(ServiceLifetime.Transient);
    }

    [Fact]
    public void AddTrade360InplaySnapshotClient_WithoutOptions_ShouldRegisterService()
    {
        // Act - Don't configure options, just register the service
        _services.AddTrade360InplaySnapshotClient();

        // Assert
        var serviceProvider = _services.BuildServiceProvider();
        var httpClientFactory = serviceProvider.GetService<IHttpClientFactory>();

        httpClientFactory.Should().NotBeNull("HttpClientFactory should be registered");
        
        // Verify service descriptor registration
        var serviceDescriptors = _services.ToList();
        var inplayDescriptor = serviceDescriptors
            .FirstOrDefault(s => s.ServiceType == typeof(ISnapshotInplayApiClient));
        
        inplayDescriptor.Should().NotBeNull("ISnapshotInplayApiClient should be registered");
        inplayDescriptor!.Lifetime.Should().Be(ServiceLifetime.Transient);
    }

    [Fact]
    public void AddTrade360Services_WithMultipleServiceCollections_ShouldWorkIndependently()
    {
        // Arrange
        var services1 = new ServiceCollection();
        var services2 = new ServiceCollection();

        // Act
        services1.AddTrade360CustomerApiClient(_configuration);
        services2.AddTrade360CustomerApiClient(_configuration);

        // Assert
        var provider1 = services1.BuildServiceProvider();
        var provider2 = services2.BuildServiceProvider();

        var factory1 = provider1.GetService<ICustomersApiFactory>();
        var factory2 = provider2.GetService<ICustomersApiFactory>();

        factory1.Should().NotBeNull();
        factory2.Should().NotBeNull();
        factory1.Should().NotBeSameAs(factory2, "Different service providers should create independent instances");
    }

    [Fact]
    public void AddTrade360Services_WithServiceRegistrationValidation_ShouldHaveCorrectImplementationTypes()
    {
        // Act
        _services.AddTrade360CustomerApiClient(_configuration);
        _services.AddTrade360PrematchSnapshotClient();
        _services.AddTrade360InplaySnapshotClient();

        // Assert
        var serviceDescriptors = _services.ToList();

        // Verify CustomersApiFactory implementation
        var factoryDescriptor = serviceDescriptors
            .FirstOrDefault(s => s.ServiceType == typeof(ICustomersApiFactory));
        factoryDescriptor.Should().NotBeNull();
        factoryDescriptor!.ImplementationType?.Name.Should().Be("CustomersApiFactory");

        // Verify Snapshot client implementations
        var prematchDescriptor = serviceDescriptors
            .FirstOrDefault(s => s.ServiceType == typeof(ISnapshotPrematchApiClient));
        prematchDescriptor.Should().NotBeNull();
        prematchDescriptor!.ImplementationType?.Name.Should().Be("SnapshotPrematchApiClient");

        var inplayDescriptor = serviceDescriptors
            .FirstOrDefault(s => s.ServiceType == typeof(ISnapshotInplayApiClient));
        inplayDescriptor.Should().NotBeNull();
        inplayDescriptor!.ImplementationType?.Name.Should().Be("SnapshotInplayApiClient");
    }

    [Fact]
    public void AddTrade360Services_WithHttpClientNameValidation_ShouldUseCorrectClientNames()
    {
        // Act
        _services.AddTrade360CustomerApiClient(_configuration);
        _services.AddTrade360PrematchSnapshotClient();
        _services.AddTrade360InplaySnapshotClient();

        // Assert
        var serviceProvider = _services.BuildServiceProvider();
        var httpClientFactory = serviceProvider.GetRequiredService<IHttpClientFactory>();

        // Test that specific client names work
        var expectedClientNames = new[]
        {
            "Trade360SDK.CustomersApi.Interfaces.IMetadataHttpClient",
            "Trade360SDK.CustomersApi.Interfaces.IPackageDistributionHttpClient", 
            "Trade360SDK.CustomersApi.Interfaces.ISubscriptionHttpClient",
            "Trade360SDK.SnapshotApi.Interfaces.ISnapshotPrematchApiClient",
            "Trade360SDK.SnapshotApi.Interfaces.ISnapshotInplayApiClient"
        };

        foreach (var clientName in expectedClientNames)
        {
            var client = httpClientFactory.CreateClient(clientName);
            client.Should().NotBeNull($"Client '{clientName}' should be registered");
            client.GetType().Name.Should().Be("HttpClient");
        }
    }

    [Fact]
    public void AddTrade360Services_WithAutoMapperConfiguration_ShouldHaveValidConfiguration()
    {
        // Act
        _services.AddTrade360CustomerApiClient(_configuration);

        // Assert
        var serviceProvider = _services.BuildServiceProvider();
        var mapper = serviceProvider.GetRequiredService<AutoMapper.IMapper>();

        mapper.Should().NotBeNull();
        mapper.ConfigurationProvider.Should().NotBeNull("AutoMapper should have configuration provider");
    }

    [Fact]
    public void AddTrade360Services_WithServiceLifetimeConsistency_ShouldMaintainTransientLifetime()
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

        // Act
        _services.AddTrade360CustomerApiClient(_configuration);
        _services.AddTrade360PrematchSnapshotClient();
        _services.AddTrade360InplaySnapshotClient();

        // Assert
        var serviceProvider = _services.BuildServiceProvider();

        // Test multiple resolutions to ensure transient behavior
        var factory1 = serviceProvider.GetService<ICustomersApiFactory>();
        var factory2 = serviceProvider.GetService<ICustomersApiFactory>();
        var factory3 = serviceProvider.GetService<ICustomersApiFactory>();

        factory1.Should().NotBeNull();
        factory2.Should().NotBeNull();
        factory3.Should().NotBeNull();
        
        // All should be different instances (transient)
        factory1.Should().NotBeSameAs(factory2);
        factory1.Should().NotBeSameAs(factory3);
        factory2.Should().NotBeSameAs(factory3);

        // Test snapshot clients
        var prematch1 = serviceProvider.GetService<ISnapshotPrematchApiClient>();
        var prematch2 = serviceProvider.GetService<ISnapshotPrematchApiClient>();
        
        prematch1.Should().NotBeNull();
        prematch2.Should().NotBeNull();
        prematch1.Should().NotBeSameAs(prematch2, "Prematch clients should be transient");

        var inplay1 = serviceProvider.GetService<ISnapshotInplayApiClient>();
        var inplay2 = serviceProvider.GetService<ISnapshotInplayApiClient>();
        
        inplay1.Should().NotBeNull();
        inplay2.Should().NotBeNull();
        inplay1.Should().NotBeSameAs(inplay2, "Inplay clients should be transient");
    }

    [Fact]
    public void AddTrade360Services_WithServiceProviderDisposal_ShouldNotThrow()
    {
        // Arrange
        _services.AddTrade360CustomerApiClient(_configuration);

        // Act & Assert
        using var serviceProvider = _services.BuildServiceProvider();
        var factory = serviceProvider.GetService<ICustomersApiFactory>();
        
        factory.Should().NotBeNull();
        
        // Disposal should not throw
        var act = () => serviceProvider.Dispose();
        act.Should().NotThrow("Service provider disposal should be clean");
    }

    [Fact]
    public void AddTrade360Services_WithServiceCollectionModification_ShouldAllowFurtherRegistrations()
    {
        // Act
        _services.AddTrade360CustomerApiClient(_configuration);
        
        // Add additional services after Trade360 registration
        _services.AddTransient<string>(provider => "test-service");
        _services.AddSingleton<object>(new object());

        // Assert
        var serviceProvider = _services.BuildServiceProvider();
        
        var factory = serviceProvider.GetService<ICustomersApiFactory>();
        var testString = serviceProvider.GetService<string>();
        var testObject = serviceProvider.GetService<object>();

        factory.Should().NotBeNull("Trade360 services should still work");
        testString.Should().Be("test-service", "Additional services should be registered");
        testObject.Should().NotBeNull("Additional services should be registered");
    }

    [Fact]
    public void AddTrade360Services_WithHttpClientFactoryConfiguration_ShouldPreserveDefaultBehavior()
    {
        // Act
        _services.AddTrade360CustomerApiClient(_configuration);

        // Assert
        var serviceProvider = _services.BuildServiceProvider();
        var httpClientFactory = serviceProvider.GetRequiredService<IHttpClientFactory>();

        // Create default client
        var defaultClient = httpClientFactory.CreateClient();
        defaultClient.Should().NotBeNull("Default HTTP client should still be available");
        
        // Create named client
        var namedClient = httpClientFactory.CreateClient("custom-client");
        namedClient.Should().NotBeNull("Named HTTP client should still be available");
        
        // They should be different instances
        defaultClient.Should().NotBeSameAs(namedClient);
    }

    [Fact]
    public void AddTrade360Services_WithOptionsPatternIntegration_ShouldWorkWithOptionsMonitor()
    {
        // Arrange
        _services.Configure<Trade360Settings>(options =>
        {
            options.SnapshotApiBaseUrl = "https://initial.example.com/";
        });
        _services.AddTrade360PrematchSnapshotClient();

        // Act
        var serviceProvider = _services.BuildServiceProvider();
        var optionsMonitor = serviceProvider.GetService<IOptionsMonitor<Trade360Settings>>();

        // Assert
        optionsMonitor.Should().NotBeNull("IOptionsMonitor should be available");
        optionsMonitor!.CurrentValue.SnapshotApiBaseUrl.Should().Be("https://initial.example.com/");
    }

    [Fact]
    public void AddTrade360Services_WithServiceDescriptorCounting_ShouldRegisterExpectedNumberOfServices()
    {
        // Arrange
        var initialCount = _services.Count;

        // Act
        _services.AddTrade360CustomerApiClient(_configuration);
        var afterCustomersCount = _services.Count;
        
        _services.AddTrade360PrematchSnapshotClient();
        var afterPrematchCount = _services.Count;
        
        _services.AddTrade360InplaySnapshotClient();
        var finalCount = _services.Count;

        // Assert
        var customersServicesAdded = afterCustomersCount - initialCount;
        var prematchServicesAdded = afterPrematchCount - afterCustomersCount;
        var inplayServicesAdded = finalCount - afterPrematchCount;

        customersServicesAdded.Should().BeGreaterThan(5, "AddTrade360CustomerApiClient should register multiple services");
        prematchServicesAdded.Should().BeGreaterThan(2, "AddTrade360PrematchSnapshotClient should register services");
        inplayServicesAdded.Should().BeGreaterThan(2, "AddTrade360InplaySnapshotClient should register services");
        
        var totalServicesAdded = finalCount - initialCount;
        totalServicesAdded.Should().BeGreaterThan(10, "All methods should register significant number of services");
    }

    [Fact]
    public void AddTrade360Services_WithMethodChaining_ShouldReturnOriginalServiceCollection()
    {
        // Act
        var result1 = _services.AddTrade360CustomerApiClient(_configuration);
        var result2 = result1.AddTrade360PrematchSnapshotClient();
        var result3 = result2.AddTrade360InplaySnapshotClient();

        // Assert
        result1.Should().BeSameAs(_services, "AddTrade360CustomerApiClient should return original collection");
        result2.Should().BeSameAs(_services, "AddTrade360PrematchSnapshotClient should return original collection");
        result3.Should().BeSameAs(_services, "AddTrade360InplaySnapshotClient should return original collection");
        
        // Verify we can continue chaining
        var result4 = result3.AddTransient<string>(provider => "chained-service");
        result4.Should().BeSameAs(_services, "Should be able to continue chaining after Trade360 methods");
    }

    [Fact]
    public void AddTrade360Services_WithComplexServiceResolution_ShouldResolveAllDependencies()
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

        // Act
        _services.AddTrade360CustomerApiClient(_configuration);
        _services.AddTrade360PrematchSnapshotClient();
        _services.AddTrade360InplaySnapshotClient();

        // Assert
        var serviceProvider = _services.BuildServiceProvider();

        // Verify all services can be resolved without throwing
        var resolutionTests = new Func<object?>[]
        {
            () => serviceProvider.GetService<ICustomersApiFactory>(),
            () => serviceProvider.GetService<ISnapshotPrematchApiClient>(),
            () => serviceProvider.GetService<ISnapshotInplayApiClient>(),
            () => serviceProvider.GetService<AutoMapper.IMapper>(),
            () => serviceProvider.GetService<IHttpClientFactory>(),
            () => serviceProvider.GetService<IOptions<Trade360Settings>>(),
            () => serviceProvider.GetService<IOptionsMonitor<Trade360Settings>>()
        };

        foreach (var test in resolutionTests)
        {
            var act = () => test();
            act.Should().NotThrow("All services should be resolvable");
            test().Should().NotBeNull("All services should resolve to non-null instances");
        }
    }
} 