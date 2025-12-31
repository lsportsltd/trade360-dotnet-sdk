using Trade360SDK.CustomersApi.Entities.Base;

namespace Trade360SDK.CustomersApi.Entities.MetadataApi.Requests
{
    public class GetToursRequest : BaseRequest
    {
        public int? TourId { get; set; }
        public int? SportId { get; set; }
    }
}