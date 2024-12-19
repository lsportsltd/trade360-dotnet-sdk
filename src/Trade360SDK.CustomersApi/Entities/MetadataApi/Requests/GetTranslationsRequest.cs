using System.Collections.Generic;

namespace Trade360SDK.CustomersApi.Entities.MetadataApi.Requests
{
    public class GetTranslationsRequest : BaseMetadataRequest
    {
        public IEnumerable<int>? SportIds { get; set; }
        public IEnumerable<int>? LocationIds { get; set; }
        public IEnumerable<int>? LeagueIds { get; set; }
        public IEnumerable<int>? MarketIds { get; set; }
        public IEnumerable<int>? ParticipantIds { get; set; }
        public IEnumerable<int>? Languages { get; set; }
    }
}
