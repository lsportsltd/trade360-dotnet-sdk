using Trade360SDK.Common.Entities.Enums;

namespace Trade360SDK.Common.Entities.Livescore
{
    public class Clock
    {
        public ClockStatus? Status { get; set; }
        
        public int Seconds { get; set; }
    }
}