using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using System.Text.Json;
using Trade360SDK.Common.Models;
using Trade360SDK.Feed.FeedType;
using Trade360SDK.Feed.RabbitMQ.Resolvers;
using Trade360SDK.Common.Entities.MessageTypes;

namespace Trade360SDK.Feed.RabbitMQ.Tests;

public class MessageProcessorAdvancedBusinessLogicTests
{
    private readonly Mock<IServiceProvider> _mockServiceProvider;
    private readonly Mock<ILoggerFactory> _mockLoggerFactory;
    private readonly Mock<ILogger<MessageProcessor<MarketUpdate, InPlay>>> _mockLogger;
    private readonly Mock<IEntityHandler<MarketUpdate, InPlay>> _mockHandler;
    private readonly MessageProcessor<MarketUpdate, InPlay> _processor;

    public MessageProcessorAdvancedBusinessLogicTests()
    {
        _mockServiceProvider = new Mock<IServiceProvider>();
        _mockLoggerFactory = new Mock<ILoggerFactory>();
        _mockLogger = new Mock<ILogger<MessageProcessor<MarketUpdate, InPlay>>>();
        _mockHandler = new Mock<IEntityHandler<MarketUpdate, InPlay>>();

        _mockLoggerFactory.Setup(x => x.CreateLogger(It.IsAny<string>()))
            .Returns(_mockLogger.Object);
        _mockServiceProvider.Setup(x => x.GetService(typeof(IEntityHandler<MarketUpdate, InPlay>))).Returns(_mockHandler.Object);

        _processor = new MessageProcessor<MarketUpdate, InPlay>(_mockServiceProvider.Object, _mockLoggerFactory.Object);
    }

    [Fact]
    public async Task ProcessAsync_WithValidMarketUpdateJson_ShouldDeserializeAndProcess()
    {
        var header = new MessageHeader
        {
            Type = 3,
            MessageTimestamp = DateTime.UtcNow
        };

        var marketUpdateJson = """
        {
            "Events": [
                {
                    "Id": 12345,
                    "Status": 1,
                    "Markets": [
                        {
                            "Id": 67890,
                            "Name": "Match Winner",
                            "Status": 1,
                            "Bets": [
                                {
                                    "Id": 111,
                                    "Name": "Team A",
                                    "Status": 1,
                                    "Price": "2.50",
                                    "Probability": 0.4
                                }
                            ]
                        }
                    ]
                }
            ]
        }
        """;

        await _processor.ProcessAsync(typeof(MarketUpdate), header, marketUpdateJson);

        _mockHandler.Verify(h => h.ProcessAsync(
            It.Is<MessageHeader>(mh => mh.Type == 3),
            It.Is<MarketUpdate>(mu => mu.Events != null && mu.Events.Any())), Times.Once);
    }

