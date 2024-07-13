using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Trade360SDK.CustomersApi;
using Trade360SDK.CustomersApi.Extensions;
using Trade360SDK.CustomersApi.Interfaces;

namespace Trade360SDK.Feed.RabbitMQ.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddT360RmqFeedSdk(this IServiceCollection services, IConfiguration configuration)
        {
            // Register the factory
            services.AddSingleton<IFeedFactory, RabbitMqFeedFactory>();
            services.AddT360ApiClient(configuration);
            return services;
        }
    }
}