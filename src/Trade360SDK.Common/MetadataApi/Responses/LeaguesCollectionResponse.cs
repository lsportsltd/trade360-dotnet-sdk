using System.Collections.Generic;

namespace Trade360SDK.Api.Abstraction.MetadataApi.Responses
{
    public class LeaguesCollectionResponse
    {
        public IEnumerable<League>? Leagues { get; set; }
    }
    public class League
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Season { get; set; }
        public int SportId { get; set; }
        public int LocationId { get; set; }
    }
}
