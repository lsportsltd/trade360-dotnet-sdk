namespace Trade360SDK.Common.Models
{
    public class MessageHeader
    {
        public string? CreationDate { get; set; }

        public int Type { get; set; }

        public int? MsgSeq { get; set; }
        public string? MsgGuid { get; set; }
        public long ServerTimestamp { get; set; }
        
        public DateTime? ReceivedTimestamp { get; set; }
        
        public DateTime? SourceTimestamp { get; set; }
        
    }
}