    [Fact]
    public async Task ProcessAsync_WithInvalidJson_ShouldLogWarningAndProcessNull()
    {
        var header = new MessageHeader
        {
            Type = 3,
            MessageTimestamp = DateTime.UtcNow
        };

        var invalidJson = "{ invalid json structure }";

        await _processor.ProcessAsync(typeof(MarketUpdate), header, invalidJson);

        _mockLogger.Verify(
            x => x.Log(
                LogLevel.Warning,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Failed to deserialize message body")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()),
            Times.Once);

        _mockHandler.Verify(h => h.ProcessAsync(
            It.IsAny<MessageHeader>(),
            It.Is<MarketUpdate>(mu => mu == null)), Times.Once);
    }

    [Fact]
    public async Task ProcessAsync_WithEmptyBody_ShouldNotLogWarningAndProcessNull()
    {
        var header = new MessageHeader
        {
            Type = 3,
            MessageTimestamp = DateTime.UtcNow
        };

        await _processor.ProcessAsync(typeof(MarketUpdate), header, "");

        _mockLogger.Verify(
            x => x.Log(
                LogLevel.Warning,
                It.IsAny<EventId>(),
                It.IsAny<It.IsAnyType>(),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()),
            Times.Never);

        _mockHandler.Verify(h => h.ProcessAsync(
            It.IsAny<MessageHeader>(),
            It.Is<MarketUpdate>(mu => mu == null)), Times.Once);
    }

    [Fact]
    public async Task ProcessAsync_WithNullBody_ShouldNotLogWarningAndProcessNull()
    {
        var header = new MessageHeader
        {
            Type = 3,
            MessageTimestamp = DateTime.UtcNow
        };

        await _processor.ProcessAsync(typeof(MarketUpdate), header, null);

        _mockLogger.Verify(
            x => x.Log(
                LogLevel.Warning,
                It.IsAny<EventId>(),
                It.IsAny<It.IsAnyType>(),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()),
            Times.Never);

        _mockHandler.Verify(h => h.ProcessAsync(
            It.IsAny<MessageHeader>(),
            It.Is<MarketUpdate>(mu => mu == null)), Times.Once);
    }

    [Fact]
    public async Task ProcessAsync_WithComplexMarketData_ShouldDeserializeCompleteStructure()
    {
        var header = new MessageHeader
        {
            Type = 3,
            MessageTimestamp = DateTime.UtcNow,
            MessageBrokerTimestamp = DateTime.UtcNow.AddMilliseconds(-100)
        };

        var complexMarketJson = """
        {
            "Events": [
                {
                    "Id": 12345,
                    "Status": 1,
                    "Markets": [
                        {
                            "Id": 67890,
                            "Name": "Over/Under 2.5 Goals",
                            "Status": 1,
                            "Bets": [
                                {
                                    "Id": 111,
                                    "Name": "Over 2.5",
                                    "Status": 1,
                                    "Price": "1.85",
                                    "PriceUS": "+185",
                                    "PriceUK": "17/20",
                                    "Probability": 0.54,
                                    "LastUpdate": "2024-01-01T12:00:00Z"
                                },
                                {
                                    "Id": 112,
                                    "Name": "Under 2.5",
                                    "Status": 1,
                                    "Price": "2.10",
                                    "PriceUS": "+110",
                                    "PriceUK": "11/10",
                                    "Probability": 0.46,
                                    "LastUpdate": "2024-01-01T12:00:00Z"
                                }
                            ]
                        }
                    ]
                }
            ]
        }
        """;

        await _processor.ProcessAsync(typeof(MarketUpdate), header, complexMarketJson);

        _mockHandler.Verify(h => h.ProcessAsync(
            It.Is<MessageHeader>(mh => mh.Type == 3 && mh.MessageBrokerTimestamp.HasValue),
            It.Is<MarketUpdate>(mu => 
                mu.Events != null && 
                mu.Events.Any() && 
                mu.Events.First().Markets != null &&
                mu.Events.First().Markets.Any() &&
                mu.Events.First().Markets.First().Bets != null &&
                mu.Events.First().Markets.First().Bets.Count() == 2)), Times.Once);
    }

    [Fact]
    public async Task ProcessAsync_WithMalformedJsonStructure_ShouldHandleGracefully()
    {
        var header = new MessageHeader { Type = 3 };
        var malformedJson = """
        {
            "Events": [
                {
                    "Id": "not_a_number",
                    "Status": "invalid_status",
                    "Markets": "should_be_array"
                }
            ]
        }
        """;

        await _processor.ProcessAsync(typeof(MarketUpdate), header, malformedJson);

        _mockLogger.Verify(
            x => x.Log(
                LogLevel.Warning,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Failed to deserialize message body")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()),
            Times.Once);

        _mockHandler.Verify(h => h.ProcessAsync(
            It.IsAny<MessageHeader>(),
            It.Is<MarketUpdate>(mu => mu == null)), Times.Once);
    }

    [Fact]
    public async Task ProcessAsync_WithHandlerException_ShouldPropagateException()
    {
        var header = new MessageHeader { Type = 3 };
        var validJson = """{"Events": []}""";

        _mockHandler.Setup(h => h.ProcessAsync(It.IsAny<MessageHeader>(), It.IsAny<MarketUpdate>()))
                   .ThrowsAsync(new InvalidOperationException("Handler processing failed"));

        var act = async () => await _processor.ProcessAsync(typeof(MarketUpdate), header, validJson);

        await act.Should().ThrowAsync<InvalidOperationException>()
                 .WithMessage("Handler processing failed");
    }

    [Fact]
    public void GetTypeOfTType_ShouldReturnCorrectType()
    {
        var result = _processor.GetTypeOfTType();
        
        result.Should().Be(typeof(MarketUpdate));
    }

    [Fact]
    public void GetTypeOfTFlow_ShouldReturnCorrectFlowType()
    {
        var result = _processor.GetTypeOfTFlow();
        
        result.Should().Be(typeof(InPlay));
    }

    [Fact]
    public async Task ProcessAsync_WithNoHandlerFound_ShouldThrowArgumentException()
    {
        _mockServiceProvider.Setup(x => x.GetService(typeof(IEntityHandler<MarketUpdate, InPlay>)))
                           .Returns(null);

        var header = new MessageHeader { Type = 3 };
        var validJson = """{"Events": []}""";

        var act = async () => await _processor.ProcessAsync(typeof(MarketUpdate), header, validJson);

        await act.Should().ThrowAsync<InvalidOperationException>();
    }

    [Fact]
    public async Task ProcessAsync_WithLargeMessageBody_ShouldProcessEfficiently()
    {
        var header = new MessageHeader { Type = 3 };
        
        var largeEventsArray = Enumerable.Range(1, 1000).Select(i => new
        {
            Id = i,
            Status = 1,
            Markets = Enumerable.Range(1, 10).Select(j => new
            {
                Id = i * 1000 + j,
                Name = $"Market {j}",
                Status = 1,
                Bets = Enumerable.Range(1, 5).Select(k => new
                {
                    Id = i * 10000 + j * 100 + k,
                    Name = $"Bet {k}",
                    Status = 1,
                    Price = "2.00",
                    Probability = 0.2
                }).ToArray()
            }).ToArray()
        }).ToArray();

        var largeJson = JsonSerializer.Serialize(new { Events = largeEventsArray });

        var stopwatch = System.Diagnostics.Stopwatch.StartNew();
        await _processor.ProcessAsync(typeof(MarketUpdate), header, largeJson);
        stopwatch.Stop();

        stopwatch.ElapsedMilliseconds.Should().BeLessThan(5000);

        _mockHandler.Verify(h => h.ProcessAsync(
            It.IsAny<MessageHeader>(),
            It.Is<MarketUpdate>(mu => mu.Events != null && mu.Events.Count() == 1000)), Times.Once);
    }
}
