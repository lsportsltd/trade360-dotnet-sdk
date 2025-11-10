using System;
using Trade360SDK.Common.Entities.Enums;

namespace Trade360SDK.Common.Entities.Livescore
{
    public class CurrentIncident
    {
        public IncidentType? Id { get; set; }

        public string? Name { get; set; }
        
        public DateTime LastUpdate { get; set; }
        
        public IncidentConfirmation? Confirmation { get; set; }
    }
}
