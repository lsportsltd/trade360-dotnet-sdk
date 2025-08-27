using FluentAssertions;
using System.Text;
using Trade360SDK.Common.Models;

namespace Trade360SDK.Common.Tests.Models;

public class TransportMessageHeadersTests
{
    [Fact]
    public void CreateFromProperties_WithValidProperties_ShouldCreateInstance()
    {
        // Arrange
        var properties = new Dictionary<string, object>
        {
            { "MessageType", "TestType" },
            { "MessageSequence", "123456789" },
            { "MessageGuid", "abc-def-123" },
            { "FixtureId", "fixture-456" },
            { "timestamp_in_ms", "1640995200000" }
        };

        // Act
        var result = TransportMessageHeaders.CreateFromProperties(properties);

        // Assert
        result.Should().NotBeNull();
        result.MessageType.Should().Be("TestType");
        result.MessageSequence.Should().Be("123456789");
        result.MessageGuid.Should().Be("abc-def-123");
        result.FixtureId.Should().Be("fixture-456");
        result.TimestampInMs.Should().Be("1640995200000");
    }

    [Fact]
    public void CreateFromProperties_WithNullProperties_ShouldThrowArgumentNullException()
    {
        // Act & Assert
        var act = () => TransportMessageHeaders.CreateFromProperties(null!);
        
        act.Should().Throw<ArgumentNullException>()
           .WithParameterName("properties");
    }

    [Theory]
    [InlineData("MessageType")]
    [InlineData("MessageGuid")]
    [InlineData("timestamp_in_ms")]
    public void CreateFromProperties_WithMissingRequiredProperty_ShouldThrowArgumentException(string missingProperty)
    {
        // Arrange
        var properties = new Dictionary<string, object>
        {
            { "MessageType", "TestType" },
            { "MessageGuid", "abc-def-123" },
            { "timestamp_in_ms", "1640995200000" }
        };
        properties.Remove(missingProperty);

        // Act & Assert
        var act = () => TransportMessageHeaders.CreateFromProperties(properties);
        
        act.Should().Throw<ArgumentException>()
           .WithParameterName("properties");
    }

    [Theory]
    [InlineData("MessageType")]
    [InlineData("MessageGuid")]
    [InlineData("timestamp_in_ms")]
    public void CreateFromProperties_WithNullRequiredProperty_ShouldThrowArgumentException(string nullProperty)
    {
        // Arrange
        var properties = new Dictionary<string, object>
        {
            { "MessageType", "TestType" },
            { "MessageGuid", "abc-def-123" },
            { "timestamp_in_ms", "1640995200000" }
        };
        properties[nullProperty] = null!;

        // Act & Assert
        var act = () => TransportMessageHeaders.CreateFromProperties(properties);
        
        act.Should().Throw<ArgumentException>()
           .WithParameterName("properties");
    }

    [Theory]
    [InlineData(123)]
    [InlineData(true)]
    [InlineData(45.67)]
    public void CreateFromProperties_WithNonStringPropertyValues_ShouldConvertToString(object value)
    {
        // Arrange
        var properties = new Dictionary<string, object>
        {
            { "MessageType", value },
            { "MessageGuid", "abc-def-123" },
            { "timestamp_in_ms", "1640995200000" }
        };

        // Act
        var result = TransportMessageHeaders.CreateFromProperties(properties);

        // Assert
        result.MessageType.Should().Be(value.ToString());
    }

    [Fact]
    public void CreateFromProperties_WithEmptyDictionary_ShouldThrowArgumentException()
    {
        // Arrange
        var properties = new Dictionary<string, object>();

        // Act & Assert
        var act = () => TransportMessageHeaders.CreateFromProperties(properties);
        
        act.Should().Throw<ArgumentException>()
           .WithParameterName("properties");
    }

    [Fact]
    public void Properties_ShouldHaveInternalSetters()
    {
        // Assert
        var type = typeof(TransportMessageHeaders);
        
        var messageTypeProperty = type.GetProperty(nameof(TransportMessageHeaders.MessageType));
        var messageSequenceProperty = type.GetProperty(nameof(TransportMessageHeaders.MessageSequence));
        var messageGuidProperty = type.GetProperty(nameof(TransportMessageHeaders.MessageGuid));
        var fixtureIdProperty = type.GetProperty(nameof(TransportMessageHeaders.FixtureId));

        messageTypeProperty!.CanRead.Should().BeTrue();
        messageTypeProperty.SetMethod!.IsAssembly.Should().BeTrue();

        messageSequenceProperty!.CanRead.Should().BeTrue();
        messageSequenceProperty.SetMethod!.IsAssembly.Should().BeTrue();

        messageGuidProperty!.CanRead.Should().BeTrue();
        messageGuidProperty.SetMethod!.IsAssembly.Should().BeTrue();

        fixtureIdProperty!.CanRead.Should().BeTrue();
        fixtureIdProperty.SetMethod!.IsAssembly.Should().BeTrue();
    }

