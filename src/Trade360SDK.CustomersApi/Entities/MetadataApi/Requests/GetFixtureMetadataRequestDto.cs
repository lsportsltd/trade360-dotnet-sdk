using System;

namespace Trade360SDK.CustomersApi.Entities.MetadataApi.Requests
{
    public class GetFixtureMetadataRequestDto
    {
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public int? LanguageId { get; set; }
    }
}
