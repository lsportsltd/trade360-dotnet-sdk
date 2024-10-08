﻿using System.Collections.Generic;

namespace Trade360SDK.Common.Entities.Markets
{
    public class MarketEvent
    {
        public int FixtureId { get; set; }
        public IEnumerable<Market>? Markets { get; set; }
    }
}
