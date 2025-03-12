using System;
using Trade360SDK.Common.Entities.Enums;

namespace Trade360SDK.Common.Entities.Livescore
{
    public class DangerIndicator
    {
        public DangerIndicatorType Type { get; set; }

        public DangerIndicatorStatus Status { get; set; }
        
        public DateTime LastUpdate { get; set; }
    }
}
