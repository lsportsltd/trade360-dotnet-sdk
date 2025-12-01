using Microsoft.Extensions.DependencyInjection;
using Trade360SDK.Common.Entities.MessageTypes;
using Trade360SDK.Feed.Example.Handlers.Inplay;
using Trade360SDK.Feed.Example.Handlers.Prematch;
using Trade360SDK.Feed.FeedType;

namespace Trade360SDK.Feed.Example;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddTrade360Handlers(this IServiceCollection services)
    {
        services
            .AddScoped<IEntityHandler<FixtureMetadataUpdate, InPlay>, FixtureMetadataUpdateHandlerInplay>()
            .AddScoped<IEntityHandler<HeartbeatUpdate, InPlay>, HeartbeatHandlerInplay>()
            .AddScoped<IEntityHandler<LivescoreUpdate, InPlay>, LivescoreUpdateHandlerInplay>()
            .AddScoped<IEntityHandler<KeepAliveUpdate, InPlay>, KeepAliveUpdateHandlerInplay>()
            .AddScoped<IEntityHandler<SettlementUpdate, InPlay>, SettlementUpdateHandlerInplay>()
            .AddScoped<IEntityHandler<MarketUpdate, InPlay>, FixtureMarketUpdateHandlerInplay>();

        services
            .AddScoped<IEntityHandler<OutrightLeagueMarketUpdate, InPlay>, OutrightLeagueMarketsUpdateHandlerInplay>()
            .AddScoped<IEntityHandler<OutrightLeagueUpdate, InPlay>, OutrightLeagueUpdateHandlerInplay>()
            .AddScoped<IEntityHandler<OutrightLeagueSettlementUpdate, InPlay>, OutrightLeagueSettlementsUpdateHandlerInplay>();

        services
            .AddScoped<IEntityHandler<FixtureMetadataUpdate, PreMatch>, FixtureMetadataUpdateHandlerPrematch>()
            .AddScoped<IEntityHandler<HeartbeatUpdate, PreMatch>, HeartbeatHandlerPrematch>()
            .AddScoped<IEntityHandler<LivescoreUpdate, PreMatch>, LivescoreUpdateHandlerPrematch>()
            .AddScoped<IEntityHandler<SettlementUpdate, PreMatch>, SettlementUpdateHandlerPrematch>()
            .AddScoped<IEntityHandler<MarketUpdate, PreMatch>, FixtureMarketUpdateHandlerPrematch>();
        
        services
            .AddScoped<IEntityHandler<OutrightFixtureUpdate, PreMatch>, OutrightFixtureUpdateHandlerPrematch>()
            .AddScoped<IEntityHandler<OutrightScoreUpdate, PreMatch>, OutrightScoreUpdateHandlerPrematch>()
            .AddScoped<IEntityHandler<OutrightSettlementsUpdate, PreMatch>, OutrightSettlementsUpdateHandlerPrematch>()
            .AddScoped<IEntityHandler<OutrightFixtureMarketUpdate, PreMatch>,
                OutrightFixtureMarketUpdateHandlerPrematch>();
        
        services
            .AddScoped<IEntityHandler<OutrightLeagueMarketUpdate, PreMatch>, OutrightLeagueMarketsUpdateHandlerPrematch>()
            .AddScoped<IEntityHandler<OutrightLeagueUpdate, PreMatch>, OutrightLeagueUpdateHandlerPrematch>()
            .AddScoped<IEntityHandler<OutrightLeagueSettlementUpdate, PreMatch>, OutrightLeagueSettlementsUpdateHandlerPrematch>();
        
        return services;
    }
}