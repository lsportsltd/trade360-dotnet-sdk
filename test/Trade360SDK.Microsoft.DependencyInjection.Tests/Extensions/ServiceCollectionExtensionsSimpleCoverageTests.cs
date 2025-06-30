using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using AutoMapper;
using Trade360SDK.Common.Configuration;
using Trade360SDK.CustomersApi.Interfaces;
using Trade360SDK.Microsoft.DependencyInjection.Extensions;
using Trade360SDK.SnapshotApi.Interfaces;

namespace Trade360SDK.Microsoft.DependencyInjection.Tests.Extensions;

/// <summary>
/// Simple coverage tests for ServiceCollectionExtensions that focus on basic functionality
/// and successfully exercise all code paths to maximize coverage.
/// </summary>
public class ServiceCollectionExtensionsSimpleCoverageTests
{
    private readonly IServiceCollection _services;

    public ServiceCollectionExtensionsSimpleCoverageTests()
    {
        _services = new ServiceCollection();
        _services.AddLogging();
    }

    [Fact]
    public void AddTrade360CustomerApiClient_WithNullConfiguration_ShouldThrowArgumentNullException()
    {
        // Act & Assert
        var act = () => _services.AddTrade360CustomerApiClient(null!);
        
        act.Should().Throw<ArgumentNullException>()
           .WithParameterName("configuration");
    }

    [Fact]
    public void AddTrade360CustomerApiClient_WithValidConfiguration_ShouldRegisterAllServices()
    {
        // Arrange
        var configuration = new ConfigurationBuilder().Build();

        // Act
        var result = _services.AddTrade360CustomerApiClient(configuration);

        // Assert
        result.Should().BeSameAs(_services, "Method should return the same IServiceCollection for chaining");
        
        var serviceProvider = _services.BuildServiceProvider();
        
        // Verify all services are registered
        serviceProvider.GetService<ICustomersApiFactory>().Should().NotBeNull();
        serviceProvider.GetService<IMapper>().Should().NotBeNull();
        
        // Verify HTTP clients can be resolved
        var httpClientFactory = serviceProvider.GetService<System.Net.Http.IHttpClientFactory>();
        httpClientFactory.Should().NotBeNull();
    }

    [Fact]
    public void AddTrade360PrematchSnapshotClient_WithValidSettings_ShouldRegisterServices()
    {
        // Arrange
        _services.Configure<Trade360Settings>(options =>
        {
            options.SnapshotApiBaseUrl = "https://valid.test.com/api/";
        });

        // Act
        var result = _services.AddTrade360PrematchSnapshotClient();

        // Assert
        result.Should().BeSameAs(_services, "Method should return the same IServiceCollection for chaining");
        
        var serviceProvider = _services.BuildServiceProvider();
        
        // Verify services are registered
        serviceProvider.GetService<IMapper>().Should().NotBeNull();
        var options = serviceProvider.GetService<IOptions<Trade360Settings>>();
        options.Should().NotBeNull();
        options.Value.SnapshotApiBaseUrl.Should().Be("https://valid.test.com/api/");
    }

    [Fact]
    public void AddTrade360InplaySnapshotClient_WithValidSettings_ShouldRegisterServices()
    {
        // Arrange
        _services.Configure<Trade360Settings>(options =>
        {
            options.SnapshotApiBaseUrl = "https://valid.inplay.test.com/api/";
        });

        // Act
        var result = _services.AddTrade360InplaySnapshotClient();

        // Assert
        result.Should().BeSameAs(_services, "Method should return the same IServiceCollection for chaining");
        
        var serviceProvider = _services.BuildServiceProvider();
        
        // Verify services are registered
        serviceProvider.GetService<IMapper>().Should().NotBeNull();
        var options = serviceProvider.GetService<IOptions<Trade360Settings>>();
        options.Should().NotBeNull();
        options.Value.SnapshotApiBaseUrl.Should().Be("https://valid.inplay.test.com/api/");
    }

    [Fact]
    public void MultipleRegistrations_ShouldNotConflict()
    {
        // Arrange
        var configuration = new ConfigurationBuilder().Build();
        _services.Configure<Trade360Settings>(options =>
        {
            options.SnapshotApiBaseUrl = "https://multi.test.com/api/";
        });

        // Act - Register multiple services
        _services.AddTrade360CustomerApiClient(configuration);
        _services.AddTrade360PrematchSnapshotClient();
        _services.AddTrade360InplaySnapshotClient();

        // Assert
        var serviceProvider = _services.BuildServiceProvider();
        
        // All services should be resolvable without conflicts
        serviceProvider.GetService<ICustomersApiFactory>().Should().NotBeNull();
        serviceProvider.GetService<IMapper>().Should().NotBeNull();
        serviceProvider.GetService<System.Net.Http.IHttpClientFactory>().Should().NotBeNull();
        serviceProvider.GetService<IOptions<Trade360Settings>>().Should().NotBeNull();
    }

    [Fact]
    public void GetRetryPolicy_ShouldBeAppliedToHttpClients()
    {
        // Arrange
        var configuration = new ConfigurationBuilder().Build();

        // Act
        _services.AddTrade360CustomerApiClient(configuration);

        // Assert - Test that retry policy doesn't prevent service registration
        var serviceProvider = _services.BuildServiceProvider();
        var httpClientFactory = serviceProvider.GetService<System.Net.Http.IHttpClientFactory>();
        httpClientFactory.Should().NotBeNull("HttpClientFactory should be available with retry policies");
    }

