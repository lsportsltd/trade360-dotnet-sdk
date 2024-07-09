namespace Trade360SDK.Api.Abstraction.MetadataApi.Requests
{
    public class GetFixtureMetadataRequest : BaseRequest
    {
        public string FromDate { get; set; }
        public string ToDate { get; set; }
    }
}
