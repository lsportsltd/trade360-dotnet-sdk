using System.IO;
using System.Xml.Serialization;
using Trade360SDK.Common.Models;

namespace Trade360SDK.Feed.Converters
{
    public class XmlWrappedMessageObjectConverter
    {
        public static WrappedMessage ConvertXmlToMessage(string rawXml)
        {
            var serializer = new XmlSerializer(typeof(WrappedMessage));
            using var reader = new StringReader(rawXml);
            return (WrappedMessage)serializer.Deserialize(reader);
        }
    }
}
