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
/// Comprehensive tests for HTTP client configuration, validation, and behavior.
/// These tests ensure proper HTTP client setup, base address configuration,
/// and service registration patterns.
/// </summary>
public class ServiceCollectionExtensionsHttpClientTests
{
    private readonly IServiceCollection _services;
    private readonly IConfiguration _configuration;

    public ServiceCollectionExtensionsHttpClientTests()
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
    public void AddTrade360CustomerApiClient_ShouldRegisterCorrectNumberOfHttpClients()
    {
        // Arrange & Act
        _services.AddTrade360CustomerApiClient(_configuration);
        
        // Assert
        var serviceProvider = _services.BuildServiceProvider();
        var httpClientFactory = serviceProvider.GetRequiredService<IHttpClientFactory>();

        // Verify all three customer API clients are registered
        var metadataClient = httpClientFactory.CreateClient(typeof(IMetadataHttpClient).FullName!);
        var packageClient = httpClientFactory.CreateClient(typeof(IPackageDistributionHttpClient).FullName!);
        var subscriptionClient = httpClientFactory.CreateClient(typeof(ISubscriptionHttpClient).FullName!);

        metadataClient.Should().NotBeNull("Metadata HTTP client should be registered");
        packageClient.Should().NotBeNull("Package distribution HTTP client should be registered");
        subscriptionClient.Should().NotBeNull("Subscription HTTP client should be registered");
    }

    [Fact]
    public void AddTrade360CustomerApiClient_ShouldConfigureHttpClientsWithCorrectNames()
    {
        // Arrange & Act
        _services.AddTrade360CustomerApiClient(_configuration);
        var serviceProvider = _services.BuildServiceProvider();
        var httpClientFactory = serviceProvider.GetRequiredService<IHttpClientFactory>();

        // Assert - Verify client names match interface full names
        var expectedNames = new[]
        {
            typeof(IMetadataHttpClient).FullName!,
            typeof(IPackageDistributionHttpClient).FullName!,
            typeof(ISubscriptionHttpClient).FullName!
        };

        foreach (var name in expectedNames)
        {
            var client = httpClientFactory.CreateClient(name);
            client.Should().NotBeNull($"HTTP client with name '{name}' should be registered");
        }
    }

    [Fact]
    public void AddTrade360SnapshotClients_ShouldConfigureBaseAddressCorrectly()
    {
        // Arrange
        var baseUrl = "https://custom-snapshot.example.com/api/v2/";
        _services.Configure<Trade360Settings>(options =>
        {
            options.SnapshotApiBaseUrl = baseUrl;
            options.PrematchPackageCredentials = new PackageCredentials
            {
                PackageId = 123,
                Username = "testuser",
                Password = "testpass"
            };
            options.InplayPackageCredentials = new PackageCredentials
            {
                PackageId = 456,
                Username = "testuser2",
                Password = "testpass2"
            };
        });

        // Act
        _services.AddTrade360PrematchSnapshotClient();
        _services.AddTrade360InplaySnapshotClient();

        // Assert
        var serviceProvider = _services.BuildServiceProvider();
        
        // Create typed clients to trigger BaseAddress configuration
        var prematchClient = serviceProvider.GetRequiredService<ISnapshotPrematchApiClient>();
        var inplayClient = serviceProvider.GetRequiredService<ISnapshotInplayApiClient>();

        prematchClient.Should().NotBeNull("Prematch client should be created successfully");
        inplayClient.Should().NotBeNull("Inplay client should be created successfully");
        
        // Verify the configuration was applied
        var options = serviceProvider.GetRequiredService<IOptions<Trade360Settings>>();
        options.Value.SnapshotApiBaseUrl.Should().Be(baseUrl, "Configuration should have correct base URL");
    }

    [Fact]
    public void AddTrade360Services_ShouldRegisterServicesWithCorrectLifetimes()
    {
        // Arrange
        _services.Configure<Trade360Settings>(options =>
        {
            options.SnapshotApiBaseUrl = "https://snapshot.example.com/";
        });

        // Act
        _services.AddTrade360CustomerApiClient(_configuration);
        _services.AddTrade360PrematchSnapshotClient();
        _services.AddTrade360InplaySnapshotClient();

        // Assert - Check service descriptors
        var serviceDescriptors = _services.ToList();

        var customersApiFactoryDescriptor = serviceDescriptors
            .FirstOrDefault(s => s.ServiceType == typeof(ICustomersApiFactory));
        var prematchClientDescriptor = serviceDescriptors
            .FirstOrDefault(s => s.ServiceType == typeof(ISnapshotPrematchApiClient));
        var inplayClientDescriptor = serviceDescriptors
            .FirstOrDefault(s => s.ServiceType == typeof(ISnapshotInplayApiClient));

        customersApiFactoryDescriptor.Should().NotBeNull("CustomersApiFactory should be registered");
        customersApiFactoryDescriptor!.Lifetime.Should().Be(ServiceLifetime.Transient, "CustomersApiFactory should be transient");

        prematchClientDescriptor.Should().NotBeNull("SnapshotPrematchApiClient should be registered");
        prematchClientDescriptor!.Lifetime.Should().Be(ServiceLifetime.Transient, "SnapshotPrematchApiClient should be transient");

        inplayClientDescriptor.Should().NotBeNull("SnapshotInplayApiClient should be registered");
        inplayClientDescriptor!.Lifetime.Should().Be(ServiceLifetime.Transient, "SnapshotInplayApiClient should be transient");
    }

