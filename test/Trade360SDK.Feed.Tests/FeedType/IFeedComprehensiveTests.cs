using FluentAssertions;
using Moq;
using Trade360SDK.Feed;
using System.Threading;
using System.Threading.Tasks;

namespace Trade360SDK.Feed.Tests;

public class IFeedComprehensiveTests : IDisposable
{
    private readonly Mock<IFeed> _mockFeed;
    private readonly CancellationTokenSource _cancellationTokenSource;

    public IFeedComprehensiveTests()
    {
        _mockFeed = new Mock<IFeed>();
        _cancellationTokenSource = new CancellationTokenSource();
    }

    [Fact]
    public async Task StartAsync_WithValidParameters_ShouldExecuteSuccessfully()
    {
        // Arrange
        _mockFeed.Setup(f => f.StartAsync(true, It.IsAny<CancellationToken>()))
                 .Returns(Task.CompletedTask);

        // Act
        var act = async () => await _mockFeed.Object.StartAsync(true, _cancellationTokenSource.Token);

        // Assert
        await act.Should().NotThrowAsync();
        _mockFeed.Verify(f => f.StartAsync(true, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task StartAsync_WithConnectAtStartFalse_ShouldExecuteSuccessfully()
    {
        // Arrange
        _mockFeed.Setup(f => f.StartAsync(false, It.IsAny<CancellationToken>()))
                 .Returns(Task.CompletedTask);

        // Act
        var act = async () => await _mockFeed.Object.StartAsync(false, _cancellationTokenSource.Token);

        // Assert
        await act.Should().NotThrowAsync();
        _mockFeed.Verify(f => f.StartAsync(false, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public async Task StartAsync_WithDifferentConnectAtStartValues_ShouldHandleCorrectly(bool connectAtStart)
    {
        // Arrange
        _mockFeed.Setup(f => f.StartAsync(connectAtStart, It.IsAny<CancellationToken>()))
                 .Returns(Task.CompletedTask);

        // Act
        await _mockFeed.Object.StartAsync(connectAtStart, _cancellationTokenSource.Token);

        // Assert
        _mockFeed.Verify(f => f.StartAsync(connectAtStart, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task StartAsync_WithCancellationToken_ShouldRespectCancellation()
    {
        // Arrange
        var cancellationTokenSource = new CancellationTokenSource();
        _mockFeed.Setup(f => f.StartAsync(It.IsAny<bool>(), It.IsAny<CancellationToken>()))
                 .Returns(async (bool connectAtStart, CancellationToken ct) =>
                 {
                     await Task.Delay(1000, ct);
                 });

        cancellationTokenSource.Cancel();

        // Act & Assert
        var act = async () => await _mockFeed.Object.StartAsync(true, cancellationTokenSource.Token);
        await act.Should().ThrowAsync<OperationCanceledException>();
    }

    [Fact]
    public async Task StartAsync_WithException_ShouldPropagateException()
    {
        // Arrange
        _mockFeed.Setup(f => f.StartAsync(It.IsAny<bool>(), It.IsAny<CancellationToken>()))
                 .ThrowsAsync(new InvalidOperationException("Feed start failed"));

        // Act & Assert
        var act = async () => await _mockFeed.Object.StartAsync(true, _cancellationTokenSource.Token);
        await act.Should().ThrowAsync<InvalidOperationException>()
                 .WithMessage("Feed start failed");
    }

    [Fact]
    public async Task StartAsync_CalledMultipleTimes_ShouldInvokeEachTime()
    {
        // Arrange
        _mockFeed.Setup(f => f.StartAsync(It.IsAny<bool>(), It.IsAny<CancellationToken>()))
                 .Returns(Task.CompletedTask);

        // Act
        await _mockFeed.Object.StartAsync(true, _cancellationTokenSource.Token);
        await _mockFeed.Object.StartAsync(false, _cancellationTokenSource.Token);
        await _mockFeed.Object.StartAsync(true, _cancellationTokenSource.Token);

        // Assert
        _mockFeed.Verify(f => f.StartAsync(It.IsAny<bool>(), It.IsAny<CancellationToken>()), Times.Exactly(3));
    }

    [Fact]
    public async Task StopAsync_WithValidCancellationToken_ShouldExecuteSuccessfully()
    {
        // Arrange
        _mockFeed.Setup(f => f.StopAsync(It.IsAny<CancellationToken>()))
                 .Returns(Task.CompletedTask);

        // Act
        var act = async () => await _mockFeed.Object.StopAsync(_cancellationTokenSource.Token);

        // Assert
        await act.Should().NotThrowAsync();
        _mockFeed.Verify(f => f.StopAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task StopAsync_WithCancellationToken_ShouldRespectCancellation()
    {
        // Arrange
        var cancellationTokenSource = new CancellationTokenSource();
        _mockFeed.Setup(f => f.StopAsync(It.IsAny<CancellationToken>()))
                 .Returns(async (CancellationToken ct) =>
                 {
                     await Task.Delay(1000, ct);
                 });

        cancellationTokenSource.Cancel();

        // Act & Assert
        var act = async () => await _mockFeed.Object.StopAsync(cancellationTokenSource.Token);
        await act.Should().ThrowAsync<OperationCanceledException>();
    }

    [Fact]
    public async Task StopAsync_WithException_ShouldPropagateException()
    {
        // Arrange
        _mockFeed.Setup(f => f.StopAsync(It.IsAny<CancellationToken>()))
                 .ThrowsAsync(new InvalidOperationException("Feed stop failed"));

        // Act & Assert
        var act = async () => await _mockFeed.Object.StopAsync(_cancellationTokenSource.Token);
        await act.Should().ThrowAsync<InvalidOperationException>()
                 .WithMessage("Feed stop failed");
    }

    [Fact]
    public async Task StartAndStopAsync_InSequence_ShouldExecuteCorrectly()
    {
        // Arrange
        _mockFeed.Setup(f => f.StartAsync(It.IsAny<bool>(), It.IsAny<CancellationToken>()))
                 .Returns(Task.CompletedTask);
        _mockFeed.Setup(f => f.StopAsync(It.IsAny<CancellationToken>()))
                 .Returns(Task.CompletedTask);

        // Act
        await _mockFeed.Object.StartAsync(true, _cancellationTokenSource.Token);
        await _mockFeed.Object.StopAsync(_cancellationTokenSource.Token);

        // Assert
        _mockFeed.Verify(f => f.StartAsync(true, It.IsAny<CancellationToken>()), Times.Once);
        _mockFeed.Verify(f => f.StopAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task ConcurrentStartStopOperations_ShouldHandleCorrectly()
    {
        // Arrange
        _mockFeed.Setup(f => f.StartAsync(It.IsAny<bool>(), It.IsAny<CancellationToken>()))
                 .Returns(Task.Delay(100));
        _mockFeed.Setup(f => f.StopAsync(It.IsAny<CancellationToken>()))
                 .Returns(Task.Delay(100));

        // Act
        var startTask = _mockFeed.Object.StartAsync(true, _cancellationTokenSource.Token);
        var stopTask = _mockFeed.Object.StopAsync(_cancellationTokenSource.Token);

        await Task.WhenAll(startTask, stopTask);

        // Assert
        _mockFeed.Verify(f => f.StartAsync(true, It.IsAny<CancellationToken>()), Times.Once);
        _mockFeed.Verify(f => f.StopAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public void Dispose_ShouldNotThrow()
    {
        // Arrange & Act
        var act = () => _mockFeed.Object.Dispose();

        // Assert
        act.Should().NotThrow();
        _mockFeed.Verify(f => f.Dispose(), Times.Once);
    }

    [Fact]
    public void Dispose_CalledMultipleTimes_ShouldNotThrow()
    {
        // Arrange & Act
        _mockFeed.Object.Dispose();
        _mockFeed.Object.Dispose();
        _mockFeed.Object.Dispose();

        // Assert
        _mockFeed.Verify(f => f.Dispose(), Times.Exactly(3));
    }

    [Fact]
    public async Task StartAsync_WithTimeoutCancellation_ShouldHandleGracefully()
    {
        // Arrange
        using var timeoutCts = new CancellationTokenSource(TimeSpan.FromMilliseconds(50));
        _mockFeed.Setup(f => f.StartAsync(It.IsAny<bool>(), It.IsAny<CancellationToken>()))
                 .Returns(async (bool connectAtStart, CancellationToken ct) =>
                 {
                     await Task.Delay(200, ct); // Longer than timeout
                 });

        // Act & Assert
        var act = async () => await _mockFeed.Object.StartAsync(true, timeoutCts.Token);
        await act.Should().ThrowAsync<OperationCanceledException>();
    }

    [Fact]
    public async Task StopAsync_WithTimeoutCancellation_ShouldHandleGracefully()
    {
        // Arrange
        using var timeoutCts = new CancellationTokenSource(TimeSpan.FromMilliseconds(50));
        _mockFeed.Setup(f => f.StopAsync(It.IsAny<CancellationToken>()))
                 .Returns(async (CancellationToken ct) =>
                 {
                     await Task.Delay(200, ct); // Longer than timeout
                 });

        // Act & Assert
        var act = async () => await _mockFeed.Object.StopAsync(timeoutCts.Token);
        await act.Should().ThrowAsync<OperationCanceledException>();
    }

    [Fact]
    public async Task StartAsync_WithTaskCancelledException_ShouldPropagateCorrectly()
    {
        // Arrange
        _mockFeed.Setup(f => f.StartAsync(It.IsAny<bool>(), It.IsAny<CancellationToken>()))
                 .ThrowsAsync(new TaskCanceledException("Operation was cancelled"));

        // Act & Assert
        var act = async () => await _mockFeed.Object.StartAsync(true, _cancellationTokenSource.Token);
        await act.Should().ThrowAsync<TaskCanceledException>()
                 .WithMessage("Operation was cancelled");
    }

    [Fact]
    public async Task StopAsync_WithTaskCancelledException_ShouldPropagateCorrectly()
    {
        // Arrange
        _mockFeed.Setup(f => f.StopAsync(It.IsAny<CancellationToken>()))
                 .ThrowsAsync(new TaskCanceledException("Stop operation was cancelled"));

        // Act & Assert
        var act = async () => await _mockFeed.Object.StopAsync(_cancellationTokenSource.Token);
        await act.Should().ThrowAsync<TaskCanceledException>()
                 .WithMessage("Stop operation was cancelled");
    }

    [Fact]
    public async Task FeedLifecycle_CompleteStartStopCycle_ShouldExecuteInOrder()
    {
        // Arrange
        var callOrder = new List<string>();
        _mockFeed.Setup(f => f.StartAsync(It.IsAny<bool>(), It.IsAny<CancellationToken>()))
                 .Returns(Task.CompletedTask)
                 .Callback(() => callOrder.Add("Start"));
        _mockFeed.Setup(f => f.StopAsync(It.IsAny<CancellationToken>()))
                 .Returns(Task.CompletedTask)
                 .Callback(() => callOrder.Add("Stop"));
        _mockFeed.Setup(f => f.Dispose())
                 .Callback(() => callOrder.Add("Dispose"));

        // Act
        await _mockFeed.Object.StartAsync(true, _cancellationTokenSource.Token);
        await _mockFeed.Object.StopAsync(_cancellationTokenSource.Token);
        _mockFeed.Object.Dispose();

        // Assert
        callOrder.Should().Equal("Start", "Stop", "Dispose");
    }

    private void Dispose(bool disposing)
    {
        if (disposing)
        {
            _cancellationTokenSource?.Dispose();
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
} 