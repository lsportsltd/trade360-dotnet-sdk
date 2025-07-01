using System;
using FluentAssertions;
using Trade360SDK.Feed.Configuration;
using Trade360SDK.Feed.RabbitMQ.Validators;
using Xunit;

namespace Trade360SDK.Feed.RabbitMQ.Tests.Validators
{
    /// <summary>
    /// Comprehensive unit tests for RmqConnectionSettingsValidator covering all validation rules,
    /// edge cases, and error scenarios to maximize code coverage.
    /// </summary>
    public class RmqConnectionSettingsValidatorTests
    {
        #region Valid Settings Tests

        [Fact]
        public void Validate_WithValidSettings_ShouldNotThrowException()
        {
            // Arrange
            var validSettings = CreateValidSettings();

            // Act & Assert
            var act = () => RmqConnectionSettingsValidator.Validate(validSettings);
            act.Should().NotThrow();
        }

        [Fact]
        public void Validate_WithMinimumValidValues_ShouldNotThrowException()
        {
            // Arrange
            var settings = new RmqConnectionSettings
            {
                Host = "h",                     // Minimum non-empty string
                Port = 1,                       // Minimum positive integer
                VirtualHost = "/",              // Minimum non-empty string
                PackageId = 1,                  // Minimum positive integer
                UserName = "u",                 // Minimum non-empty string
                Password = "p",                 // Minimum non-empty string
                RequestedHeartbeatSeconds = 11, // Minimum valid value (> 10)
                NetworkRecoveryInterval = 16    // Minimum valid value (> 15)
            };

            // Act & Assert
            var act = () => RmqConnectionSettingsValidator.Validate(settings);
            act.Should().NotThrow();
        }

        #endregion

        #region Host Validation Tests

        [Fact]
        public void Validate_WithNullHost_ShouldThrowArgumentException()
        {
            // Arrange
            var settings = CreateValidSettings();
            settings.Host = null!;

            // Act & Assert
            var act = () => RmqConnectionSettingsValidator.Validate(settings);
            act.Should().Throw<ArgumentException>()
                .WithMessage("Host is required.*")
                .And.ParamName.Should().Be("Host");
        }

        [Fact]
        public void Validate_WithEmptyHost_ShouldThrowArgumentException()
        {
            // Arrange
            var settings = CreateValidSettings();
            settings.Host = string.Empty;

            // Act & Assert
            var act = () => RmqConnectionSettingsValidator.Validate(settings);
            act.Should().Throw<ArgumentException>()
                .WithMessage("Host is required.*")
                .And.ParamName.Should().Be("Host");
        }

        [Fact]
        public void Validate_WithWhitespaceHost_ShouldThrowArgumentException()
        {
            // Arrange
            var settings = CreateValidSettings();
            settings.Host = "   ";

            // Act & Assert
            var act = () => RmqConnectionSettingsValidator.Validate(settings);
            act.Should().Throw<ArgumentException>()
                .WithMessage("Host is required.*")
                .And.ParamName.Should().Be("Host");
        }

        [Theory]
        [InlineData("\t")]
        [InlineData("\n")]
        [InlineData("\r")]
        [InlineData(" \t\n\r ")]
        public void Validate_WithVariousWhitespaceOnlyHosts_ShouldThrowArgumentException(string whitespaceHost)
        {
            // Arrange
            var settings = CreateValidSettings();
            settings.Host = whitespaceHost;

            // Act & Assert
            var act = () => RmqConnectionSettingsValidator.Validate(settings);
            act.Should().Throw<ArgumentException>()
                .WithMessage("Host is required.*")
                .And.ParamName.Should().Be("Host");
        }

        #endregion

        #region Port Validation Tests

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(-100)]
        [InlineData(int.MinValue)]
        public void Validate_WithInvalidPort_ShouldThrowArgumentException(int invalidPort)
        {
            // Arrange
            var settings = CreateValidSettings();
            settings.Port = invalidPort;

            // Act & Assert
            var act = () => RmqConnectionSettingsValidator.Validate(settings);
            act.Should().Throw<ArgumentException>()
                .WithMessage("Port must be a positive integer.*")
                .And.ParamName.Should().Be("Port");
        }

        [Theory]
        [InlineData(1)]
        [InlineData(5672)]
        [InlineData(65535)]
        [InlineData(int.MaxValue)]
        public void Validate_WithValidPort_ShouldNotThrowException(int validPort)
        {
            // Arrange
            var settings = CreateValidSettings();
            settings.Port = validPort;

            // Act & Assert
            var act = () => RmqConnectionSettingsValidator.Validate(settings);
            act.Should().NotThrow();
        }

