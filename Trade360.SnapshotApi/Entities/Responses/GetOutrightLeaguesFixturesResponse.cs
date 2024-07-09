using System;
using System.Collections.Generic;
using Trade360SDK.Common.Entities.Enums;
using Trade360SDK.Common.Entities.Fixtures;
using Trade360SDK.Common.Entities.Shared;

namespace Trade360SDK.SnapshotApi.Entities.Responses
{
    public class GetOutrightLeaguesFixturesResponse
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public int Type { get; set; }
        public IEnumerable<SnapshotOutrightCompetitionsResponse>? Competitions { get; set; }
    }

    public class SnapshotOutrightCompetitionsResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Type { get; set; }
        public IEnumerable<CompetitionResponse>? Events { get; set; }
    }

    public class CompetitionResponse
    {
        public int FixtureId { get; set; }
        public OutrightLeagueFixtureSnapshotResponse OutrightLeague { get; set; }
    }

    public class OutrightLeagueFixtureSnapshotResponse
    {
        public Subscription? Subscription { get; set; }
        public Sport? Sport { get; set; }

        public Location? Location { get; set; }

        public DateTime LastUpdate { get; set; }

        public FixtureStatus Status { get; set; }

        public IEnumerable<NameValuePair>? ExtraData { get; set; }
    }

}
