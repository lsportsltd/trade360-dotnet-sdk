using System;

namespace Trade360SDK.Api.Abstraction.MetadataApi.Requests
{
    public class GetFixtureMetadataRequestDto
    {
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
    }
}
