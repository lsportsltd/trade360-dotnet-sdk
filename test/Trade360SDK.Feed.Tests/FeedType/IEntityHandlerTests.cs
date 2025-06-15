using FluentAssertions;
using Moq;
using Trade360SDK.Common.Models;
using Trade360SDK.Feed.FeedType;

namespace Trade360SDK.Feed.Tests;

public class IEntityHandlerTests
{
    public class TestEntity
    {
        public string? Name { get; set; }
        public int Id { get; set; }
    }

    public class TestFlow : IFlow
    {
    }

    [Fact]
    public async Task ProcessAsync_ShouldBeCallable()
    {
        var mockHandler = new Mock<IEntityHandler<TestEntity, TestFlow>>();
        var header = new MessageHeader { MsgGuid = "test-id" };
        var entity = new TestEntity { Name = "Test", Id = 1 };

        await mockHandler.Object.ProcessAsync(header, entity);

        mockHandler.Verify(h => h.ProcessAsync(header, entity), Times.Once);
    }

    [Fact]
    public async Task ProcessAsync_WithNullHeader_ShouldBeCallable()
    {
        var mockHandler = new Mock<IEntityHandler<TestEntity, TestFlow>>();
        var entity = new TestEntity { Name = "Test", Id = 1 };

        await mockHandler.Object.ProcessAsync(null, entity);

        mockHandler.Verify(h => h.ProcessAsync(null, entity), Times.Once);
    }

    [Fact]
    public async Task ProcessAsync_WithNullEntity_ShouldBeCallable()
    {
        var mockHandler = new Mock<IEntityHandler<TestEntity, TestFlow>>();
        var header = new MessageHeader { MsgGuid = "test-id" };

        await mockHandler.Object.ProcessAsync(header, null);

        mockHandler.Verify(h => h.ProcessAsync(header, null), Times.Once);
    }
}
