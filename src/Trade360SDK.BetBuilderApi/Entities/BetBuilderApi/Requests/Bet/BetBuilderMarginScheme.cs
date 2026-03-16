namespace Trade360SDK.BetBuilderApi.Entities.BetBuilderApi.Requests
{
    public class BetBuilderMarginScheme
    {
        public string? Type { get; set; }

        public bool IncludeSelectionOddsInLadder { get; set; }

        public BetBuilderLaddering? Laddering { get; set; }
    }
}
