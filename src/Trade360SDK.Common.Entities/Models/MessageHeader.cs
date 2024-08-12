using System;
using System.Xml.Serialization;

namespace Trade360SDK.Common.Models
{
    [XmlRoot("Header")]
    public class MessageHeader
    {
        [XmlAttribute("CreationDate")]
        public string? CreationDate { get; set; }

        [XmlElement("Type")]
        public int Type { get; set; }

        [XmlElement("MsgSeq")]
        public int MsgSeq { get; set; }

        [XmlElement("MsgGuid")]
        public string? MsgGuid { get; set; }

        [XmlElement("ServerTimestamp")]
        public long ServerTimestamp { get; set; }
    }
}
