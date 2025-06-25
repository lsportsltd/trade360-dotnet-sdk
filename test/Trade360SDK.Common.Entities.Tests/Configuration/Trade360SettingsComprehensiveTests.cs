using System;
using FluentAssertions;
using Trade360SDK.Common.Configuration;
using Xunit;

namespace Trade360SDK.Common.Entities.Tests.Configuration
{
    public class Trade360SettingsComprehensiveTests
    {
        #region Trade360Settings Tests

        [Fact]
        public void Trade360Settings_DefaultConstructor_ShouldInitializeWithNullValues()
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
        public void Trade360Settings_CustomersApiBaseUrlProperty_ShouldBeSettableAndGettable()
        {
            // Arrange
            var settings = new Trade360Settings();
            var url = "https://api.example.com/";

            // Act
            settings.CustomersApiBaseUrl = url;

            // Assert
            settings.CustomersApiBaseUrl.Should().Be(url);
        }

        [Fact]
        public void Trade360Settings_SnapshotApiBaseUrlProperty_ShouldBeSettableAndGettable()
        {
            // Arrange
            var settings = new Trade360Settings();
            var url = "https://snapshot.example.com/";

            // Act
            settings.SnapshotApiBaseUrl = url;

            // Assert
            settings.SnapshotApiBaseUrl.Should().Be(url);
        }

        [Fact]
        public void Trade360Settings_InplayPackageCredentialsProperty_ShouldBeSettableAndGettable()
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
            settings.InplayPackageCredentials.Should().BeSameAs(credentials);
            settings.InplayPackageCredentials.PackageId.Should().Be(123);
            settings.InplayPackageCredentials.Username.Should().Be("testuser");
            settings.InplayPackageCredentials.Password.Should().Be("testpass");
        }

        [Fact]
        public void Trade360Settings_PrematchPackageCredentialsProperty_ShouldBeSettableAndGettable()
        {
            // Arrange
            var settings = new Trade360Settings();
            var credentials = new PackageCredentials
            {
                PackageId = 456,
                Username = "prematchuser",
                Password = "prematchpass"
            };

            // Act
            settings.PrematchPackageCredentials = credentials;

            // Assert
            settings.PrematchPackageCredentials.Should().NotBeNull();
            settings.PrematchPackageCredentials.Should().BeSameAs(credentials);
            settings.PrematchPackageCredentials.PackageId.Should().Be(456);
            settings.PrematchPackageCredentials.Username.Should().Be("prematchuser");
            settings.PrematchPackageCredentials.Password.Should().Be("prematchpass");
        }

        [Fact]
        public void Trade360Settings_WithCompleteConfiguration_ShouldMaintainAllProperties()
        {
            // Arrange
            var customersUrl = "https://api.example.com/";
            var snapshotUrl = "https://snapshot.example.com/";
            var inplayCredentials = new PackageCredentials
            {
                PackageId = 123,
                Username = "inplayuser",
                Password = "inplaypass",
                MessageFormat = "json"
            };
            var prematchCredentials = new PackageCredentials
            {
                PackageId = 456,
                Username = "prematchuser",
                Password = "prematchpass",
                MessageFormat = "xml"
            };

            // Act
            var settings = new Trade360Settings
            {
                CustomersApiBaseUrl = customersUrl,
                SnapshotApiBaseUrl = snapshotUrl,
                InplayPackageCredentials = inplayCredentials,
                PrematchPackageCredentials = prematchCredentials
            };

            // Assert
            settings.Should().NotBeNull();
            settings.CustomersApiBaseUrl.Should().Be(customersUrl);
            settings.SnapshotApiBaseUrl.Should().Be(snapshotUrl);
            settings.InplayPackageCredentials.Should().BeSameAs(inplayCredentials);
            settings.PrematchPackageCredentials.Should().BeSameAs(prematchCredentials);
        }

