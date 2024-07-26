using System;
using System.Collections.Generic;

namespace Trade360SDK.CustomersApi.Entities.MetadataApi.Responses
{
    public class GetFixtureMetadataCollectionResponse
    {
        public IEnumerable<SubscribedFixture>? SubscribedFixtures { get; set; }
    }

    public class SubscribedFixture
    {
        public int FixtureId { get; set; }
        public string? FixtureName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime LastUpdate { get; set; }
        public int SportId { get; set; }
        public int LocationId { get; set; }
        public int LeagueId { get; set; }
        public int FixtureStatus { get; set; }
        public List<Participant>? Participants { get; set; }
    }

    public class Participant
    {
        public int ParticipantId { get; set; }
        public string? ParticipantName { get; set; }
    }
}
