using Trade360SDK.Common.Entities.Shared;

namespace Trade360SDK.Common.Entities.Tests.Entities.Shared;

public class IdNamePairTests
{
    [Fact]
    public void DefaultConstructor_ShouldInitializeWithNullValues()
    {
        // Act
        var idNamePair = new IdNamePair();

        // Assert
        Assert.Null(idNamePair.Id);
        Assert.Null(idNamePair.Name);
    }

    [Fact]
    public void Properties_ShouldBeSettable()
    {
        // Arrange
        var expectedId = 123;
        var expectedName = "Test Name";

        // Act
        var idNamePair = new IdNamePair
        {
            Id = expectedId,
            Name = expectedName
        };

        // Assert
        Assert.Equal(expectedId, idNamePair.Id);
        Assert.Equal(expectedName, idNamePair.Name);
    }

    [Fact]
    public void Id_ShouldAcceptNullValue()
    {
        // Arrange & Act
        var idNamePair = new IdNamePair { Id = null };

        // Assert
        Assert.Null(idNamePair.Id);
    }

    [Fact]
    public void Name_ShouldAcceptNullValue()
    {
        // Arrange & Act
        var idNamePair = new IdNamePair { Name = null };

        // Assert
        Assert.Null(idNamePair.Name);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(-1)]
    [InlineData(int.MaxValue)]
    [InlineData(int.MinValue)]
    public void Id_ShouldAcceptValidIntegerValues(int id)
    {
        // Act
        var idNamePair = new IdNamePair { Id = id };

        // Assert
        Assert.Equal(id, idNamePair.Id);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("Test")]
    [InlineData("Very Long Name With Special Characters !@#$%")]
    public void Name_ShouldAcceptValidStringValues(string name)
    {
        // Act
        var idNamePair = new IdNamePair { Name = name };

        // Assert
        Assert.Equal(name, idNamePair.Name);
    }

    [Fact]
    public void SetAllProperties_ShouldWorkCorrectly()
    {
        // Arrange
        var expectedId = 456;
        var expectedName = "Complete Test Name";

        // Act
        var idNamePair = new IdNamePair
        {
            Id = expectedId,
            Name = expectedName
        };

        // Assert
        Assert.Equal(expectedId, idNamePair.Id);
        Assert.Equal(expectedName, idNamePair.Name);
    }
}
