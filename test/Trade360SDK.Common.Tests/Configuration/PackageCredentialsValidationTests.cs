using FluentAssertions;
using Trade360SDK.Common.Configuration;

namespace Trade360SDK.Common.Tests;

public class PackageCredentialsValidationTests
{
    [Fact]
    public void PackageCredentials_WithValidData_ShouldInitializeCorrectly()
    {
        var credentials = new PackageCredentials
        {
            Username = "testuser",
            Password = "testpass",
            PackageId = 123
        };

        credentials.Username.Should().Be("testuser");
        credentials.Password.Should().Be("testpass");
        credentials.PackageId.Should().Be(123);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void PackageCredentials_WithInvalidUsername_ShouldAllowButBeInvalid(string invalidUsername)
    {
        var credentials = new PackageCredentials
        {
            Username = invalidUsername,
            Password = "testpass",
            PackageId = 123
        };

        credentials.Username.Should().Be(invalidUsername);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void PackageCredentials_WithInvalidPassword_ShouldAllowButBeInvalid(string invalidPassword)
    {
        var credentials = new PackageCredentials
        {
            Username = "testuser",
            Password = invalidPassword,
            PackageId = 123
        };

        credentials.Password.Should().Be(invalidPassword);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-999)]
    public void PackageCredentials_WithInvalidPackageId_ShouldAllowButBeInvalid(int invalidPackageId)
    {
        var credentials = new PackageCredentials
        {
            Username = "testuser",
            Password = "testpass",
            PackageId = invalidPackageId
        };

        credentials.PackageId.Should().Be(invalidPackageId);
    }

    [Fact]
    public void PackageCredentials_WithLargePackageId_ShouldWork()
    {
        var credentials = new PackageCredentials
        {
            Username = "testuser",
            Password = "testpass",
            PackageId = int.MaxValue
        };

        credentials.PackageId.Should().Be(int.MaxValue);
    }

    [Fact]
    public void PackageCredentials_WithLongUsername_ShouldWork()
    {
        var longUsername = new string('a', 1000);
        var credentials = new PackageCredentials
        {
            Username = longUsername,
            Password = "testpass",
            PackageId = 123
        };

        credentials.Username.Should().Be(longUsername);
        credentials.Username.Length.Should().Be(1000);
    }

    [Fact]
    public void PackageCredentials_WithLongPassword_ShouldWork()
    {
        var longPassword = new string('b', 1000);
        var credentials = new PackageCredentials
        {
            Username = "testuser",
            Password = longPassword,
            PackageId = 123
        };

        credentials.Password.Should().Be(longPassword);
        credentials.Password.Length.Should().Be(1000);
    }

    [Fact]
    public void PackageCredentials_WithSpecialCharacters_ShouldWork()
    {
        var credentials = new PackageCredentials
        {
            Username = "test@user.com",
            Password = "p@ssw0rd!#$%",
            PackageId = 123
        };

        credentials.Username.Should().Be("test@user.com");
        credentials.Password.Should().Be("p@ssw0rd!#$%");
    }

    [Fact]
    public void PackageCredentials_WithUnicodeCharacters_ShouldWork()
    {
        var credentials = new PackageCredentials
        {
            Username = "тестовый_пользователь",
            Password = "密码测试",
            PackageId = 123
        };

        credentials.Username.Should().Be("тестовый_пользователь");
        credentials.Password.Should().Be("密码测试");
    }

    [Fact]
    public void PackageCredentials_DefaultValues_ShouldBeCorrect()
    {
        var credentials = new PackageCredentials();

        credentials.Username.Should().BeNull();
        credentials.Password.Should().BeNull();
        credentials.PackageId.Should().Be(0);
    }
}
