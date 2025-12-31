using System;

namespace Trade360SDK.Common.Entities.Fixtures
{
    public class Player
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public int? TeamId { get; set; }
        public int? NationalityId { get; set; }
        public DateTime? BirthDate { get; set; }
        public PlayerType? Type { get; set; }
        public int? NationalTeamId { get; set; }
    }
}