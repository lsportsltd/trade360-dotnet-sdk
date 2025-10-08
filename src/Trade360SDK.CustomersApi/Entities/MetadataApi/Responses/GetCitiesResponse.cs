using System.Collections.Generic;
using Trade360SDK.Common.Entities.Shared;

namespace Trade360SDK.CustomersApi.Entities.MetadataApi.Responses
{
    public class GetCitiesResponse
    {
        public IEnumerable<City>? Data { get; set; }
    }

    public class City
    {
        public int CityId { get; set; }
        public string Name { get; set; }
        public IdNamePair? Country { get; set; }
        public IdNamePair? State { get; set; }
    }
}