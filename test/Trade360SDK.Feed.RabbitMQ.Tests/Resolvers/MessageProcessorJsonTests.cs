using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using System.Text.Json;
using Trade360SDK.Common.Models;
using Trade360SDK.Feed.FeedType;
using Trade360SDK.Feed.RabbitMQ.Resolvers;

namespace Trade360SDK.Feed.RabbitMQ.Tests.Resolvers;

public class MessageProcessorJsonTests
{
    private readonly Mock<IServiceProvider> _mockServiceProvider;
    private readonly Mock<IEntityHandler<TestEntity, TestFlow>> _mockEntityHandler;

    public MessageProcessorJsonTests()
    {
        _mockServiceProvider = new Mock<IServiceProvider>();
        var mockLoggerFactory = new Mock<ILoggerFactory>();
        _mockEntityHandler = new Mock<IEntityHandler<TestEntity, TestFlow>>();

        mockLoggerFactory.Setup(f => f.CreateLogger(It.IsAny<string>()))
                         .Returns(new Mock<ILogger>().Object);

        _mockServiceProvider.Setup(sp => sp.GetService(typeof(ILoggerFactory)))
                           .Returns(mockLoggerFactory.Object);
        _mockServiceProvider.Setup(sp => sp.GetService(typeof(IEntityHandler<TestEntity, TestFlow>)))
                           .Returns(_mockEntityHandler.Object);
    }

    public class TestEntity
    {
        public string? Name { get; set; }
        public int Id { get; set; }
        public string? PlayerId { get; set; }
        public double? Value { get; set; }
    }

    public class TestFlow : IFlow
    {
    }

    [Fact]
    public async Task ProcessAsync_WithCamelCaseJson_ShouldDeserializeCorrectly()
    {
        // Arrange
        var mockLoggerFactory = new Mock<ILoggerFactory>();
        var processor = new MessageProcessor<TestEntity, TestFlow>(_mockServiceProvider.Object, mockLoggerFactory.Object);
        var header = new MessageHeader { MsgGuid = "test-id" };
        var json = @"{""name"":""Test"",""id"":123,""playerId"":""456"",""value"":78.9}";

        // Act
        await processor.ProcessAsync(typeof(TestEntity), null, header, json);

        // Assert
        _mockEntityHandler.Verify(h => h.ProcessAsync(
            It.IsAny<TransportMessageHeaders>(), 
            header, 
            It.Is<TestEntity>(e => 
                e.Name == "Test" && 
                e.Id == 123 && 
                e.PlayerId == "456" && 
                e.Value == 78.9
            )), Times.Once);
    }

    [Fact]
    public async Task ProcessAsync_WithPascalCaseJson_ShouldDeserializeCorrectly()
    {
        // Arrange
        var mockLoggerFactory = new Mock<ILoggerFactory>();
        var processor = new MessageProcessor<TestEntity, TestFlow>(_mockServiceProvider.Object, mockLoggerFactory.Object);
        var header = new MessageHeader { MsgGuid = "test-id" };
        var json = @"{""Name"":""Test"",""Id"":123,""PlayerId"":""456"",""Value"":78.9}";

        // Act
        await processor.ProcessAsync(typeof(TestEntity), null, header, json);

        // Assert
        _mockEntityHandler.Verify(h => h.ProcessAsync(
            It.IsAny<TransportMessageHeaders>(), 
            header, 
            It.Is<TestEntity>(e => 
                e.Name == "Test" && 
                e.Id == 123 && 
                e.PlayerId == "456" && 
                e.Value == 78.9
            )), Times.Once);
    }

    [Fact]
    public async Task ProcessAsync_WithMixedCaseJson_ShouldDeserializeCorrectly()
    {
        // Arrange
        var mockLoggerFactory = new Mock<ILoggerFactory>();
        var processor = new MessageProcessor<TestEntity, TestFlow>(_mockServiceProvider.Object, mockLoggerFactory.Object);
        var header = new MessageHeader { MsgGuid = "test-id" };
        var json = @"{""NAME"":""Test"",""iD"":123,""pLayeRId"":""456""}";

        // Act
        await processor.ProcessAsync(typeof(TestEntity), null, header, json);

        // Assert
        _mockEntityHandler.Verify(h => h.ProcessAsync(
            It.IsAny<TransportMessageHeaders>(), 
            header, 
            It.Is<TestEntity>(e => 
                e.Name == "Test" && 
                e.Id == 123 && 
                e.PlayerId == "456"
            )), Times.Once);
    }

    [Fact]
    public async Task ProcessAsync_WithStringPlayerId_ShouldDeserializeCorrectly()
    {
        // Arrange
        var mockLoggerFactory = new Mock<ILoggerFactory>();
        var processor = new MessageProcessor<TestEntity, TestFlow>(_mockServiceProvider.Object, mockLoggerFactory.Object);
        var header = new MessageHeader { MsgGuid = "test-id" };
        var json = @"{""playerId"":""299919"",""name"":""PlayerTest""}";

        // Act
        await processor.ProcessAsync(typeof(TestEntity), null, header, json);

        // Assert
        _mockEntityHandler.Verify(h => h.ProcessAsync(
            It.IsAny<TransportMessageHeaders>(), 
            header, 
            It.Is<TestEntity>(e => 
                e.PlayerId == "299919" && 
                e.Name == "PlayerTest"
            )), Times.Once);
    }

