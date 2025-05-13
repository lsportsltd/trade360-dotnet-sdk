using System;
using System.Collections.Generic;

namespace Trade360SDK.CustomersApi.Entities.MetadataApi.Requests
{
    public class GetIncidentsRequestDto
    {
        public IncidentFilterDto? Filter { get; set; }
    }
    
    public class IncidentFilterDto
    {
        public IEnumerable<int>? Ids { get; set; }
        public IEnumerable<int>? Sports { get; set; }
        public DateTime? From { get; set; }
        public IEnumerable<string>? SearchText { get; set; }
    }
}