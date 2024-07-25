using System.Collections.Generic;
using Trade360SDK.Common.Entities.Enums;

namespace Trade360SDK.CustomersApi.Entities.MetadataApi.Responses
{
    public class CompetitionCollectionResponse
    {
        public IEnumerable<Competition>? Competitions { get; set; }
    }

    public class Competition
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public CompetitionType Type { get; set; }
        public int TrackId { get; set; }
    }
}
