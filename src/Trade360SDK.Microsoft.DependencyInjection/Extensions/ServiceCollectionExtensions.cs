using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Polly;
using Polly.Extensions.Http;
using System.Net.Http;
using Trade360SDK.CustomersApi;
using Trade360SDK.CustomersApi.Interfaces;
using Trade360SDK.CustomersApi.Mapper;
using Trade360SDK.Common.Configuration;
using Trade360SDK.SnapshotApi;
using Trade360SDK.SnapshotApi.Interfaces;
using Microsoft.Extensions.Configuration;

namespace Trade360SDK.Microsoft.DependencyInjection.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddTrade360CustomerApiClient(this IServiceCollection services, IConfiguration configuration)
        {
            // Register HttpClients with resiliency policies
            services.AddHttpClient<IMetadataApiClient, MetadataApiClient>()
                .AddPolicyHandler(GetRetryPolicy())
                .AddPolicyHandler(GetCircuitBreakerPolicy());

            services.AddHttpClient<IPackageDistributionApiClient, PackageDistributionApiClient>()
                .AddPolicyHandler(GetRetryPolicy())
                .AddPolicyHandler(GetCircuitBreakerPolicy());

            services.AddHttpClient<ISubscriptionApiClient, SubscriptionApiClient>()
                .AddPolicyHandler(GetRetryPolicy())
                .AddPolicyHandler(GetCircuitBreakerPolicy());

            services.AddTransient<ICustomersApiFactory, CustomersApiFactory>();
            services.AddAutoMapper(typeof(MappingProfile));
            return services;
        }

        public static IServiceCollection AddTrade360PrematchSnapshotClient(this IServiceCollection services)
        {
            services.AddHttpClient<ISnapshotPrematchApiClient, SnapshotPrematchApiClient>()
                .ConfigureHttpClient((serviceProvider, client) =>
                {
                    var options = serviceProvider.GetRequiredService<IOptions<Trade360Settings>>().Value;
                    client.BaseAddress = new Uri(options.SnapshotApiBaseUrl ?? throw new ArgumentNullException(nameof(options.SnapshotApiBaseUrl)));
                })
                .AddPolicyHandler(GetRetryPolicy())
                .AddPolicyHandler(GetCircuitBreakerPolicy());

            services.AddTransient<ISnapshotPrematchApiClient, SnapshotPrematchApiClient>();
            services.AddAutoMapper(typeof(MappingProfile));

            return services;
        }

        public static IServiceCollection AddTrade360InplaySnapshotClient(this IServiceCollection services)
        {
            // Register HttpClients with resiliency policies
            services.AddHttpClient<ISnapshotInplayApiClient, SnapshotInplayApiClient>()
                .ConfigureHttpClient((serviceProvider, client) =>
                {
                    var options = serviceProvider.GetRequiredService<IOptions<Trade360Settings>>().Value;
                    client.BaseAddress = new Uri(options.SnapshotApiBaseUrl ?? throw new ArgumentNullException(nameof(options.SnapshotApiBaseUrl)));
                })
                .AddPolicyHandler(GetRetryPolicy())
                .AddPolicyHandler(GetCircuitBreakerPolicy());

            services.AddTransient<ISnapshotInplayApiClient, SnapshotInplayApiClient>();
            services.AddAutoMapper(typeof(MappingProfile));

            return services;
        }

        private static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
        {
            return HttpPolicyExtensions
                .HandleTransientHttpError()
                .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
        }

        private static IAsyncPolicy<HttpResponseMessage> GetCircuitBreakerPolicy()
        {
            return HttpPolicyExtensions
                .HandleTransientHttpError()
                .CircuitBreakerAsync(5, TimeSpan.FromSeconds(30));
        }
    }
}
