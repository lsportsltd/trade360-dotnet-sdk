using Trade360SDK.CustomersApi.Entities.Base;

namespace Trade360SDK.CustomersApi.Entities.MetadataApi.Requests
{
    public class GetSeasonsRequest : BaseRequest
    {
        public int? SeasonId { get; set; }
    }
}