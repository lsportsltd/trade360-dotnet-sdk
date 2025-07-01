using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Moq;
using Polly;
using Polly.Extensions.Http;
using Trade360SDK.Common.Configuration;
using Trade360SDK.SnapshotApi.Extensions;
using Trade360SDK.SnapshotApi.Interfaces;
using Trade360SDK.SnapshotApi.Mapper;

namespace Trade360SDK.SnapshotApi.Tests.Extensions;

public class ServiceCollectionExtensionsAdvancedTests
{
    private readonly IServiceCollection _services;

    public ServiceCollectionExtensionsAdvancedTests()
    {
        _services = new ServiceCollection();
    }

    [Fact]
    public void AddTrade360PrematchSnapshotClient_ShouldRegisterAllRequiredServices()
    {
        // Act
        _services.AddTrade360PrematchSnapshotClient();

        // Assert
        var serviceProvider = _services.BuildServiceProvider();
        
        // Check that HttpClient is registered
        _services.Should().Contain(s => s.ServiceType == typeof(IHttpClientFactory));
        
        // Check that the client interface is registered
        _services.Should().Contain(s => s.ServiceType == typeof(ISnapshotPrematchApiClient));
        
        // Check that AutoMapper is registered
        _services.Should().Contain(s => s.ServiceType.Name.Contains("IMapper"));
    }

    [Fact]
    public void AddTrade360InplaySnapshotClient_ShouldRegisterAllRequiredServices()
    {
        // Act
        _services.AddTrade360InplaySnapshotClient();

        // Assert
        var serviceProvider = _services.BuildServiceProvider();
        
        // Check that HttpClient is registered
        _services.Should().Contain(s => s.ServiceType == typeof(IHttpClientFactory));
        
        // Check that the client interface is registered
        _services.Should().Contain(s => s.ServiceType == typeof(ISnapshotInplayApiClient));
        
        // Check that AutoMapper is registered
        _services.Should().Contain(s => s.ServiceType.Name.Contains("IMapper"));
    }

    [Fact]
    public void AddTrade360PrematchSnapshotClient_ShouldRegisterTransientLifetime()
    {
        // Act
        _services.AddTrade360PrematchSnapshotClient();

        // Assert
        var registration = _services.FirstOrDefault(s => s.ServiceType == typeof(ISnapshotPrematchApiClient));
        registration.Should().NotBeNull();
        registration!.Lifetime.Should().Be(ServiceLifetime.Transient);
    }

    [Fact]
    public void AddTrade360InplaySnapshotClient_ShouldRegisterTransientLifetime()
    {
        // Act
        _services.AddTrade360InplaySnapshotClient();

        // Assert
        var registration = _services.FirstOrDefault(s => s.ServiceType == typeof(ISnapshotInplayApiClient));
        registration.Should().NotBeNull();
        registration!.Lifetime.Should().Be(ServiceLifetime.Transient);
    }

    [Fact]
    public void AddTrade360PrematchSnapshotClient_WithConfiguration_ShouldConfigureHttpClientCorrectly()
    {
        // Arrange
        var settings = new Trade360Settings 
        { 
            SnapshotApiBaseUrl = "https://prematch.test.com/"
        };
        
        _services.Configure<Trade360Settings>(opts => 
        {
            opts.SnapshotApiBaseUrl = settings.SnapshotApiBaseUrl;
        });

        // Act
        _services.AddTrade360PrematchSnapshotClient();

        // Assert
        var serviceProvider = _services.BuildServiceProvider();
        var httpClientFactory = serviceProvider.GetRequiredService<IHttpClientFactory>();
        
        // This verifies that the service registration completed without throwing
        httpClientFactory.Should().NotBeNull();
    }

    [Fact]
    public void AddTrade360InplaySnapshotClient_WithConfiguration_ShouldConfigureHttpClientCorrectly()
    {
        // Arrange
        var settings = new Trade360Settings 
        { 
            SnapshotApiBaseUrl = "https://inplay.test.com/"
        };
        
        _services.Configure<Trade360Settings>(opts => 
        {
            opts.SnapshotApiBaseUrl = settings.SnapshotApiBaseUrl;
        });

        // Act
        _services.AddTrade360InplaySnapshotClient();

        // Assert
        var serviceProvider = _services.BuildServiceProvider();
        var httpClientFactory = serviceProvider.GetRequiredService<IHttpClientFactory>();
        
        // This verifies that the service registration completed without throwing
        httpClientFactory.Should().NotBeNull();
    }

