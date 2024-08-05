using System;

namespace Trade360SDK.Common.Models
{
    public class MessageHeader
    {
        public int Type { get; set; }

        public int? MsgSeq { get; set; }

        public string? MsgGuid { get; set; }

        public DateTime? CreationDate { get; set; }

        public long ServerTimestamp { get; set; }
    }
}
