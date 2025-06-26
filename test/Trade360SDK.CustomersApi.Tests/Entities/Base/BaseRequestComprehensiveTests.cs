using System;
using FluentAssertions;
using Trade360SDK.CustomersApi.Entities.Base;
using Xunit;

namespace Trade360SDK.CustomersApi.Tests.Entities.Base
{
    public class BaseRequestComprehensiveTests
    {
        #region Constructor Tests

        [Fact]
        public void BaseRequest_DefaultConstructor_ShouldInitializeCorrectly()
        {
            // Act
            var request = new BaseRequest();

            // Assert
            request.Should().NotBeNull();
            request.PackageId.Should().Be(0);
            request.UserName.Should().BeNull();
            request.Password.Should().BeNull();
        }

        #endregion

        #region Property Tests

        [Fact]
        public void PackageId_ShouldBeSettableAndGettable()
        {
            // Arrange
            var request = new BaseRequest();
            var expectedPackageId = 12345;

            // Act
            request.PackageId = expectedPackageId;

            // Assert
            request.PackageId.Should().Be(expectedPackageId);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(999999)]
        [InlineData(-1)]
        [InlineData(int.MaxValue)]
        [InlineData(int.MinValue)]
        public void PackageId_WithVariousValues_ShouldStoreCorrectly(int packageId)
        {
            // Arrange
            var request = new BaseRequest();

            // Act
            request.PackageId = packageId;

            // Assert
            request.PackageId.Should().Be(packageId);
        }

        [Fact]
        public void UserName_ShouldBeSettableAndGettable()
        {
            // Arrange
            var request = new BaseRequest();
            var expectedUserName = "testuser";

            // Act
            request.UserName = expectedUserName;

            // Assert
            request.UserName.Should().Be(expectedUserName);
        }

        [Theory]
        [InlineData("")]
        [InlineData("a")]
        [InlineData("validuser")]
        [InlineData("user_with_underscore")]
        [InlineData("user-with-dash")]
        [InlineData("user123")]
        [InlineData("VeryLongUserNameThatExceedsNormalLengthLimits")]
        public void UserName_WithVariousValidValues_ShouldStoreCorrectly(string userName)
        {
            // Arrange
            var request = new BaseRequest();

            // Act
            request.UserName = userName;

            // Assert
            request.UserName.Should().Be(userName);
        }

        [Fact]
        public void UserName_WithNull_ShouldStoreNull()
        {
            // Arrange
            var request = new BaseRequest();

            // Act
            request.UserName = null;

            // Assert
            request.UserName.Should().BeNull();
        }

        [Fact]
        public void Password_ShouldBeSettableAndGettable()
        {
            // Arrange
            var request = new BaseRequest();
            var expectedPassword = "securepassword";

            // Act
            request.Password = expectedPassword;

            // Assert
            request.Password.Should().Be(expectedPassword);
        }

        [Theory]
        [InlineData("")]
        [InlineData("p")]
        [InlineData("password123")]
        [InlineData("P@ssw0rd!")]
        [InlineData("VeryLongPasswordWithSpecialCharacters!@#$%^&*()")]
        public void Password_WithVariousValidValues_ShouldStoreCorrectly(string password)
        {
            // Arrange
            var request = new BaseRequest();

            // Act
            request.Password = password;

            // Assert
            request.Password.Should().Be(password);
        }

        [Fact]
        public void Password_WithNull_ShouldStoreNull()
        {
            // Arrange
            var request = new BaseRequest();

            // Act
            request.Password = null;

            // Assert
            request.Password.Should().BeNull();
        }

        #endregion

        #region Object Initialization Tests

        [Fact]
        public void BaseRequest_WithObjectInitializer_ShouldSetAllProperties()
        {
            // Arrange
            var packageId = 98765;
            var userName = "inituser";
            var password = "initpass";

            // Act
            var request = new BaseRequest
            {
                PackageId = packageId,
                UserName = userName,
                Password = password
            };

            // Assert
            request.PackageId.Should().Be(packageId);
            request.UserName.Should().Be(userName);
            request.Password.Should().Be(password);
        }

        [Fact]
        public void BaseRequest_WithPartialObjectInitializer_ShouldSetSpecifiedProperties()
        {
            // Arrange
            var packageId = 54321;

            // Act
            var request = new BaseRequest
            {
                PackageId = packageId
                // UserName and Password left as default
            };

            // Assert
            request.PackageId.Should().Be(packageId);
            request.UserName.Should().BeNull();
            request.Password.Should().BeNull();
        }

        #endregion

        #region Property Independence Tests

        [Fact]
        public void Properties_ShouldBeIndependent()
        {
            // Arrange
            var request = new BaseRequest();

            // Act
            request.PackageId = 111;
            request.UserName = "user1";
            request.Password = "pass1";

            // Modify one property
            request.UserName = "user2";

            // Assert
            request.PackageId.Should().Be(111); // Should remain unchanged
            request.UserName.Should().Be("user2"); // Should be updated
            request.Password.Should().Be("pass1"); // Should remain unchanged
        }

        #endregion

        #region Multiple Instance Tests

