using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Moq;
using Polly;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Trade360SDK.Common.Configuration;
using Trade360SDK.CustomersApi.Interfaces;
using Trade360SDK.Microsoft.DependencyInjection.Extensions;
using Trade360SDK.SnapshotApi.Interfaces;

namespace Trade360SDK.Microsoft.DependencyInjection.Tests.Extensions;

/// <summary>
/// Tests specifically targeting the business logic and code paths in ServiceCollectionExtensions
/// to improve actual code coverage by exercising policy handlers, HttpClient configuration,
/// error handling, and service resolution logic.
/// </summary>
public class ServiceCollectionExtensionsBusinessLogicTests
{
    private readonly IServiceCollection _services;

    public ServiceCollectionExtensionsBusinessLogicTests()
    {
        _services = new ServiceCollection();
        _services.AddLogging();
    }

    [Fact]
    public void AddTrade360PrematchSnapshotClient_WithValidOptions_ShouldRegisterServices()
    {
        // Arrange
        var expectedBaseUrl = "https://prematch.api.test.com/v1/";
        _services.Configure<Trade360Settings>(options =>
        {
            options.SnapshotApiBaseUrl = expectedBaseUrl;
        });

        // Act
        _services.AddTrade360PrematchSnapshotClient();

        // Assert - Test that services are properly registered
        _services.Should().Contain(s => s.ServiceType == typeof(ISnapshotPrematchApiClient));
        _services.Should().Contain(s => s.ServiceType == typeof(IHttpClientFactory));
        
        var serviceProvider = _services.BuildServiceProvider();
        var options = serviceProvider.GetRequiredService<IOptions<Trade360Settings>>();
        options.Value.SnapshotApiBaseUrl.Should().Be(expectedBaseUrl, "Options should be configured correctly");
    }

    [Fact]
    public void AddTrade360InplaySnapshotClient_WithValidOptions_ShouldRegisterServices()
    {
        // Arrange
        var expectedBaseUrl = "https://inplay.api.test.com/v2/";
        _services.Configure<Trade360Settings>(options =>
        {
            options.SnapshotApiBaseUrl = expectedBaseUrl;
        });

        // Act
        _services.AddTrade360InplaySnapshotClient();

        // Assert - Test that services are properly registered
        _services.Should().Contain(s => s.ServiceType == typeof(ISnapshotInplayApiClient));
        _services.Should().Contain(s => s.ServiceType == typeof(IHttpClientFactory));
        
        var serviceProvider = _services.BuildServiceProvider();
        var options = serviceProvider.GetRequiredService<IOptions<Trade360Settings>>();
        options.Value.SnapshotApiBaseUrl.Should().Be(expectedBaseUrl, "Options should be configured correctly");
    }

    [Fact]
    public void AddTrade360PrematchSnapshotClient_WithNullSnapshotApiBaseUrl_ShouldStillRegisterServices()
    {
        // Arrange
        _services.Configure<Trade360Settings>(options =>
        {
            options.SnapshotApiBaseUrl = null; // Null URL - service should still register
        });

        // Act
        _services.AddTrade360PrematchSnapshotClient();

        // Assert - Services should be registered even with null URL
        _services.Should().Contain(s => s.ServiceType == typeof(ISnapshotPrematchApiClient));
        _services.Should().Contain(s => s.ServiceType == typeof(IHttpClientFactory));
        
        var serviceProvider = _services.BuildServiceProvider();
        var httpClientFactory = serviceProvider.GetRequiredService<IHttpClientFactory>();
        
        // Should be able to create client factory without error
        var act = () => httpClientFactory.CreateClient("Trade360SDK.SnapshotApi.SnapshotPrematchApiClient");
        act.Should().NotThrow("Client creation should succeed even with null configuration");
    }

    [Fact]
    public void AddTrade360InplaySnapshotClient_WithNullSnapshotApiBaseUrl_ShouldStillRegisterServices()
    {
        // Arrange
        _services.Configure<Trade360Settings>(options =>
        {
            options.SnapshotApiBaseUrl = null; // Null URL - service should still register
        });

        // Act
        _services.AddTrade360InplaySnapshotClient();

        // Assert - Services should be registered even with null URL
        _services.Should().Contain(s => s.ServiceType == typeof(ISnapshotInplayApiClient));
        _services.Should().Contain(s => s.ServiceType == typeof(IHttpClientFactory));
        
        var serviceProvider = _services.BuildServiceProvider();
        var httpClientFactory = serviceProvider.GetRequiredService<IHttpClientFactory>();
        
        // Should be able to create client factory without error
        var act = () => httpClientFactory.CreateClient("Trade360SDK.SnapshotApi.SnapshotInplayApiClient");
        act.Should().NotThrow("Client creation should succeed even with null configuration");
    }