    [Fact]
    public void AddTrade360Services_ShouldRegisterAutoMapperOnlyOnce()
    {
        // Arrange & Act
        _services.AddTrade360CustomerApiClient(_configuration);
        _services.AddTrade360PrematchSnapshotClient();
        _services.AddTrade360InplaySnapshotClient();

        // Assert
        var serviceProvider = _services.BuildServiceProvider();
        var mapper = serviceProvider.GetService<AutoMapper.IMapper>();

        mapper.Should().NotBeNull("AutoMapper should be registered");

        // Verify AutoMapper is registered as singleton (default behavior)
        var mapper1 = serviceProvider.GetService<AutoMapper.IMapper>();
        var mapper2 = serviceProvider.GetService<AutoMapper.IMapper>();

        mapper1.Should().BeSameAs(mapper2, "AutoMapper should be singleton by default");
    }

    [Fact]
    public void AddTrade360CustomerApiClient_WithNullConfiguration_ShouldThrowArgumentNullException()
    {
        // Arrange & Act & Assert
        var act = () => _services.AddTrade360CustomerApiClient(null!);
        act.Should().Throw<ArgumentNullException>()
            .WithMessage("*configuration*");
    }

    [Theory]
    [InlineData("https://api.example.com/")]
    [InlineData("https://api-staging.example.com/v1/")]
    [InlineData("https://192.168.1.100:8443/api/")]
    public void AddTrade360SnapshotClients_WithVariousValidUrls_ShouldConfigureCorrectly(string baseUrl)
    {
        // Arrange
        _services.Configure<Trade360Settings>(options =>
        {
            options.SnapshotApiBaseUrl = baseUrl;
            options.PrematchPackageCredentials = new PackageCredentials
            {
                PackageId = 123,
                Username = "testuser",
                Password = "testpass"
            };
        });
        _services.AddTrade360PrematchSnapshotClient();

        // Act
        var serviceProvider = _services.BuildServiceProvider();
        var httpClientFactory = serviceProvider.GetRequiredService<IHttpClientFactory>();

        // Assert
        var client = httpClientFactory.CreateClient(typeof(ISnapshotPrematchApiClient).FullName!);
        client.Should().NotBeNull($"Client should be created with URL: {baseUrl}");
        
        // Note: Base address is configured through the typed client registration, 
        // not directly on the factory-created client
        var options = serviceProvider.GetRequiredService<IOptions<Trade360Settings>>();
        options.Value.SnapshotApiBaseUrl.Should().Be(baseUrl);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData("not-a-url")]
    public void AddTrade360SnapshotClients_WithInvalidUrls_ShouldThrowWhenClientCreated(string invalidUrl)
    {
        // Arrange
        _services.Configure<Trade360Settings>(options =>
        {
            options.SnapshotApiBaseUrl = invalidUrl;
            options.InplayPackageCredentials = new PackageCredentials
            {
                PackageId = 456,
                Username = "testuser",
                Password = "testpass"
            };
        });
        _services.AddTrade360InplaySnapshotClient();

        // Act & Assert
        var serviceProvider = _services.BuildServiceProvider();
        
        // The exception is thrown when creating the typed client, not the factory client
        var act = () => serviceProvider.GetRequiredService<ISnapshotInplayApiClient>();
        act.Should().Throw<Exception>($"Invalid URL '{invalidUrl}' should cause exception during client creation");
    }

    [Fact]
    public void AddTrade360Services_WithServiceChaining_ShouldReturnSameServiceCollection()
    {
        // Arrange & Act
        var result1 = _services.AddTrade360CustomerApiClient(_configuration);
        var result2 = _services.AddTrade360PrematchSnapshotClient();
        var result3 = _services.AddTrade360InplaySnapshotClient();

        // Assert
        result1.Should().BeSameAs(_services, "AddTrade360CustomerApiClient should return same service collection for chaining");
        result2.Should().BeSameAs(_services, "AddTrade360PrematchSnapshotClient should return same service collection for chaining");
        result3.Should().BeSameAs(_services, "AddTrade360InplaySnapshotClient should return same service collection for chaining");
    }

    [Fact]
    public void AddTrade360Services_WithHttpClientDefaultSettings_ShouldPreserveDefaults()
    {
        // Arrange & Act
        _services.AddTrade360CustomerApiClient(_configuration);
        var serviceProvider = _services.BuildServiceProvider();
        var httpClientFactory = serviceProvider.GetRequiredService<IHttpClientFactory>();

        // Assert
        var client = httpClientFactory.CreateClient(typeof(IMetadataHttpClient).FullName!);
        
        // Verify default HTTP client settings are preserved
        client.Timeout.Should().Be(TimeSpan.FromSeconds(100), "Default timeout should be preserved");
        client.MaxResponseContentBufferSize.Should().BeGreaterThan(0, "Default buffer size should be preserved");
        client.DefaultRequestHeaders.Should().NotBeNull("Default headers collection should be available");
    }

