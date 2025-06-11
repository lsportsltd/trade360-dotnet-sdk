using System.Collections.Generic;
using System.Net;

namespace Trade360SDK.CustomersApi.Entities.Base
{
    public class HeaderResponse
    {
        public string? RequestId { get; set; }
        public HttpStatusCode HttpStatusCode { get; set; }
        public IEnumerable<Error>? Errors { get; set; }
    }

    public class Error
    {
        public string? Message { get; set; }
    }
}