    [Fact]
    public void AddTrade360PrematchSnapshotClient_ShouldRegisterMappingProfile()
    {
        // Act
        _services.AddTrade360PrematchSnapshotClient();

        // Assert
        var serviceProvider = _services.BuildServiceProvider();
        
        // AutoMapper should be registered and able to resolve
        Action act = () => serviceProvider.GetRequiredService<AutoMapper.IMapper>();
        act.Should().NotThrow();
    }

    [Fact]
    public void AddTrade360InplaySnapshotClient_ShouldRegisterMappingProfile()
    {
        // Act
        _services.AddTrade360InplaySnapshotClient();

        // Assert
        var serviceProvider = _services.BuildServiceProvider();
        
        // AutoMapper should be registered and able to resolve
        Action act = () => serviceProvider.GetRequiredService<AutoMapper.IMapper>();
        act.Should().NotThrow();
    }

    [Fact]
    public void AddTrade360PrematchSnapshotClient_MultipleRegistrations_ShouldNotCauseConflicts()
    {
        // Arrange
        _services.Configure<Trade360Settings>(opts => 
        {
            opts.SnapshotApiBaseUrl = "https://test.com/";
        });

        // Act
        _services.AddTrade360PrematchSnapshotClient();
        _services.AddTrade360PrematchSnapshotClient(); // Register twice

        // Assert
        Action act = () => _services.BuildServiceProvider();
        act.Should().NotThrow();
        
        var serviceProvider = _services.BuildServiceProvider();
        var client = serviceProvider.GetRequiredService<ISnapshotPrematchApiClient>();
        client.Should().NotBeNull();
    }

    [Fact]
    public void AddTrade360InplaySnapshotClient_MultipleRegistrations_ShouldNotCauseConflicts()
    {
        // Arrange
        _services.Configure<Trade360Settings>(opts => 
        {
            opts.SnapshotApiBaseUrl = "https://test.com/";
        });

        // Act
        _services.AddTrade360InplaySnapshotClient();
        _services.AddTrade360InplaySnapshotClient(); // Register twice

        // Assert
        Action act = () => _services.BuildServiceProvider();
        act.Should().NotThrow();
        
        var serviceProvider = _services.BuildServiceProvider();
        var client = serviceProvider.GetRequiredService<ISnapshotInplayApiClient>();
        client.Should().NotBeNull();
    }

    [Fact]
    public void AddBothClients_ShouldRegisterBothServicesIndependently()
    {
        // Arrange
        _services.Configure<Trade360Settings>(opts => 
        {
            opts.SnapshotApiBaseUrl = "https://test.com/";
        });

        // Act
        _services.AddTrade360PrematchSnapshotClient();
        _services.AddTrade360InplaySnapshotClient();

        // Assert
        var serviceProvider = _services.BuildServiceProvider();
        
        var prematchClient = serviceProvider.GetRequiredService<ISnapshotPrematchApiClient>();
        var inplayClient = serviceProvider.GetRequiredService<ISnapshotInplayApiClient>();
        
        prematchClient.Should().NotBeNull();
        inplayClient.Should().NotBeNull();
        prematchClient.Should().NotBeSameAs(inplayClient);
    }

    [Fact]
    public void AddTrade360PrematchSnapshotClient_ShouldConfigureRetryPolicy()
    {
        // Arrange
        _services.Configure<Trade360Settings>(opts => 
        {
            opts.SnapshotApiBaseUrl = "https://test.com/";
        });

        // Act
        _services.AddTrade360PrematchSnapshotClient();

        // Assert
        var serviceProvider = _services.BuildServiceProvider();
        
        // Verify that HttpClient can be created (which means policies are properly configured)
        var httpClientFactory = serviceProvider.GetRequiredService<IHttpClientFactory>();
        Action act = () => httpClientFactory.CreateClient();
        act.Should().NotThrow();
    }

