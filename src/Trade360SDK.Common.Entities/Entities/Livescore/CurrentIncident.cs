using System;

namespace Trade360SDK.Common.Entities.Livescore
{
    public class CurrentIncident
    {
        public long Id { get; set; }

        public string? Name { get; set; }
        
        public DateTime LastUpdate { get; set; }
    }
}
