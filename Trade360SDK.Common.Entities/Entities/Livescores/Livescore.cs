using System.Collections.Generic;
using Trade360SDK.Common.Entities.Shared;

namespace Trade360SDK.Common.Entities.Livescores
{
    public class Livescore
    {
        public Scoreboard? Scoreboard { get; set; }

        public IEnumerable<Period>? Periods { get; set; }

        public IEnumerable<Statistic>? Statistics { get; set; }

        public IEnumerable<NameValuePair>? LivescoreExtraData { get; set; }
    }
}
