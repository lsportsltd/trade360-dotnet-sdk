using System.Collections.Generic;
using Trade360SDK.Common.Entities.Enums;

namespace Trade360SDK.Common.Entities.Livescore
{
    public class Statistic
    {
        public StatisticType Type { get; set; }

        public IEnumerable<Result>? Results { get; set; }

        public IEnumerable<Incident>? Incidents { get; set; }
    }
}
