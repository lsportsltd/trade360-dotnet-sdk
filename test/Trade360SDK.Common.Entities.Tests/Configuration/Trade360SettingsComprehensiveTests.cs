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
        public void Trade360Settings_DefaultConstructor_ShouldInitializeWithDefaults()
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
        public void Trade360Settings_SetCustomersApiBaseUrl_ShouldSetCorrectly()
        {
            // Arrange
            var settings = new Trade360Settings();
            var baseUrl = "https://api.trade360.com";

            // Act
            settings.CustomersApiBaseUrl = baseUrl;

            // Assert
            settings.CustomersApiBaseUrl.Should().Be(baseUrl);
        }

        [Fact]
        public void Trade360Settings_SetSnapshotApiBaseUrl_ShouldSetCorrectly()
        {
            // Arrange
            var settings = new Trade360Settings();
            var snapshotUrl = "https://snapshot.trade360.com";

            // Act
            settings.SnapshotApiBaseUrl = snapshotUrl;

            // Assert
            settings.SnapshotApiBaseUrl.Should().Be(snapshotUrl);
        }

        [Fact]
        public void Trade360Settings_SetInplayPackageCredentials_ShouldSetCorrectly()
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
            settings.InplayPackageCredentials.Should().Be(credentials);
            settings.InplayPackageCredentials?.PackageId.Should().Be(123);
            settings.InplayPackageCredentials?.Username.Should().Be("testuser");
            settings.InplayPackageCredentials?.Password.Should().Be("testpass");
        }

        [Fact]
        public void Trade360Settings_SetPrematchPackageCredentials_ShouldSetCorrectly()
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
            settings.PrematchPackageCredentials.Should().Be(credentials);
            settings.PrematchPackageCredentials?.PackageId.Should().Be(456);
            settings.PrematchPackageCredentials?.Username.Should().Be("prematchuser");
            settings.PrematchPackageCredentials?.Password.Should().Be("prematchpass");
        }

        [Fact]
        public void Trade360Settings_SetAllProperties_ShouldSetAllCorrectly()
        {
            // Arrange
            var settings = new Trade360Settings();
            var customersUrl = "https://customers.trade360.com";
            var snapshotUrl = "https://snapshot.trade360.com";
            var inplayCredentials = new PackageCredentials
            {
                PackageId = 123,
                Username = "inplayuser",
                Password = "inplaypass"
            };
            var prematchCredentials = new PackageCredentials
            {
                PackageId = 456,
                Username = "prematchuser",
                Password = "prematchpass"
            };

            // Act
            settings.CustomersApiBaseUrl = customersUrl;
            settings.SnapshotApiBaseUrl = snapshotUrl;
            settings.InplayPackageCredentials = inplayCredentials;
            settings.PrematchPackageCredentials = prematchCredentials;

            // Assert
            settings.CustomersApiBaseUrl.Should().Be(customersUrl);
            settings.SnapshotApiBaseUrl.Should().Be(snapshotUrl);
            settings.InplayPackageCredentials.Should().Be(inplayCredentials);
            settings.PrematchPackageCredentials.Should().Be(prematchCredentials);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void Trade360Settings_SetCustomersApiBaseUrlToNullOrEmpty_ShouldAcceptValue(string baseUrl)
        {
            // Arrange
            var settings = new Trade360Settings();

            // Act
            settings.CustomersApiBaseUrl = baseUrl;

            // Assert
            settings.CustomersApiBaseUrl.Should().Be(baseUrl);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void Trade360Settings_SetSnapshotApiBaseUrlToNullOrEmpty_ShouldAcceptValue(string snapshotUrl)
        {
            // Arrange
            var settings = new Trade360Settings();

            // Act
            settings.SnapshotApiBaseUrl = snapshotUrl;

            // Assert
            settings.SnapshotApiBaseUrl.Should().Be(snapshotUrl);
        }

        [Fact]
        public void Trade360Settings_SetCredentialsToNull_ShouldAcceptNullValues()
        {
            // Arrange
            var settings = new Trade360Settings();

            // Act
            settings.InplayPackageCredentials = null;
            settings.PrematchPackageCredentials = null;

            // Assert
            settings.InplayPackageCredentials.Should().BeNull();
            settings.PrematchPackageCredentials.Should().BeNull();
        }

        [Fact]
        public void Trade360Settings_SetPropertiesMultipleTimes_ShouldRetainLastValue()
        {
            // Arrange
            var settings = new Trade360Settings();

            // Act
            settings.CustomersApiBaseUrl = "https://first.com";
            settings.CustomersApiBaseUrl = "https://second.com";
            settings.CustomersApiBaseUrl = "https://final.com";

            settings.SnapshotApiBaseUrl = "https://snapshot1.com";
            settings.SnapshotApiBaseUrl = "https://snapshot2.com";
            settings.SnapshotApiBaseUrl = "https://snapshot-final.com";

            // Assert
            settings.CustomersApiBaseUrl.Should().Be("https://final.com");
            settings.SnapshotApiBaseUrl.Should().Be("https://snapshot-final.com");
        }

        [Fact]
        public void Trade360Settings_PropertyAssignmentChaining_ShouldWork()
        {
            // Arrange & Act
            var settings = new Trade360Settings
            {
                CustomersApiBaseUrl = "https://customers.trade360.com",
                SnapshotApiBaseUrl = "https://snapshot.trade360.com",
                InplayPackageCredentials = new PackageCredentials
                {
                    PackageId = 123,
                    Username = "inplayuser",
                    Password = "inplaypass"
                },
                PrematchPackageCredentials = new PackageCredentials
                {
                    PackageId = 456,
                    Username = "prematchuser",
                    Password = "prematchpass"
                }
            };

            // Assert
            settings.CustomersApiBaseUrl.Should().Be("https://customers.trade360.com");
            settings.SnapshotApiBaseUrl.Should().Be("https://snapshot.trade360.com");
            settings.InplayPackageCredentials?.PackageId.Should().Be(123);
            settings.PrematchPackageCredentials?.PackageId.Should().Be(456);
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
            // Arrange & Act
            var credentials = new PackageCredentials
            {
                PackageId = 12345,
                Username = "testuser",
                Password = "testpass",
                MessageFormat = "xml"
            };

            // Assert
            credentials.PackageId.Should().Be(12345);
            credentials.Username.Should().Be("testuser");
            credentials.Password.Should().Be("testpass");
            credentials.MessageFormat.Should().Be("xml");
        }

        [Theory]
        [InlineData(int.MinValue)]
        [InlineData(-1)]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(int.MaxValue)]
        public void PackageCredentials_SetPackageIdToVariousValues_ShouldAcceptValue(int packageId)
        {
            // Arrange
            var credentials = new PackageCredentials();

            // Act
            credentials.PackageId = packageId;

            // Assert
            credentials.PackageId.Should().Be(packageId);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        [InlineData("validuser")]
        public void PackageCredentials_SetUsernameToVariousValues_ShouldAcceptValue(string username)
        {
            // Arrange
            var credentials = new PackageCredentials();

            // Act
            credentials.Username = username;

            // Assert
            credentials.Username.Should().Be(username);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        [InlineData("validpass")]
        public void PackageCredentials_SetPasswordToVariousValues_ShouldAcceptValue(string password)
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
        [InlineData("csv")]
        [InlineData("")]
        public void PackageCredentials_SetMessageFormatToVariousValues_ShouldAcceptValue(string messageFormat)
        {
            // Arrange
            var credentials = new PackageCredentials();

            // Act
            credentials.MessageFormat = messageFormat;

            // Assert
            credentials.MessageFormat.Should().Be(messageFormat);
        }

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

        #endregion
    }
} 