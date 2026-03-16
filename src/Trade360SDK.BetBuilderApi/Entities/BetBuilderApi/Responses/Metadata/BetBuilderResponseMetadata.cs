using Trade360SDK.BetBuilderApi.Entities.BetBuilderApi.Requests;

namespace Trade360SDK.BetBuilderApi.Entities.BetBuilderApi.Responses
{
    public class BetBuilderResponseMetadata
    {
        public string? RequestId { get; set; }

        public BetBuilderRequestMetadata? RequestMetadata { get; set; }

        public string? CorrelationId { get; set; }

        public string? Build { get; set; }

        public double DeriveTime { get; set; }

        public double CalcTime { get; set; }

        public BetBuilderDeriveMetadata? DeriveMetadata { get; set; }
    }
}
