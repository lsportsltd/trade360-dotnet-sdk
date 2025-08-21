using System;
using System.Collections.Generic;
using System.Text;

namespace Trade360SDK.Common.Models
{
    public class TransportMessageHeaders
    {
        private const string MessageGuidKey = "MessageGuid";
        private const string MessageTypeKey = "MessageType";
        private const string TimestampInMsKey = "timestamp_in_ms";
        private const string MessageSequenceKey = "MessageSequence";
        private const string FixtureIdKey = "FixtureId";
        
        public string MessageType { get; internal set; }
        public string MessageSequence { get; internal set; }
        public string MessageGuid { get; internal set; }
        public string FixtureId { get; internal set; }
        public string TimestampInMs { get; internal set; }
        

        internal TransportMessageHeaders() { }
        
        public static TransportMessageHeaders CreateFromProperties(IDictionary<string, object> properties)
        {
            if (properties == null)
            {
                throw new ArgumentNullException(nameof(properties));
            }
            
            return new TransportMessageHeaders()
            {
                MessageGuid = GetRequiredProperty(properties, MessageGuidKey),
                MessageType = GetRequiredProperty(properties, MessageTypeKey),
                TimestampInMs = GetRequiredProperty(properties, TimestampInMsKey),
                MessageSequence = GetRequiredProperty(properties, MessageSequenceKey, false),
                FixtureId = GetRequiredProperty(properties, FixtureIdKey, false)
            };
        }

        private static string GetRequiredProperty(IDictionary<string, object> properties, string key, bool required = true)
        {
            if (properties.TryGetValue(key, out var value) && value != null)
            {
                return value switch
                {
                    byte[] bytes => Encoding.UTF8.GetString(bytes),
                    _ => value.ToString()
                };
            }
            
            if (required)
            {
                throw new ArgumentException($"Header '{key}' is missing or null in message properties object.",
                    nameof(properties));
            }
            return string.Empty;
        }
    }
}