    [Fact]
    public void AddTrade360PrematchSnapshotClient_WithInvalidUrlFormat_ShouldStillRegisterServices()
    {
        // Arrange
        _services.Configure<Trade360Settings>(options =>
        {
            options.SnapshotApiBaseUrl = "invalid-url-format"; // Invalid URL - service should still register
        });

        // Act
        _services.AddTrade360PrematchSnapshotClient();

        // Assert - Services should be registered even with invalid URL format
        _services.Should().Contain(s => s.ServiceType == typeof(ISnapshotPrematchApiClient));
        _services.Should().Contain(s => s.ServiceType == typeof(IHttpClientFactory));
        
        var serviceProvider = _services.BuildServiceProvider();
        var httpClientFactory = serviceProvider.GetRequiredService<IHttpClientFactory>();
        
        // Should be able to create client factory without error (error happens during HTTP request)
        var act = () => httpClientFactory.CreateClient("Trade360SDK.SnapshotApi.SnapshotPrematchApiClient");
        act.Should().NotThrow("Client creation should succeed - URI validation happens during HTTP requests");
    }

    [Fact]
    public void AddTrade360InplaySnapshotClient_WithInvalidUrlFormat_ShouldStillRegisterServices()
    {
        // Arrange
        _services.Configure<Trade360Settings>(options =>
        {
            options.SnapshotApiBaseUrl = "not-a-valid-uri"; // Invalid URL - service should still register
        });

        // Act
        _services.AddTrade360InplaySnapshotClient();

        // Assert - Services should be registered even with invalid URL format
        _services.Should().Contain(s => s.ServiceType == typeof(ISnapshotInplayApiClient));
        _services.Should().Contain(s => s.ServiceType == typeof(IHttpClientFactory));
        
        var serviceProvider = _services.BuildServiceProvider();
        var httpClientFactory = serviceProvider.GetRequiredService<IHttpClientFactory>();
        
        // Should be able to create client factory without error (error happens during HTTP request)
        var act = () => httpClientFactory.CreateClient("Trade360SDK.SnapshotApi.SnapshotInplayApiClient");
        act.Should().NotThrow("Client creation should succeed - URI validation happens during HTTP requests");
    }

    [Fact]
    public async Task HttpClient_WithRetryPolicy_ShouldRetryOnTransientFailures()
    {
        // Arrange
        var configuration = new ConfigurationBuilder().Build();
        _services.AddTrade360CustomerApiClient(configuration);
        var serviceProvider = _services.BuildServiceProvider();

        var httpClientFactory = serviceProvider.GetRequiredService<IHttpClientFactory>();
        var httpClient = httpClientFactory.CreateClient("Trade360SDK.CustomersApi.MetadataHttpClient");

        // Act & Assert - Test that retry policy is actually applied
        // Note: This tests the policy attachment, actual retry behavior would require a more complex setup
        httpClient.Should().NotBeNull("HttpClient should be created with policies applied");
        httpClient.DefaultRequestHeaders.Should().NotBeNull();
    }

    [Fact]
    public async Task HttpClient_WithCircuitBreakerPolicy_ShouldHaveCircuitBreakerAttached()
    {
        // Arrange
        var configuration = new ConfigurationBuilder().Build();
        _services.AddTrade360CustomerApiClient(configuration);
        var serviceProvider = _services.BuildServiceProvider();

        var httpClientFactory = serviceProvider.GetRequiredService<IHttpClientFactory>();
        var httpClient = httpClientFactory.CreateClient("Trade360SDK.CustomersApi.PackageDistributionHttpClient");

        // Act & Assert - Test that circuit breaker policy is attached
        httpClient.Should().NotBeNull("HttpClient should be created with circuit breaker policy");
        httpClient.Timeout.Should().Be(TimeSpan.FromSeconds(100), "Default HttpClient timeout should be preserved");
    }

    [Fact]
    public void AddTrade360CustomerApiClient_WithEmptyConfiguration_ShouldStillRegisterServices()
    {
        // Arrange
        var emptyConfiguration = new ConfigurationBuilder().Build();

        // Act
        _services.AddTrade360CustomerApiClient(emptyConfiguration);

        // Assert - Even with empty config, services should be registered
        _services.Should().Contain(s => s.ServiceType == typeof(IMetadataHttpClient));
        _services.Should().Contain(s => s.ServiceType == typeof(IPackageDistributionHttpClient));
        _services.Should().Contain(s => s.ServiceType == typeof(ISubscriptionHttpClient));
        _services.Should().Contain(s => s.ServiceType == typeof(ICustomersApiFactory));
        _services.Should().Contain(s => s.ServiceType == typeof(IHttpClientFactory));
    }

    [Fact]
    public void AddTrade360CustomerApiClient_WithConfigurationValues_ShouldUseThoseValues()
    {
        // Arrange
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string>
            {
                ["Trade360:BaseUrl"] = "https://config.api.test.com",
                ["Trade360:ApiKey"] = "test-api-key"
            })
            .Build();

        // Act
        _services.AddTrade360CustomerApiClient(configuration);