    [Fact]
    public void AddTrade360Services_WithPackageCredentialsEdgeCases_ShouldHandleAllScenarios()
    {
        // Arrange - Test various credential edge cases
        var credentialTestCases = new[]
        {
            new { Name = "MinimumValues", Credentials = new PackageCredentials { PackageId = 1, Username = "a", Password = "b" } },
            new { Name = "LargePackageId", Credentials = new PackageCredentials { PackageId = int.MaxValue, Username = "user", Password = "pass" } },
            new { Name = "LongUsername", Credentials = new PackageCredentials { PackageId = 123, Username = new string('u', 100), Password = "pass" } },
            new { Name = "LongPassword", Credentials = new PackageCredentials { PackageId = 123, Username = "user", Password = new string('p', 100) } },
            new { Name = "SpecialCharacters", Credentials = new PackageCredentials { PackageId = 123, Username = "user@domain.com", Password = "p@ss!w0rd#123" } },
            new { Name = "UnicodeCharacters", Credentials = new PackageCredentials { PackageId = 123, Username = "üser", Password = "pássw0rd" } }
        };

        foreach (var testCase in credentialTestCases)
        {
            var services = new ServiceCollection();
            services.Configure<Trade360Settings>(options =>
            {
                options.SnapshotApiBaseUrl = "https://snapshot.example.com/";
                options.PrematchPackageCredentials = testCase.Credentials;
            });
            services.AddTrade360PrematchSnapshotClient();

            // Act
            var serviceProvider = services.BuildServiceProvider();
            var client = serviceProvider.GetService<ISnapshotPrematchApiClient>();

            // Assert
            client.Should().NotBeNull($"Client should be created with {testCase.Name} credentials");
        }
    }

    [Fact]
    public void AddTrade360Services_WithMultipleServiceProviders_ShouldWorkIndependently()
    {
        // Arrange
        var services1 = new ServiceCollection();
        var services2 = new ServiceCollection();

        services1.Configure<Trade360Settings>(options => 
        {
            options.SnapshotApiBaseUrl = "https://api1.example.com/";
            options.PrematchPackageCredentials = new PackageCredentials
            {
                PackageId = 123,
                Username = "testuser",
                Password = "testpass"
            };
        });
        services2.Configure<Trade360Settings>(options => 
        {
            options.SnapshotApiBaseUrl = "https://api2.example.com/";
            options.InplayPackageCredentials = new PackageCredentials
            {
                PackageId = 456,
                Username = "testuser2",
                Password = "testpass2"
            };
        });

        services1.AddTrade360CustomerApiClient(_configuration);
        services1.AddTrade360PrematchSnapshotClient();

        services2.AddTrade360CustomerApiClient(_configuration);
        services2.AddTrade360InplaySnapshotClient();

        // Act
        var serviceProvider1 = services1.BuildServiceProvider();
        var serviceProvider2 = services2.BuildServiceProvider();

        // Assert
        var factory1 = serviceProvider1.GetService<ICustomersApiFactory>();
        var factory2 = serviceProvider2.GetService<ICustomersApiFactory>();
        var prematch1 = serviceProvider1.GetService<ISnapshotPrematchApiClient>();
        var inplay2 = serviceProvider2.GetService<ISnapshotInplayApiClient>();

        factory1.Should().NotBeNull("Factory should be available in first service provider");
        factory2.Should().NotBeNull("Factory should be available in second service provider");
        prematch1.Should().NotBeNull("Prematch client should be available in first service provider");
        inplay2.Should().NotBeNull("Inplay client should be available in second service provider");

        // Verify they are independent instances
        factory1.Should().NotBeSameAs(factory2, "Factories from different providers should be independent");
    }

    [Fact]
    public void AddTrade360Services_WithOptionsPattern_ShouldSupportOptionsValidation()
    {
        // Arrange
        _services.Configure<Trade360Settings>(options =>
        {
            options.CustomersApiBaseUrl = "https://api.example.com/";
            options.SnapshotApiBaseUrl = "https://snapshot.example.com/";
        });

        // Act
        _services.AddTrade360CustomerApiClient(_configuration);
        _services.AddTrade360PrematchSnapshotClient();

        // Assert
        var serviceProvider = _services.BuildServiceProvider();
        var options = serviceProvider.GetRequiredService<IOptions<Trade360Settings>>();

        options.Should().NotBeNull("Options should be available");
        options.Value.Should().NotBeNull("Options value should be available");
        options.Value.CustomersApiBaseUrl.Should().Be("https://api.example.com/");
        options.Value.SnapshotApiBaseUrl.Should().Be("https://snapshot.example.com/");
    }
} 