    [Fact]
    public async Task ProcessAsync_WithNumericStringPlayerId_ShouldPreserveAsString()
    {
        // Arrange
        var mockLoggerFactory = new Mock<ILoggerFactory>();
        var processor = new MessageProcessor<TestEntity, TestFlow>(_mockServiceProvider.Object, mockLoggerFactory.Object);
        var header = new MessageHeader { MsgGuid = "test-id" };
        var json = @"{""playerId"":""12345""}";

        // Act
        await processor.ProcessAsync(typeof(TestEntity), null, header, json);

        // Assert
        _mockEntityHandler.Verify(h => h.ProcessAsync(
            It.IsAny<TransportMessageHeaders>(), 
            header, 
            It.Is<TestEntity>(e => e.PlayerId == "12345")
        ), Times.Once);
    }

    [Fact]
    public async Task ProcessAsync_WithAlphanumericPlayerId_ShouldDeserializeCorrectly()
    {
        // Arrange
        var mockLoggerFactory = new Mock<ILoggerFactory>();
        var processor = new MessageProcessor<TestEntity, TestFlow>(_mockServiceProvider.Object, mockLoggerFactory.Object);
        var header = new MessageHeader { MsgGuid = "test-id" };
        var json = @"{""playerId"":""ABC123""}";

        // Act
        await processor.ProcessAsync(typeof(TestEntity), null, header, json);

        // Assert
        _mockEntityHandler.Verify(h => h.ProcessAsync(
            It.IsAny<TransportMessageHeaders>(), 
            header, 
            It.Is<TestEntity>(e => e.PlayerId == "ABC123")
        ), Times.Once);
    }

    [Fact]
    public async Task ProcessAsync_WithNullPlayerId_ShouldDeserializeAsNull()
    {
        // Arrange
        var mockLoggerFactory = new Mock<ILoggerFactory>();
        var processor = new MessageProcessor<TestEntity, TestFlow>(_mockServiceProvider.Object, mockLoggerFactory.Object);
        var header = new MessageHeader { MsgGuid = "test-id" };
        var json = @"{""playerId"":null,""name"":""Test""}";

        // Act
        await processor.ProcessAsync(typeof(TestEntity), null, header, json);

        // Assert
        _mockEntityHandler.Verify(h => h.ProcessAsync(
            It.IsAny<TransportMessageHeaders>(), 
            header, 
            It.Is<TestEntity>(e => e.PlayerId == null && e.Name == "Test")
        ), Times.Once);
    }

    [Fact]
    public async Task ProcessAsync_WithMissingPlayerId_ShouldDeserializeAsNull()
    {
        // Arrange
        var mockLoggerFactory = new Mock<ILoggerFactory>();
        var processor = new MessageProcessor<TestEntity, TestFlow>(_mockServiceProvider.Object, mockLoggerFactory.Object);
        var header = new MessageHeader { MsgGuid = "test-id" };
        var json = @"{""name"":""Test"",""id"":1}";

        // Act
        await processor.ProcessAsync(typeof(TestEntity), null, header, json);

        // Assert
        _mockEntityHandler.Verify(h => h.ProcessAsync(
            It.IsAny<TransportMessageHeaders>(), 
            header, 
            It.Is<TestEntity>(e => e.PlayerId == null)
        ), Times.Once);
    }

    [Fact]
    public async Task ProcessAsync_WithComplexRealWorldJson_ShouldDeserializeCorrectly()
    {
        // Arrange
        var mockLoggerFactory = new Mock<ILoggerFactory>();
        var processor = new MessageProcessor<TestEntity, TestFlow>(_mockServiceProvider.Object, mockLoggerFactory.Object);
        var header = new MessageHeader { MsgGuid = "test-id" };
        
        // Real-world example from the error message
        var json = @"{
            ""name"":""Michela Cambiaghi"",
            ""id"":196587133,
            ""playerId"":""299919"",
            ""value"":45.5
        }";

        // Act
        await processor.ProcessAsync(typeof(TestEntity), null, header, json);

        // Assert
        _mockEntityHandler.Verify(h => h.ProcessAsync(
            It.IsAny<TransportMessageHeaders>(), 
            header, 
            It.Is<TestEntity>(e => 
                e.Name == "Michela Cambiaghi" &&
                e.Id == 196587133 &&
                e.PlayerId == "299919" &&
                e.Value == 45.5
            )), Times.Once);
    }

    [Theory]
    [InlineData(@"{""playerId"":""1""}")]
    [InlineData(@"{""playerId"":""999""}")]
    [InlineData(@"{""playerId"":""299919""}")]
    [InlineData(@"{""playerId"":""ABC""}")]
    [InlineData(@"{""playerId"":""123ABC""}")]
    public async Task ProcessAsync_WithVariousPlayerIdFormats_ShouldDeserializeAsString(string json)
    {
        // Arrange
        var mockLoggerFactory = new Mock<ILoggerFactory>();
        var processor = new MessageProcessor<TestEntity, TestFlow>(_mockServiceProvider.Object, mockLoggerFactory.Object);
        var header = new MessageHeader { MsgGuid = "test-id" };

        // Act
        await processor.ProcessAsync(typeof(TestEntity), null, header, json);

        // Assert
        _mockEntityHandler.Verify(h => h.ProcessAsync(
            It.IsAny<TransportMessageHeaders>(), 
            header, 
            It.Is<TestEntity>(e => e.PlayerId != null)
        ), Times.Once);
    }
}

