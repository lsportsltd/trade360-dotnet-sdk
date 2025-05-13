using System.Collections.Generic;
using Trade360SDK.Common.Entities.Incidents;

namespace Trade360SDK.CustomersApi.Entities.MetadataApi.Responses
{
    public class GetIncidentsResponse {
        public IEnumerable<Incident>? Data { get; set; }
    }
}