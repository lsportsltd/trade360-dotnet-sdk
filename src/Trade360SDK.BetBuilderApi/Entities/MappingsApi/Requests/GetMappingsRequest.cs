namespace Trade360SDK.BetBuilderApi.Entities.MappingsApi.Requests
{
    public class GetMappingsRequest
    {
        public int SportId { get; set; }

        public int? MarketId { get; set; }

        public string? Version { get; set; }
    }
}
