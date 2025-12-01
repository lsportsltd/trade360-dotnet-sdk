using System.Collections.Generic;

namespace Trade360SDK.CustomersApi.Entities.MetadataApi.Requests
{
    public class GetCitiesRequestDto
    {
        public CityFilterDto? Filter { get; set; }
    }
    
    public class CityFilterDto
    {
        public IEnumerable<int>? CountryIds { get; set; }
        public IEnumerable<int>? StateIds { get; set; }
        public IEnumerable<int>? CityIds { get; set; }
    }
}