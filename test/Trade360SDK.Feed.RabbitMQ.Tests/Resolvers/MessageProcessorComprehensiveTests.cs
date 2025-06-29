using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Text.Json;
using System.Threading.Tasks;
using Trade360SDK.Common.Models;
using Trade360SDK.Feed.FeedType;
using Trade360SDK.Feed.RabbitMQ.Resolvers;
using Xunit;

namespace Trade360SDK.Feed.RabbitMQ.Tests.Resolvers;

/// <summary>
/// Comprehensive tests for MessageProcessor class covering all code paths,
/// error handling scenarios, and type operations.
/// </summary>
public class MessageProcessorComprehensiveTests
{
    private readonly Mock<IServiceProvider> _mockServiceProvider;
    private readonly Mock<ILoggerFactory> _mockLoggerFactory;
    private readonly Mock<ILogger<MessageProcessor<TestEntity, InPlay>>> _mockLogger;
    private readonly Mock<IEntityHandler<TestEntity, InPlay>> _mockHandler;
    private readonly MessageProcessor<TestEntity, InPlay> _messageProcessor;

    public MessageProcessorComprehensiveTests()
    {
        _mockServiceProvider = new Mock<IServiceProvider>();
        _mockLoggerFactory = new Mock<ILoggerFactory>();
        _mockLogger = new Mock<ILogger<MessageProcessor<TestEntity, InPlay>>>();
        _mockHandler = new Mock<IEntityHandler<TestEntity, InPlay>>();

        _mockLoggerFactory.Setup(x => x.CreateLogger(It.IsAny<string>()))
            .Returns(_mockLogger.Object);

        _mockServiceProvider.Setup(x => x.GetService(typeof(IEntityHandler<TestEntity, InPlay>)))
            .Returns(_mockHandler.Object);

        _messageProcessor = new MessageProcessor<TestEntity, InPlay>(
            _mockServiceProvider.Object, 
            _mockLoggerFactory.Object);
    }

    #region Constructor Tests

