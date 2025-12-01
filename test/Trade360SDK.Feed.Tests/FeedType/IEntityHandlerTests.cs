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
        var transportMessageHeaders = new TransportMessageHeaders
        {
            FixtureId = "123456789-test-id",
            MessageGuid = header.MsgGuid,
            MessageSequence = "123456789-test-seq",
            MessageType = entity.Id.ToString() 
        };
        
        await mockHandler.Object.ProcessAsync(transportMessageHeaders, header, entity);

        mockHandler.Verify(h => h.ProcessAsync(transportMessageHeaders, header, entity), Times.Once);
    }

    [Fact]
    public async Task ProcessAsync_WithNullHeader_ShouldBeCallable()
    {
        var mockHandler = new Mock<IEntityHandler<TestEntity, TestFlow>>();
        var entity = new TestEntity { Name = "Test", Id = 1 };
        var transportMessageHeaders = new TransportMessageHeaders
        {
            FixtureId = "123456789-test-id",
            MessageSequence = "123456789-test-seq",
            MessageType = entity.Id.ToString() 
        };

        await mockHandler.Object.ProcessAsync(transportMessageHeaders, null, entity);

        mockHandler.Verify(h => h.ProcessAsync(transportMessageHeaders, null, entity), Times.Once);
    }

    [Fact]
    public async Task ProcessAsync_WithNullEntity_ShouldBeCallable()
    {
        var mockHandler = new Mock<IEntityHandler<TestEntity, TestFlow>>();
        var header = new MessageHeader { MsgGuid = "test-id" };
        var transportMessageHeaders = new TransportMessageHeaders
        {
            FixtureId = "123456789-test-id",
            MessageGuid = header.MsgGuid,
            MessageSequence = "123456789-test-seq",
        };

        await mockHandler.Object.ProcessAsync(transportMessageHeaders, header, null);

        mockHandler.Verify(h => h.ProcessAsync(transportMessageHeaders, header, null), Times.Once);
    }
}
