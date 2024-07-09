namespace Trade360SDK.CustomersApi.Entities.MetadataApi
{
    internal class Response<TBody>
        where TBody : class
    {
        public Header? Header { get; set; }
        public TBody? Body { get; set; }
    }
}
