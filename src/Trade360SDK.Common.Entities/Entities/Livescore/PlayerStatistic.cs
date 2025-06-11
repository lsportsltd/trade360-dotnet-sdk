using System.Collections.Generic;

namespace Trade360SDK.Common.Entities.Livescore
{
    public class PlayerStatistic
    {
        public int PlayerId { get; set; }
        
        public List<StatisticValue>? Statistics { get; set; }
        
        public string? PlayerName { get; set; }
        
        public int TeamId { get; set; }
        
        public bool HasPlayed { get; set; }
    }
}