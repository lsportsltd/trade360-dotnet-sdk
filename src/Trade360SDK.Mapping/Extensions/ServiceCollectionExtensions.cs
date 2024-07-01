using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using Trade360SDK.Common;
using Trade360SDK.CustomersApi.MetadataApi;
using Trade360SDK.Metadata;
using AutoMapper;
using Trade360SDK.CustomersApi.Mapper;



public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddT360ApiClient(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<Trade360ApiSettings>(configuration.GetSection("Trade360:CustomersApi"));

        // Register HttpClient and clients
        services.AddHttpClient<IMetadataClient, MetadataClient>((serviceProvider, client) =>
        {
            var options = serviceProvider.GetRequiredService<IOptions<Trade360ApiSettings>>().Value;
            client.BaseAddress = new Uri(options.BaseUrl);
        });

        // Register services
        services.AddTransient<IMetadataClient, MetadataClient>();
        services.AddAutoMapper(typeof(MappingProfile));

        return services;
    }
}