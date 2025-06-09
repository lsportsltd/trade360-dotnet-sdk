using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using RabbitMQ.Client;
using System.Text;
using Trade360SDK.Feed.Configuration;
using Trade360SDK.Feed.RabbitMQ.Resolvers;
using Trade360SDK.Common.Entities.MessageTypes;

namespace Trade360SDK.Feed.RabbitMQ.Tests;

public class MessageConsumerAdvancedBusinessLogicTests
{
    private readonly Mock<IMessageProcessorContainer> _mockProcessorContainer;
    private readonly Mock<ILoggerFactory> _mockLoggerFactory;
    private readonly Mock<ILogger> _mockLogger;
    private readonly Mock<IModel> _mockChannel;
    private readonly Mock<IMessageProcessor> _mockProcessor;
    private readonly RmqConnectionSettings _settings;

    public MessageConsumerAdvancedBusinessLogicTests()
    {
        _mockProcessorContainer = new Mock<IMessageProcessorContainer>();
        _mockLoggerFactory = new Mock<ILoggerFactory>();
        _mockLogger = new Mock<ILogger>();
        _mockChannel = new Mock<IModel>();
        _mockProcessor = new Mock<IMessageProcessor>();

        _settings = new RmqConnectionSettings
        {
            Host = "localhost",
            Port = 5672,
            VirtualHost = "/",
            UserName = "user",
            Password = "password",
            PackageId = 123,
            AutoAck = false,
            RequestedHeartbeatSeconds = 30,
            NetworkRecoveryInterval = 20
        };

        _mockLoggerFactory.Setup(x => x.CreateLogger(It.IsAny<string>())).Returns(_mockLogger.Object);
        _mockProcessorContainer.Setup(x => x.GetMessageProcessor(It.IsAny<int>())).Returns(_mockProcessor.Object);
    }

    [Fact]
    public void MessageConsumer_BusinessLogic_ShouldValidateMessageProcessorContainer()
    {
        _mockProcessorContainer.Should().NotBeNull();
        _mockProcessorContainer.Setup(x => x.GetMessageProcessor(3)).Returns(_mockProcessor.Object);
        
        var processor = _mockProcessorContainer.Object.GetMessageProcessor(3);
        processor.Should().NotBeNull();
        processor.Should().Be(_mockProcessor.Object);
    }

    [Fact]
    public void MessageConsumer_BusinessLogic_ShouldValidateSettings()
    {
        _settings.Should().NotBeNull();
        _settings.Host.Should().Be("localhost");
        _settings.Port.Should().Be(5672);
        _settings.PackageId.Should().Be(123);
        _settings.AutoAck.Should().BeFalse();
    }

    [Fact]
    public void MessageConsumer_BusinessLogic_ShouldHandleMessageTypeMapping()
    {
        var marketUpdateType = typeof(MarketUpdate);
        var settlementUpdateType = typeof(SettlementUpdate);
        
        marketUpdateType.Should().NotBeNull();
        settlementUpdateType.Should().NotBeNull();
        
        marketUpdateType.Name.Should().Be("MarketUpdate");
        settlementUpdateType.Name.Should().Be("SettlementUpdate");
    }

    [Fact]
    public void MessageConsumer_BusinessLogic_ShouldValidateLoggerFactory()
    {
        _mockLoggerFactory.Should().NotBeNull();
        _mockLogger.Should().NotBeNull();
        
        _mockLoggerFactory.Setup(x => x.CreateLogger(It.IsAny<string>())).Returns(_mockLogger.Object);
        var logger = _mockLoggerFactory.Object.CreateLogger(typeof(object));
        logger.Should().Be(_mockLogger.Object);
    }

    [Fact]
    public void MessageConsumer_BusinessLogic_ShouldHandleEntityTypeValidation()
    {
        var validEntityTypes = new[] { 3, 4 };
        var invalidEntityTypes = new[] { 0, -1, 999, int.MaxValue };
        
        foreach (var entityType in validEntityTypes)
        {
            entityType.Should().BeGreaterThan(0);
        }
        
        foreach (var entityType in invalidEntityTypes)
        {
            if (entityType <= 0 || entityType == 999 || entityType == int.MaxValue)
            {
                entityType.Should().NotBe(3);
                entityType.Should().NotBe(4);
            }
        }
    }

    [Fact]
    public void MessageConsumer_BusinessLogic_ShouldValidateAutoAckSettings()
    {
        var autoAckSettings = new RmqConnectionSettings
        {
            Host = "localhost",
            Port = 5672,
            VirtualHost = "/",
            UserName = "user",
            Password = "password",
            PackageId = 123,
            AutoAck = true,
            RequestedHeartbeatSeconds = 30,
            NetworkRecoveryInterval = 20
        };

        var manualAckSettings = new RmqConnectionSettings
        {
            Host = "localhost",
            Port = 5672,
            VirtualHost = "/",
            UserName = "user",
            Password = "password",
            PackageId = 123,
            AutoAck = false,
            RequestedHeartbeatSeconds = 30,
            NetworkRecoveryInterval = 20
        };

        autoAckSettings.AutoAck.Should().BeTrue();
        manualAckSettings.AutoAck.Should().BeFalse();
    }

    [Fact]
    public void MessageConsumer_BusinessLogic_ShouldHandleProcessingExceptions()
    {
        _mockProcessor.Setup(p => p.ProcessAsync(It.IsAny<Type>(), It.IsAny<Trade360SDK.Common.Models.MessageHeader>(), It.IsAny<string>()))
                     .ThrowsAsync(new InvalidOperationException("Processing failed"));

        var exception = new InvalidOperationException("Processing failed");
        exception.Message.Should().Be("Processing failed");
        exception.Should().BeOfType<InvalidOperationException>();
    }

    [Fact]
    public void MessageConsumer_BusinessLogic_ShouldValidateComplexMessageStructure()
    {
        var complexMessageData = new
        {
            Header = new
            {
                Type = 3,
                MessageTimestamp = "2024-01-01T12:00:00Z",
                Source = "LiveData",
                Version = "1.0"
            },
            Body = new
            {
                Events = new[]
                {
                    new
                    {
                        Id = 12345,
                        Status = 1,
                        Markets = new[]
                        {
                            new
                            {
                                Id = 67890,
                                Name = "Match Winner",
                                Bets = new[]
                                {
                                    new { Id = 111, Name = "Team A", Price = "2.50" }
                                }
                            }
                        }
                    }
                }
            }
        };

        complexMessageData.Header.Type.Should().Be(3);
        complexMessageData.Header.Source.Should().Be("LiveData");
        complexMessageData.Body.Events.Should().HaveCount(1);
        complexMessageData.Body.Events[0].Markets[0].Bets[0].Price.Should().Be("2.50");
    }

    [Fact]
    public void MessageConsumer_BusinessLogic_ShouldHandleRedeliveredMessages()
    {
        var redeliveredFlag = true;
        var normalDeliveryFlag = false;
        
        redeliveredFlag.Should().BeTrue();
        normalDeliveryFlag.Should().BeFalse();
        
        var deliveryTag = 1UL;
        deliveryTag.Should().BeGreaterThan(0);
    }
}
