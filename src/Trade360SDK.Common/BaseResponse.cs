namespace Trade360SDK.Api.Common.Models.Responses.Base
{
    internal class BaseResponse<TBody>
        where TBody : class
    {
        public HeaderResponse? Header { get; set; }
        public TBody? Body { get; set; }
    }
}