        #endregion

        #region VirtualHost Validation Tests

        [Fact]
        public void Validate_WithNullVirtualHost_ShouldThrowArgumentException()
        {
            // Arrange
            var settings = CreateValidSettings();
            settings.VirtualHost = null!;

            // Act & Assert
            var act = () => RmqConnectionSettingsValidator.Validate(settings);
            act.Should().Throw<ArgumentException>()
                .WithMessage("VirtualHost is required.*")
                .And.ParamName.Should().Be("VirtualHost");
        }

        [Fact]
        public void Validate_WithEmptyVirtualHost_ShouldThrowArgumentException()
        {
            // Arrange
            var settings = CreateValidSettings();
            settings.VirtualHost = string.Empty;

            // Act & Assert
            var act = () => RmqConnectionSettingsValidator.Validate(settings);
            act.Should().Throw<ArgumentException>()
                .WithMessage("VirtualHost is required.*")
                .And.ParamName.Should().Be("VirtualHost");
        }

        [Fact]
        public void Validate_WithWhitespaceVirtualHost_ShouldThrowArgumentException()
        {
            // Arrange
            var settings = CreateValidSettings();
            settings.VirtualHost = "   ";

            // Act & Assert
            var act = () => RmqConnectionSettingsValidator.Validate(settings);
            act.Should().Throw<ArgumentException>()
                .WithMessage("VirtualHost is required.*")
                .And.ParamName.Should().Be("VirtualHost");
        }

        #endregion

