using System.Text.Json;

namespace Trade360SDK.BetBuilderApi.Entities.BetBuilderApi.Responses
{
    public class BetBuilderDeriveMetadata
    {
        public bool UsedCache { get; set; }

        public JsonElement? MarginalsToSkip { get; set; }

        public JsonElement? MarginalsToDerive { get; set; }

        public JsonElement? MarketsToDerive { get; set; }

        public bool SkippedDerivation { get; set; }

        public double? CacheCheckTime { get; set; }

        public double? NewtonRaphsonTime { get; set; }
    }
}
