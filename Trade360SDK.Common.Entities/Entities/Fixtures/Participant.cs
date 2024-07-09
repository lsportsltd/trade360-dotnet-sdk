namespace Trade360SDK.Common.Entities.Fixtures
{
    public class Participant
    {
        public int Id { get; set; }

        public string? Name { get; set; }

        public string? Position { get; set; }

        public int? RotationId { get; set; }

        public int IsActive { get; set; } = -1;
    }
}
