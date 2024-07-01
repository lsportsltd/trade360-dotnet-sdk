using Microsoft.Extensions.DependencyInjection;
using Trade360SDK.Feed.RabbitMQ;


public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddT360RmqFeedSdk(this IServiceCollection services)
    {
        // Register the factory
        services.AddSingleton<IRabbitMQFeedFactory, RabbitMQFeedFactory>();
        return services;
    }
}