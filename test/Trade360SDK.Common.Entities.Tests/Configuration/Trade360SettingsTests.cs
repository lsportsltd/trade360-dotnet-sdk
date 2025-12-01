using System;
using FluentAssertions;
using Xunit;
using Trade360SDK.Common.Configuration;

namespace Trade360SDK.Common.Entities.Tests.Configuration
{
    public class Trade360SettingsTests
    {
        [Fact]
        public void Trade360Settings_DefaultConstructor_ShouldCreateInstanceWithNullProperties()
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

        [Fact]
        public void Trade360Settings_SetCustomersApiBaseUrl_ShouldSetValue()
        {
            // Arrange
            var settings = new Trade360Settings();
            var baseUrl = "https://api.customers.example.com";

            // Act
            settings.CustomersApiBaseUrl = baseUrl;

            // Assert
            settings.CustomersApiBaseUrl.Should().Be(baseUrl);
        }

        [Fact]
        public void Trade360Settings_SetSnapshotApiBaseUrl_ShouldSetValue()
        {
            // Arrange
            var settings = new Trade360Settings();
            var baseUrl = "https://api.snapshot.example.com";

            // Act
            settings.SnapshotApiBaseUrl = baseUrl;

            // Assert
            settings.SnapshotApiBaseUrl.Should().Be(baseUrl);
        }

        [Fact]
        public void Trade360Settings_SetInplayPackageCredentials_ShouldSetValue()
        {
            // Arrange
            var settings = new Trade360Settings();
            var credentials = new PackageCredentials
            {
                PackageId = 123,
                Username = "inplay-user",
                Password = "inplay-pass"
            };

            // Act
            settings.InplayPackageCredentials = credentials;

            // Assert
            settings.InplayPackageCredentials.Should().Be(credentials);
            settings.InplayPackageCredentials.PackageId.Should().Be(123);
            settings.InplayPackageCredentials.Username.Should().Be("inplay-user");
            settings.InplayPackageCredentials.Password.Should().Be("inplay-pass");
        }

        [Fact]
        public void Trade360Settings_SetPrematchPackageCredentials_ShouldSetValue()
        {
            // Arrange
            var settings = new Trade360Settings();
            var credentials = new PackageCredentials
            {
                PackageId = 456,
                Username = "prematch-user",
                Password = "prematch-pass"
            };

            // Act
            settings.PrematchPackageCredentials = credentials;

            // Assert
            settings.PrematchPackageCredentials.Should().Be(credentials);
            settings.PrematchPackageCredentials.PackageId.Should().Be(456);
            settings.PrematchPackageCredentials.Username.Should().Be("prematch-user");
            settings.PrematchPackageCredentials.Password.Should().Be("prematch-pass");
        }

        [Fact]
        public void Trade360Settings_SetAllProperties_ShouldSetAllValues()
        {
            // Arrange
            var settings = new Trade360Settings();
            var customersApiUrl = "https://api.customers.example.com";
            var snapshotApiUrl = "https://api.snapshot.example.com";
            var inplayCredentials = new PackageCredentials { PackageId = 123, Username = "inplay", Password = "pass1" };
            var prematchCredentials = new PackageCredentials { PackageId = 456, Username = "prematch", Password = "pass2" };

            // Act
            settings.CustomersApiBaseUrl = customersApiUrl;
            settings.SnapshotApiBaseUrl = snapshotApiUrl;
            settings.InplayPackageCredentials = inplayCredentials;
            settings.PrematchPackageCredentials = prematchCredentials;

            // Assert
            settings.CustomersApiBaseUrl.Should().Be(customersApiUrl);
            settings.SnapshotApiBaseUrl.Should().Be(snapshotApiUrl);
            settings.InplayPackageCredentials.Should().Be(inplayCredentials);
            settings.PrematchPackageCredentials.Should().Be(prematchCredentials);
        }

        [Theory]
        [InlineData("")]
        [InlineData("   ")]
        [InlineData("https://api.test.com")]
        [InlineData("http://localhost:8080")]
        [InlineData("https://api.example.com/v1/")]
        public void Trade360Settings_SetVariousUrls_ShouldSetValues(string url)
        {
            // Arrange
            var settings = new Trade360Settings();

            // Act
            settings.CustomersApiBaseUrl = url;
            settings.SnapshotApiBaseUrl = url;

            // Assert
            settings.CustomersApiBaseUrl.Should().Be(url);
            settings.SnapshotApiBaseUrl.Should().Be(url);
        }

        [Fact]
        public void Trade360Settings_PropertySettersGetters_ShouldWorkCorrectly()
        {
            // Arrange
            var settings = new Trade360Settings();

            // Act & Assert - Test that we can set and get each property multiple times
            settings.CustomersApiBaseUrl = "url1";
            settings.CustomersApiBaseUrl.Should().Be("url1");
            settings.CustomersApiBaseUrl = "url2";
            settings.CustomersApiBaseUrl.Should().Be("url2");

            settings.SnapshotApiBaseUrl = "snapshot1";
            settings.SnapshotApiBaseUrl.Should().Be("snapshot1");
            settings.SnapshotApiBaseUrl = null;
            settings.SnapshotApiBaseUrl.Should().BeNull();
        }
    }

