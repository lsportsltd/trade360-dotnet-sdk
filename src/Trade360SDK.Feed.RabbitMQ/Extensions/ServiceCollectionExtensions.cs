using Microsoft.Extensions.DependencyInjection;

namespace Trade360SDK.Feed.RabbitMQ.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddT360RmqFeedSdk(this IServiceCollection services)
        {
            // Register the factory
            services.AddSingleton<IFeedFactory, RabbitMqFeedFactory>();
            return services;
        }
    }
}