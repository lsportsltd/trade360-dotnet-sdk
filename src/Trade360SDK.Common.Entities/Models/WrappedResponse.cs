namespace Trade360SDK.Common.Models
{
    public class WrappedResponse<T>
    {
        public MessageHeader? Header { get; set; }
        public T Body { get; set; }
    }
}
