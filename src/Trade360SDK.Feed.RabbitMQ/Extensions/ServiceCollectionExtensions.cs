using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Trade360SDK.Microsoft.DependencyInjection.Extensions;

namespace Trade360SDK.Feed.RabbitMQ.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddT360RmqFeedSdk(this IServiceCollection services, IConfiguration configuration)
        {
            // Register the factory
            services.AddSingleton<IFeedFactory, RabbitMqFeedFactory>();
            services.AddTrade360CustomerApiClient(configuration);
            return services;
        }
    }
}