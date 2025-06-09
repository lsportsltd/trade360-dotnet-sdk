using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using System.Text.Json;
using Trade360SDK.Common.Models;
using Trade360SDK.Feed.FeedType;
using Trade360SDK.Feed.RabbitMQ.Resolvers;

namespace Trade360SDK.Feed.RabbitMQ.Tests;

public class MessageProcessorTests
{
    private readonly Mock<IServiceProvider> _mockServiceProvider;
    private readonly Mock<ILoggerFactory> _mockLoggerFactory;
    private readonly Mock<ILogger> _mockLogger;
    private readonly Mock<IEntityHandler<TestEntity, TestFlow>> _mockEntityHandler;

    public MessageProcessorTests()
    {
        _mockServiceProvider = new Mock<IServiceProvider>();
        _mockLoggerFactory = new Mock<ILoggerFactory>();
        _mockLogger = new Mock<ILogger>();
        _mockEntityHandler = new Mock<IEntityHandler<TestEntity, TestFlow>>();

        _mockLoggerFactory.Setup(f => f.CreateLogger(It.IsAny<string>()))
                         .Returns(_mockLogger.Object);

        _mockServiceProvider.Setup(sp => sp.GetService(typeof(IEntityHandler<TestEntity, TestFlow>)))
                           .Returns(_mockEntityHandler.Object);
    }

    public class TestEntity
    {
        public string? Name { get; set; }
        public int Id { get; set; }
    }

    public class TestFlow : IFlow
    {
    }

    [Fact]
    public void Constructor_ShouldInitializeCorrectly()
    {
        var processor = new MessageProcessor<TestEntity, TestFlow>(_mockServiceProvider.Object, _mockLoggerFactory.Object);

        processor.Should().NotBeNull();
    }

    [Fact]
    public void GetTypeOfTType_ShouldReturnCorrectType()
    {
        var processor = new MessageProcessor<TestEntity, TestFlow>(_mockServiceProvider.Object, _mockLoggerFactory.Object);

        var result = processor.GetTypeOfTType();

        result.Should().Be(typeof(TestEntity));
    }

    [Fact]
    public void GetTypeOfTFlow_ShouldReturnCorrectType()
    {
        var processor = new MessageProcessor<TestEntity, TestFlow>(_mockServiceProvider.Object, _mockLoggerFactory.Object);

        var result = processor.GetTypeOfTFlow();

        result.Should().Be(typeof(TestFlow));
    }

    [Fact]
    public async Task ProcessAsync_WithValidJson_ShouldDeserializeAndCallHandler()
    {
        var processor = new MessageProcessor<TestEntity, TestFlow>(_mockServiceProvider.Object, _mockLoggerFactory.Object);
        var header = new MessageHeader { MsgGuid = "test-id" };
        var entity = new TestEntity { Name = "Test", Id = 1 };
        var json = JsonSerializer.Serialize(entity);

        await processor.ProcessAsync(typeof(TestEntity), header, json);

        _mockEntityHandler.Verify(h => h.ProcessAsync(header, It.Is<TestEntity>(e => e.Name == "Test" && e.Id == 1)), Times.Once);
    }

    [Fact]
    public async Task ProcessAsync_WithInvalidJson_ShouldCallHandlerWithNull()
    {
        var processor = new MessageProcessor<TestEntity, TestFlow>(_mockServiceProvider.Object, _mockLoggerFactory.Object);
        var header = new MessageHeader { MsgGuid = "test-id" };
        var invalidJson = "{ invalid json }";

        await processor.ProcessAsync(typeof(TestEntity), header, invalidJson);

        _mockEntityHandler.Verify(h => h.ProcessAsync(header, null), Times.Once);
    }

    [Fact]
    public async Task ProcessAsync_WithNullBody_ShouldCallHandlerWithNull()
    {
        var processor = new MessageProcessor<TestEntity, TestFlow>(_mockServiceProvider.Object, _mockLoggerFactory.Object);
        var header = new MessageHeader { MsgGuid = "test-id" };

        await processor.ProcessAsync(typeof(TestEntity), header, null);

        _mockEntityHandler.Verify(h => h.ProcessAsync(header, null), Times.Once);
    }

    [Fact]
    public async Task ProcessAsync_WithEmptyBody_ShouldCallHandlerWithNull()
    {
        var processor = new MessageProcessor<TestEntity, TestFlow>(_mockServiceProvider.Object, _mockLoggerFactory.Object);
        var header = new MessageHeader { MsgGuid = "test-id" };

        await processor.ProcessAsync(typeof(TestEntity), header, string.Empty);

        _mockEntityHandler.Verify(h => h.ProcessAsync(header, null), Times.Once);
    }

    [Fact]
    public async Task ProcessAsync_WithMissingHandler_ShouldThrowArgumentException()
    {
        _mockServiceProvider.Setup(sp => sp.GetService(typeof(IEntityHandler<TestEntity, TestFlow>)))
                           .Returns((IEntityHandler<TestEntity, TestFlow>?)null);

        var processor = new MessageProcessor<TestEntity, TestFlow>(_mockServiceProvider.Object, _mockLoggerFactory.Object);
        var header = new MessageHeader { MsgGuid = "test-id" };

        Func<Task> act = async () => await processor.ProcessAsync(typeof(TestEntity), header, "{}");

        await act.Should().ThrowAsync<InvalidOperationException>()
                 .WithMessage("No service for type*");
    }
}
