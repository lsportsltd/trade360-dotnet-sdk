using System.Text.Json;
using Trade360SDK.Common.Models;

namespace Trade360SDK.Feed.Converters
{
    public class JsonWrappedMessageObjectConverter
    {

        public static WrappedMessage ConvertJsonToMessage(string rawJson)
        {
            using var doc = JsonDocument.Parse(rawJson);
            var root = doc.RootElement;

            var header = JsonSerializer.Deserialize<MessageHeader>(root.GetProperty("Header").GetRawText());
            var body = root.TryGetProperty("Body", out var bodyElement) ? bodyElement.GetRawText() : null;

            return new WrappedMessage
            {
                Header = header,
                Body = body
            };
        }
    }
}