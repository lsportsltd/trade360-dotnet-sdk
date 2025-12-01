using System.Collections.Generic;
using Trade360SDK.Common.Entities.Shared;

namespace Trade360SDK.Common.Entities.Livescore
{
    public class Livescore
    {
        public Scoreboard? Scoreboard { get; set; }

        public IEnumerable<Period>? Periods { get; set; }

        public IEnumerable<Statistic>? Statistics { get; set; }
        
        public List<PlayerStatistic>? PlayerStatistics { get; set; }

        public IEnumerable<NameValuePair>? LivescoreExtraData { get; set; }
        
        public CurrentIncident? CurrentIncident { get; set; }
        
        public IEnumerable<DangerIndicator>? DangerTriggers { get; set; }
    }
}