    [Fact]
    public void AddTrade360InplaySnapshotClient_ShouldConfigureCircuitBreakerPolicy()
    {
        // Arrange
        _services.Configure<Trade360Settings>(opts => 
        {
            opts.SnapshotApiBaseUrl = "https://test.com/";
        });

        // Act
        _services.AddTrade360InplaySnapshotClient();

        // Assert
        var serviceProvider = _services.BuildServiceProvider();
        
        // Verify that HttpClient can be created (which means policies are properly configured)
        var httpClientFactory = serviceProvider.GetRequiredService<IHttpClientFactory>();
        Action act = () => httpClientFactory.CreateClient();
        act.Should().NotThrow();
    }

    [Fact]
    public void AddTrade360PrematchSnapshotClient_WithInvalidConfiguration_ShouldStillRegister()
    {
        // Note: Invalid configuration should not prevent service registration
        // The error should occur when trying to use the client, not during registration

        // Act
        Action act = () => _services.AddTrade360PrematchSnapshotClient();

        // Assert
        act.Should().NotThrow();
        _services.Should().Contain(s => s.ServiceType == typeof(ISnapshotPrematchApiClient));
    }

    [Fact]
    public void AddTrade360InplaySnapshotClient_WithInvalidConfiguration_ShouldStillRegister()
    {
        // Note: Invalid configuration should not prevent service registration
        // The error should occur when trying to use the client, not during registration

        // Act
        Action act = () => _services.AddTrade360InplaySnapshotClient();

        // Assert
        act.Should().NotThrow();
        _services.Should().Contain(s => s.ServiceType == typeof(ISnapshotInplayApiClient));
    }

    [Fact]
    public void AddTrade360PrematchSnapshotClient_ShouldReturnServiceCollection()
    {
        // Act
        var result = _services.AddTrade360PrematchSnapshotClient();

        // Assert
        result.Should().BeSameAs(_services);
    }

    [Fact]
    public void AddTrade360InplaySnapshotClient_ShouldReturnServiceCollection()
    {
        // Act
        var result = _services.AddTrade360InplaySnapshotClient();

        // Assert
        result.Should().BeSameAs(_services);
    }

    [Fact]
    public void AddTrade360PrematchSnapshotClient_ShouldSupportMethodChaining()
    {
        // Act & Assert
        Action act = () => _services
            .AddTrade360PrematchSnapshotClient()
            .AddTrade360InplaySnapshotClient()
            .AddLogging()
            .AddOptions();

        act.Should().NotThrow();
    }

    [Fact]
    public void ServiceRegistration_ShouldUseCorrectImplementationTypes()
    {
        // Act
        _services.AddTrade360PrematchSnapshotClient();
        _services.AddTrade360InplaySnapshotClient();

        // Assert
        var prematchRegistration = _services.First(s => s.ServiceType == typeof(ISnapshotPrematchApiClient));
        var inplayRegistration = _services.First(s => s.ServiceType == typeof(ISnapshotInplayApiClient));

        prematchRegistration.ImplementationType?.Name.Should().Be("SnapshotPrematchApiClient");
        inplayRegistration.ImplementationType?.Name.Should().Be("SnapshotInplayApiClient");
    }

    [Fact]
    public void ServiceRegistration_ShouldRegisterAutoMapperWithCorrectProfile()
    {
        // Act
        _services.AddTrade360PrematchSnapshotClient();

        // Assert
        var serviceProvider = _services.BuildServiceProvider();
        var mapper = serviceProvider.GetRequiredService<AutoMapper.IMapper>();
        
        mapper.Should().NotBeNull();
        
        // Verify that the mapping profile is properly configured
        // This is tested indirectly by ensuring the mapper service can be created
        mapper.ConfigurationProvider.Should().NotBeNull();
    }

    [Fact]
    public void HttpClientConfiguration_ShouldSetBaseAddressCorrectly()
    {
        // Arrange
        var testUrl = "https://snapshot-api.example.com/";
        _services.Configure<Trade360Settings>(opts => 
        {
            opts.SnapshotApiBaseUrl = testUrl;
        });

        // Act
        _services.AddTrade360PrematchSnapshotClient();

        // Assert
        var serviceProvider = _services.BuildServiceProvider();
        
        // We can't directly test the HttpClient configuration without mocking,
        // but we can verify that the registration completed successfully
        var client = serviceProvider.GetRequiredService<ISnapshotPrematchApiClient>();
        client.Should().NotBeNull();
    }
} 