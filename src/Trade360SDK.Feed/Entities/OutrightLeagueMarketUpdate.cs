﻿using System.Collections.Generic;
using Trade360SDK.Feed.Attributes;
using Trade360SDK.Feed.Entities.Markets;
using Trade360SDK.Feed.Entities.Outright;

namespace Trade360SDK.Feed.Entities
{
    [Trade360Entity(40)]
    public class OutrightLeagueMarketUpdate
    {
        public IEnumerable<OutrightCompetition<MarketEvent>>? Competitions { get; set; }
    }
}