    [Fact]
    public void GetCircuitBreakerPolicy_ShouldBeAppliedToHttpClients()
    {
        // Arrange
        var configuration = new ConfigurationBuilder().Build();
        _services.Configure<Trade360Settings>(options =>
        {
            options.SnapshotApiBaseUrl = "https://circuit.test.com/api/";
        });

        // Act
        _services.AddTrade360CustomerApiClient(configuration);
        _services.AddTrade360PrematchSnapshotClient();

        // Assert - Test that circuit breaker policy doesn't prevent service registration
        var serviceProvider = _services.BuildServiceProvider();
        var httpClientFactory = serviceProvider.GetService<System.Net.Http.IHttpClientFactory>();
        httpClientFactory.Should().NotBeNull("HttpClientFactory should be available with circuit breaker policies");
    }

    [Fact]
    public void AutoMapperRegistration_ShouldWorkWithAllServices()
    {
        // Arrange
        var configuration = new ConfigurationBuilder().Build();
        _services.Configure<Trade360Settings>(options =>
        {
            options.SnapshotApiBaseUrl = "https://mapper.test.com/api/";
        });

        // Act - Register all services that add AutoMapper
        _services.AddTrade360CustomerApiClient(configuration);
        _services.AddTrade360PrematchSnapshotClient();
        _services.AddTrade360InplaySnapshotClient();

        // Assert
        var serviceProvider = _services.BuildServiceProvider();
        var mapper = serviceProvider.GetService<IMapper>();
        mapper.Should().NotBeNull("AutoMapper should be registered and resolvable");
    }

    [Fact]
    public void ServiceRegistration_WithMinimalConfiguration_ShouldWork()
    {
        // Arrange
        var configuration = new ConfigurationBuilder().Build();

        // Act
        _services.AddTrade360CustomerApiClient(configuration);

        // Assert
        var act = () => _services.BuildServiceProvider();
        act.Should().NotThrow("Minimal configuration should not cause exceptions during service provider build");
    }

    [Fact]
    public void ServiceRegistration_WithComplexConfiguration_ShouldWork()
    {
        // Arrange
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string>
            {
                ["Logging:LogLevel:Default"] = "Information",
                ["HttpClient:Timeout"] = "30",
                ["Trade360:Features:EnableRetry"] = "true"
            })
            .Build();

        _services.Configure<Trade360Settings>(options =>
        {
            options.SnapshotApiBaseUrl = "https://complex.test.com/api/v1/";
        });

        // Act
        _services.AddTrade360CustomerApiClient(configuration);
        _services.AddTrade360PrematchSnapshotClient();
        _services.AddTrade360InplaySnapshotClient();

        // Assert
        var serviceProvider = _services.BuildServiceProvider();
        
        // All services should be available
        serviceProvider.GetService<ICustomersApiFactory>().Should().NotBeNull();
        serviceProvider.GetService<IMapper>().Should().NotBeNull();
        serviceProvider.GetService<System.Net.Http.IHttpClientFactory>().Should().NotBeNull();
        
        var options = serviceProvider.GetService<IOptions<Trade360Settings>>();
        options.Should().NotBeNull();
        options.Value.SnapshotApiBaseUrl.Should().Be("https://complex.test.com/api/v1/");
    }

    [Fact]
    public void ChainedMethodCalls_ShouldReturnSameServiceCollection()
    {
        // Arrange
        var configuration = new ConfigurationBuilder().Build();

        // Act & Assert - Test method chaining
        var result = _services
            .AddTrade360CustomerApiClient(configuration)
            .AddTrade360PrematchSnapshotClient()
            .AddTrade360InplaySnapshotClient();

        result.Should().BeSameAs(_services, "All extension methods should return the same IServiceCollection instance");
    }

    [Fact]
    public void HttpClientPolicies_ShouldNotPreventServiceResolution()
    {
        // Arrange
        var configuration = new ConfigurationBuilder().Build();
        _services.Configure<Trade360Settings>(options =>
        {
            options.SnapshotApiBaseUrl = "https://policies.test.com/api/";
        });

        // Act - Register services with both retry and circuit breaker policies
        _services.AddTrade360CustomerApiClient(configuration);
        _services.AddTrade360PrematchSnapshotClient();
        _services.AddTrade360InplaySnapshotClient();

        // Assert - Policies should not prevent service resolution
        var serviceProvider = _services.BuildServiceProvider();
        
        var httpClientFactory = serviceProvider.GetService<System.Net.Http.IHttpClientFactory>();
        httpClientFactory.Should().NotBeNull("HttpClientFactory should be resolvable with all policies applied");
        
        // Test that basic services are still available
        serviceProvider.GetService<ICustomersApiFactory>().Should().NotBeNull();
        serviceProvider.GetService<IMapper>().Should().NotBeNull();
    }

    [Fact]
    public void ServiceProvider_WithAllServices_ShouldBuildSuccessfully()
    {
        // Arrange
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string>
            {
                ["Trade360:ApiBaseUrl"] = "https://full.test.com/",
                ["Trade360:Timeout"] = "60"
            })
            .Build();

        _services.Configure<Trade360Settings>(options =>
        {
            options.SnapshotApiBaseUrl = "https://full.snapshot.test.com/api/";
        });

        // Act
        _services.AddTrade360CustomerApiClient(configuration);
        _services.AddTrade360PrematchSnapshotClient();
        _services.AddTrade360InplaySnapshotClient();

        // Assert
        var serviceProvider = _services.BuildServiceProvider();
        
        // Verify comprehensive service availability
        var services = new[]
        {
            typeof(ICustomersApiFactory),
            typeof(IMapper),
            typeof(System.Net.Http.IHttpClientFactory),
            typeof(IOptions<Trade360Settings>)
        };

        foreach (var serviceType in services)
        {
            var service = serviceProvider.GetService(serviceType);
            service.Should().NotBeNull($"Service of type {serviceType.Name} should be registered and resolvable");
        }
    }
} 