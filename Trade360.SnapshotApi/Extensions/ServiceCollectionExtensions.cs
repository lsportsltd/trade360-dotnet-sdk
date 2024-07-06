using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using Trade360SDK.CustomersApi.Mapper;
using Trade360SDK.SnapshotApi.Interfaces;
using Trade360SDK.SnapshotApi.Configuration;
using Trade360SDK.SnapshotApi;



public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddT360ApiClient(this IServiceCollection services, IConfiguration configuration)
    {
        // Register HttpClients
        AddHttpClientWithBaseAddress<ISnapshotInplayApiClient, InplaySnapshotApiClient>(services);

        // Register services
        services.AddTransient<ISnapshotInplayApiClient, InplaySnapshotApiClient>();
        services.AddTransient<ISnapshotApiFactory, SnapshotApiFactory>();
        services.AddAutoMapper(typeof(MappingProfile));

        return services;
    }

    private static void AddHttpClientWithBaseAddress<TClient, TImplementation>(IServiceCollection services)
    where TClient : class
    where TImplementation : class, TClient
    {
        services.AddHttpClient<TClient, TImplementation>((serviceProvider, client) =>
        {
            var options = serviceProvider.GetRequiredService<IOptions<SnapshotApiSettings>>().Value;
            client.BaseAddress = new Uri(options.BaseUrl);
        });
    }
}