        [Fact]
        public void MultipleInstances_ShouldBeIndependent()
        {
            // Arrange & Act
            var request1 = new BaseRequest
            {
                PackageId = 100,
                UserName = "user1",
                Password = "pass1"
            };

            var request2 = new BaseRequest
            {
                PackageId = 200,
                UserName = "user2",
                Password = "pass2"
            };

            // Assert
            request1.PackageId.Should().Be(100);
            request1.UserName.Should().Be("user1");
            request1.Password.Should().Be("pass1");

            request2.PackageId.Should().Be(200);
            request2.UserName.Should().Be("user2");
            request2.Password.Should().Be("pass2");

            // Verify independence
            request1.Should().NotBeSameAs(request2);
        }

        #endregion

        #region Edge Case Tests

        [Fact]
        public void BaseRequest_WithSpecialCharactersInStrings_ShouldHandleCorrectly()
        {
            // Arrange
            var request = new BaseRequest();
            var specialUserName = "user@domain.com";
            var specialPassword = "p@$$w0rd!#$%";

            // Act
            request.UserName = specialUserName;
            request.Password = specialPassword;

            // Assert
            request.UserName.Should().Be(specialUserName);
            request.Password.Should().Be(specialPassword);
        }

        [Fact]
        public void BaseRequest_WithUnicodeCharacters_ShouldHandleCorrectly()
        {
            // Arrange
            var request = new BaseRequest();
            var unicodeUserName = "用户名";
            var unicodePassword = "密码123";

            // Act
            request.UserName = unicodeUserName;
            request.Password = unicodePassword;

            // Assert
            request.UserName.Should().Be(unicodeUserName);
            request.Password.Should().Be(unicodePassword);
        }

        [Fact]
        public void BaseRequest_WithWhitespaceStrings_ShouldHandleCorrectly()
        {
            // Arrange
            var request = new BaseRequest();
            var whitespaceUserName = "   ";
            var whitespacePassword = "\t\n\r";

            // Act
            request.UserName = whitespaceUserName;
            request.Password = whitespacePassword;

            // Assert
            request.UserName.Should().Be(whitespaceUserName);
            request.Password.Should().Be(whitespacePassword);
        }

        #endregion

        #region Stress Tests

        [Fact]
        public void BaseRequest_WithVeryLargePackageId_ShouldHandleCorrectly()
        {
            // Arrange
            var request = new BaseRequest();
            var largePackageId = int.MaxValue;

            // Act
            request.PackageId = largePackageId;

            // Assert
            request.PackageId.Should().Be(largePackageId);
        }

        [Fact]
        public void BaseRequest_WithVeryLongStrings_ShouldHandleCorrectly()
        {
            // Arrange
            var request = new BaseRequest();
            var longString = new string('a', 10000);

            // Act
            request.UserName = longString;
            request.Password = longString;

            // Assert
            request.UserName.Should().Be(longString);
            request.Password.Should().Be(longString);
            request.UserName.Should().HaveLength(10000);
            request.Password.Should().HaveLength(10000);
        }

        #endregion

        #region Type Safety Tests

        [Fact]
        public void BaseRequest_ShouldBeOfCorrectType()
        {
            // Act
            var request = new BaseRequest();

            // Assert
            request.Should().BeOfType<BaseRequest>();
            request.Should().BeAssignableTo<BaseRequest>();
        }

        [Fact]
        public void BaseRequest_Properties_ShouldHaveCorrectTypes()
        {
            // Arrange
            var request = new BaseRequest();

            // Act & Assert
            request.PackageId.Should().BeOfType(typeof(int));
            
            // UserName and Password can be null, so we test after assignment
            request.UserName = "test";
            request.Password = "test";
            
            request.UserName.Should().BeOfType(typeof(string));
            request.Password.Should().BeOfType(typeof(string));
        }

        #endregion

        #region Immutability Tests

        [Fact]
        public void BaseRequest_PropertiesShouldBeMutable()
        {
            // Arrange
            var request = new BaseRequest
            {
                PackageId = 1,
                UserName = "initial",
                Password = "initial"
            };

            // Act
            request.PackageId = 2;
            request.UserName = "modified";
            request.Password = "modified";

            // Assert
            request.PackageId.Should().Be(2);
            request.UserName.Should().Be("modified");
            request.Password.Should().Be("modified");
        }

        #endregion

        #region Business Logic Tests

        [Fact]
        public void BaseRequest_WithValidCredentials_ShouldRepresentValidState()
        {
            // Arrange
            var request = new BaseRequest
            {
                PackageId = 123,
                UserName = "validuser",
                Password = "validpass"
            };

            // Act & Assert
            request.PackageId.Should().BeGreaterThan(0);
            request.UserName.Should().NotBeNullOrEmpty();
            request.Password.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public void BaseRequest_WithInvalidCredentials_ShouldRepresentInvalidState()
        {
            // Arrange
            var request = new BaseRequest
            {
                PackageId = 0,
                UserName = "",
                Password = null
            };

            // Act & Assert
            request.PackageId.Should().Be(0);
            request.UserName.Should().BeEmpty();
            request.Password.Should().BeNull();
        }

        #endregion
    }
} 