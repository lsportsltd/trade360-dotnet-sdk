namespace Trade360SDK.BetBuilderApi.Entities.MarketsApi.Requests
{
    public class GetMarketsRequest
    {
        public int CustomerId { get; set; }

        public string? UserId { get; set; }

        public int SportId { get; set; }
    }
}
