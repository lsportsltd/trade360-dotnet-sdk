using System.Collections.Generic;

namespace Trade360SDK.CustomersApi.Entities.MetadataApi.Responses
{
    public class MarketsCollectionResponse
    {
        public IEnumerable<Market>? Markets { get; set; }
    }

    public class Market
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public bool IsSettleable { get; set; }
    }
}
