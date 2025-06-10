using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Trade360SDK.Feed.RabbitMQ.Resolvers;
using Trade360SDK.Feed;

namespace Trade360SDK.Feed.RabbitMQ.Tests;

public class MessageProcessorContainerComprehensiveTests
{
    private readonly Mock<IServiceProvider> _mockServiceProvider;
    private readonly MessageProcessorContainer<object> _container;

    public MessageProcessorContainerComprehensiveTests()
    {
        _mockServiceProvider = new Mock<IServiceProvider>();
        _container = new MessageProcessorContainer<object>(new List<IMessageProcessor>());
    }

    [Fact]
    public void Constructor_WithValidServiceProvider_ShouldInitializeCorrectly()
    {
        _container.Should().NotBeNull();
    }

    [Fact]
    public void Constructor_WithNullMessageProcessors_ShouldThrowNullReferenceException()
    {
        var act = () => new MessageProcessorContainer<object>(null);

        act.Should().Throw<NullReferenceException>();
    }

    [Fact]
    public void GetMessageProcessor_WithValidEntityType_ShouldThrowKeyNotFoundException()
    {
        var mockProcessor = new Mock<IMessageProcessor>();
        var processors = new List<IMessageProcessor> { mockProcessor.Object };
        var container = new MessageProcessorContainer<object>(processors);

        var act = () => container.GetMessageProcessor(3);
        act.Should().Throw<KeyNotFoundException>();
    }

    [Fact]
    public void GetMessageProcessor_WithMultipleEntityTypes_ShouldThrowKeyNotFoundException()
    {
        var mockProcessor1 = new Mock<IMessageProcessor>();
        var mockProcessor2 = new Mock<IMessageProcessor>();
        var processors = new List<IMessageProcessor> { mockProcessor1.Object, mockProcessor2.Object };
        var container = new MessageProcessorContainer<object>(processors);

        var act1 = () => container.GetMessageProcessor(3);
        var act2 = () => container.GetMessageProcessor(4);

        act1.Should().Throw<KeyNotFoundException>();
        act2.Should().Throw<KeyNotFoundException>();
    }

    [Fact]
    public void GetMessageProcessor_WithUnregisteredEntityType_ShouldThrowKeyNotFoundException()
    {
        var act = () => _container.GetMessageProcessor(999);

        act.Should().Throw<KeyNotFoundException>();
    }

    [Fact]
    public void GetMessageProcessor_WithNegativeEntityType_ShouldThrowKeyNotFoundException()
    {
        var act = () => _container.GetMessageProcessor(-1);

        act.Should().Throw<KeyNotFoundException>();
    }

    [Fact]
    public void GetMessageProcessor_WithZeroEntityType_ShouldThrowKeyNotFoundException()
    {
        var act = () => _container.GetMessageProcessor(0);

        act.Should().Throw<KeyNotFoundException>();
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    [InlineData(4)]
    [InlineData(5)]
    public void GetMessageProcessor_WithValidEntityTypes_ShouldThrowKeyNotFoundException(int entityType)
    {
        var act = () => _container.GetMessageProcessor(entityType);
        act.Should().Throw<KeyNotFoundException>();
    }

    [Fact]
    public void Constructor_WithProcessorsOfDifferentFlowTypes_ShouldThrowArgumentNullException()
    {
        var mockProcessor1 = new Mock<IMessageProcessor>();
        var mockProcessor2 = new Mock<IMessageProcessor>();
        
        mockProcessor1.Setup(p => p.GetTypeOfTFlow()).Returns(typeof(object));
        mockProcessor2.Setup(p => p.GetTypeOfTFlow()).Returns(typeof(string));

        var processors = new List<IMessageProcessor> { mockProcessor1.Object, mockProcessor2.Object };
        
        var act = () => new MessageProcessorContainer<object>(processors);
        act.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void GetMessageProcessor_WithInvalidKey_ShouldThrowKeyNotFoundException()
    {
        var act = () => _container.GetMessageProcessor(999);

        act.Should().Throw<KeyNotFoundException>();
    }

    [Fact]
    public void GetMessageProcessor_WithLargeEntityType_ShouldThrowKeyNotFoundException()
    {
        var largeEntityType = int.MaxValue;
        var act = () => _container.GetMessageProcessor(largeEntityType);

        act.Should().Throw<KeyNotFoundException>();
    }

    [Fact]
    public void Constructor_WithEmptyProcessorList_ShouldCreateEmptyContainer()
    {
        var emptyContainer = new MessageProcessorContainer<object>(new List<IMessageProcessor>());
        
        emptyContainer.Should().NotBeNull();
        
        var act = () => emptyContainer.GetMessageProcessor(1);
        act.Should().Throw<KeyNotFoundException>();
    }

    [Fact]
    public void Constructor_WithNullProcessor_ShouldThrowNullReferenceException()
    {
        var processors = new List<IMessageProcessor> { null };
        
        var act = () => new MessageProcessorContainer<object>(processors);
        
        act.Should().Throw<NullReferenceException>();
    }
}
