namespace Trade360SDK.CustomersApi.Entities.Base
{
    public class BaseResponse<TBody>
        where TBody : class
    {
        public HeaderResponse? Header { get; set; }
        public TBody? Body { get; set; }
    }
}
