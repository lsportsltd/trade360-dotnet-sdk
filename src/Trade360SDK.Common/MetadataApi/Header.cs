using System.Collections.Generic;
using System.Net;

namespace Trade360SDK.Common.Metadata
{
    internal class Header
    {
        public HttpStatusCode HttpStatusCode { get; set; }
        public IEnumerable<Error>? Errors { get; set; }
    }
}
