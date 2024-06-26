using System;

namespace Trade360SDK.Feed.RabbitMQ.Models
{
    internal class MessageHeader
    {
        public int Type { get; set; }
        
        public int? MsgSeq { get; set; }
        
        public string? MsgGuid { get; set; }

        public DateTime CreationDate { get; set; }
        
        public long ServerTimestamp { get; set; }
    }
}
