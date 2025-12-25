using System.Collections.Generic;

namespace Trade360SDK.CustomersApi.Entities.MetadataApi.Responses
{
    public class GetSeasonsResponse
    {
        public IEnumerable<Season>? Seasons { get; set; }
    }

    public class Season
    {
        public int SeasonId { get; set; }
        public string? SeasonName { get; set; }
    }
}

