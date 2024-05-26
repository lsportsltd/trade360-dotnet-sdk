using System.Collections.Generic;
using System.Net;

namespace Trade360SDK.Subscription.Models
{
    internal class Header
    {
        public HttpStatusCode HttpStatusCode { get; set; }
        public IEnumerable<Error>? Errors { get; set; }
    }
}
