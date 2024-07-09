using System.Collections.Generic;
using Trade360SDK.Api.Abstraction.Enums;

namespace Trade360SDK.Api.Abstraction.MetadataApi.Responses
{
    public class CompetitionCollectionResponse
    {
        public IEnumerable<Competition>? Competitions { get; set; }
    }

    public class Competition
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public CompetitionTypeEnum Type { get; set; }
        public int TrackId { get; set; }
    }
}
