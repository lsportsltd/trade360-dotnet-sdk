using Trade360SDK.CustomersApi.Entities.Base;

namespace Trade360SDK.CustomersApi.Entities.MetadataApi.Requests
{
    public class GetFixtureMetadataRequest : BaseMetadataRequest
    {
        public string? FromDate { get; set; }
        public string? ToDate { get; set; }
    }
}
