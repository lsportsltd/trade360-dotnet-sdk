using FluentAssertions;
using Trade360SDK.Feed.FeedType;

namespace Trade360SDK.Feed.Tests;

public class FlowTypesComprehensiveTests
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
    public void PreMatch_ShouldImplementIFlow()
    {
        // Arrange & Act
        var preMatch = new PreMatch();

        // Assert
        preMatch.Should().NotBeNull();
        preMatch.Should().BeAssignableTo<IFlow>();
    }

    [Fact]
    public void InPlay_ShouldBeInstantiable()
    {
        // Arrange & Act
        var act = () => new InPlay();

        // Assert
        act.Should().NotThrow();
        var instance = act();
        instance.Should().NotBeNull();
        instance.Should().BeOfType<InPlay>();
    }

    [Fact]
    public void PreMatch_ShouldBeInstantiable()
    {
        // Arrange & Act
        var act = () => new PreMatch();

        // Assert
        act.Should().NotThrow();
        var instance = act();
        instance.Should().NotBeNull();
        instance.Should().BeOfType<PreMatch>();
    }

    [Fact]
    public void InPlay_MultipleInstances_ShouldBeIndependent()
    {
        // Arrange & Act
        var inPlay1 = new InPlay();
        var inPlay2 = new InPlay();

        // Assert
        inPlay1.Should().NotBeNull();
        inPlay2.Should().NotBeNull();
        inPlay1.Should().NotBeSameAs(inPlay2);
        inPlay1.Should().BeOfType<InPlay>();
        inPlay2.Should().BeOfType<InPlay>();
    }

    [Fact]
    public void PreMatch_MultipleInstances_ShouldBeIndependent()
    {
        // Arrange & Act
        var preMatch1 = new PreMatch();
        var preMatch2 = new PreMatch();

        // Assert
        preMatch1.Should().NotBeNull();
        preMatch2.Should().NotBeNull();
        preMatch1.Should().NotBeSameAs(preMatch2);
        preMatch1.Should().BeOfType<PreMatch>();
        preMatch2.Should().BeOfType<PreMatch>();
    }

    [Fact]
    public void InPlay_ShouldHaveCorrectTypeInformation()
    {
        // Arrange & Act
        var inPlay = new InPlay();
        var type = inPlay.GetType();

        // Assert
        type.Name.Should().Be("InPlay");
        type.Namespace.Should().Be("Trade360SDK.Feed.FeedType");
        type.IsClass.Should().BeTrue();
        type.IsAbstract.Should().BeFalse();
        type.IsSealed.Should().BeFalse();
    }

    [Fact]
    public void PreMatch_ShouldHaveCorrectTypeInformation()
    {
        // Arrange & Act
        var preMatch = new PreMatch();
        var type = preMatch.GetType();

        // Assert
        type.Name.Should().Be("PreMatch");
        type.Namespace.Should().Be("Trade360SDK.Feed.FeedType");
        type.IsClass.Should().BeTrue();
        type.IsAbstract.Should().BeFalse();
        type.IsSealed.Should().BeFalse();
    }

    [Fact]
    public void IFlow_Interface_ShouldHaveCorrectProperties()
    {
        // Arrange & Act
        var flowType = typeof(IFlow);

        // Assert
        flowType.IsInterface.Should().BeTrue();
        flowType.Name.Should().Be("IFlow");
        flowType.Namespace.Should().Be("Trade360SDK.Feed.FeedType");
    }

    [Fact]
    public void InPlay_ShouldImplementsIFlowInterface()
    {
        // Arrange
        var inPlay = new InPlay();

        // Act
        var interfaces = inPlay.GetType().GetInterfaces();

        // Assert
        interfaces.Should().Contain(typeof(IFlow));
    }

    [Fact]
    public void PreMatch_ShouldImplementsIFlowInterface()
    {
        // Arrange
        var preMatch = new PreMatch();

        // Act
        var interfaces = preMatch.GetType().GetInterfaces();

        // Assert
        interfaces.Should().Contain(typeof(IFlow));
    }

    [Fact]
    public void InPlay_CanBeUsedAsIFlow()
    {
        // Arrange
        var inPlay = new InPlay();

        // Act
        IFlow flow = inPlay;

        // Assert
        flow.Should().NotBeNull();
        flow.Should().BeSameAs(inPlay);
        flow.Should().BeAssignableTo<InPlay>();
    }

    [Fact]
    public void PreMatch_CanBeUsedAsIFlow()
    {
        // Arrange
        var preMatch = new PreMatch();

        // Act
        IFlow flow = preMatch;

        // Assert
        flow.Should().NotBeNull();
        flow.Should().BeSameAs(preMatch);
        flow.Should().BeAssignableTo<PreMatch>();
    }

    [Fact]
    public void FlowTypes_ShouldHaveDifferentTypeIdentities()
    {
        // Arrange
        var inPlay = new InPlay();
        var preMatch = new PreMatch();

        // Act & Assert
        inPlay.GetType().Should().NotBe(preMatch.GetType());
        inPlay.Should().NotBeOfType<PreMatch>();
        preMatch.Should().NotBeOfType<InPlay>();
    }

    [Fact]
    public void FlowTypes_InCollection_ShouldMaintainTypeIdentity()
    {
        // Arrange
        var inPlay = new InPlay();
        var preMatch = new PreMatch();
        var flows = new List<IFlow> { inPlay, preMatch };

        // Act & Assert
        flows.Should().HaveCount(2);
        flows[0].Should().BeOfType<InPlay>();
        flows[1].Should().BeOfType<PreMatch>();
        flows[0].Should().BeSameAs(inPlay);
        flows[1].Should().BeSameAs(preMatch);
    }

    [Theory]
    [InlineData(typeof(InPlay))]
    [InlineData(typeof(PreMatch))]
    public void FlowTypes_ShouldBeCreatableViaReflection(Type flowType)
    {
        // Arrange & Act
        var instance = Activator.CreateInstance(flowType);

        // Assert
        instance.Should().NotBeNull();
        instance.Should().BeAssignableTo<IFlow>();
        instance!.GetType().Should().Be(flowType);
    }

    [Fact]
    public void InPlay_ToString_ShouldReturnTypeName()
    {
        // Arrange
        var inPlay = new InPlay();

        // Act
        var result = inPlay.ToString();

        // Assert
        result.Should().NotBeNullOrEmpty();
        result!.Should().Contain("InPlay");
    }

    [Fact]
    public void PreMatch_ToString_ShouldReturnTypeName()
    {
        // Arrange
        var preMatch = new PreMatch();

        // Act
        var result = preMatch.ToString();

        // Assert
        result.Should().NotBeNullOrEmpty();
        result!.Should().Contain("PreMatch");
    }

    [Fact]
    public void InPlay_GetHashCode_ShouldBeConsistent()
    {
        // Arrange
        var inPlay = new InPlay();

        // Act
        var hashCode1 = inPlay.GetHashCode();
        var hashCode2 = inPlay.GetHashCode();

        // Assert
        hashCode1.Should().Be(hashCode2);
    }

    [Fact]
    public void PreMatch_GetHashCode_ShouldBeConsistent()
    {
        // Arrange
        var preMatch = new PreMatch();

        // Act
        var hashCode1 = preMatch.GetHashCode();
        var hashCode2 = preMatch.GetHashCode();

        // Assert
        hashCode1.Should().Be(hashCode2);
    }

    [Fact]
    public void InPlay_Equals_ShouldWorkCorrectly()
    {
        // Arrange
        var inPlay1 = new InPlay();
        var inPlay2 = new InPlay();

        // Act & Assert
        inPlay1.Equals(inPlay1).Should().BeTrue(); // Same reference
        inPlay1.Equals(inPlay2).Should().BeFalse(); // Different instances
        inPlay1.Equals(null).Should().BeFalse(); // Null comparison
        inPlay1.Equals("string").Should().BeFalse(); // Different type
    }

    [Fact]
    public void PreMatch_Equals_ShouldWorkCorrectly()
    {
        // Arrange
        var preMatch1 = new PreMatch();
        var preMatch2 = new PreMatch();

        // Act & Assert
        preMatch1.Equals(preMatch1).Should().BeTrue(); // Same reference
        preMatch1.Equals(preMatch2).Should().BeFalse(); // Different instances
        preMatch1.Equals(null).Should().BeFalse(); // Null comparison
        preMatch1.Equals("string").Should().BeFalse(); // Different type
    }

    [Fact]
    public void FlowTypes_TypeComparison_ShouldWorkCorrectly()
    {
        // Arrange
        var inPlay = new InPlay();
        var preMatch = new PreMatch();

        // Act & Assert
        (inPlay is IFlow).Should().BeTrue();
        (preMatch is IFlow).Should().BeTrue();
        (inPlay is InPlay).Should().BeTrue();
        (preMatch is PreMatch).Should().BeTrue();
        // Removed problematic comparisons that caused CS0184 warnings
        inPlay.Should().NotBeOfType<PreMatch>();
        preMatch.Should().NotBeOfType<InPlay>();
    }
} 