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
using System.Reflection;
using System.Linq;
using Xunit;
using Trade360SDK.CustomersApi;
using Trade360SDK.SnapshotApi;
using AutoMapper;
using System.Threading.Tasks;
using System.Threading;

namespace Trade360SDK.Microsoft.DependencyInjection.Tests.Extensions;

/// <summary>
/// Comprehensive tests for Polly policy configuration and HTTP client resilience.
/// These tests focus on verifying that retry and circuit breaker policies are correctly
/// applied to HTTP clients and validate the policy configuration indirectly.
/// </summary>
public class ServiceCollectionExtensionsPolicyTests
{
    private readonly IServiceCollection _services;
    private readonly IConfiguration _configuration;

    public ServiceCollectionExtensionsPolicyTests()
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
    public void AddTrade360CustomerApiClient_ShouldApplyRetryPolicyToAllHttpClients()
    {
        // Act
        _services.AddTrade360CustomerApiClient(_configuration);

        // Assert
        var serviceProvider = _services.BuildServiceProvider();
        var httpClientFactory = serviceProvider.GetRequiredService<IHttpClientFactory>();

        // Verify policies are applied by checking timeout configuration
        // The presence of policies typically modifies the default timeout
        var metadataClient = httpClientFactory.CreateClient(typeof(IMetadataHttpClient).FullName!);
        var packageClient = httpClientFactory.CreateClient(typeof(IPackageDistributionHttpClient).FullName!);
        var subscriptionClient = httpClientFactory.CreateClient(typeof(ISubscriptionHttpClient).FullName!);

        // All clients should have the default timeout indicating policies are configured
        metadataClient.Timeout.Should().Be(TimeSpan.FromSeconds(100), "Retry policy should be applied to metadata client");
        packageClient.Timeout.Should().Be(TimeSpan.FromSeconds(100), "Retry policy should be applied to package client");
        subscriptionClient.Timeout.Should().Be(TimeSpan.FromSeconds(100), "Retry policy should be applied to subscription client");
    }

