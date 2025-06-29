using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Trade360SDK.Microsoft.DependencyInjection.Extensions;

namespace Trade360SDK.Microsoft.DependencyInjection.Tests;

public class ServiceCollectionExtensionsActualTests
{
    [Fact]
    public void AddTrade360CustomerApiClient_WithNullConfiguration_ShouldThrowArgumentNullException()
    {
        var services = new ServiceCollection();

        var act = () => services.AddTrade360CustomerApiClient(null!);

        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void AddTrade360CustomerApiClient_WithValidConfiguration_ShouldRegisterHttpClients()
    {
        var services = new ServiceCollection();
        var configuration = new ConfigurationBuilder().Build();

        var result = services.AddTrade360CustomerApiClient(configuration);

        result.Should().NotBeNull();
        services.Should().NotBeEmpty();
    }

    [Fact]
    public void AddTrade360CustomerApiClient_WithNullConfiguration_ShouldExecuteActualValidationPath()
    {
        var services = new ServiceCollection();

        var act = () => services.AddTrade360CustomerApiClient(null!);

        act.Should().Throw<ArgumentNullException>();
    }




}
