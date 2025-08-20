using System;
using System.Collections.Generic;

namespace Trade360SDK.Common.Models
{
    public class RabbitMessageProperties
    {
        public string MessageType { get; internal set; }
        public string MessageSequence { get; internal set; }
        public string MessageGuid { get; internal set; }
        public string FixtureId { get; internal set; }

        internal RabbitMessageProperties() { }
        
        public static RabbitMessageProperties CreateFromProperties(IDictionary<string, object> properties)
        {
            if (properties == null)
            {
                throw new ArgumentNullException(nameof(properties));
            }
            
            return new RabbitMessageProperties
            {
                MessageType = GetRequiredProperty(properties, "MessageType"),
                MessageSequence = GetRequiredProperty(properties, "MessageSequence"),
                MessageGuid = GetRequiredProperty(properties, "MessageGuid"),
                FixtureId = GetRequiredProperty(properties, "FixtureId")
            };
        }

        private static string GetRequiredProperty(IDictionary<string, object> properties, string key)
        {
            if (!properties.TryGetValue(key, out var value) || value == null)
            {
                throw new ArgumentException($"Header '{key}' is missing or null in message properties object.", nameof(properties));
            }
            return value.ToString();
        }
    }
}