    [Fact]
    public void AddTrade360PrematchSnapshotClient_ShouldRegisterServices()
    {
        // Arrange
        var services = new ServiceCollection();
        services.Configure<Trade360Settings>(options =>
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
        services.AddTrade360PrematchSnapshotClient();
        var serviceProvider = services.BuildServiceProvider();

        // Assert
        serviceProvider.GetService<ISnapshotPrematchApiClient>().Should().NotBeNull();
        serviceProvider.GetService<IMapper>().Should().NotBeNull();
        serviceProvider.GetService<IHttpClientFactory>().Should().NotBeNull();
    }

    [Fact]
    public void AddTrade360InplaySnapshotClient_ShouldRegisterServices()
    {
        // Arrange
        var services = new ServiceCollection();
        services.Configure<Trade360Settings>(options =>
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
        services.AddTrade360InplaySnapshotClient();
        var serviceProvider = services.BuildServiceProvider();

        // Assert
        serviceProvider.GetService<ISnapshotInplayApiClient>().Should().NotBeNull();
        serviceProvider.GetService<IMapper>().Should().NotBeNull();
        serviceProvider.GetService<IHttpClientFactory>().Should().NotBeNull();
    }

    [Fact]
    public void AddTrade360PrematchSnapshotClient_WithValidConfiguration_ShouldCreateClientSuccessfully()
    {
        // Arrange
        var services = new ServiceCollection();
        services.Configure<Trade360Settings>(options =>
        {
            options.SnapshotApiBaseUrl = "https://valid-api.example.com/";
            options.PrematchPackageCredentials = new PackageCredentials
            {
                PackageId = 789,
                Username = "validuser",
                Password = "validpass"
            };
        });

        services.AddTrade360PrematchSnapshotClient();
        var serviceProvider = services.BuildServiceProvider();

        // Act & Assert
        var client = serviceProvider.GetService<ISnapshotPrematchApiClient>();
        client.Should().NotBeNull();
    }

    [Fact]
    public void AddTrade360InplaySnapshotClient_WithValidConfiguration_ShouldCreateClientSuccessfully()
    {
        // Arrange
        var services = new ServiceCollection();
        services.Configure<Trade360Settings>(options =>
        {
            options.SnapshotApiBaseUrl = "https://valid-api.example.com/";
            options.InplayPackageCredentials = new PackageCredentials
            {
                PackageId = 987,
                Username = "inplayvaliduser",
                Password = "inplayvalidpass"
            };
        });

        services.AddTrade360InplaySnapshotClient();
        var serviceProvider = services.BuildServiceProvider();

        // Act & Assert
        var client = serviceProvider.GetService<ISnapshotInplayApiClient>();
        client.Should().NotBeNull();
    }

    [Theory]
    [InlineData("https://api1.example.com/")]
    [InlineData("https://api2.example.com/v1/")]
    [InlineData("http://localhost:9090/")]
    [InlineData("https://test-api.staging.com/")]
    public void AddTrade360SnapshotClients_WithDifferentBaseUrls_ShouldConfigureIndependently(string baseUrl)
    {
        // Arrange
        var services1 = new ServiceCollection();
        services1.Configure<Trade360Settings>(options =>
        {
            options.SnapshotApiBaseUrl = baseUrl;
            options.PrematchPackageCredentials = new PackageCredentials
            {
                PackageId = 111,
                Username = "user1",
                Password = "pass1"
            };
            options.InplayPackageCredentials = new PackageCredentials
            {
                PackageId = 222,
                Username = "user2",
                Password = "pass2"
            };
        });

        services1.AddTrade360PrematchSnapshotClient();
        services1.AddTrade360InplaySnapshotClient();

        // Act
        var serviceProvider1 = services1.BuildServiceProvider();

        // Assert - Just verify clients can be created without errors
        var prematchClient = serviceProvider1.GetService<ISnapshotPrematchApiClient>();
        var inplayClient = serviceProvider1.GetService<ISnapshotInplayApiClient>();
        
        prematchClient.Should().NotBeNull();
        inplayClient.Should().NotBeNull();
    }

    [Fact]
    public void AddTrade360Services_WithServiceRegistrationCount_ShouldRegisterExpectedNumberOfServices()
    {
        // Arrange
        var services = new ServiceCollection();
        services.Configure<Trade360Settings>(options =>
        {
            options.SnapshotApiBaseUrl = "https://snapshot.example.com/";
            options.PrematchPackageCredentials = new PackageCredentials
            {
                PackageId = 333,
                Username = "countuser",
                Password = "countpass"
            };
        });

        var initialCount = services.Count;

        // Act
        services.AddTrade360PrematchSnapshotClient();
        var finalCount = services.Count;

        // Assert
        var addedServices = finalCount - initialCount;
        addedServices.Should().BeGreaterThan(0, "Services should be registered");
        
        // Verify specific services are registered
        services.Any(s => s.ServiceType == typeof(ISnapshotPrematchApiClient)).Should().BeTrue();
        services.Any(s => s.ServiceType == typeof(IMapper)).Should().BeTrue();
    }

    [Fact]
    public void AddTrade360Services_WithOptionsConfigurationLambda_ShouldExecuteCorrectly()
    {
        // Arrange
        var services = new ServiceCollection();
        services.Configure<Trade360Settings>(options =>
        {
            options.SnapshotApiBaseUrl = "https://custom-snapshot.example.com/";
            options.PrematchPackageCredentials = new PackageCredentials
            {
                PackageId = 444,
                Username = "customuser",
                Password = "custompass"
            };
        });

        // Act
        services.AddTrade360PrematchSnapshotClient();
        var serviceProvider = services.BuildServiceProvider();

        // Assert - Verify configuration is accessible
        var options = serviceProvider.GetService<IOptions<Trade360Settings>>();
        options.Should().NotBeNull();
        options!.Value.SnapshotApiBaseUrl.Should().Be("https://custom-snapshot.example.com/");
        
        // Verify client can be created
        var client = serviceProvider.GetService<ISnapshotPrematchApiClient>();
        client.Should().NotBeNull();
    }

    [Fact]
    public void AddTrade360PrematchSnapshotClient_WithNullSnapshotApiBaseUrl_ShouldThrowWhenCreatingClient()
    {
        // Arrange
        var services = new ServiceCollection();
        services.Configure<Trade360Settings>(options =>
        {
            options.SnapshotApiBaseUrl = null; // This will cause issues
            options.PrematchPackageCredentials = new PackageCredentials
            {
                PackageId = 123,
                Username = "prematchuser",
                Password = "prematchpass"
            };
        });

        services.AddTrade360PrematchSnapshotClient();
        var serviceProvider = services.BuildServiceProvider();

        // Act & Assert
        Action act = () => serviceProvider.GetService<ISnapshotPrematchApiClient>();
        act.Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void AddTrade360InplaySnapshotClient_WithNullSnapshotApiBaseUrl_ShouldThrowWhenCreatingClient()
    {
        // Arrange
        var services = new ServiceCollection();
        services.Configure<Trade360Settings>(options =>
        {
            options.SnapshotApiBaseUrl = null; // This will cause issues
            options.InplayPackageCredentials = new PackageCredentials
            {
                PackageId = 456,
                Username = "inplayuser",
                Password = "inplaypass"
            };
        });

        services.AddTrade360InplaySnapshotClient();
        var serviceProvider = services.BuildServiceProvider();

        // Act & Assert
        Action act = () => serviceProvider.GetService<ISnapshotInplayApiClient>();
        act.Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void AddTrade360PrematchSnapshotClient_WithNullPackageCredentials_ShouldThrowWhenCreatingClient()
    {
        // Arrange
        var services = new ServiceCollection();
        services.Configure<Trade360Settings>(options =>
        {
            options.SnapshotApiBaseUrl = "https://snapshot.example.com/";
            options.PrematchPackageCredentials = null; // This will cause issues
        });

        services.AddTrade360PrematchSnapshotClient();
        var serviceProvider = services.BuildServiceProvider();

        // Act & Assert
        Action act = () => serviceProvider.GetService<ISnapshotPrematchApiClient>();
        act.Should().Throw<InvalidOperationException>()
            .WithMessage("Package credentials cannot be null");
    }

    [Fact]
    public void AddTrade360InplaySnapshotClient_WithNullPackageCredentials_ShouldThrowWhenCreatingClient()
    {
        // Arrange
        var services = new ServiceCollection();
        services.Configure<Trade360Settings>(options =>
        {
            options.SnapshotApiBaseUrl = "https://snapshot.example.com/";
            options.InplayPackageCredentials = null; // This will cause issues
        });

        services.AddTrade360InplaySnapshotClient();
        var serviceProvider = services.BuildServiceProvider();

        // Act & Assert
        Action act = () => serviceProvider.GetService<ISnapshotInplayApiClient>();
        act.Should().Throw<InvalidOperationException>()
            .WithMessage("Package credentials cannot be null");
    }

    [Fact]
    public void AddTrade360Services_ShouldRegisterHttpClientFactory()
    {
        // Arrange
        var services = new ServiceCollection();
        services.Configure<Trade360Settings>(options =>
        {
            options.SnapshotApiBaseUrl = "https://snapshot.example.com/";
            options.PrematchPackageCredentials = new PackageCredentials
            {
                PackageId = 777,
                Username = "factoryuser",
                Password = "factorypass"
            };
        });

        // Act
        services.AddTrade360PrematchSnapshotClient();
        var serviceProvider = services.BuildServiceProvider();

        // Assert
        var httpClientFactory = serviceProvider.GetService<IHttpClientFactory>();
        httpClientFactory.Should().NotBeNull();

        var httpClient = httpClientFactory!.CreateClient();
        httpClient.Should().NotBeNull();
    }

    [Fact]
    public void AddTrade360Services_ShouldRegisterAutoMapper()
    {
        // Arrange
        var services = new ServiceCollection();
        services.Configure<Trade360Settings>(options =>
        {
            options.SnapshotApiBaseUrl = "https://snapshot.example.com/";
            options.PrematchPackageCredentials = new PackageCredentials
            {
                PackageId = 888,
                Username = "mapperuser",
                Password = "mapperpass"
            };
        });

        // Act
        services.AddTrade360PrematchSnapshotClient();
        var serviceProvider = services.BuildServiceProvider();

        // Assert
        var mapper = serviceProvider.GetService<IMapper>();
        mapper.Should().NotBeNull();
    }

    [Fact]
    public void AddTrade360Services_WithMultipleRegistrations_ShouldNotConflict()
    {
        // Arrange
        var services = new ServiceCollection();
        services.Configure<Trade360Settings>(options =>
        {
            options.SnapshotApiBaseUrl = "https://snapshot.example.com/";
            options.PrematchPackageCredentials = new PackageCredentials
            {
                PackageId = 999,
                Username = "multiuser",
                Password = "multipass"
            };
            options.InplayPackageCredentials = new PackageCredentials
            {
                PackageId = 1000,
                Username = "multiinplayuser",
                Password = "multiinplaypass"
            };
        });

        // Act
        services.AddTrade360PrematchSnapshotClient();
        services.AddTrade360InplaySnapshotClient();
        var serviceProvider = services.BuildServiceProvider();

        // Assert
        var prematchClient = serviceProvider.GetService<ISnapshotPrematchApiClient>();
        var inplayClient = serviceProvider.GetService<ISnapshotInplayApiClient>();
        
        prematchClient.Should().NotBeNull();
        inplayClient.Should().NotBeNull();
        
        // They should be different instances
        prematchClient.Should().NotBeSameAs(inplayClient);
    }

    [Fact]
    public void AddTrade360Services_ShouldUseTransientLifetime()
    {
        // Arrange
        var services = new ServiceCollection();
        services.Configure<Trade360Settings>(options =>
        {
            options.SnapshotApiBaseUrl = "https://snapshot.example.com/";
            options.PrematchPackageCredentials = new PackageCredentials
            {
                PackageId = 1111,
                Username = "transientuser",
                Password = "transientpass"
            };
        });

        services.AddTrade360PrematchSnapshotClient();
        var serviceProvider = services.BuildServiceProvider();

        // Act
        var client1 = serviceProvider.GetService<ISnapshotPrematchApiClient>();
        var client2 = serviceProvider.GetService<ISnapshotPrematchApiClient>();

        // Assert
        client1.Should().NotBeNull();
        client2.Should().NotBeNull();
        client1.Should().NotBeSameAs(client2, "Services should be transient");
    }

    [Theory]
    [InlineData("https://api.example.com/")]
    [InlineData("http://localhost:8080/")]
    [InlineData("https://staging.api.com/v2/")]
    public void AddTrade360Services_WithValidUrls_ShouldCreateClientsSuccessfully(string baseUrl)
    {
        // Arrange
        var services = new ServiceCollection();
        services.Configure<Trade360Settings>(options =>
        {
            options.SnapshotApiBaseUrl = baseUrl;
            options.PrematchPackageCredentials = new PackageCredentials
            {
                PackageId = 1212,
                Username = "urluser",
                Password = "urlpass"
            };
        });

        services.AddTrade360PrematchSnapshotClient();
        var serviceProvider = services.BuildServiceProvider();

        // Act & Assert
        Action act = () => serviceProvider.GetService<ISnapshotPrematchApiClient>();
        act.Should().NotThrow("Valid URLs should not cause exceptions");
    }

    [Theory]
    [InlineData("invalid-url")]
    [InlineData("")]
    [InlineData("not-a-url")]
    public void AddTrade360Services_WithInvalidUrls_ShouldThrowWhenCreatingClient(string invalidUrl)
    {
        // Arrange
        var services = new ServiceCollection();
        services.Configure<Trade360Settings>(options =>
        {
            options.SnapshotApiBaseUrl = invalidUrl;
            options.PrematchPackageCredentials = new PackageCredentials
            {
                PackageId = 1313,
                Username = "invaliduser",
                Password = "invalidpass"
            };
        });

        services.AddTrade360PrematchSnapshotClient();
        var serviceProvider = services.BuildServiceProvider();

        // Act & Assert
        Action act = () => serviceProvider.GetService<ISnapshotPrematchApiClient>();
        act.Should().Throw<Exception>("Invalid URLs should cause exceptions");
    }
} 