    [Fact]
    public void Constructor_ShouldBeInternal()
    {
        // Assert
        var type = typeof(TransportMessageHeaders);
        var constructors = type.GetConstructors(System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        
        constructors.Should().HaveCount(1);
        constructors[0].IsAssembly.Should().BeTrue(); // internal constructor
    }

    [Fact]
    public void Type_ShouldBeInCorrectNamespace()
    {
        // Assert
        var type = typeof(TransportMessageHeaders);
        type.Namespace.Should().Be("Trade360SDK.Common.Models");
    }

    [Fact]
    public void CreateFromProperties_WithByteArrayPropertyValues_ShouldConvertToStringUsingUTF8()
    {
        // Arrange
        var messageTypeBytes = Encoding.UTF8.GetBytes("MarketUpdate");
        var messageGuidBytes = Encoding.UTF8.GetBytes("abc-def-123");
        var messageSequenceBytes = Encoding.UTF8.GetBytes("987654321");
        var fixtureIdBytes = Encoding.UTF8.GetBytes("fixture-789");
        
        var properties = new Dictionary<string, object>
        {
            { "MessageType", messageTypeBytes },
            { "MessageGuid", messageGuidBytes },
            { "MessageSequence", messageSequenceBytes },
            { "FixtureId", fixtureIdBytes },
            { "timestamp_in_ms", "1755778318057" }
        };

        // Act
        var result = TransportMessageHeaders.CreateFromProperties(properties);

        // Assert
        result.Should().NotBeNull();
        result.MessageType.Should().Be("MarketUpdate");
        result.MessageGuid.Should().Be("abc-def-123");
        result.MessageSequence.Should().Be("987654321");
        result.FixtureId.Should().Be("fixture-789");
        result.TimestampInMs.Should().Be("1755778318057");
    }

    [Fact]
    public void CreateFromProperties_WithTimestampInMsAsNumber_ShouldConvertToString()
    {
        // Arrange
        var properties = new Dictionary<string, object>
        {
            { "MessageType", "MarketUpdate" },
            { "MessageSequence", "123456789" },
            { "MessageGuid", "abc-def-123" },
            { "FixtureId", "fixture-456" },
            { "timestamp_in_ms", 1640995200000L }
        };

        // Act
        var result = TransportMessageHeaders.CreateFromProperties(properties);

        // Assert
        result.Should().NotBeNull();
        result.TimestampInMs.Should().Be("1640995200000");
    }

    [Theory]
    [InlineData("1640995200000")]
    [InlineData("0")]
    [InlineData("9223372036854775807")]
    public void CreateFromProperties_WithVariousTimestampFormats_ShouldStoreCorrectly(string timestamp)
    {
        // Arrange
        var properties = new Dictionary<string, object>
        {
            { "MessageType", "TestType" },
            { "MessageSequence", "123456789" },
            { "MessageGuid", "abc-def-123" },
            { "FixtureId", "fixture-456" },
            { "timestamp_in_ms", timestamp }
        };

        // Act
        var result = TransportMessageHeaders.CreateFromProperties(properties);

        // Assert
        result.Should().NotBeNull();
        result.TimestampInMs.Should().Be(timestamp);
    }

    [Fact]
    public void CreateFromProperties_WithMissingOptionalProperties_ShouldUseEmptyString()
    {
        // Arrange - Only required properties
        var properties = new Dictionary<string, object>
        {
            { "MessageType", "TestType" },
            { "MessageGuid", "test-guid-123" },
            { "timestamp_in_ms", "1640995200000" }
            // MessageSequence and FixtureId are missing (optional)
        };

        // Act
        var result = TransportMessageHeaders.CreateFromProperties(properties);

        // Assert
        result.Should().NotBeNull();
        result.MessageType.Should().Be("TestType");
        result.MessageGuid.Should().Be("test-guid-123");
        result.TimestampInMs.Should().Be("1640995200000");
        result.MessageSequence.Should().Be(string.Empty);
        result.FixtureId.Should().Be(string.Empty);
    }

    [Theory]
    [InlineData("MessageSequence")]
    [InlineData("FixtureId")]
    public void CreateFromProperties_WithNullOptionalProperty_ShouldUseEmptyString(string nullProperty)
    {
        // Arrange
        var properties = new Dictionary<string, object>
        {
            { "MessageType", "TestType" },
            { "MessageGuid", "test-guid-123" },
            { "MessageSequence", "sequence-123" },
            { "FixtureId", "fixture-456" },
            { "timestamp_in_ms", "1640995200000" }
        };
        properties[nullProperty] = null!;

        // Act
        var result = TransportMessageHeaders.CreateFromProperties(properties);

        // Assert
        result.Should().NotBeNull();
        
        if (nullProperty == "MessageSequence")
        {
            result.MessageSequence.Should().Be(string.Empty);
            result.FixtureId.Should().Be("fixture-456");
        }
        else if (nullProperty == "FixtureId")
        {
            result.MessageSequence.Should().Be("sequence-123");
            result.FixtureId.Should().Be(string.Empty);
        }
    }
}
