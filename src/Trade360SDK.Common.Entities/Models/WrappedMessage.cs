using System.Xml.Serialization;

namespace Trade360SDK.Common.Models
{
    [XmlRoot("Message")]
    public class WrappedMessage
    {
        [XmlElement("Header")]
        public MessageHeader? Header { get; set; }
        [XmlElement("Body")]
        public string? Body { get; set; }
    }
}
