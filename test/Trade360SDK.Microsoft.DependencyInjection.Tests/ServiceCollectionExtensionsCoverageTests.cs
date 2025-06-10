using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Trade360SDK.Microsoft.DependencyInjection.Extensions;

namespace Trade360SDK.Microsoft.DependencyInjection.Tests;

public class ServiceCollectionExtensionsCoverageTests
{
    [Fact]
    public void AddTrade360CustomerApiClient_WithNullConfiguration_ShouldExecuteValidation()
    {
        var services = new ServiceCollection();

        var act = () => services.AddTrade360CustomerApiClient(null!);

        act.Should().Throw<ArgumentNullException>()
           .WithParameterName("configuration");
    }

    [Fact]
    public void AddTrade360CustomerApiClient_WithValidConfiguration_ShouldRegisterServices()
    {
        var services = new ServiceCollection();
        var configuration = new ConfigurationBuilder().Build();

        var result = services.AddTrade360CustomerApiClient(configuration);

        result.Should().NotBeNull();
        result.Should().BeSameAs(services);
    }
}
