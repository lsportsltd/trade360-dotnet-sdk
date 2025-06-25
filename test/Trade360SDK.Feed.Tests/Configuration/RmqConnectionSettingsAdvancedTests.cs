using System;
using FluentAssertions;
using Xunit;
using Trade360SDK.Feed.Configuration;

namespace Trade360SDK.Feed.Tests.Configuration
{
    public class RmqConnectionSettingsAdvancedTests
    {
        [Fact]
        public void RmqConnectionSettings_Constructor_ShouldInitializeWithDefaultValues()
        {
            // Act
            var settings = new RmqConnectionSettings();

            // Assert
            settings.Should().NotBeNull();
            settings.Host.Should().BeNull();
            settings.Port.Should().Be(0);
            settings.VirtualHost.Should().BeNull();
            settings.PackageId.Should().Be(0);
            settings.UserName.Should().BeNull();
            settings.Password.Should().BeNull();
            settings.PrefetchCount.Should().Be(100); // Default value
            settings.DispatchConsumersAsync.Should().BeTrue(); // Default value
            settings.AutomaticRecoveryEnabled.Should().BeTrue(); // Default value
            settings.AutoAck.Should().BeTrue(); // Default value
            settings.RequestedHeartbeatSeconds.Should().Be(30); // Default value
            settings.NetworkRecoveryInterval.Should().Be(30); // Default value
        }

        [Fact]
        public void RmqConnectionSettings_SetHost_ShouldAcceptValidString()
        {
            // Arrange
            var settings = new RmqConnectionSettings();
            var testHost = "rabbitmq.example.com";

            // Act
            settings.Host = testHost;

            // Assert
            settings.Host.Should().Be(testHost);
        }

        [Fact]
        public void RmqConnectionSettings_SetPort_ShouldAcceptValidInteger()
        {
            // Arrange
            var settings = new RmqConnectionSettings();
            var testPort = 5672;

            // Act
            settings.Port = testPort;

            // Assert
            settings.Port.Should().Be(testPort);
        }

        [Fact]
        public void RmqConnectionSettings_SetVirtualHost_ShouldAcceptValidString()
        {
            // Arrange
            var settings = new RmqConnectionSettings();
            var testVirtualHost = "/test";

            // Act
            settings.VirtualHost = testVirtualHost;

            // Assert
            settings.VirtualHost.Should().Be(testVirtualHost);
        }

        [Fact]
        public void RmqConnectionSettings_SetPackageId_ShouldAcceptValidInteger()
        {
            // Arrange
            var settings = new RmqConnectionSettings();
            var testPackageId = 12345;

            // Act
            settings.PackageId = testPackageId;

            // Assert
            settings.PackageId.Should().Be(testPackageId);
        }

        [Fact]
        public void RmqConnectionSettings_SetUserName_ShouldAcceptValidString()
        {
            // Arrange
            var settings = new RmqConnectionSettings();
            var testUserName = "testuser";

            // Act
            settings.UserName = testUserName;

            // Assert
            settings.UserName.Should().Be(testUserName);
        }

