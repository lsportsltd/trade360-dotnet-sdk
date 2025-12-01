using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Trade360SDK.Common.Entities.MessageTypes;
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
            services.AddSingleton<MessageProcessorContainer<InPlay>>();
            services.AddSingleton<MessageProcessorContainer<PreMatch>>();

            services
                .AddSingleton<IMessageProcessor, MessageProcessor<FixtureMetadataUpdate, InPlay>>()
                .AddSingleton<IMessageProcessor, MessageProcessor<HeartbeatUpdate, InPlay>>()
                .AddSingleton<IMessageProcessor, MessageProcessor<LivescoreUpdate, InPlay>>()
                .AddSingleton<IMessageProcessor, MessageProcessor<KeepAliveUpdate, InPlay>>()
                .AddSingleton<IMessageProcessor, MessageProcessor<SettlementUpdate, InPlay>>()
                .AddSingleton<IMessageProcessor, MessageProcessor<MarketUpdate, InPlay>>();
            
            services
                .AddSingleton<IMessageProcessor, MessageProcessor<OutrightLeagueUpdate, InPlay>>()
                .AddSingleton<IMessageProcessor, MessageProcessor<OutrightLeagueMarketUpdate, InPlay>>()
                .AddSingleton<IMessageProcessor, MessageProcessor<OutrightLeagueSettlementUpdate, InPlay>>();
        
            services
                .AddSingleton<IMessageProcessor, MessageProcessor<FixtureMetadataUpdate, PreMatch>>()
                .AddSingleton<IMessageProcessor, MessageProcessor<HeartbeatUpdate, PreMatch>>()
                .AddSingleton<IMessageProcessor, MessageProcessor<LivescoreUpdate, PreMatch>>()
                .AddSingleton<IMessageProcessor, MessageProcessor<SettlementUpdate, PreMatch>>()
                .AddSingleton<IMessageProcessor, MessageProcessor<MarketUpdate, PreMatch>>();

            services
                .AddSingleton<IMessageProcessor, MessageProcessor<OutrightFixtureUpdate, PreMatch>>()
                .AddSingleton<IMessageProcessor, MessageProcessor<OutrightFixtureMarketUpdate, PreMatch>>()
                .AddSingleton<IMessageProcessor, MessageProcessor<OutrightScoreUpdate, PreMatch>>()
                .AddSingleton<IMessageProcessor, MessageProcessor<OutrightSettlementsUpdate, PreMatch>>();
                    
            services
                .AddSingleton<IMessageProcessor, MessageProcessor<OutrightLeagueUpdate, PreMatch>>()
                .AddSingleton<IMessageProcessor, MessageProcessor<OutrightLeagueMarketUpdate, PreMatch>>()
                .AddSingleton<IMessageProcessor, MessageProcessor<OutrightLeagueSettlementUpdate, PreMatch>>();
            
            // Add CustomersApi client
            services.AddTrade360CustomerApiClient(configuration);
            return services;
        }
    }
}