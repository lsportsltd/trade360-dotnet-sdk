using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Trade360SDK.Feed.FeedType;
using Trade360SDK.Feed.RabbitMQ.Resolvers;
using Trade360SDK.Microsoft.DependencyInjection.Extensions;

namespace Trade360SDK.Feed.RabbitMQ.Extensions
{
    public static class FeedServiceCollectionExtensions
    {
        public static IServiceCollection AddT360RmqFeedSdk(this IServiceCollection services, IConfiguration configuration)
        {
            // Register the factory
            services.AddSingleton<IFeedFactory, RabbitMqFeedFactory>();
            // Add resolvers to find the appropriate client's handlers
            services.AddSingleton<HandlerTypeResolver<InPlay>>();
            services.AddSingleton<HandlerTypeResolver<PreMatch>>();
            // Add CustomersApi client
            services.AddTrade360CustomerApiClient(configuration);
            return services;
        }
    }
}