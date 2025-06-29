using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Trade360SDK.Microsoft.DependencyInjection.Extensions;

namespace Trade360SDK.Microsoft.DependencyInjection.Tests;

public class ServiceCollectionExtensionsValidationTests
{
    [Fact]
    public void AddTrade360CustomerApiClient_WithNullConfiguration_ShouldThrowArgumentNullException()
    {
        var services = new ServiceCollection();

        var act = () => services.AddTrade360CustomerApiClient(null!);

        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void AddTrade360CustomerApiClient_WithValidConfiguration_ShouldRegisterServices()
    {
        var services = new ServiceCollection();
        var configuration = new ConfigurationBuilder().Build();

        var act = () => services.AddTrade360CustomerApiClient(configuration);

        act.Should().NotThrow();
        services.Should().NotBeEmpty();
    }
}
