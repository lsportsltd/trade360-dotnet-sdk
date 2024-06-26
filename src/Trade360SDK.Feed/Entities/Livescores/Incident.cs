using System.Collections.Generic;

namespace Trade360SDK.Feed.Entities.Livescores
{
    public class Incident
    {
        public int Period { get; set; }

        public int IncidentType { get; set; }

        public int Seconds { get; set; }

        public string? ParticipantPosition { get; set; }

        public string? PlayerId { get; set; }

        public string? PlayerName { get; set; }

        public IEnumerable<Result>? Results { get; set; }
    }
}
