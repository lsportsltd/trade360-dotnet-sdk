using Trade360SDK.Mapping.Enums;

namespace Trade360SDK.Api.Mapping
{
    public class Competition
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public CompetitionType Type { get; set; }
        public int TrackId { get; set; }
    }
}