        #region PackageId Validation Tests

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(-100)]
        [InlineData(int.MinValue)]
        public void Validate_WithInvalidPackageId_ShouldThrowArgumentException(int invalidPackageId)
        {
            // Arrange
            var settings = CreateValidSettings();
            settings.PackageId = invalidPackageId;

            // Act & Assert
            var act = () => RmqConnectionSettingsValidator.Validate(settings);
            act.Should().Throw<ArgumentException>()
                .WithMessage("PackageId must be a positive integer.*")
                .And.ParamName.Should().Be("PackageId");
        }

        [Theory]
        [InlineData(1)]
        [InlineData(123)]
        [InlineData(999999)]
        [InlineData(int.MaxValue)]
        public void Validate_WithValidPackageId_ShouldNotThrowException(int validPackageId)
        {
            // Arrange
            var settings = CreateValidSettings();
            settings.PackageId = validPackageId;

            // Act & Assert
            var act = () => RmqConnectionSettingsValidator.Validate(settings);
            act.Should().NotThrow();
        }

        #endregion

        #region UserName Validation Tests

        [Fact]
        public void Validate_WithNullUserName_ShouldThrowArgumentException()
        {
            // Arrange
            var settings = CreateValidSettings();
            settings.UserName = null!;

            // Act & Assert
            var act = () => RmqConnectionSettingsValidator.Validate(settings);
            act.Should().Throw<ArgumentException>()
                .WithMessage("UserName is required.*")
                .And.ParamName.Should().Be("UserName");
        }

        [Fact]
        public void Validate_WithEmptyUserName_ShouldThrowArgumentException()
        {
            // Arrange
            var settings = CreateValidSettings();
            settings.UserName = string.Empty;

            // Act & Assert
            var act = () => RmqConnectionSettingsValidator.Validate(settings);
            act.Should().Throw<ArgumentException>()
                .WithMessage("UserName is required.*")
                .And.ParamName.Should().Be("UserName");
        }

        [Fact]
        public void Validate_WithWhitespaceUserName_ShouldThrowArgumentException()
        {
            // Arrange
            var settings = CreateValidSettings();
            settings.UserName = "   ";

            // Act & Assert
            var act = () => RmqConnectionSettingsValidator.Validate(settings);
            act.Should().Throw<ArgumentException>()
                .WithMessage("UserName is required.*")
                .And.ParamName.Should().Be("UserName");
        }

        #endregion

        #region Password Validation Tests

        [Fact]
        public void Validate_WithNullPassword_ShouldThrowArgumentException()
        {
            // Arrange
            var settings = CreateValidSettings();
            settings.Password = null!;

            // Act & Assert
            var act = () => RmqConnectionSettingsValidator.Validate(settings);
            act.Should().Throw<ArgumentException>()
                .WithMessage("Password is required.*")
                .And.ParamName.Should().Be("Password");
        }

        [Fact]
        public void Validate_WithEmptyPassword_ShouldThrowArgumentException()
        {
            // Arrange
            var settings = CreateValidSettings();
            settings.Password = string.Empty;

            // Act & Assert
            var act = () => RmqConnectionSettingsValidator.Validate(settings);
            act.Should().Throw<ArgumentException>()
                .WithMessage("Password is required.*")
                .And.ParamName.Should().Be("Password");
        }

        [Fact]
        public void Validate_WithWhitespacePassword_ShouldThrowArgumentException()
        {
            // Arrange
            var settings = CreateValidSettings();
            settings.Password = "   ";

            // Act & Assert
            var act = () => RmqConnectionSettingsValidator.Validate(settings);
            act.Should().Throw<ArgumentException>()
                .WithMessage("Password is required.*")
                .And.ParamName.Should().Be("Password");
        }

        #endregion

        #region RequestedHeartbeatSeconds Validation Tests

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(10)]
        [InlineData(-1)]
        [InlineData(int.MinValue)]
        public void Validate_WithInvalidRequestedHeartbeatSeconds_ShouldThrowArgumentException(int invalidHeartbeat)
        {
            // Arrange
            var settings = CreateValidSettings();
            settings.RequestedHeartbeatSeconds = invalidHeartbeat;

            // Act & Assert
            var act = () => RmqConnectionSettingsValidator.Validate(settings);
            act.Should().Throw<ArgumentException>()
                .WithMessage("RequestedHeartbeatSeconds must be a positive integer - Larger then 10.*")
                .And.ParamName.Should().Be("RequestedHeartbeatSeconds");
        }

        [Theory]
        [InlineData(11)]
        [InlineData(30)]
        [InlineData(60)]
        [InlineData(int.MaxValue)]
        public void Validate_WithValidRequestedHeartbeatSeconds_ShouldNotThrowException(int validHeartbeat)
        {
            // Arrange
            var settings = CreateValidSettings();
            settings.RequestedHeartbeatSeconds = validHeartbeat;

            // Act & Assert
            var act = () => RmqConnectionSettingsValidator.Validate(settings);
            act.Should().NotThrow();
        }

        #endregion

        #region NetworkRecoveryInterval Validation Tests

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(15)]
        [InlineData(-1)]
        [InlineData(int.MinValue)]
        public void Validate_WithInvalidNetworkRecoveryInterval_ShouldThrowArgumentException(int invalidInterval)
        {
            // Arrange
            var settings = CreateValidSettings();
            settings.NetworkRecoveryInterval = invalidInterval;

            // Act & Assert
            var act = () => RmqConnectionSettingsValidator.Validate(settings);
            act.Should().Throw<ArgumentException>()
                .WithMessage("NetworkRecoveryInterval must be a positive integer.*")
                .And.ParamName.Should().Be("NetworkRecoveryInterval");
        }

        [Theory]
        [InlineData(16)]
        [InlineData(30)]
        [InlineData(60)]
        [InlineData(int.MaxValue)]
        public void Validate_WithValidNetworkRecoveryInterval_ShouldNotThrowException(int validInterval)
        {
            // Arrange
            var settings = CreateValidSettings();
            settings.NetworkRecoveryInterval = validInterval;

            // Act & Assert
            var act = () => RmqConnectionSettingsValidator.Validate(settings);
            act.Should().NotThrow();
        }

        #endregion

        #region Multiple Invalid Properties Tests

        [Fact]
        public void Validate_WithMultipleInvalidProperties_ShouldThrowForFirstEncounteredError()
        {
            // Arrange - Create settings with multiple invalid properties
            var settings = new RmqConnectionSettings
            {
                Host = null!,                   // Invalid - should be first error
                Port = -1,                      // Invalid
                VirtualHost = "",               // Invalid
                PackageId = 0,                  // Invalid
                UserName = null!,               // Invalid
                Password = "",                  // Invalid
                RequestedHeartbeatSeconds = 5,  // Invalid
                NetworkRecoveryInterval = 10    // Invalid
            };

            // Act & Assert - Should throw for Host first (validates in order)
            var act = () => RmqConnectionSettingsValidator.Validate(settings);
            act.Should().Throw<ArgumentException>()
                .WithMessage("Host is required.*")
                .And.ParamName.Should().Be("Host");
        }

        #endregion

        #region Helper Methods

        private static RmqConnectionSettings CreateValidSettings()
        {
            return new RmqConnectionSettings
            {
                Host = "localhost",
                Port = 5672,
                VirtualHost = "/",
                PackageId = 123,
                UserName = "guest",
                Password = "password",
                RequestedHeartbeatSeconds = 30,
                NetworkRecoveryInterval = 20,
                PrefetchCount = 1,
                AutoAck = false,
                DispatchConsumersAsync = true
            };
        }

        #endregion
    }
} 