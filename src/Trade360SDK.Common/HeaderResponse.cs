using System.Collections.Generic;
using System.Net;

namespace Trade360SDK.Api.Abstraction
{
    internal class HeaderResponse
    {
        public HttpStatusCode HttpStatusCode { get; set; }
        public IEnumerable<Error>? Errors { get; set; }
    }

    internal class Error
    {
        public string? Message { get; set; }
    }
}
