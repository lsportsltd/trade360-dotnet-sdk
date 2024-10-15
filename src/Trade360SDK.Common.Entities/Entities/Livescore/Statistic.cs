using System.Collections.Generic;

namespace Trade360SDK.Common.Entities.Livescore
{
    public class Statistic
    {
        public int Type { get; set; }

        public IEnumerable<Result>? Results { get; set; }

        public IEnumerable<Incident>? Incidents { get; set; }
    }
}
