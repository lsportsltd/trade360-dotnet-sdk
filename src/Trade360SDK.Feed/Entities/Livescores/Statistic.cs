using System.Collections.Generic;
using Trade360SDK.Feed.Entities.Enums;

namespace Trade360SDK.Feed.Entities.Livescores
{
    public class Statistic
    {
        public StatisticType Type { get; set; }

        public IEnumerable<Result>? Results { get; set; }

        public IEnumerable<Incident>? Incidents { get; set; }
    }
}
