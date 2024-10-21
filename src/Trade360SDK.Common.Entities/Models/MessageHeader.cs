using System;

namespace Trade360SDK.Common.Models
{
    public class MessageHeader
    {
        public string? CreationDate { get; set; }
        public int Type { get; set; }
        public int? MsgSeq { get; set; }
        public string? MsgGuid { get; set; }
        public long? ServerTimestamp { get; set; }
        public DateTime? PlatformTimestamp { get; set; }
        public DateTime? BasicDeliverTimestamp { get; set; }
    }
}
