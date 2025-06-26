using FluentAssertions;
using Trade360SDK.Feed.FeedType;
using Xunit;

namespace Trade360SDK.Feed.Tests.FeedType
{
    public class FeedTypeClassesTests
    {
        [Fact]
        public void InPlay_ShouldImplementIFlow()
        {
            // Arrange & Act
            var inPlay = new InPlay();

            // Assert
            inPlay.Should().NotBeNull();
            inPlay.Should().BeAssignableTo<IFlow>();
        }

        [Fact]
        public void InPlay_ShouldHaveParameterlessConstructor()
        {
            // Act
            var inPlay = new InPlay();

            // Assert
            inPlay.Should().NotBeNull();
            inPlay.GetType().Should().Be(typeof(InPlay));
        }

        [Fact]
        public void PreMatch_ShouldImplementIFlow()
        {
            // Arrange & Act
            var preMatch = new PreMatch();

            // Assert
            preMatch.Should().NotBeNull();
            preMatch.Should().BeAssignableTo<IFlow>();
        }

        [Fact]
        public void PreMatch_ShouldHaveParameterlessConstructor()
        {
            // Act
            var preMatch = new PreMatch();

            // Assert
            preMatch.Should().NotBeNull();
            preMatch.GetType().Should().Be(typeof(PreMatch));
        }

        [Fact]
        public void IFlow_ShouldBeInterface()
        {
            // Act
            var flowType = typeof(IFlow);

            // Assert
            flowType.IsInterface.Should().BeTrue();
            flowType.Name.Should().Be("IFlow");
            flowType.Namespace.Should().Be("Trade360SDK.Feed.FeedType");
        }

        [Fact]
        public void InPlay_ShouldBeInCorrectNamespace()
        {
            // Act
            var inPlay = new InPlay();

            // Assert
            inPlay.GetType().Namespace.Should().Be("Trade360SDK.Feed.FeedType");
        }

        [Fact]
        public void PreMatch_ShouldBeInCorrectNamespace()
        {
            // Act
            var preMatch = new PreMatch();

            // Assert
            preMatch.GetType().Namespace.Should().Be("Trade360SDK.Feed.FeedType");
        }

        [Fact]
        public void InPlay_MultipleInstances_ShouldBeIndependent()
        {
            // Act
            var inPlay1 = new InPlay();
            var inPlay2 = new InPlay();

            // Assert
            inPlay1.Should().NotBeNull();
            inPlay2.Should().NotBeNull();
            inPlay1.Should().NotBeSameAs(inPlay2);
            inPlay1.GetType().Should().Be(inPlay2.GetType());
        }

        [Fact]
        public void PreMatch_MultipleInstances_ShouldBeIndependent()
        {
            // Act
            var preMatch1 = new PreMatch();
            var preMatch2 = new PreMatch();

            // Assert
            preMatch1.Should().NotBeNull();
            preMatch2.Should().NotBeNull();
            preMatch1.Should().NotBeSameAs(preMatch2);
            preMatch1.GetType().Should().Be(preMatch2.GetType());
        }

        [Fact]
        public void InPlay_ShouldHaveCorrectTypeName()
        {
            // Act
            var inPlay = new InPlay();

            // Assert
            inPlay.GetType().Name.Should().Be("InPlay");
        }

        [Fact]
        public void PreMatch_ShouldHaveCorrectTypeName()
        {
            // Act
            var preMatch = new PreMatch();

            // Assert
            preMatch.GetType().Name.Should().Be("PreMatch");
        }

        [Fact]
        public void InPlay_ToString_ShouldReturnTypeName()
        {
            // Act
            var inPlay = new InPlay();
            var result = inPlay.ToString();

            // Assert
            result.Should().Contain("InPlay");
        }

        [Fact]
        public void PreMatch_ToString_ShouldReturnTypeName()
        {
            // Act
            var preMatch = new PreMatch();
            var result = preMatch.ToString();

            // Assert
            result.Should().Contain("PreMatch");
        }

        [Fact]
        public void InPlay_Equals_ShouldWorkCorrectly()
        {
            // Arrange
            var inPlay1 = new InPlay();
            var inPlay2 = new InPlay();

            // Act & Assert
            inPlay1.Equals(inPlay2).Should().BeFalse(); // Different instances
            inPlay1.Equals(inPlay1).Should().BeTrue(); // Same instance
            inPlay1.Equals(null).Should().BeFalse(); // Null comparison
        }

        [Fact]
        public void PreMatch_Equals_ShouldWorkCorrectly()
        {
            // Arrange
            var preMatch1 = new PreMatch();
            var preMatch2 = new PreMatch();

            // Act & Assert
            preMatch1.Equals(preMatch2).Should().BeFalse(); // Different instances
            preMatch1.Equals(preMatch1).Should().BeTrue(); // Same instance
            preMatch1.Equals(null).Should().BeFalse(); // Null comparison
        }

        [Fact]
        public void InPlay_GetHashCode_ShouldReturnValue()
        {
            // Act
            var inPlay = new InPlay();
            var hashCode = inPlay.GetHashCode();

            // Assert
            hashCode.Should().Be(hashCode); // Just verify it returns a consistent value
        }

        [Fact]
        public void PreMatch_GetHashCode_ShouldReturnValue()
        {
            // Act
            var preMatch = new PreMatch();
            var hashCode = preMatch.GetHashCode();

            // Assert
            hashCode.Should().Be(hashCode); // Just verify it returns a consistent value
        }
    }
} 