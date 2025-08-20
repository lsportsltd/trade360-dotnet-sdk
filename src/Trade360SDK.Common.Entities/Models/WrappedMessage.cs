namespace Trade360SDK.Common.Models
{
    public class WrappedMessage
    {
        public RabbitMessageProperties RabbitMessageProperties { get; set; }
        public MessageHeader? Header { get; set; }
        public string? Body { get; set; }
    }
}
