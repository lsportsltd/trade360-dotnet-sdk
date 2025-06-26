using System;
using FluentAssertions;
using Trade360SDK.Common.Configuration;
using Xunit;

namespace Trade360SDK.Common.Entities.Tests.Configuration
{
    public class Trade360SettingsFixed
    {
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
    }
} 