using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Trade360SDK.Feed;
using Xunit;

namespace Trade360SDK.Feed.Tests
{
    public class IFeedInterfaceTests
    {
        [Fact]
        public void IFeed_ShouldBeInterface()
        {
            // Act
            var feedType = typeof(IFeed);

            // Assert
            feedType.IsInterface.Should().BeTrue();
            feedType.Name.Should().Be("IFeed");
            feedType.Namespace.Should().Be("Trade360SDK.Feed");
        }

        [Fact]
        public void IFeed_ShouldImplementIDisposable()
        {
            // Act
            var feedType = typeof(IFeed);

            // Assert
            feedType.Should().BeAssignableTo<IDisposable>();
        }

        [Fact]
        public async Task IFeed_StartAsync_ShouldBeCallableWithConnectAtStartTrue()
        {
            // Arrange
            var mockFeed = new Mock<IFeed>();
            var cancellationToken = CancellationToken.None;

            // Act
            await mockFeed.Object.StartAsync(true, cancellationToken);

            // Assert
            mockFeed.Verify(f => f.StartAsync(true, cancellationToken), Times.Once);
        }

        [Fact]
        public async Task IFeed_StartAsync_ShouldBeCallableWithConnectAtStartFalse()
        {
            // Arrange
            var mockFeed = new Mock<IFeed>();
            var cancellationToken = CancellationToken.None;

            // Act
            await mockFeed.Object.StartAsync(false, cancellationToken);

            // Assert
            mockFeed.Verify(f => f.StartAsync(false, cancellationToken), Times.Once);
        }

        [Fact]
        public async Task IFeed_StartAsync_ShouldBeCallableWithCancellationToken()
        {
            // Arrange
            var mockFeed = new Mock<IFeed>();
            var cancellationTokenSource = new CancellationTokenSource();
            var cancellationToken = cancellationTokenSource.Token;

            // Act
            await mockFeed.Object.StartAsync(true, cancellationToken);

            // Assert
            mockFeed.Verify(f => f.StartAsync(true, cancellationToken), Times.Once);
        }

        [Fact]
        public async Task IFeed_StopAsync_ShouldBeCallable()
        {
            // Arrange
            var mockFeed = new Mock<IFeed>();
            var cancellationToken = CancellationToken.None;

            // Act
            await mockFeed.Object.StopAsync(cancellationToken);

            // Assert
            mockFeed.Verify(f => f.StopAsync(cancellationToken), Times.Once);
        }

        [Fact]
        public async Task IFeed_StopAsync_ShouldBeCallableWithCancellationToken()
        {
            // Arrange
            var mockFeed = new Mock<IFeed>();
            var cancellationTokenSource = new CancellationTokenSource();
            var cancellationToken = cancellationTokenSource.Token;

            // Act
            await mockFeed.Object.StopAsync(cancellationToken);

            // Assert
            mockFeed.Verify(f => f.StopAsync(cancellationToken), Times.Once);
        }

        [Fact]
        public void IFeed_Dispose_ShouldBeCallable()
        {
            // Arrange
            var mockFeed = new Mock<IFeed>();

            // Act
            mockFeed.Object.Dispose();

            // Assert
            mockFeed.Verify(f => f.Dispose(), Times.Once);
        }

        [Fact]
        public async Task IFeed_StartAndStopSequence_ShouldWork()
        {
            // Arrange
            var mockFeed = new Mock<IFeed>();
            var cancellationToken = CancellationToken.None;

            // Act
            await mockFeed.Object.StartAsync(true, cancellationToken);
            await mockFeed.Object.StopAsync(cancellationToken);

            // Assert
            mockFeed.Verify(f => f.StartAsync(true, cancellationToken), Times.Once);
            mockFeed.Verify(f => f.StopAsync(cancellationToken), Times.Once);
        }

        [Fact]
        public async Task IFeed_MultipleStartCalls_ShouldBeTracked()
        {
            // Arrange
            var mockFeed = new Mock<IFeed>();
            var cancellationToken = CancellationToken.None;

            // Act
            await mockFeed.Object.StartAsync(true, cancellationToken);
            await mockFeed.Object.StartAsync(false, cancellationToken);

            // Assert
            mockFeed.Verify(f => f.StartAsync(It.IsAny<bool>(), cancellationToken), Times.Exactly(2));
        }

        [Fact]
        public async Task IFeed_MultipleStopCalls_ShouldBeTracked()
        {
            // Arrange
            var mockFeed = new Mock<IFeed>();
            var cancellationToken = CancellationToken.None;

            // Act
            await mockFeed.Object.StopAsync(cancellationToken);
            await mockFeed.Object.StopAsync(cancellationToken);

            // Assert
            mockFeed.Verify(f => f.StopAsync(cancellationToken), Times.Exactly(2));
        }

        [Fact]
        public void IFeed_MultipleDisposeCalls_ShouldBeTracked()
        {
            // Arrange
            var mockFeed = new Mock<IFeed>();

            // Act
            mockFeed.Object.Dispose();
            mockFeed.Object.Dispose();

            // Assert
            mockFeed.Verify(f => f.Dispose(), Times.Exactly(2));
        }

        [Fact]
        public async Task IFeed_WithCancelledToken_ShouldStillBeCalled()
        {
            // Arrange
            var mockFeed = new Mock<IFeed>();
            var cancellationTokenSource = new CancellationTokenSource();
            cancellationTokenSource.Cancel();
            var cancellationToken = cancellationTokenSource.Token;

            // Act
            await mockFeed.Object.StartAsync(true, cancellationToken);
            await mockFeed.Object.StopAsync(cancellationToken);

            // Assert
            mockFeed.Verify(f => f.StartAsync(true, cancellationToken), Times.Once);
            mockFeed.Verify(f => f.StopAsync(cancellationToken), Times.Once);
        }

        [Fact]
        public void IFeed_HasCorrectMethodSignatures()
        {
            // Act
            var feedType = typeof(IFeed);
            var startMethod = feedType.GetMethod("StartAsync");
            var stopMethod = feedType.GetMethod("StopAsync");

            // Assert
            startMethod.Should().NotBeNull();
            startMethod!.ReturnType.Should().Be(typeof(Task));
            startMethod.GetParameters().Should().HaveCount(2);
            startMethod.GetParameters()[0].ParameterType.Should().Be(typeof(bool));
            startMethod.GetParameters()[1].ParameterType.Should().Be(typeof(CancellationToken));

            stopMethod.Should().NotBeNull();
            stopMethod!.ReturnType.Should().Be(typeof(Task));
            stopMethod.GetParameters().Should().HaveCount(1);
            stopMethod.GetParameters()[0].ParameterType.Should().Be(typeof(CancellationToken));
        }
    }
} 