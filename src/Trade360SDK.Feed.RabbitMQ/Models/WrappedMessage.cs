namespace Trade360SDK.Feed.RabbitMQ.Models
{
    internal class WrappedMessage
    {
        public MessageHeader? Header { get; set; }
        public string? Body { get; set; }
    }
}
