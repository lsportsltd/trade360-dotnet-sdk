using System.Collections.Generic;
using Trade360SDK.CustomersApi.Entities.Base;

namespace Trade360SDK.CustomersApi.Entities.MetadataApi.Requests
{
    public class GetFixtureMetadataRequest : BaseRequest
    {
        public string? FromDate { get; set; }
        public string? ToDate { get; set; }
        public IEnumerable<int>? SportIds { get; set; }
        public IEnumerable<int>? LocationIds { get; set; }
        public IEnumerable<int>? LeagueIds { get; set; }
    }
}
