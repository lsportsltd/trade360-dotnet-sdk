﻿using Trade360SDK.Common.Entities.MessageTypes;
using Trade360SDK.Common.Models;
using Trade360SDK.Feed.FeedType;

namespace Trade360SDK.Feed.Example.Handlers.Prematch
{
    internal class FixtureMarketUpdateHandlerPrematch : IEntityHandler<MarketUpdate, PreMatch>
    {
        public Task ProcessAsync(MessageHeader? header, MarketUpdate? entity)
        {
            Console.WriteLine("MarketUpdate received");
            return Task.CompletedTask;
        }
    }
}
