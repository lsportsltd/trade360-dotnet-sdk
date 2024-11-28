using System;
using System.Collections.Generic;

namespace Trade360SDK.CustomersApi.Entities.MetadataApi.Requests
{
    public class GetFixtureMetadataRequestDto
    {
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public IEnumerable<int>? SportIds { get; set; }
        public IEnumerable<int>? LocationIds { get; set; }
        public IEnumerable<int>? LeagueIds { get; set; }
    }
}
