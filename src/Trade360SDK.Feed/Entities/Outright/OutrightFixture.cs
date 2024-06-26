using System;
using System.Collections.Generic;
using Trade360SDK.Feed.Entities.Enums;
using Trade360SDK.Feed.Entities.Fixtures;
using Trade360SDK.Feed.Entities.Shared;

namespace Trade360SDK.Feed.Entities.Outright
{
    public class OutrightFixture
    {
        public Sport? Sport { get; set; }

        public Location? Location { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime LastUpdate { get; set; }

        public FixtureStatus Status { get; set; }

        public IEnumerable<Participant>? Participants { get; set; }

        public IEnumerable<NameValuePair>? FixtureExtraData { get; set; }

        public string? ExternalFixtureId { get; set; }
    }
}