    [Fact]
    public void Constructor_WithValidParameters_ShouldCreateInstance()
    {
        // Act & Assert
        _messageProcessor.Should().NotBeNull();
        _mockLoggerFactory.Verify(x => x.CreateLogger(It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public void Constructor_WithNullServiceProvider_ShouldCreateInstance()
    {
        // Act & Assert - Constructor doesn't validate parameters
        var processor = new MessageProcessor<TestEntity, InPlay>(null!, _mockLoggerFactory.Object);
        processor.Should().NotBeNull();
    }

    [Fact]
    public void Constructor_WithNullLoggerFactory_ShouldThrowArgumentNullException()
    {
        // Act & Assert
        var act = () => new MessageProcessor<TestEntity, InPlay>(_mockServiceProvider.Object, null!);
        act.Should().Throw<ArgumentNullException>();
    }

    #endregion

    #region ProcessAsync Tests

    [Fact]
    public async Task ProcessAsync_WithValidMessageAndHandler_ShouldProcessSuccessfully()
    {
        // Arrange
        var header = new MessageHeader { Type = 1, MessageTimestamp = DateTime.UtcNow };
        var testEntity = new TestEntity { Id = 123, Name = "Test" };
        var body = JsonSerializer.Serialize(testEntity);

        // Act
        await _messageProcessor.ProcessAsync(typeof(TestEntity), header, body);

        // Assert
        _mockHandler.Verify(x => x.ProcessAsync(header, It.Is<TestEntity>(e => e.Id == 123 && e.Name == "Test")), Times.Once);
    }

    [Fact]
    public async Task ProcessAsync_WithNullHeader_ShouldStillProcessMessage()
    {
        // Arrange
        var testEntity = new TestEntity { Id = 456, Name = "NullHeader" };
        var body = JsonSerializer.Serialize(testEntity);

        // Act
        await _messageProcessor.ProcessAsync(typeof(TestEntity), null, body);

        // Assert
        _mockHandler.Verify(x => x.ProcessAsync(null, It.Is<TestEntity>(e => e.Id == 456 && e.Name == "NullHeader")), Times.Once);
    }

    [Fact]
    public async Task ProcessAsync_WithNullBody_ShouldProcessWithNullMessage()
    {
        // Arrange
        var header = new MessageHeader { Type = 1, MessageTimestamp = DateTime.UtcNow };

        // Act
        await _messageProcessor.ProcessAsync(typeof(TestEntity), header, null);

        // Assert
        _mockHandler.Verify(x => x.ProcessAsync(header, null), Times.Once);
    }

    [Fact]
    public async Task ProcessAsync_WithEmptyBody_ShouldProcessWithNullMessage()
    {
        // Arrange
        var header = new MessageHeader { Type = 1, MessageTimestamp = DateTime.UtcNow };

        // Act
        await _messageProcessor.ProcessAsync(typeof(TestEntity), header, "");

        // Assert
        _mockHandler.Verify(x => x.ProcessAsync(header, null), Times.Once);
    }

    [Fact]
    public async Task ProcessAsync_WithInvalidJsonBody_ShouldLogWarningAndProcessWithNull()
    {
        // Arrange
        var header = new MessageHeader { Type = 1, MessageTimestamp = DateTime.UtcNow };
        var invalidJson = "{ invalid json }";

        // Act
        await _messageProcessor.ProcessAsync(typeof(TestEntity), header, invalidJson);

        // Assert
        _mockHandler.Verify(x => x.ProcessAsync(header, null), Times.Once);
        
        // Verify warning was logged
        _mockLogger.Verify(
            x => x.Log(
                LogLevel.Warning,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("Failed to deserialize message body")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
    }

    [Fact]
    public async Task ProcessAsync_WithMalformedJsonBody_ShouldLogWarningAndProcessWithNull()
    {
        // Arrange
        var header = new MessageHeader { Type = 1, MessageTimestamp = DateTime.UtcNow };
        var malformedJson = "{ \"Id\": \"not_a_number\", \"Name\": 123 }";

        // Act
        await _messageProcessor.ProcessAsync(typeof(TestEntity), header, malformedJson);

        // Assert
        _mockHandler.Verify(x => x.ProcessAsync(header, null), Times.Once);
        
        // Verify warning was logged with the actual body
        _mockLogger.Verify(
            x => x.Log(
                LogLevel.Warning,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains($"Failed to deserialize message body {malformedJson}")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
    }

    [Fact]
    public async Task ProcessAsync_WithNoHandlerFound_ShouldThrowInvalidOperationException()
    {
        // Arrange
        _mockServiceProvider.Setup(x => x.GetService(typeof(IEntityHandler<TestEntity, InPlay>)))
            .Returns((IEntityHandler<TestEntity, InPlay>)null!);

        var header = new MessageHeader { Type = 1, MessageTimestamp = DateTime.UtcNow };
        var body = "{}";

        // Act & Assert
        var act = async () => await _messageProcessor.ProcessAsync(typeof(TestEntity), header, body);
        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("*has been registered*");
    }

    [Fact]
    public async Task ProcessAsync_WithHandlerException_ShouldPropagateException()
    {
        // Arrange
        var header = new MessageHeader { Type = 1, MessageTimestamp = DateTime.UtcNow };
        var body = JsonSerializer.Serialize(new TestEntity { Id = 789, Name = "ExceptionTest" });
        var expectedException = new InvalidOperationException("Handler processing failed");

        _mockHandler.Setup(x => x.ProcessAsync(It.IsAny<MessageHeader>(), It.IsAny<TestEntity>()))
            .ThrowsAsync(expectedException);

        // Act & Assert
        var act = async () => await _messageProcessor.ProcessAsync(typeof(TestEntity), header, body);
        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("Handler processing failed");
    }

    [Fact]
    public async Task ProcessAsync_WithComplexObject_ShouldDeserializeCorrectly()
    {
        // Arrange
        var header = new MessageHeader 
        { 
            Type = 1, 
            MessageTimestamp = DateTime.UtcNow,
            MessageBrokerTimestamp = DateTime.UtcNow.AddMinutes(-1)
        };
        
        var complexEntity = new TestEntity 
        { 
            Id = 999, 
            Name = "Complex Test",
            Timestamp = DateTime.UtcNow,
            IsActive = true,
            Score = 95.5
        };
        
        var body = JsonSerializer.Serialize(complexEntity);

        // Act
        await _messageProcessor.ProcessAsync(typeof(TestEntity), header, body);

        // Assert
        _mockHandler.Verify(x => x.ProcessAsync(
            header, 
            It.Is<TestEntity>(e => 
                e.Id == 999 && 
                e.Name == "Complex Test" && 
                e.IsActive == true && 
                Math.Abs(e.Score - 95.5) < 0.001)), 
            Times.Once);
    }

    #endregion

    #region Type Information Tests

    [Fact]
    public void GetTypeOfTType_ShouldReturnCorrectType()
    {
        // Act
        var result = _messageProcessor.GetTypeOfTType();

        // Assert
        result.Should().Be(typeof(TestEntity));
    }

    [Fact]
    public void GetTypeOfTFlow_ShouldReturnCorrectFlowType()
    {
        // Act
        var result = _messageProcessor.GetTypeOfTFlow();

        // Assert
        result.Should().Be(typeof(InPlay));
    }

    #endregion

    #region Different Flow Types Tests

    [Fact]
    public void MessageProcessor_WithPreMatchFlow_ShouldWorkCorrectly()
    {
        // Arrange
        var mockPreMatchHandler = new Mock<IEntityHandler<TestEntity, PreMatch>>();
        var mockPreMatchLogger = new Mock<ILogger<MessageProcessor<TestEntity, PreMatch>>>();
        
        _mockLoggerFactory.Setup(x => x.CreateLogger(typeof(MessageProcessor<TestEntity, PreMatch>).FullName))
            .Returns(mockPreMatchLogger.Object);
        
        _mockServiceProvider.Setup(x => x.GetService(typeof(IEntityHandler<TestEntity, PreMatch>)))
            .Returns(mockPreMatchHandler.Object);

        // Act
        var preMatchProcessor = new MessageProcessor<TestEntity, PreMatch>(
            _mockServiceProvider.Object, 
            _mockLoggerFactory.Object);

        // Assert
        preMatchProcessor.GetTypeOfTFlow().Should().Be(typeof(PreMatch));
        preMatchProcessor.GetTypeOfTType().Should().Be(typeof(TestEntity));
    }

    #endregion

    #region Edge Cases Tests

    [Fact]
    public async Task ProcessAsync_WithWhitespaceOnlyBody_ShouldProcessWithNull()
    {
        // Arrange
        var header = new MessageHeader { Type = 1, MessageTimestamp = DateTime.UtcNow };
        var whitespaceBody = "   \t\n\r   ";

        // Act
        await _messageProcessor.ProcessAsync(typeof(TestEntity), header, whitespaceBody);

        // Assert
        _mockHandler.Verify(x => x.ProcessAsync(header, null), Times.Once);
    }

    [Fact]
    public async Task ProcessAsync_WithNullJsonValue_ShouldProcessWithNull()
    {
        // Arrange
        var header = new MessageHeader { Type = 1, MessageTimestamp = DateTime.UtcNow };
        var nullJson = "null";

        // Act
        await _messageProcessor.ProcessAsync(typeof(TestEntity), header, nullJson);

        // Assert
        _mockHandler.Verify(x => x.ProcessAsync(header, null), Times.Once);
    }

    [Fact]
    public async Task ProcessAsync_WithValidJsonButWrongType_ShouldLogWarningAndProcessWithNull()
    {
        // Arrange
        var header = new MessageHeader { Type = 1, MessageTimestamp = DateTime.UtcNow };
        var wrongTypeJson = "\"this is a string not a TestEntity\"";

        // Act
        await _messageProcessor.ProcessAsync(typeof(TestEntity), header, wrongTypeJson);

        // Assert
        _mockHandler.Verify(x => x.ProcessAsync(header, null), Times.Once);
        
        // Verify warning was logged
        _mockLogger.Verify(
            x => x.Log(
                LogLevel.Warning,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("Failed to deserialize message body")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
    }

    #endregion
}

/// <summary>
/// Test entity class for MessageProcessor tests
/// </summary>
public class TestEntity
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public DateTime Timestamp { get; set; }
    public bool IsActive { get; set; }
    public double Score { get; set; }
} 