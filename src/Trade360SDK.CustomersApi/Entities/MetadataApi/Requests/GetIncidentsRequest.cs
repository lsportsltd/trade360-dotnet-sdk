using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using Trade360SDK.CustomersApi.Entities.Base;

namespace Trade360SDK.CustomersApi.Entities.MetadataApi.Requests
{
    public class GetIncidentsRequest : BaseRequest
    {
        public IncidentFilter? Filters { get; set; }
    }
    
    public class IncidentFilter
    {
        public IEnumerable<int>? Ids { get; set; }
    
        public IEnumerable<int>? Sports { get; set; }

        [JsonPropertyName("creationDate")]
        public DateRangeFilter? CreationDate { get; set; }
    
        [JsonPropertyName("searchText")]
        public IEnumerable<string>? SearchText { get; set; }
    }

    public class DateRangeFilter
    {
        public DateTime? From { get; set; }

        public DateTime? To { get; set; }
    } 
}