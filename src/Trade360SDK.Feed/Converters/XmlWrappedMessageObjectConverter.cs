using System.IO;
using System.Xml.Linq;
using System.Xml;
using System.Xml.Serialization;
using Trade360SDK.Common.Models;

namespace Trade360SDK.Feed.Converters
{
    public class XmlWrappedMessageObjectConverter
    {
        public static WrappedMessage ConvertXmlToMessage(string rawXml)
        {
            var wrappedMessage = new WrappedMessage();

            // Deserialize the Header
            var serializer = new XmlSerializer(typeof(MessageHeader));
            using (var reader = new StringReader(rawXml))
            {
                var xmlReader = XmlReader.Create(reader);
                xmlReader.ReadToFollowing("Header");
                wrappedMessage.Header = (MessageHeader)serializer.Deserialize(xmlReader);
            }

            // Extract the Body as a string
            var xdoc = XDocument.Parse(rawXml);
            var bodyElement = xdoc.Root.Element("Body");
            wrappedMessage.Body = bodyElement?.ToString();

            return wrappedMessage;
        }
    }
}
