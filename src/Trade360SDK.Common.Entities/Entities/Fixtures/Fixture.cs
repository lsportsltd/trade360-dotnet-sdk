using System;
using System.Collections.Generic;
using Trade360SDK.Common.Entities.Enums;
using Trade360SDK.Common.Entities.Shared;

namespace Trade360SDK.Common.Entities.Fixtures
{
    public class Fixture
    {
        public string? FixtureName { get; set; }
        
        public Sport? Sport { get; set; }

        public Location? Location { get; set; }

        public League? League { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime LastUpdate { get; set; }

        public FixtureStatus Status { get; set; }

        public IEnumerable<Participant>? Participants { get; set; }

        public IEnumerable<NameValuePair>? FixtureExtraData { get; set; }

        public string? ExternalFixtureId { get; set; }

        public Subscription? Subscription { get; set; }

        public FixtureVenue Venue { get; set; }

        public IdNamePair Stage { get; set; }

        public IdNamePair Round { get; set; }
        
        public IdNamePair Season { get; set; }
    }
}
