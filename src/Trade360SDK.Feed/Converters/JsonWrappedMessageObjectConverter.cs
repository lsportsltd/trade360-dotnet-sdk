using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using Trade360SDK.Common.Models;

namespace Trade360SDK.Feed.Converters
{
    public class JsonWrappedMessageObjectConverter : JsonConverter<WrappedMessage>
    {
        public override WrappedMessage Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            using var doc = JsonDocument.ParseValue(ref reader);
            var root = doc.RootElement;
            var header = JsonSerializer.Deserialize<MessageHeader>(root.GetProperty("Header").GetRawText(), options);
            var body = root.TryGetProperty("Body", out var bodyElement) ? bodyElement.GetRawText() : null;
            return new WrappedMessage
            {
                Header = header,
                Body = body
            };
        }

        public override void Write(Utf8JsonWriter writer, WrappedMessage value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();
            writer.WritePropertyName("Header");
            JsonSerializer.Serialize(writer, value.Header, options);
            writer.WritePropertyName("Body");
            if (value.Body != null)
            {
                using var doc = JsonDocument.Parse(value.Body);
                doc.RootElement.WriteTo(writer);
            }
            else
            {
                writer.WriteNullValue();
            }
            writer.WriteEndObject();
        }

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
