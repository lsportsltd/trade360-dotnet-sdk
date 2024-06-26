using System.Collections.Generic;
using Trade360SDK.Feed.Entities.Enums;

namespace Trade360SDK.Feed.Entities.Livescores
{
    public class Scoreboard
    {
        public FixtureStatus Status { get; set; }

        public StatusDescription Description { get; set; }

        public int CurrentPeriod { get; set; }

        public string? Time { get; set; }

        public IEnumerable<Result>? Results { get; set; }
    }
}

