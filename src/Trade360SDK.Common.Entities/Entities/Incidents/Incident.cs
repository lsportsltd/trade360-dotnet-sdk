using System;

namespace Trade360SDK.Common.Entities.Incidents
{
    public class Incident
    {
        public int SportId { get; set; }
        public string SportName { get; set; }
        public int IncidentId { get; set; }
        public string IncidentName { get; set; }
        public string Description { get; set; }
        public DateTime LastUpdate { get; set; }
        public DateTime CreationDate { get; set; }
    }
}