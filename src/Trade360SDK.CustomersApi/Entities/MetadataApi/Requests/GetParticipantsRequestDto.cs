using System.Collections.Generic;

namespace Trade360SDK.CustomersApi.Entities.MetadataApi.Requests
{
    public class GetParticipantsRequestDto
    {
        public ParticipantFilterDto? Filter { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
    }
    
    public class ParticipantFilterDto
    {
        public IEnumerable<int>? Ids { get; set; }
        public IEnumerable<int>? SportIds { get; set; }
        public IEnumerable<int>? LocationIds { get; set; }
        public string? Name { get; set; }
        public int? Gender { get; set; }
        public int? AgeCategory { get; set; }
        public int? Type { get; set; }
    }
}
