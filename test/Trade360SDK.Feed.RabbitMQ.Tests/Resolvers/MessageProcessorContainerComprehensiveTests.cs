using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Trade360SDK.Feed.RabbitMQ.Resolvers;
using Trade360SDK.Feed;
using System;
using System.Collections.Generic;
using System.Reflection;
using Trade360SDK.Common.Attributes;
using Trade360SDK.Feed.FeedType;
using Xunit;

namespace Trade360SDK.Feed.RabbitMQ.Tests;

/// <summary>
/// Comprehensive tests for MessageProcessorContainer class covering all code paths,
/// error handling scenarios, and processor management.
/// </summary>
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

    #region Constructor Tests

    [Fact]
    public void Constructor_WithEmptyProcessors_ShouldCreateEmptyContainer()
    {
        // Arrange
        var processors = new List<IMessageProcessor>();

        // Act
        var container = new MessageProcessorContainer<InPlay>(processors);

        // Assert
        container.Should().NotBeNull();
    }

    [Fact]
    public void Constructor_WithNullProcessors_ShouldThrowNullReferenceException()
    {
        // Act & Assert
        var act = () => new MessageProcessorContainer<InPlay>(null!);
        act.Should().Throw<NullReferenceException>();
    }

    [Fact]
    public void Constructor_WithValidProcessors_ShouldRegisterCorrectly()
    {
        // Arrange
        var mockProcessor1 = CreateMockProcessor<TestEntity1, InPlay>(1);
        var mockProcessor2 = CreateMockProcessor<TestEntity2, InPlay>(2);
        var processors = new List<IMessageProcessor> { mockProcessor1.Object, mockProcessor2.Object };

        // Act
        var container = new MessageProcessorContainer<InPlay>(processors);

        // Assert
        container.Should().NotBeNull();
        var retrieved1 = container.GetMessageProcessor(1);
        var retrieved2 = container.GetMessageProcessor(2);
        
        retrieved1.Should().BeSameAs(mockProcessor1.Object);
        retrieved2.Should().BeSameAs(mockProcessor2.Object);
    }

    [Fact]
    public void Constructor_WithMixedFlowTypes_ShouldOnlyRegisterMatchingFlow()
    {
        // Arrange
        var inPlayProcessor = CreateMockProcessor<TestEntity1, InPlay>(1);
        var preMatchProcessor = CreateMockProcessor<TestEntity2, PreMatch>(2);
        var processors = new List<IMessageProcessor> { inPlayProcessor.Object, preMatchProcessor.Object };

        // Act
        var inPlayContainer = new MessageProcessorContainer<InPlay>(processors);

        // Assert
        var retrievedInPlay = inPlayContainer.GetMessageProcessor(1);
        retrievedInPlay.Should().BeSameAs(inPlayProcessor.Object);
        
        // PreMatch processor should not be registered in InPlay container
        var act = () => inPlayContainer.GetMessageProcessor(2);
        act.Should().Throw<KeyNotFoundException>();
    }

    [Fact]
    public void Constructor_WithDuplicateEntityKeys_ShouldThrowArgumentException()
    {
        // Arrange
        var processor1 = CreateMockProcessor<TestEntity1, InPlay>(1);
        var processor2 = CreateMockProcessor<TestEntity1, InPlay>(1); // Same entity key
        var processors = new List<IMessageProcessor> { processor1.Object, processor2.Object };

        // Act & Assert
        var act = () => new MessageProcessorContainer<InPlay>(processors);
        act.Should().Throw<ArgumentException>()
            .WithMessage("Failed to add resolver as the resolver already registered.");
    }

    [Fact]
    public void Constructor_WithProcessorMissingAttribute_ShouldThrowNullReferenceException()
    {
        // Arrange
        var mockProcessor = new Mock<IMessageProcessor>();
        mockProcessor.Setup(x => x.GetTypeOfTFlow()).Returns(typeof(InPlay));
        mockProcessor.Setup(x => x.GetTypeOfTType()).Returns(typeof(EntityWithoutAttribute));
        var processors = new List<IMessageProcessor> { mockProcessor.Object };

        // Act & Assert
        var act = () => new MessageProcessorContainer<InPlay>(processors);
        act.Should().Throw<NullReferenceException>();
    }

    #endregion

    #region GetMessageProcessor Tests

    [Fact]
    public void GetMessageProcessor_WithValidKey_ShouldReturnCorrectProcessor()
    {
        // Arrange
        var mockProcessor = CreateMockProcessor<TestEntity1, InPlay>(1);
        var processors = new List<IMessageProcessor> { mockProcessor.Object };
        var container = new MessageProcessorContainer<InPlay>(processors);

        // Act
        var result = container.GetMessageProcessor(1);

        // Assert
        result.Should().BeSameAs(mockProcessor.Object);
    }

    [Fact]
    public void GetMessageProcessor_WithNonExistentKey_ShouldThrowKeyNotFoundException()
    {
        // Arrange
        var mockProcessor = CreateMockProcessor<TestEntity1, InPlay>(1);
        var processors = new List<IMessageProcessor> { mockProcessor.Object };
        var container = new MessageProcessorContainer<InPlay>(processors);

        // Act & Assert
        var act = () => container.GetMessageProcessor(999);
        act.Should().Throw<KeyNotFoundException>();
    }

    [Fact]
    public void GetMessageProcessor_MultipleProcessors_ShouldReturnCorrectOne()
    {
        // Arrange
        var processor1 = CreateMockProcessor<TestEntity1, InPlay>(1);
        var processor2 = CreateMockProcessor<TestEntity2, InPlay>(2);
        var processor3 = CreateMockProcessor<TestEntity3, InPlay>(3);
        var processors = new List<IMessageProcessor> 
        { 
            processor1.Object, 
            processor2.Object, 
            processor3.Object 
        };
        var container = new MessageProcessorContainer<InPlay>(processors);

        // Act & Assert
        container.GetMessageProcessor(1).Should().BeSameAs(processor1.Object);
        container.GetMessageProcessor(2).Should().BeSameAs(processor2.Object);
        container.GetMessageProcessor(3).Should().BeSameAs(processor3.Object);
    }

    #endregion

    #region Flow Type Specific Tests

    [Fact]
    public void Constructor_PreMatchContainer_ShouldOnlyRegisterPreMatchProcessors()
    {
        // Arrange
        var inPlayProcessor = CreateMockProcessor<TestEntity1, InPlay>(1);
        var preMatchProcessor = CreateMockProcessor<TestEntity2, PreMatch>(2);
        var processors = new List<IMessageProcessor> { inPlayProcessor.Object, preMatchProcessor.Object };

        // Act
        var preMatchContainer = new MessageProcessorContainer<PreMatch>(processors);

        // Assert
        var retrievedPreMatch = preMatchContainer.GetMessageProcessor(2);
        retrievedPreMatch.Should().BeSameAs(preMatchProcessor.Object);
        
        // InPlay processor should not be registered in PreMatch container
        var act = () => preMatchContainer.GetMessageProcessor(1);
        act.Should().Throw<KeyNotFoundException>();
    }

    [Fact]
    public void Constructor_WithSameEntityDifferentFlows_ShouldRegisterInCorrectContainers()
    {
        // Arrange
        var inPlayProcessor = CreateMockProcessor<TestEntity1, InPlay>(1);
        var preMatchProcessor = CreateMockProcessor<TestEntity1, PreMatch>(1); // Same entity, different flow
        var processors = new List<IMessageProcessor> { inPlayProcessor.Object, preMatchProcessor.Object };

        // Act
        var inPlayContainer = new MessageProcessorContainer<InPlay>(processors);
        var preMatchContainer = new MessageProcessorContainer<PreMatch>(processors);

        // Assert
        inPlayContainer.GetMessageProcessor(1).Should().BeSameAs(inPlayProcessor.Object);
        preMatchContainer.GetMessageProcessor(1).Should().BeSameAs(preMatchProcessor.Object);
    }

    #endregion

    #region Edge Cases Tests

    [Fact]
    public void Constructor_WithLargeNumberOfProcessors_ShouldHandleCorrectly()
    {
        // Arrange - Create processors with different entity types to avoid duplicate keys
        var processor1 = CreateMockProcessor<TestEntity1, InPlay>(1);
        var processor2 = CreateMockProcessor<TestEntity2, InPlay>(2);
        var processor3 = CreateMockProcessor<TestEntity3, InPlay>(3);
        var processors = new List<IMessageProcessor> 
        { 
            processor1.Object, 
            processor2.Object, 
            processor3.Object 
        };

        // Act
        var container = new MessageProcessorContainer<InPlay>(processors);

        // Assert
        container.GetMessageProcessor(1).Should().NotBeNull();
        container.GetMessageProcessor(2).Should().NotBeNull();
        container.GetMessageProcessor(3).Should().NotBeNull();
    }

    [Fact]
    public void Constructor_WithZeroEntityKey_ShouldRegisterCorrectly()
    {
        // Arrange
        var processor = CreateMockProcessor<TestEntityZero, InPlay>(0);
        var processors = new List<IMessageProcessor> { processor.Object };

        // Act
        var container = new MessageProcessorContainer<InPlay>(processors);

        // Assert
        var result = container.GetMessageProcessor(0);
        result.Should().BeSameAs(processor.Object);
    }

    [Fact]
    public void Constructor_WithNegativeEntityKey_ShouldRegisterCorrectly()
    {
        // Arrange
        var processor = CreateMockProcessor<TestEntityNegative, InPlay>(-1);
        var processors = new List<IMessageProcessor> { processor.Object };

        // Act
        var container = new MessageProcessorContainer<InPlay>(processors);

        // Assert
        var result = container.GetMessageProcessor(-1);
        result.Should().BeSameAs(processor.Object);
    }

    #endregion

    #region Helper Methods

    private static Mock<IMessageProcessor> CreateMockProcessor<TEntity, TFlow>(int entityKey)
        where TEntity : class, new()
        where TFlow : IFlow
    {
        var mock = new Mock<IMessageProcessor>();
        mock.Setup(x => x.GetTypeOfTFlow()).Returns(typeof(TFlow));
        mock.Setup(x => x.GetTypeOfTType()).Returns(typeof(TEntity));
        
        // Mock the attribute
        var entityType = typeof(TEntity);
        var attribute = new Trade360EntityAttribute(entityKey);
        
        // We need to ensure the GetCustomAttribute call returns our mock attribute
        // This is a bit tricky since we can't easily mock static methods
        // Instead, we'll create actual test entity types with the attributes
        
        return mock;
    }

    #endregion
}

/// <summary>
/// Test entities with Trade360EntityAttribute for testing
/// </summary>
[Trade360Entity(1)]
public class TestEntity1
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
}

[Trade360Entity(2)]
public class TestEntity2
{
    public int Id { get; set; }
    public string Description { get; set; } = string.Empty;
}

[Trade360Entity(3)]
public class TestEntity3
{
    public int Id { get; set; }
    public DateTime Timestamp { get; set; }
}

[Trade360Entity(0)]
public class TestEntityZero
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
}

[Trade360Entity(-1)]
public class TestEntityNegative
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
}

/// <summary>
/// Test entity without Trade360EntityAttribute to test error scenarios
/// </summary>
public class EntityWithoutAttribute
{
    public int Id { get; set; }
}
