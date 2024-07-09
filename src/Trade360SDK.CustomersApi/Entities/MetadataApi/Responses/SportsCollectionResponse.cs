using System.Collections.Generic;

namespace Trade360SDK.CustomersApi.Entities.MetadataApi.Responses
{
    public class SportsCollectionResponse
    {
        public IEnumerable<Sport>? Sports { get; set; }
    }

    public class Sport
    {
        public int Id { get; set; }
        public string? Name { get; set; }
    }
}
