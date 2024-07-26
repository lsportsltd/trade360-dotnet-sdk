using Microsoft.Extensions.DependencyInjection;
using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Trade360SDK.CustomersApi;
using Trade360SDK.CustomersApi.Configuration;
using Trade360SDK.CustomersApi.Interfaces;
using Trade360SDK.CustomersApi.Mapper;

namespace Trade360SDK.Microsoft.DependencyInjection.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddT360ApiClient(this IServiceCollection services, IConfiguration configuration)
        {
            // Register HttpClients
            AddHttpClientWithBaseAddress<IMetadataApiClient, MetadataApiClient>(services);
            AddHttpClientWithBaseAddress<IPackageDistributionApiClient, PackageDistributionApiClient>(services);

            // Register services
            services.AddTransient<IMetadataApiClient, MetadataApiClient>();
            services.AddTransient<IPackageDistributionApiClient, PackageDistributionApiClient>();
            services.AddTransient<ICustomersApiFactory, CustomersApiFactory>();
            services.AddAutoMapper(typeof(MappingProfile));

            return services;
        }

        private static void AddHttpClientWithBaseAddress<TClient, TImplementation>(IServiceCollection services)
            where TClient : class
            where TImplementation : class, TClient
        {
            services.AddHttpClient<TClient, TImplementation>((serviceProvider, client) =>
            {
                var options = serviceProvider.GetRequiredService<IOptions<CustomersApiSettings>>().Value;
                client.BaseAddress = new Uri(options.BaseUrl);
            });
        }
    }
}