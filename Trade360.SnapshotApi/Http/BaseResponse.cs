using Trade360SDK.Common.Models;

namespace Trade360SDK.SnapshotApi.Http
{
    public class BaseResponse<TBody>
        where TBody : class
    {
        public MessageHeader? Header { get; set; }
        public TBody? Body { get; set; }
    }
}