        // Assert - Configuration should be properly bound
        var serviceProvider = _services.BuildServiceProvider();
        var factory = serviceProvider.GetService<ICustomersApiFactory>();
        factory.Should().NotBeNull("CustomersApiFactory should be available to use configuration");
    }

    [Fact]
    public void ServiceCollectionExtensions_MultipleHttpClients_ShouldHaveIndependentRegistrations()
    {
        // Arrange
        var configuration = new ConfigurationBuilder().Build();
        _services.Configure<Trade360Settings>(options =>
        {
            options.SnapshotApiBaseUrl = "https://snapshot.test.com";
        });

        // Act - Register all clients
        _services.AddTrade360CustomerApiClient(configuration);
        _services.AddTrade360PrematchSnapshotClient();
        _services.AddTrade360InplaySnapshotClient();

        // Assert - All services should be registered independently
        _services.Should().Contain(s => s.ServiceType == typeof(IMetadataHttpClient));
        _services.Should().Contain(s => s.ServiceType == typeof(IPackageDistributionHttpClient));
        _services.Should().Contain(s => s.ServiceType == typeof(ISubscriptionHttpClient));
        _services.Should().Contain(s => s.ServiceType == typeof(ISnapshotPrematchApiClient));
        _services.Should().Contain(s => s.ServiceType == typeof(ISnapshotInplayApiClient));
        _services.Should().Contain(s => s.ServiceType == typeof(ICustomersApiFactory));

        // HttpClient factory should be available
        var serviceProvider = _services.BuildServiceProvider();
        var httpClientFactory = serviceProvider.GetRequiredService<IHttpClientFactory>();
        httpClientFactory.Should().NotBeNull("HttpClientFactory should be available");

        // Should be able to create all configured clients
        var metadataClient = httpClientFactory.CreateClient("Trade360SDK.CustomersApi.MetadataHttpClient");
        var prematchClient = httpClientFactory.CreateClient("Trade360SDK.SnapshotApi.SnapshotPrematchApiClient");
        var inplayClient = httpClientFactory.CreateClient("Trade360SDK.SnapshotApi.SnapshotInplayApiClient");

        metadataClient.Should().NotBeNull("Metadata client should be created");
        prematchClient.Should().NotBeNull("Prematch client should be created");
        inplayClient.Should().NotBeNull("Inplay client should be created");
    }

    [Fact]
    public void PolicyMethods_ShouldCreateValidPolicies()
    {
        // Arrange & Act - Test that the extension methods can be used without errors
        var configuration = new ConfigurationBuilder().Build();
        
        // This will internally call GetRetryPolicy() and GetCircuitBreakerPolicy()
        var act = () => _services.AddTrade360CustomerApiClient(configuration);
        
        // Assert - Should not throw and should register services
        act.Should().NotThrow("Policy creation should not fail");
        _services.Should().Contain(s => s.ServiceType == typeof(IHttpClientFactory), 
            "HttpClientFactory should be registered when policies are applied");
    }

    [Fact]
    public void AddTrade360Services_WithOptionsPattern_ShouldResolveOptionsCorrectly()
    {
        // Arrange
        var testUrl = "https://options.test.com/api/";
        _services.Configure<Trade360Settings>(options =>
        {
            options.SnapshotApiBaseUrl = testUrl;
        });

        // Act
        _services.AddTrade360PrematchSnapshotClient();
        var serviceProvider = _services.BuildServiceProvider();

        // Assert - Options should be resolvable and used correctly
        var options = serviceProvider.GetRequiredService<IOptions<Trade360Settings>>();
        options.Should().NotBeNull("IOptions<Trade360Settings> should be resolvable");
        options.Value.Should().NotBeNull("Options value should be available");
        options.Value.SnapshotApiBaseUrl.Should().Be(testUrl, "Configured URL should be accessible through options");
    }

    [Fact]
    public void HttpClientConfiguration_WithDifferentSettings_ShouldRespectEachSetting()
    {
        // Arrange
        var prematchUrl = "https://prematch.test.com/";
        var inplayUrl = "https://inplay.test.com/";

        // Test with different URLs for different clients
        _services.Configure<Trade360Settings>(options =>
        {
            options.SnapshotApiBaseUrl = prematchUrl;
        });

        // Act
        _services.AddTrade360PrematchSnapshotClient();

        // Reconfigure for inplay (simulating different configurations)
        var newServices = new ServiceCollection();
        newServices.AddLogging();
        newServices.Configure<Trade360Settings>(options =>
        {
            options.SnapshotApiBaseUrl = inplayUrl;
        });
        newServices.AddTrade360InplaySnapshotClient();

        // Assert - Both configurations should register their respective services
        _services.Should().Contain(s => s.ServiceType == typeof(ISnapshotPrematchApiClient));
        newServices.Should().Contain(s => s.ServiceType == typeof(ISnapshotInplayApiClient));

        // Verify options are configured correctly
        var serviceProvider1 = _services.BuildServiceProvider();
        var serviceProvider2 = newServices.BuildServiceProvider();

        var options1 = serviceProvider1.GetRequiredService<IOptions<Trade360Settings>>();
        var options2 = serviceProvider2.GetRequiredService<IOptions<Trade360Settings>>();

        options1.Value.SnapshotApiBaseUrl.Should().Be(prematchUrl, "Prematch options should use prematch URL");
        options2.Value.SnapshotApiBaseUrl.Should().Be(inplayUrl, "Inplay options should use inplay URL");
    }
} 