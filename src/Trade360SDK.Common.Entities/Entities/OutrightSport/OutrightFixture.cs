using System;
using System.Collections.Generic;
using Trade360SDK.Common.Entities.Enums;
using Trade360SDK.Common.Entities.Fixtures;
using Trade360SDK.Common.Entities.Shared;

namespace Trade360SDK.Common.Entities.OutrightSport
{
    public class OutrightFixture
    {
        public string? FixtureName { get; set; }
        
        public Sport? Sport { get; set; }

        public Location? Location { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime LastUpdate { get; set; }

        public FixtureStatus Status { get; set; }

        public IEnumerable<OutrightFixtureParticipant>? Participants { get; set; }

        public IEnumerable<NameValuePair>? ExtraData { get; set; }
        
        public Subscription? Subscription { get; set; }
        
        public FixtureVenue? Venue { get; set; }
        
        public IdNamePair? Season { get; set; }
    }
}
