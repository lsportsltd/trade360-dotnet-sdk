using System;
using System.Net.Http;
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using Trade360SDK.SnapshotApi.Configuration;
using Trade360SDK.SnapshotApi.Interfaces;
using Trade360SDK.SnapshotApi.Mapper;

namespace Trade360SDK.SnapshotApi.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddTrade360PrematchSnapshotClient(this IServiceCollection services, SnapshotApiSettings inplaySnapshotApiSettings)
        {

            services.AddHttpClient<ISnapshotPrematchApiClient, SnapshotPrematchApiClient>((serviceProvider, client) =>
            {
                client.BaseAddress = new Uri(inplaySnapshotApiSettings.BaseUrl ?? throw new ArgumentNullException());
            });


            services.AddTransient<ISnapshotPrematchApiClient>(provider =>
            {
                var httpClientFactory = provider.GetRequiredService<IHttpClientFactory>();
                var mapper = provider.GetRequiredService<IMapper>();
                return new SnapshotPrematchApiClient(httpClientFactory, inplaySnapshotApiSettings, mapper);
            });

            services.AddAutoMapper(typeof(MappingProfile));

            return services;
        }

        public static IServiceCollection AddTrade360InplaySnapshotClient(this IServiceCollection services, SnapshotApiSettings prematchSnapshotApiSettings)
        {
            // Register HttpClients
            services.AddHttpClient<ISnapshotInplayApiClient, SnapshotInplayApiClient>((serviceProvider, client) =>
            {
                client.BaseAddress = new Uri(prematchSnapshotApiSettings.BaseUrl ?? throw new ArgumentNullException());
            });

           

            // Register services
            services.AddTransient<ISnapshotInplayApiClient>(provider =>
            {
                var httpClientFactory = provider.GetRequiredService<IHttpClientFactory>();
                var mapper = provider.GetRequiredService<IMapper>();
                return new SnapshotInplayApiClient(httpClientFactory, prematchSnapshotApiSettings, mapper);
            });

            services.AddAutoMapper(typeof(MappingProfile));

            return services;
        }
    }
}
