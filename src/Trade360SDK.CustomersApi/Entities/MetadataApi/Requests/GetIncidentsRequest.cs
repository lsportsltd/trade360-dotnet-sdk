using System;
using System.Collections.Generic;
using Trade360SDK.CustomersApi.Entities.Base;

namespace Trade360SDK.CustomersApi.Entities.MetadataApi.Requests
{
    public class GetIncidentsRequest : BaseRequest
    {
        public IncidentFilter? Filter { get; set; }
    }
    
    public class IncidentFilter
    {
        public IEnumerable<int>? Ids { get; set; }
        public IEnumerable<int>? Sports { get; set; }
        public DateTime? From { get; set; }
        public IEnumerable<string>? SearchText { get; set; }
    }
}