using System;
using System.Net.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Polly;
using Polly.Extensions.Http;
using Trade360SDK.Common.Configuration;
using Trade360SDK.SnapshotApi.Interfaces;
using Trade360SDK.SnapshotApi.Mapper;

namespace Trade360SDK.SnapshotApi.Extensions
{
    public static class ServiceCollectionExtensions
    {
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
            // Register HttpClients
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
