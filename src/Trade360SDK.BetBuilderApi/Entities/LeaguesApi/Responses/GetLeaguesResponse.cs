using System.Collections.Generic;

namespace Trade360SDK.BetBuilderApi.Entities.LeaguesApi.Responses
{
    public class GetLeaguesResponse
    {
        public IEnumerable<LeagueEntry>? Leagues { get; set; }

        public string? CurrentVersion { get; set; }
    }

    public class LeagueEntry
    {
        public int SportId { get; set; }

        public string? SportName { get; set; }

        public int LeagueId { get; set; }

        public string? LeagueName { get; set; }

        public string? FormatName { get; set; }

        public string? FormatType { get; set; }

        public string? League { get; set; }

        public bool SupportsExtraTime { get; set; }

        public bool SupportsPlayoff { get; set; }

        public IEnumerable<string>? Periods { get; set; }

        public string? Status { get; set; }

        public string? Version { get; set; }
    }
}
