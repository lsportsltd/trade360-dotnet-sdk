namespace Trade360SDK.Api.Abstraction
{
    internal class BaseResponse<TBody>
        where TBody : class
    {
        public HeaderResponse? Header { get; set; }
        public TBody? Body { get; set; }
    }
}
