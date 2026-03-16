namespace Trade360SDK.BetBuilderApi.Entities.BetBuilderApi.Responses
{
    public class BetBuilderResponse
    {
        public string? Status { get; set; }

        public BetBuilderResponseMetadata? Metadata { get; set; }

        public BetBuilderResponseBody? Response { get; set; }
    }
}
