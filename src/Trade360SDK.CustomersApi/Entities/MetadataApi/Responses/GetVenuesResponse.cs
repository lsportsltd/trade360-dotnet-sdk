using System.Collections.Generic;
using Trade360SDK.Common.Entities.Fixtures;

namespace Trade360SDK.CustomersApi.Entities.MetadataApi.Responses
{
    public class GetVenuesResponse
    {
        public IEnumerable<Venue>? Data { get; set; }
    }
}