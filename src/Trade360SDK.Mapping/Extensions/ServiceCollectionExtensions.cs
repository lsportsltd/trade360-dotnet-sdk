using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using Trade360SDK.Common;
using Trade360SDK.CustomersApi.MetadataApi;
using Trade360SDK.Metadata;
using Trade360SDK.CustomersApi.Mapper;
using Trade360SDK.CustomersApi.PackageDistributionClient;



public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddT360ApiClient(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<Trade360ApiSettings>(configuration.GetSection("Trade360:CustomersApi"));

        // Register HttpClients
        AddHttpClientWithBaseAddress<IMetadataClient, MetadataClient>(services);
        AddHttpClientWithBaseAddress<IPackageDistributionClient, PackageDistributionClient>(services);

        // Register services
        services.AddTransient<IMetadataClient, MetadataClient>();
        services.AddTransient<IPackageDistributionClient, PackageDistributionClient>();
        services.AddAutoMapper(typeof(MappingProfile));

        return services;
    }

    private static void AddHttpClientWithBaseAddress<TClient, TImplementation>(IServiceCollection services)
    where TClient : class
    where TImplementation : class, TClient
    {
        services.AddHttpClient<TClient, TImplementation>((serviceProvider, client) =>
        {
            var options = serviceProvider.GetRequiredService<IOptions<Trade360ApiSettings>>().Value;
            client.BaseAddress = new Uri(options.BaseUrl);
        });
    }
}