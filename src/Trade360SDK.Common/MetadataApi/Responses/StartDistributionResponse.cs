namespace Trade360SDK.Api.Abstraction.MetadataApi.Responses
{
    public class GetDistributionStatusResponse
    {
        public bool IsDistributionOn { get; set; }
        public int[]? Consumers { get; set; }
        public int NumberMessagesInQueue { get; set; }
        public double MessagesPerSecond { get; set; }

    }
}
