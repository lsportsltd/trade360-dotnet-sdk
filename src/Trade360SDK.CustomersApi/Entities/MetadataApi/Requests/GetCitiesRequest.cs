using System.Collections.Generic;

namespace Trade360SDK.CustomersApi.Entities.MetadataApi.Requests
{
    public class GetCitiesRequest : BaseMetadataRequest
    {
        public CityFilter? Filter { get; set; }
    }
    
    public class CityFilter
    {
        public IEnumerable<int>? CountryIds { get; set; }
        public IEnumerable<int>? StateIds { get; set; }
        public IEnumerable<int>? CityIds { get; set; }
    }
}