        [Fact]
        public void RmqConnectionSettings_SetPassword_ShouldAcceptValidString()
        {
            // Arrange
            var settings = new RmqConnectionSettings();
            var testPassword = "testpassword";

            // Act
            settings.Password = testPassword;

            // Assert
            settings.Password.Should().Be(testPassword);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(50)]
        [InlineData(100)]
        [InlineData(500)]
        [InlineData(1000)]
        [InlineData(ushort.MaxValue)]
        public void RmqConnectionSettings_SetPrefetchCount_ShouldAcceptValidValues(ushort prefetchCount)
        {
            // Arrange
            var settings = new RmqConnectionSettings();

            // Act
            settings.PrefetchCount = prefetchCount;

            // Assert
            settings.PrefetchCount.Should().Be(prefetchCount);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void RmqConnectionSettings_SetDispatchConsumersAsync_ShouldAcceptBooleanValues(bool value)
        {
            // Arrange
            var settings = new RmqConnectionSettings();

            // Act
            settings.DispatchConsumersAsync = value;

            // Assert
            settings.DispatchConsumersAsync.Should().Be(value);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void RmqConnectionSettings_SetAutomaticRecoveryEnabled_ShouldAcceptBooleanValues(bool value)
        {
            // Arrange
            var settings = new RmqConnectionSettings();

            // Act
            settings.AutomaticRecoveryEnabled = value;

            // Assert
            settings.AutomaticRecoveryEnabled.Should().Be(value);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void RmqConnectionSettings_SetAutoAck_ShouldAcceptBooleanValues(bool value)
        {
            // Arrange
            var settings = new RmqConnectionSettings();

            // Act
            settings.AutoAck = value;

            // Assert
            settings.AutoAck.Should().Be(value);
        }

        [Theory]
        [InlineData(15)]
        [InlineData(30)]
        [InlineData(60)]
        [InlineData(120)]
        [InlineData(300)]
        [InlineData(int.MaxValue)]
        public void RmqConnectionSettings_SetRequestedHeartbeatSeconds_ShouldAcceptValidValues(int seconds)
        {
            // Arrange
            var settings = new RmqConnectionSettings();

            // Act
            settings.RequestedHeartbeatSeconds = seconds;

            // Assert
            settings.RequestedHeartbeatSeconds.Should().Be(seconds);
        }

        [Theory]
        [InlineData(20)]
        [InlineData(30)]
        [InlineData(60)]
        [InlineData(120)]
        [InlineData(300)]
        [InlineData(int.MaxValue)]
        public void RmqConnectionSettings_SetNetworkRecoveryInterval_ShouldAcceptValidValues(int interval)
        {
            // Arrange
            var settings = new RmqConnectionSettings();

            // Act
            settings.NetworkRecoveryInterval = interval;

            // Assert
            settings.NetworkRecoveryInterval.Should().Be(interval);
        }

        [Fact]
        public void RmqConnectionSettings_SetAllProperties_ShouldStoreAllValues()
        {
            // Arrange
            var settings = new RmqConnectionSettings();

            // Act
            settings.Host = "rabbitmq.test.com";
            settings.Port = 5673;
            settings.VirtualHost = "/testing";
            settings.PackageId = 99999;
            settings.UserName = "admin";
            settings.Password = "secret123";
            settings.PrefetchCount = 250;
            settings.DispatchConsumersAsync = false;
            settings.AutomaticRecoveryEnabled = false;
            settings.AutoAck = false;
            settings.RequestedHeartbeatSeconds = 45;
            settings.NetworkRecoveryInterval = 25;

            // Assert
            settings.Host.Should().Be("rabbitmq.test.com");
            settings.Port.Should().Be(5673);
            settings.VirtualHost.Should().Be("/testing");
            settings.PackageId.Should().Be(99999);
            settings.UserName.Should().Be("admin");
            settings.Password.Should().Be("secret123");
            settings.PrefetchCount.Should().Be(250);
            settings.DispatchConsumersAsync.Should().BeFalse();
            settings.AutomaticRecoveryEnabled.Should().BeFalse();
            settings.AutoAck.Should().BeFalse();
            settings.RequestedHeartbeatSeconds.Should().Be(45);
            settings.NetworkRecoveryInterval.Should().Be(25);
        }

        [Fact]
        public void RmqConnectionSettings_WithNullValues_ShouldAcceptNull()
        {
            // Arrange
            var settings = new RmqConnectionSettings();

            // Act
            settings.Host = null;
            settings.VirtualHost = null;
            settings.UserName = null;
            settings.Password = null;

            // Assert
            settings.Host.Should().BeNull();
            settings.VirtualHost.Should().BeNull();
            settings.UserName.Should().BeNull();
            settings.Password.Should().BeNull();
        }

        [Fact]
        public void RmqConnectionSettings_WithEmptyStrings_ShouldAcceptEmpty()
        {
            // Arrange
            var settings = new RmqConnectionSettings();

            // Act
            settings.Host = string.Empty;
            settings.VirtualHost = string.Empty;
            settings.UserName = string.Empty;
            settings.Password = string.Empty;

            // Assert
            settings.Host.Should().Be(string.Empty);
            settings.VirtualHost.Should().Be(string.Empty);
            settings.UserName.Should().Be(string.Empty);
            settings.Password.Should().Be(string.Empty);
        }

        [Fact]
        public void RmqConnectionSettings_WithWhitespaceStrings_ShouldPreserveWhitespace()
        {
            // Arrange
            var settings = new RmqConnectionSettings();
            var whitespace = "   ";

            // Act
            settings.Host = whitespace;
            settings.VirtualHost = whitespace;
            settings.UserName = whitespace;
            settings.Password = whitespace;

            // Assert
            settings.Host.Should().Be(whitespace);
            settings.VirtualHost.Should().Be(whitespace);
            settings.UserName.Should().Be(whitespace);
            settings.Password.Should().Be(whitespace);
        }

        [Fact]
        public void RmqConnectionSettings_WithSpecialCharacters_ShouldPreserveCharacters()
        {
            // Arrange
            var settings = new RmqConnectionSettings();
            var specialHost = "rabbit-mq.test_env.com";
            var specialVHost = "/test/environment";
            var specialUser = "user@domain.com";
            var specialPass = "P@ssw0rd!#$";

            // Act
            settings.Host = specialHost;
            settings.VirtualHost = specialVHost;
            settings.UserName = specialUser;
            settings.Password = specialPass;

            // Assert
            settings.Host.Should().Be(specialHost);
            settings.VirtualHost.Should().Be(specialVHost);
            settings.UserName.Should().Be(specialUser);
            settings.Password.Should().Be(specialPass);
        }

        [Fact]
        public void RmqConnectionSettings_WithNegativeNumbers_ShouldStoreValues()
        {
            // Arrange
            var settings = new RmqConnectionSettings();

            // Act
            settings.Port = -1;
            settings.PackageId = -999;
            settings.RequestedHeartbeatSeconds = -30;
            settings.NetworkRecoveryInterval = -25;

            // Assert
            settings.Port.Should().Be(-1);
            settings.PackageId.Should().Be(-999);
            settings.RequestedHeartbeatSeconds.Should().Be(-30);
            settings.NetworkRecoveryInterval.Should().Be(-25);
        }

        [Fact]
        public void RmqConnectionSettings_WithMaxValues_ShouldStoreValues()
        {
            // Arrange
            var settings = new RmqConnectionSettings();

            // Act
            settings.Port = int.MaxValue;
            settings.PackageId = int.MaxValue;
            settings.PrefetchCount = ushort.MaxValue;
            settings.RequestedHeartbeatSeconds = int.MaxValue;
            settings.NetworkRecoveryInterval = int.MaxValue;

            // Assert
            settings.Port.Should().Be(int.MaxValue);
            settings.PackageId.Should().Be(int.MaxValue);
            settings.PrefetchCount.Should().Be(ushort.MaxValue);
            settings.RequestedHeartbeatSeconds.Should().Be(int.MaxValue);
            settings.NetworkRecoveryInterval.Should().Be(int.MaxValue);
        }

        [Fact]
        public void RmqConnectionSettings_WithMinValues_ShouldStoreValues()
        {
            // Arrange
            var settings = new RmqConnectionSettings();

            // Act
            settings.Port = int.MinValue;
            settings.PackageId = int.MinValue;
            settings.PrefetchCount = ushort.MinValue;
            settings.RequestedHeartbeatSeconds = int.MinValue;
            settings.NetworkRecoveryInterval = int.MinValue;

            // Assert
            settings.Port.Should().Be(int.MinValue);
            settings.PackageId.Should().Be(int.MinValue);
            settings.PrefetchCount.Should().Be(ushort.MinValue);
            settings.RequestedHeartbeatSeconds.Should().Be(int.MinValue);
            settings.NetworkRecoveryInterval.Should().Be(int.MinValue);
        }



        [Theory]
        [InlineData("localhost", 5672, "/", 123, "user", "pass")]
        [InlineData("rabbit.example.com", 5673, "/test", 456, "admin", "secret")]
        [InlineData("rmq-cluster.local", 5674, "/prod", 789, "service", "token")]
        public void RmqConnectionSettings_WithVariousConfigurations_ShouldStoreCorrectly(
            string host, int port, string virtualHost, int packageId, string userName, string password)
        {
            // Arrange
            var settings = new RmqConnectionSettings();

            // Act
            settings.Host = host;
            settings.Port = port;
            settings.VirtualHost = virtualHost;
            settings.PackageId = packageId;
            settings.UserName = userName;
            settings.Password = password;

            // Assert
            settings.Host.Should().Be(host);
            settings.Port.Should().Be(port);
            settings.VirtualHost.Should().Be(virtualHost);
            settings.PackageId.Should().Be(packageId);
            settings.UserName.Should().Be(userName);
            settings.Password.Should().Be(password);
        }

        [Fact]
        public void RmqConnectionSettings_ResetToDefaults_ShouldRestoreDefaultValues()
        {
            // Arrange
            var settings = new RmqConnectionSettings
            {
                Host = "customhost",
                Port = 9999,
                PrefetchCount = 500,
                DispatchConsumersAsync = false,
                AutoAck = false,
                RequestedHeartbeatSeconds = 120,
                NetworkRecoveryInterval = 60
            };

            // Act - Reset to defaults
            settings.Host = null;
            settings.Port = 0;
            settings.PrefetchCount = 100;
            settings.DispatchConsumersAsync = true;
            settings.AutoAck = true;
            settings.RequestedHeartbeatSeconds = 30;
            settings.NetworkRecoveryInterval = 30;

            // Assert
            settings.Host.Should().BeNull();
            settings.Port.Should().Be(0);
            settings.PrefetchCount.Should().Be(100);
            settings.DispatchConsumersAsync.Should().BeTrue();
            settings.AutoAck.Should().BeTrue();
            settings.RequestedHeartbeatSeconds.Should().Be(30);
            settings.NetworkRecoveryInterval.Should().Be(30);
        }
    }
} 