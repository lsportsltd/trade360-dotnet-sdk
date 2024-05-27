namespace Trade360SDK.Common.Models
{
    internal class Response<TBody>
        where TBody : class
    {
        public Header? Header { get; set; }
        public TBody? Body { get; set; }
    }
}
