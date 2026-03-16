namespace Trade360SDK.BetBuilderApi.Entities.BetBuilderApi.Requests
{
    public class BetBuilderRequest
    {
        public int CustomerId { get; set; }

        public string? UserId { get; set; }

        public int EvId { get; set; }

        public string? MsgType { get; set; }

        public BetBuilderRequestBody? Request { get; set; }

        public BetBuilderRequestMetadata? Metadata { get; set; }
    }
}
