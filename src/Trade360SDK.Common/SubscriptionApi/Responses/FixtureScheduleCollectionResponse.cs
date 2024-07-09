using System;
using System.Collections.Generic;
using Trade360SDK.Api.Abstraction.MetadataApi.Responses;

namespace Trade360SDK.Api.Abstraction.SubscriptionApi.Responses
{
    public class FixtureScheduleCollectionResponse
    {
        public IEnumerable<FixtureSchedule>? Fixtures { get; set; }
    }

    public class FixtureSchedule
    {
        public int FixtureId { get; set; }
        public Sport Sport { get; set; }
        public Location Location { get; set; }
        public League League { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime LastUpdate { get; set; }
        public int Status { get; set; }
        public List<ParticipantSchedule> Participants { get; set; }
    }

    public class ParticipantSchedule
    {
        public int Id { get; set; }
        public int Position { get; set; }
        public string Name { get; set; }
    }
}
