using System.Collections.Generic;
using System.Linq;

namespace Trade360SDK.SnapshotApi.Entities.Requests
{
    public class BaseStandardRequest : BaseRequest
    {
        public long? Timestamp { get; set; }
        public long? FromDate { get; set; }
        public long? ToDate { get; set; }
        public IEnumerable<int> Sports { get; set; } = Enumerable.Empty<int>();
        public IEnumerable<int> Locations { get; set; } = Enumerable.Empty<int>();
        public IEnumerable<int> Fixtures { get; set; } = Enumerable.Empty<int>();
        public IEnumerable<int> Leagues { get; set; } = Enumerable.Empty<int>();
        public IEnumerable<int> Markets { get; set; } = Enumerable.Empty<int>();
    }
}
