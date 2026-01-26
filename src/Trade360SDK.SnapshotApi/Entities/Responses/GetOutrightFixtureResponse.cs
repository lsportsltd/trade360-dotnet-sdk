using System;
using System.Collections.Generic;
using Trade360SDK.Common.Entities.Enums;
using Trade360SDK.Common.Entities.Fixtures;
using Trade360SDK.Common.Entities.Shared;

namespace Trade360SDK.SnapshotApi.Entities.Responses
{
    public class GetOutrightFixtureResponse
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public int Type { get; set; }
        public IEnumerable<SnapshotOutrightFixtureEvent>? Events { get; set; }
    }

    public class SnapshotOutrightFixtureEvent
    {
        public int FixtureId { get; set; }
        public OutrightFixtureSnapshotResponse? OutrightFixture { get; set; }
    }

    public class OutrightFixtureSnapshotResponse
    {
        public string? FixtureName { get; set; }
        
        public Sport? Sport { get; set; }

        public Location? Location { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime LastUpdate { get; set; }

        public FixtureStatus Status { get; set; }

        public IEnumerable<Participant>? Participants { get; set; }

        public IEnumerable<NameValuePair>? ExtraData { get; set; }
        
        public Subscription? Subscription { get; set; }
        
        public FixtureVenue? Venue { get; set; }
        
        public IdNamePair? Season { get; set; }
    }

}
