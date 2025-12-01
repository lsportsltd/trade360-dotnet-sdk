using System.Collections.Generic;
using System;
using Trade360SDK.Common.Entities.Enums;
using Trade360SDK.Common.Entities.Fixtures;
using Trade360SDK.Common.Entities.Shared;

namespace Trade360SDK.Common.Entities.OutrightLeague
{
    public class OutrightLeagueFixture
    {
        public Subscription? Subscription { get; set; }
        public Sport? Sport { get; set; }

        public Location? Location { get; set; }

        public DateTime LastUpdate { get; set; }

        public FixtureStatus Status { get; set; }

        public IEnumerable<NameValuePair>? ExtraData { get; set; }
        
        public DateTime EndDate { get; set; }

    }
}
