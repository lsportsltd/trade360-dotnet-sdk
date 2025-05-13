using System;
using System.Collections.Generic;

namespace Trade360SDK.CustomersApi.Entities.MetadataApi.Responses
{
    public class GetIncidentsResponse {
        public IEnumerable<Incident>? Data { get; set; }
    }
        
    public class Incident
    {
        public int SportId { get; set; }
        public string SportName { get; set; }
        public int IncidentId { get; set; }
        public string IncidentName { get; set; }
        public DateTime LastUpdate { get; set; }
        public DateTime CreationDate { get; set; }
    }
}