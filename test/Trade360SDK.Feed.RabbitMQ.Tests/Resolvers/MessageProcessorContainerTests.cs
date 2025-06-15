using FluentAssertions;
using Moq;
using System.Reflection;
using Trade360SDK.Common.Attributes;
using Trade360SDK.Feed.FeedType;
using Trade360SDK.Feed.RabbitMQ.Resolvers;

namespace Trade360SDK.Feed.RabbitMQ.Tests;

public class MessageProcessorContainerTests
{
    private class TestFlow : IFlow
    {
    }

    private class OtherFlow : IFlow
    {
    }

    [Trade360Entity(1)]
    private class TestEntity1
    {
    }

    [Trade360Entity(2)]
    private class TestEntity2
    {
    }

    [Fact]
    public void Constructor_WithValidProcessors_ShouldInitializeCorrectly()
    {
        var mockProcessor1 = new Mock<IMessageProcessor>();
        var mockProcessor2 = new Mock<IMessageProcessor>();

        mockProcessor1.Setup(p => p.GetTypeOfTFlow()).Returns(typeof(TestFlow));
        mockProcessor1.Setup(p => p.GetTypeOfTType()).Returns(typeof(TestEntity1));

        mockProcessor2.Setup(p => p.GetTypeOfTFlow()).Returns(typeof(TestFlow));
        mockProcessor2.Setup(p => p.GetTypeOfTType()).Returns(typeof(TestEntity2));

        var processors = new[] { mockProcessor1.Object, mockProcessor2.Object };

        var container = new MessageProcessorContainer<TestFlow>(processors);

        container.Should().NotBeNull();
    }

    [Fact]
    public void Constructor_WithDifferentFlowTypes_ShouldFilterCorrectly()
    {
        var mockProcessor1 = new Mock<IMessageProcessor>();
        var mockProcessor2 = new Mock<IMessageProcessor>();

        mockProcessor1.Setup(p => p.GetTypeOfTFlow()).Returns(typeof(TestFlow));
        mockProcessor1.Setup(p => p.GetTypeOfTType()).Returns(typeof(TestEntity1));

        mockProcessor2.Setup(p => p.GetTypeOfTFlow()).Returns(typeof(OtherFlow));
        mockProcessor2.Setup(p => p.GetTypeOfTType()).Returns(typeof(TestEntity2));

        var processors = new[] { mockProcessor1.Object, mockProcessor2.Object };

        var container = new MessageProcessorContainer<TestFlow>(processors);

        var result = container.GetMessageProcessor(1);
        result.Should().Be(mockProcessor1.Object);

        Action act = () => container.GetMessageProcessor(2);
        act.Should().Throw<KeyNotFoundException>();
    }

    [Fact]
    public void Constructor_WithDuplicateEntityKeys_ShouldThrowArgumentException()
    {
        var mockProcessor1 = new Mock<IMessageProcessor>();
        var mockProcessor2 = new Mock<IMessageProcessor>();

        mockProcessor1.Setup(p => p.GetTypeOfTFlow()).Returns(typeof(TestFlow));
        mockProcessor1.Setup(p => p.GetTypeOfTType()).Returns(typeof(TestEntity1));

        mockProcessor2.Setup(p => p.GetTypeOfTFlow()).Returns(typeof(TestFlow));
        mockProcessor2.Setup(p => p.GetTypeOfTType()).Returns(typeof(TestEntity1));

        var processors = new[] { mockProcessor1.Object, mockProcessor2.Object };

        Action act = () => new MessageProcessorContainer<TestFlow>(processors);

        act.Should().Throw<ArgumentException>()
           .WithMessage("Failed to add resolver as the resolver already registered.");
    }

    [Fact]
    public void GetMessageProcessor_WithValidKey_ShouldReturnProcessor()
    {
        var mockProcessor = new Mock<IMessageProcessor>();
        mockProcessor.Setup(p => p.GetTypeOfTFlow()).Returns(typeof(TestFlow));
        mockProcessor.Setup(p => p.GetTypeOfTType()).Returns(typeof(TestEntity1));

        var processors = new[] { mockProcessor.Object };
        var container = new MessageProcessorContainer<TestFlow>(processors);

        var result = container.GetMessageProcessor(1);

        result.Should().Be(mockProcessor.Object);
    }

    [Fact]
    public void GetMessageProcessor_WithInvalidKey_ShouldThrowKeyNotFoundException()
    {
        var mockProcessor = new Mock<IMessageProcessor>();
        mockProcessor.Setup(p => p.GetTypeOfTFlow()).Returns(typeof(TestFlow));
        mockProcessor.Setup(p => p.GetTypeOfTType()).Returns(typeof(TestEntity1));

        var processors = new[] { mockProcessor.Object };
        var container = new MessageProcessorContainer<TestFlow>(processors);

        Action act = () => container.GetMessageProcessor(999);

        act.Should().Throw<KeyNotFoundException>();
    }
}
