using System.Collections.Generic;
using Trade360SDK.Common.Entities.Enums;

namespace Trade360SDK.CustomersApi.Entities.MetadataApi.Responses
{
    public class GetParticipantsResponse
    {
        public IEnumerable<ParticipantInfo>? Data { get; set; }
        public int TotalItems { get; set; }
    }

    public class ParticipantInfo
    {
        public int Id { get; set; }
        public int SportId { get; set; }
        public int LocationId { get; set; }
        public string? Name { get; set; }
        public Gender? Gender { get; set; }
        public AgeCategory? AgeCategory { get; set; }
        public ParticipantType? Type { get; set; }
    }
}
