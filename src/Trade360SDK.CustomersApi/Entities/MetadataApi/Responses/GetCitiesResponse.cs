using System.Collections.Generic;
using Trade360SDK.Common.Entities.Fixtures;

namespace Trade360SDK.CustomersApi.Entities.MetadataApi.Responses
{
    public class GetCitiesResponse
    {
        public IEnumerable<City>? Data { get; set; }
    }
}