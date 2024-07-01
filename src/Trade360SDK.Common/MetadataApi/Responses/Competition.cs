using Trade360SDK.Metadata.Enums;

namespace Trade360SDK.Common.Metadata.Responses
{
    public class Competition
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public CompetitionTypeEnum Type { get; set; }
        public int TrackId { get; set; }
    }
}
