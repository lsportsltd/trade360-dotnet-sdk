using System;
using FluentAssertions;
using Trade360SDK.Feed.Configuration;
using Trade360SDK.Feed.RabbitMQ.Validators;
using Xunit;

namespace Trade360SDK.Feed.RabbitMQ.Tests.Validators
{
    public class RmqConnectionSettingsValidatorComprehensiveTests
    {
        [Fact]
        public void Validate_WithValidSettings_ShouldNotThrow()
        {
            // Arrange
            var settings = new RmqConnectionSettings
            {
                Host = "localhost",
                Port = 5672,
                VirtualHost = "/",
                PackageId = 123,
                UserName = "user",
                Password = "password",
                RequestedHeartbeatSeconds = 30,
                NetworkRecoveryInterval = 20
            };

            // Act & Assert
            var action = () => RmqConnectionSettingsValidator.Validate(settings);
            action.Should().NotThrow();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void Validate_WithInvalidHost_ShouldThrowArgumentException(string invalidHost)
        {
            // Arrange
            var settings = new RmqConnectionSettings
            {
                Host = invalidHost,
                Port = 5672,
                VirtualHost = "/",
                PackageId = 123,
                UserName = "user",
                Password = "password",
                RequestedHeartbeatSeconds = 30,
                NetworkRecoveryInterval = 20
            };

            // Act & Assert
            var action = () => RmqConnectionSettingsValidator.Validate(settings);
            action.Should().Throw<ArgumentException>()
                .WithMessage("Host is required.*")
                .And.ParamName.Should().Be("Host");
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(-100)]
        public void Validate_WithInvalidPort_ShouldThrowArgumentException(int invalidPort)
        {
            // Arrange
            var settings = new RmqConnectionSettings
            {
                Host = "localhost",
                Port = invalidPort,
                VirtualHost = "/",
                PackageId = 123,
                UserName = "user",
                Password = "password",
                RequestedHeartbeatSeconds = 30,
                NetworkRecoveryInterval = 20
            };

            // Act & Assert
            var action = () => RmqConnectionSettingsValidator.Validate(settings);
            action.Should().Throw<ArgumentException>()
                .WithMessage("Port must be a positive integer.*")
                .And.ParamName.Should().Be("Port");
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void Validate_WithInvalidVirtualHost_ShouldThrowArgumentException(string invalidVirtualHost)
        {
            // Arrange
            var settings = new RmqConnectionSettings
            {
                Host = "localhost",
                Port = 5672,
                VirtualHost = invalidVirtualHost,
                PackageId = 123,
                UserName = "user",
                Password = "password",
                RequestedHeartbeatSeconds = 30,
                NetworkRecoveryInterval = 20
            };

            // Act & Assert
            var action = () => RmqConnectionSettingsValidator.Validate(settings);
            action.Should().Throw<ArgumentException>()
                .WithMessage("VirtualHost is required.*")
                .And.ParamName.Should().Be("VirtualHost");
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(-100)]
        public void Validate_WithInvalidPackageId_ShouldThrowArgumentException(int invalidPackageId)
        {
            // Arrange
            var settings = new RmqConnectionSettings
            {
                Host = "localhost",
                Port = 5672,
                VirtualHost = "/",
                PackageId = invalidPackageId,
                UserName = "user",
                Password = "password",
                RequestedHeartbeatSeconds = 30,
                NetworkRecoveryInterval = 20
            };

            // Act & Assert
            var action = () => RmqConnectionSettingsValidator.Validate(settings);
            action.Should().Throw<ArgumentException>()
                .WithMessage("PackageId must be a positive integer.*")
                .And.ParamName.Should().Be("PackageId");
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void Validate_WithInvalidUserName_ShouldThrowArgumentException(string invalidUserName)
        {
            // Arrange
            var settings = new RmqConnectionSettings
            {
                Host = "localhost",
                Port = 5672,
                VirtualHost = "/",
                PackageId = 123,
                UserName = invalidUserName,
                Password = "password",
                RequestedHeartbeatSeconds = 30,
                NetworkRecoveryInterval = 20
            };

            // Act & Assert
            var action = () => RmqConnectionSettingsValidator.Validate(settings);
            action.Should().Throw<ArgumentException>()
                .WithMessage("UserName is required.*")
                .And.ParamName.Should().Be("UserName");
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void Validate_WithInvalidPassword_ShouldThrowArgumentException(string invalidPassword)
        {
            // Arrange
            var settings = new RmqConnectionSettings
            {
                Host = "localhost",
                Port = 5672,
                VirtualHost = "/",
                PackageId = 123,
                UserName = "user",
                Password = invalidPassword,
                RequestedHeartbeatSeconds = 30,
                NetworkRecoveryInterval = 20
            };

            // Act & Assert
            var action = () => RmqConnectionSettingsValidator.Validate(settings);
            action.Should().Throw<ArgumentException>()
                .WithMessage("Password is required.*")
                .And.ParamName.Should().Be("Password");
        }

        [Theory]
        [InlineData(10)]
        [InlineData(5)]
        [InlineData(0)]
        [InlineData(-1)]
        public void Validate_WithInvalidRequestedHeartbeatSeconds_ShouldThrowArgumentException(int invalidHeartbeat)
        {
            // Arrange
            var settings = new RmqConnectionSettings
            {
                Host = "localhost",
                Port = 5672,
                VirtualHost = "/",
                PackageId = 123,
                UserName = "user",
                Password = "password",
                RequestedHeartbeatSeconds = invalidHeartbeat,
                NetworkRecoveryInterval = 20
            };

            // Act & Assert
            var action = () => RmqConnectionSettingsValidator.Validate(settings);
            action.Should().Throw<ArgumentException>()
                .WithMessage("RequestedHeartbeatSeconds must be a positive integer - Larger then 10.*")
                .And.ParamName.Should().Be("RequestedHeartbeatSeconds");
        }

        [Theory]
        [InlineData(15)]
        [InlineData(10)]
        [InlineData(0)]
        [InlineData(-1)]
        public void Validate_WithInvalidNetworkRecoveryInterval_ShouldThrowArgumentException(int invalidInterval)
        {
            // Arrange
            var settings = new RmqConnectionSettings
            {
                Host = "localhost",
                Port = 5672,
                VirtualHost = "/",
                PackageId = 123,
                UserName = "user",
                Password = "password",
                RequestedHeartbeatSeconds = 30,
                NetworkRecoveryInterval = invalidInterval
            };

            // Act & Assert
            var action = () => RmqConnectionSettingsValidator.Validate(settings);
            action.Should().Throw<ArgumentException>()
                .WithMessage("NetworkRecoveryInterval must be a positive integer.*")
                .And.ParamName.Should().Be("NetworkRecoveryInterval");
        }

        [Fact]
        public void Validate_WithMinimumValidValues_ShouldNotThrow()
        {
            // Arrange
            var settings = new RmqConnectionSettings
            {
                Host = "h",
                Port = 1,
                VirtualHost = "/",
                PackageId = 1,
                UserName = "u",
                Password = "p",
                RequestedHeartbeatSeconds = 11, // Minimum valid value
                NetworkRecoveryInterval = 16 // Minimum valid value
            };

            // Act & Assert
            var action = () => RmqConnectionSettingsValidator.Validate(settings);
            action.Should().NotThrow();
        }

        [Fact]
        public void Validate_WithMaximumValidValues_ShouldNotThrow()
        {
            // Arrange
            var settings = new RmqConnectionSettings
            {
                Host = new string('H', 1000),
                Port = 65535,
                VirtualHost = new string('/', 100),
                PackageId = int.MaxValue,
                UserName = new string('U', 1000),
                Password = new string('P', 1000),
                RequestedHeartbeatSeconds = int.MaxValue,
                NetworkRecoveryInterval = int.MaxValue
            };

            // Act & Assert
            var action = () => RmqConnectionSettingsValidator.Validate(settings);
            action.Should().NotThrow();
        }

        [Fact]
        public void Validate_WithSpecialCharactersInStrings_ShouldNotThrow()
        {
            // Arrange
            var settings = new RmqConnectionSettings
            {
                Host = "test-host.domain.com",
                Port = 5672,
                VirtualHost = "/test_vhost",
                PackageId = 123,
                UserName = "user@domain.com",
                Password = "P@ssw0rd!",
                RequestedHeartbeatSeconds = 30,
                NetworkRecoveryInterval = 20
            };

            // Act & Assert
            var action = () => RmqConnectionSettingsValidator.Validate(settings);
            action.Should().NotThrow();
        }

        [Fact]
        public void Validate_WithUnicodeCharacters_ShouldNotThrow()
        {
            // Arrange
            var settings = new RmqConnectionSettings
            {
                Host = "测试主机",
                Port = 5672,
                VirtualHost = "/虚拟主机",
                PackageId = 123,
                UserName = "用户名",
                Password = "密码",
                RequestedHeartbeatSeconds = 30,
                NetworkRecoveryInterval = 20
            };

            // Act & Assert
            var action = () => RmqConnectionSettingsValidator.Validate(settings);
            action.Should().NotThrow();
        }

        [Fact]
        public void Validate_WithStandardPortNumbers_ShouldNotThrow()
        {
            // Arrange
            var validPorts = new[] { 5672, 5671, 15672, 25672 };

            foreach (var port in validPorts)
            {
                var settings = new RmqConnectionSettings
                {
                    Host = "localhost",
                    Port = port,
                    VirtualHost = "/",
                    PackageId = 123,
                    UserName = "user",
                    Password = "password",
                    RequestedHeartbeatSeconds = 30,
                    NetworkRecoveryInterval = 20
                };

                // Act & Assert
                var action = () => RmqConnectionSettingsValidator.Validate(settings);
                action.Should().NotThrow($"Port {port} should be valid");
            }
        }

        [Fact]
        public void Validate_WithCommonVirtualHosts_ShouldNotThrow()
        {
            // Arrange
            var validVirtualHosts = new[] { "/", "/test", "/production", "/development", "custom_vhost" };

            foreach (var vhost in validVirtualHosts)
            {
                var settings = new RmqConnectionSettings
                {
                    Host = "localhost",
                    Port = 5672,
                    VirtualHost = vhost,
                    PackageId = 123,
                    UserName = "user",
                    Password = "password",
                    RequestedHeartbeatSeconds = 30,
                    NetworkRecoveryInterval = 20
                };

                // Act & Assert
                var action = () => RmqConnectionSettingsValidator.Validate(settings);
                action.Should().NotThrow($"VirtualHost '{vhost}' should be valid");
            }
        }
    }
} 