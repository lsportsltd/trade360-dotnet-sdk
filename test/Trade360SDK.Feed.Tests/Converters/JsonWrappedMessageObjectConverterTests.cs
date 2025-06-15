using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using Trade360SDK.Feed.Converters;
using Xunit;

namespace Trade360SDK.Feed.Tests.Converters
{
    public class JsonWrappedMessageObjectConverterTests
    {
        [Fact]
        public void CanInstantiateConverter()
        {
            var converter = new JsonWrappedMessageObjectConverter();
            Assert.NotNull(converter);
            Assert.IsType<JsonWrappedMessageObjectConverter>(converter);
            Assert.IsAssignableFrom<JsonConverter>(converter);
        }
        // Add more tests here if the converter has custom logic
    }
} 