        [Fact]
        public void Trade360Settings_PropertiesCanBeSetToNull_ShouldAllowNullValues()
        {
            // Arrange
            var settings = new Trade360Settings
            {
                CustomersApiBaseUrl = "https://api.example.com/",
                SnapshotApiBaseUrl = "https://snapshot.example.com/",
                InplayPackageCredentials = new PackageCredentials(),
                PrematchPackageCredentials = new PackageCredentials()
            };

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
        public void Trade360Settings_WithEmptyStringUrls_ShouldPreserveEmptyStrings()
        {
            // Arrange
            var settings = new Trade360Settings();

            // Act
            settings.CustomersApiBaseUrl = "";
            settings.SnapshotApiBaseUrl = "";

            // Assert
            settings.CustomersApiBaseUrl.Should().BeEmpty();
            settings.SnapshotApiBaseUrl.Should().BeEmpty();
        }

        [Fact]
        public void Trade360Settings_WithLargeUrls_ShouldHandleLargeStrings()
        {
            // Arrange
            var settings = new Trade360Settings();
            var largeUrl = "https://very-long-domain-name-that-exceeds-normal-length.example.com/" + new string('a', 1000);

            // Act
            settings.CustomersApiBaseUrl = largeUrl;
            settings.SnapshotApiBaseUrl = largeUrl;

            // Assert
            settings.CustomersApiBaseUrl.Should().Be(largeUrl);
            settings.SnapshotApiBaseUrl.Should().Be(largeUrl);
        }

        [Fact]
        public void Trade360Settings_PropertyChanges_ShouldBeIndependent()
        {
            // Arrange
            var settings = new Trade360Settings();
            var originalCustomersUrl = "https://api.example.com/";
            var originalSnapshotUrl = "https://snapshot.example.com/";

            // Act
            settings.CustomersApiBaseUrl = originalCustomersUrl;
            settings.SnapshotApiBaseUrl = originalSnapshotUrl;
            
            settings.CustomersApiBaseUrl = "https://new-api.example.com/";
            settings.SnapshotApiBaseUrl = "https://new-snapshot.example.com/";

            // Assert
            settings.CustomersApiBaseUrl.Should().Be("https://new-api.example.com/");
            settings.SnapshotApiBaseUrl.Should().Be("https://new-snapshot.example.com/");
        }

        #endregion

        #region PackageCredentials Tests

        [Fact]
        public void PackageCredentials_DefaultConstructor_ShouldInitializeWithDefaultValues()
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
        public void PackageCredentials_PackageIdProperty_ShouldBeSettableAndGettable()
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
        public void PackageCredentials_UsernameProperty_ShouldBeSettableAndGettable()
        {
            // Arrange
            var credentials = new PackageCredentials();
            var username = "testuser";

            // Act
            credentials.Username = username;

            // Assert
            credentials.Username.Should().Be(username);
        }

        [Fact]
        public void PackageCredentials_PasswordProperty_ShouldBeSettableAndGettable()
        {
            // Arrange
            var credentials = new PackageCredentials();
            var password = "testpassword";

            // Act
            credentials.Password = password;

            // Assert
            credentials.Password.Should().Be(password);
        }

        [Fact]
        public void PackageCredentials_MessageFormatProperty_ShouldBeSettableAndGettable()
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
        public void PackageCredentials_WithCompleteData_ShouldMaintainAllProperties()
        {
            // Arrange
            var packageId = 67890;
            var username = "completeuser";
            var password = "completepassword";
            var messageFormat = "protobuf";

            // Act
            var credentials = new PackageCredentials
            {
                PackageId = packageId,
                Username = username,
                Password = password,
                MessageFormat = messageFormat
            };

            // Assert
            credentials.Should().NotBeNull();
            credentials.PackageId.Should().Be(packageId);
            credentials.Username.Should().Be(username);
            credentials.Password.Should().Be(password);
            credentials.MessageFormat.Should().Be(messageFormat);
        }

        [Fact]
        public void PackageCredentials_StringPropertiesCanBeSetToNull_ShouldAllowNullValues()
        {
            // Arrange
            var credentials = new PackageCredentials
            {
                Username = "testuser",
                Password = "testpass",
                MessageFormat = "json"
            };

            // Act
            credentials.Username = null;
            credentials.Password = null;
            credentials.MessageFormat = null!; // MessageFormat has a default, but can be set to null

            // Assert
            credentials.Username.Should().BeNull();
            credentials.Password.Should().BeNull();
            credentials.MessageFormat.Should().BeNull();
        }

        [Fact]
        public void PackageCredentials_PackageIdProperty_ShouldHandleBoundaryValues()
        {
            // Arrange
            var credentials = new PackageCredentials();

            // Act & Assert
            credentials.PackageId = int.MinValue;
            credentials.PackageId.Should().Be(int.MinValue);

            credentials.PackageId = int.MaxValue;
            credentials.PackageId.Should().Be(int.MaxValue);

            credentials.PackageId = 0;
            credentials.PackageId.Should().Be(0);
        }

        [Fact]
        public void PackageCredentials_StringProperties_ShouldHandleEmptyStrings()
        {
            // Arrange
            var credentials = new PackageCredentials();

            // Act
            credentials.Username = "";
            credentials.Password = "";
            credentials.MessageFormat = "";

            // Assert
            credentials.Username.Should().BeEmpty();
            credentials.Password.Should().BeEmpty();
            credentials.MessageFormat.Should().BeEmpty();
        }

        [Fact]
        public void PackageCredentials_StringProperties_ShouldHandleLargeStrings()
        {
            // Arrange
            var credentials = new PackageCredentials();
            var largeString = new string('A', 10000);

            // Act
            credentials.Username = largeString;
            credentials.Password = largeString;
            credentials.MessageFormat = largeString;

            // Assert
            credentials.Username.Should().Be(largeString);
            credentials.Password.Should().Be(largeString);
            credentials.MessageFormat.Should().Be(largeString);
        }

        [Fact]
        public void PackageCredentials_WithSpecialCharacters_ShouldPreserveCharacters()
        {
            // Arrange
            var credentials = new PackageCredentials();
            var specialUsername = "user@domain.com";
            var specialPassword = "P@ssw0rd!#$%";
            var specialFormat = "application/json; charset=utf-8";

            // Act
            credentials.Username = specialUsername;
            credentials.Password = specialPassword;
            credentials.MessageFormat = specialFormat;

            // Assert
            credentials.Username.Should().Be(specialUsername);
            credentials.Password.Should().Be(specialPassword);
            credentials.MessageFormat.Should().Be(specialFormat);
        }

        [Theory]
        [InlineData("json")]
        [InlineData("xml")]
        [InlineData("protobuf")]
        [InlineData("avro")]
        [InlineData("msgpack")]
        public void PackageCredentials_MessageFormat_ShouldSupportVariousFormats(string format)
        {
            // Arrange
            var credentials = new PackageCredentials();

            // Act
            credentials.MessageFormat = format;

            // Assert
            credentials.MessageFormat.Should().Be(format);
        }

        [Fact]
        public void PackageCredentials_PropertyChanges_ShouldBeIndependent()
        {
            // Arrange
            var credentials = new PackageCredentials();

            // Act
            credentials.PackageId = 100;
            credentials.Username = "user1";
            credentials.Password = "pass1";
            credentials.MessageFormat = "json";
            
            credentials.PackageId = 200;
            credentials.Username = "user2";
            credentials.Password = "pass2";
            credentials.MessageFormat = "xml";

            // Assert
            credentials.PackageId.Should().Be(200);
            credentials.Username.Should().Be("user2");
            credentials.Password.Should().Be("pass2");
            credentials.MessageFormat.Should().Be("xml");
        }

        #endregion

        #region Integration Tests

        [Fact]
        public void Trade360Settings_WithBothCredentialTypes_ShouldMaintainSeparateInstances()
        {
            // Arrange
            var inplayCredentials = new PackageCredentials
            {
                PackageId = 123,
                Username = "inplayuser",
                Password = "inplaypass",
                MessageFormat = "json"
            };
            var prematchCredentials = new PackageCredentials
            {
                PackageId = 456,
                Username = "prematchuser",
                Password = "prematchpass",
                MessageFormat = "xml"
            };

            // Act
            var settings = new Trade360Settings
            {
                InplayPackageCredentials = inplayCredentials,
                PrematchPackageCredentials = prematchCredentials
            };

            // Assert
            settings.InplayPackageCredentials.Should().NotBeSameAs(settings.PrematchPackageCredentials);
            settings.InplayPackageCredentials.PackageId.Should().Be(123);
            settings.PrematchPackageCredentials.PackageId.Should().Be(456);
            settings.InplayPackageCredentials.Username.Should().Be("inplayuser");
            settings.PrematchPackageCredentials.Username.Should().Be("prematchuser");
        }

        [Fact]
        public void Trade360Settings_CredentialChanges_ShouldNotAffectEachOther()
        {
            // Arrange
            var settings = new Trade360Settings
            {
                InplayPackageCredentials = new PackageCredentials { PackageId = 123, Username = "inplay" },
                PrematchPackageCredentials = new PackageCredentials { PackageId = 456, Username = "prematch" }
            };

            // Act
            settings.InplayPackageCredentials.PackageId = 999;
            settings.InplayPackageCredentials.Username = "modified";

            // Assert
            settings.InplayPackageCredentials.PackageId.Should().Be(999);
            settings.InplayPackageCredentials.Username.Should().Be("modified");
            settings.PrematchPackageCredentials.PackageId.Should().Be(456);
            settings.PrematchPackageCredentials.Username.Should().Be("prematch");
        }

        [Fact]
        public void Trade360Settings_WithRealWorldConfiguration_ShouldWorkCorrectly()
        {
            // Arrange & Act
            var settings = new Trade360Settings
            {
                CustomersApiBaseUrl = "https://customers-api.trade360.com/v1/",
                SnapshotApiBaseUrl = "https://snapshot-api.trade360.com/v2/",
                InplayPackageCredentials = new PackageCredentials
                {
                    PackageId = 12345,
                    Username = "trade360_inplay_user",
                    Password = "SecurePassword123!",
                    MessageFormat = "json"
                },
                PrematchPackageCredentials = new PackageCredentials
                {
                    PackageId = 67890,
                    Username = "trade360_prematch_user",
                    Password = "AnotherSecurePassword456!",
                    MessageFormat = "xml"
                }
            };

            // Assert
            settings.Should().NotBeNull();
            settings.CustomersApiBaseUrl.Should().StartWith("https://");
            settings.SnapshotApiBaseUrl.Should().StartWith("https://");
            settings.InplayPackageCredentials.Should().NotBeNull();
            settings.PrematchPackageCredentials.Should().NotBeNull();
            settings.InplayPackageCredentials.PackageId.Should().BeGreaterThan(0);
            settings.PrematchPackageCredentials.PackageId.Should().BeGreaterThan(0);
            settings.InplayPackageCredentials.Username.Should().NotBeNullOrEmpty();
            settings.PrematchPackageCredentials.Username.Should().NotBeNullOrEmpty();
        }

        #endregion

        #region Edge Cases and Validation

        [Fact]
        public void PackageCredentials_WithZeroPackageId_ShouldBeValid()
        {
            // Arrange & Act
            var credentials = new PackageCredentials
            {
                PackageId = 0,
                Username = "testuser",
                Password = "testpass"
            };

            // Assert
            credentials.PackageId.Should().Be(0);
            credentials.Username.Should().Be("testuser");
            credentials.Password.Should().Be("testpass");
        }

        [Fact]
        public void PackageCredentials_WithNegativePackageId_ShouldBeValid()
        {
            // Arrange & Act
            var credentials = new PackageCredentials
            {
                PackageId = -1,
                Username = "testuser",
                Password = "testpass"
            };

            // Assert
            credentials.PackageId.Should().Be(-1);
        }

        [Fact]
        public void Trade360Settings_WithInvalidUrls_ShouldStillAcceptValues()
        {
            // Arrange & Act
            var settings = new Trade360Settings
            {
                CustomersApiBaseUrl = "not-a-valid-url",
                SnapshotApiBaseUrl = "also-not-valid"
            };

            // Assert
            settings.CustomersApiBaseUrl.Should().Be("not-a-valid-url");
            settings.SnapshotApiBaseUrl.Should().Be("also-not-valid");
        }

        #endregion
    }
} 