    public class PackageCredentialsTests
    {
        [Fact]
        public void PackageCredentials_DefaultConstructor_ShouldCreateInstanceWithDefaultValues()
        {
            // Act
            var credentials = new PackageCredentials();

            // Assert
            credentials.Should().NotBeNull();
            credentials.PackageId.Should().Be(0);
            credentials.Username.Should().BeNull();
            credentials.Password.Should().BeNull();
            credentials.MessageFormat.Should().Be("json");
        }

        [Fact]
        public void PackageCredentials_SetPackageId_ShouldSetValue()
        {
            // Arrange
            var credentials = new PackageCredentials();
            var packageId = 12345;

            // Act
            credentials.PackageId = packageId;

            // Assert
            credentials.PackageId.Should().Be(packageId);
        }

        [Fact]
        public void PackageCredentials_SetUsername_ShouldSetValue()
        {
            // Arrange
            var credentials = new PackageCredentials();
            var username = "test-user";

            // Act
            credentials.Username = username;

            // Assert
            credentials.Username.Should().Be(username);
        }

        [Fact]
        public void PackageCredentials_SetPassword_ShouldSetValue()
        {
            // Arrange
            var credentials = new PackageCredentials();
            var password = "test-password";

            // Act
            credentials.Password = password;

            // Assert
            credentials.Password.Should().Be(password);
        }

        [Fact]
        public void PackageCredentials_SetMessageFormat_ShouldSetValue()
        {
            // Arrange
            var credentials = new PackageCredentials();
            var messageFormat = "xml";

            // Act
            credentials.MessageFormat = messageFormat;

            // Assert
            credentials.MessageFormat.Should().Be(messageFormat);
        }

        [Fact]
        public void PackageCredentials_SetAllProperties_ShouldSetAllValues()
        {
            // Arrange
            var credentials = new PackageCredentials();
            var packageId = 999;
            var username = "admin";
            var password = "secret123";
            var messageFormat = "protobuf";

            // Act
            credentials.PackageId = packageId;
            credentials.Username = username;
            credentials.Password = password;
            credentials.MessageFormat = messageFormat;

            // Assert
            credentials.PackageId.Should().Be(packageId);
            credentials.Username.Should().Be(username);
            credentials.Password.Should().Be(password);
            credentials.MessageFormat.Should().Be(messageFormat);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(1)]
        [InlineData(int.MaxValue)]
        [InlineData(int.MinValue)]
        public void PackageCredentials_SetVariousPackageIds_ShouldSetValue(int packageId)
        {
            // Arrange
            var credentials = new PackageCredentials();

            // Act
            credentials.PackageId = packageId;

            // Assert
            credentials.PackageId.Should().Be(packageId);
        }

        [Theory]
        [InlineData("")]
        [InlineData("   ")]
        [InlineData("user")]
        [InlineData("very-long-username-with-special-characters@domain.com")]
        public void PackageCredentials_SetVariousUsernames_ShouldSetValue(string username)
        {
            // Arrange
            var credentials = new PackageCredentials();

            // Act
            credentials.Username = username;

            // Assert
            credentials.Username.Should().Be(username);
        }

        [Theory]
        [InlineData("")]
        [InlineData("   ")]
        [InlineData("pass")]
        [InlineData("very-long-password-with-special-characters!@#$%^&*()")]
        public void PackageCredentials_SetVariousPasswords_ShouldSetValue(string password)
        {
            // Arrange
            var credentials = new PackageCredentials();

            // Act
            credentials.Password = password;

            // Assert
            credentials.Password.Should().Be(password);
        }

        [Theory]
        [InlineData("json")]
        [InlineData("xml")]
        [InlineData("protobuf")]
        [InlineData("binary")]
        [InlineData("")]
        public void PackageCredentials_SetVariousMessageFormats_ShouldSetValue(string messageFormat)
        {
            // Arrange
            var credentials = new PackageCredentials();

            // Act
            credentials.MessageFormat = messageFormat;

            // Assert
            credentials.MessageFormat.Should().Be(messageFormat);
        }

        [Fact]
        public void PackageCredentials_PropertySettersGetters_ShouldWorkCorrectly()
        {
            // Arrange
            var credentials = new PackageCredentials();

            // Act & Assert - Test that we can set and get each property multiple times
            credentials.Username = "user1";
            credentials.Username.Should().Be("user1");
            credentials.Username = "user2";
            credentials.Username.Should().Be("user2");
            credentials.Username = null;
            credentials.Username.Should().BeNull();

            credentials.PackageId = 100;
            credentials.PackageId.Should().Be(100);
            credentials.PackageId = 200;
            credentials.PackageId.Should().Be(200);

            credentials.MessageFormat = "xml";
            credentials.MessageFormat.Should().Be("xml");
            credentials.MessageFormat = "json";
            credentials.MessageFormat.Should().Be("json");
        }

        [Fact]
        public void PackageCredentials_DefaultMessageFormat_ShouldBeJson()
        {
            // Act
            var credentials = new PackageCredentials();

            // Assert
            credentials.MessageFormat.Should().Be("json");
        }

        [Fact]
        public void PackageCredentials_NullStringValues_ShouldSetNulls()
        {
            // Arrange
            var credentials = new PackageCredentials();

            // Act
            credentials.Username = null;
            credentials.Password = null;

            // Assert
            credentials.Username.Should().BeNull();
            credentials.Password.Should().BeNull();
        }
    }
} 