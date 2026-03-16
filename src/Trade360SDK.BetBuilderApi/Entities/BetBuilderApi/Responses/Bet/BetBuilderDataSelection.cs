namespace Trade360SDK.BetBuilderApi.Entities.BetBuilderApi.Responses
{
    public class BetBuilderDataSelection
    {
        public string? StaticDefinition { get; set; }

        public BetBuilderExpectation? Expectation { get; set; }

        public bool IsRequired { get; set; }

        public string? Definition { get; set; }
    }
}
