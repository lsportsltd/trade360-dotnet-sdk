using System.Collections.Generic;

namespace Trade360SDK.CustomersApi.Entities.MetadataApi.Responses
{
    public class GetToursResponse
    {
        public IEnumerable<Tour>? Tours { get; set; }
    }

    public class Tour
    {
        public int TourId { get; set; }
        public string? TourName { get; set; }
        public int SportId { get; set; }
        public string? SportName { get; set; }
    }
}

