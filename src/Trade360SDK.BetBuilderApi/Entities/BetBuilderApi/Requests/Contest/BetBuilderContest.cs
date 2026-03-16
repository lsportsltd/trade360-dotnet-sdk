using System.Collections.Generic;

namespace Trade360SDK.BetBuilderApi.Entities.BetBuilderApi.Requests
{
    public class BetBuilderContest
    {
        public int SportId { get; set; }

        public int LocationId { get; set; }

        public int TournamentId { get; set; }

        public bool IsWithExtraTime { get; set; }

        public bool IsPlayoff { get; set; }

        public Dictionary<string, BetBuilderParticipant>? Participants { get; set; }

        public BetBuilderScoreboard? Scoreboard { get; set; }
    }

    public class BetBuilderScoreboard
    {
        public BetBuilderCurrentPhase? CurrentPhase { get; set; }

        public BetBuilderPhases? Phases { get; set; }
    }

    public class BetBuilderCurrentPhase
    {
        public string? Name { get; set; }

        public string? Status { get; set; }

        public BetBuilderCurrentState? CurrentState { get; set; }
    }

    public class BetBuilderCurrentState
    {
        public string? Possession { get; set; }

        public string? FirstPossession { get; set; }

        public string? Period { get; set; }

        public string? Time { get; set; }

        public int? Down { get; set; }

        public int? YardsToFirstDown { get; set; }

        public int? YardsToEndZone { get; set; }
    }

    public class BetBuilderPhases
    {
        public BetBuilderTimeInterval? TimeInterval { get; set; }
    }

    public class BetBuilderTimeInterval
    {
        public string? Type { get; set; }

        public string? Time { get; set; }

        public IEnumerable<BetBuilderIncident>? Incidents { get; set; }
    }

    public class BetBuilderIncident
    {
        /// <summary>
        /// Incident type (e.g., "goals", "corners", "cards").
        /// </summary>
        public string? IncidentType { get; set; }

        public string? Time { get; set; }

        public string? Team { get; set; }

        public string? Player { get; set; }

        public string? Type { get; set; }

        public string? Value { get; set; }
    }
}
