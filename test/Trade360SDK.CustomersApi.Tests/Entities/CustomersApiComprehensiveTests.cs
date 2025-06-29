using FluentAssertions;
using Trade360SDK.Common.Configuration;
using Trade360SDK.CustomersApi.Entities.MetadataApi.Responses;

namespace Trade360SDK.CustomersApi.Tests;

public class CustomersApiComprehensiveTests
{
    #region GetDistributionStatus Tests

    [Fact]
    public void GetDistributionStatus_DefaultConstructor_ShouldInitializeCorrectly()
    {
        // Act
        var response = new GetDistributionStatusResponse();

        // Assert
        response.Should().NotBeNull();
        response.IsDistributionOn.Should().BeFalse();
        response.Consumers.Should().BeNull();
        response.NumberMessagesInQueue.Should().Be(0);
        response.MessagesPerSecond.Should().Be(0.0);
    }

    [Fact]
    public void GetDistributionStatus_SetIsDistributionOn_ShouldSetCorrectly()
    {
        // Arrange
        var response = new GetDistributionStatusResponse();
        var isDistributionOn = true;

        // Act
        response.IsDistributionOn = isDistributionOn;

        // Assert
        response.IsDistributionOn.Should().Be(isDistributionOn);
    }

    [Fact]
    public void GetDistributionStatus_SetConsumers_ShouldSetCorrectly()
    {
        // Arrange
        var response = new GetDistributionStatusResponse();
        var consumers = new[] { "consumer1", "consumer2" };

        // Act
        response.Consumers = consumers;

        // Assert
        response.Consumers.Should().BeEquivalentTo(consumers);
    }

    [Fact]
    public void GetDistributionStatus_SetNumberMessagesInQueue_ShouldSetCorrectly()
    {
        // Arrange
        var response = new GetDistributionStatusResponse();
        var numberOfMessages = 1000;

        // Act
        response.NumberMessagesInQueue = numberOfMessages;

        // Assert
        response.NumberMessagesInQueue.Should().Be(numberOfMessages);
    }

    [Fact]
    public void GetDistributionStatus_SetMessagesPerSecond_ShouldSetCorrectly()
    {
        // Arrange
        var response = new GetDistributionStatusResponse();
        var messagesPerSecond = 50.5;

        // Act
        response.MessagesPerSecond = messagesPerSecond;

        // Assert
        response.MessagesPerSecond.Should().Be(messagesPerSecond);
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void GetDistributionStatus_SetIsDistributionOnToVariousValues_ShouldAcceptValue(bool isDistributionOn)
    {
        // Arrange
        var response = new GetDistributionStatusResponse();

        // Act
        response.IsDistributionOn = isDistributionOn;

        // Assert
        response.IsDistributionOn.Should().Be(isDistributionOn);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(100)]
    [InlineData(1000)]
    [InlineData(10000)]
    public void GetDistributionStatus_SetNumberMessagesInQueueToVariousValues_ShouldAcceptValue(int numberOfMessages)
    {
        // Arrange
        var response = new GetDistributionStatusResponse();

        // Act
        response.NumberMessagesInQueue = numberOfMessages;

        // Assert
        response.NumberMessagesInQueue.Should().Be(numberOfMessages);
    }

    [Theory]
    [InlineData(0.0)]
    [InlineData(25.5)]
    [InlineData(100.0)]
    [InlineData(999.999)]
    public void GetDistributionStatus_SetMessagesPerSecondToVariousValues_ShouldAcceptValue(double messagesPerSecond)
    {
        // Arrange
        var response = new GetDistributionStatusResponse();

        // Act
        response.MessagesPerSecond = messagesPerSecond;

        // Assert
        response.MessagesPerSecond.Should().Be(messagesPerSecond);
    }

    [Fact]
    public void GetDistributionStatus_SetAllProperties_ShouldSetAllCorrectly()
    {
        // Arrange
        var consumers = new[] { "consumer1", "consumer2" };
        var response = new GetDistributionStatusResponse
        {
            IsDistributionOn = true,
            Consumers = consumers,
            NumberMessagesInQueue = 1000,
            MessagesPerSecond = 50.5
        };

        // Assert
        response.IsDistributionOn.Should().BeTrue();
        response.Consumers.Should().BeEquivalentTo(consumers);
        response.NumberMessagesInQueue.Should().Be(1000);
        response.MessagesPerSecond.Should().Be(50.5);
    }

    [Fact]
    public void GetDistributionStatus_SetNullConsumers_ShouldAcceptNull()
    {
        // Arrange
        var response = new GetDistributionStatusResponse();

        // Act
        response.Consumers = null;

        // Assert
        response.Consumers.Should().BeNull();
    }

    [Fact]
    public void GetDistributionStatus_SetEmptyConsumers_ShouldAcceptEmptyArray()
    {
        // Arrange
        var response = new GetDistributionStatusResponse();
        var emptyConsumers = new string[0];

        // Act
        response.Consumers = emptyConsumers;

        // Assert
        response.Consumers.Should().NotBeNull();
        response.Consumers.Should().BeEmpty();
    }

    [Fact]
    public void GetDistributionStatus_SetNegativeValues_ShouldAcceptNegativeValues()
    {
        // Arrange
        var response = new GetDistributionStatusResponse();

        // Act
        response.NumberMessagesInQueue = -1;
        response.MessagesPerSecond = -10.5;

        // Assert
        response.NumberMessagesInQueue.Should().Be(-1);
        response.MessagesPerSecond.Should().Be(-10.5);
    }

    [Fact]
    public void GetDistributionStatus_SetMaxValues_ShouldAcceptMaxValues()
    {
        // Arrange
        var response = new GetDistributionStatusResponse();

        // Act
        response.NumberMessagesInQueue = int.MaxValue;
        response.MessagesPerSecond = double.MaxValue;

        // Assert
        response.NumberMessagesInQueue.Should().Be(int.MaxValue);
        response.MessagesPerSecond.Should().Be(double.MaxValue);
    }

    [Fact]
    public void GetDistributionStatus_SetMinValues_ShouldAcceptMinValues()
    {
        // Arrange
        var response = new GetDistributionStatusResponse();

        // Act
        response.NumberMessagesInQueue = int.MinValue;
        response.MessagesPerSecond = double.MinValue;

        // Assert
        response.NumberMessagesInQueue.Should().Be(int.MinValue);
        response.MessagesPerSecond.Should().Be(double.MinValue);
    }

    [Fact]
    public void GetDistributionStatus_SetLargeConsumerArray_ShouldHandleLargeArray()
    {
        // Arrange
        var response = new GetDistributionStatusResponse();
        var largeConsumerArray = new string[1000];
        for (int i = 0; i < 1000; i++)
        {
            largeConsumerArray[i] = $"consumer{i}";
        }

        // Act
        response.Consumers = largeConsumerArray;

        // Assert
        response.Consumers.Should().HaveCount(1000);
        response.Consumers.Should().Contain("consumer0");
        response.Consumers.Should().Contain("consumer999");
    }

    [Fact]
    public void GetDistributionStatus_SetConsumersWithSpecialCharacters_ShouldHandleSpecialCharacters()
    {
        // Arrange
        var response = new GetDistributionStatusResponse();
        var specialConsumers = new[] { "consumer@domain.com", "consumer-with-dash", "consumer_with_underscore", "消费者" };

        // Act
        response.Consumers = specialConsumers;

        // Assert
        response.Consumers.Should().BeEquivalentTo(specialConsumers);
    }

    [Fact]
    public void GetDistributionStatus_SetSpecialDoubleValues_ShouldHandleSpecialValues()
    {
        // Arrange
        var response = new GetDistributionStatusResponse();

        // Act & Assert for NaN
        response.MessagesPerSecond = double.NaN;
        response.MessagesPerSecond.Should().Be(double.NaN);

        // Act & Assert for PositiveInfinity
        response.MessagesPerSecond = double.PositiveInfinity;
        response.MessagesPerSecond.Should().Be(double.PositiveInfinity);

        // Act & Assert for NegativeInfinity
        response.MessagesPerSecond = double.NegativeInfinity;
        response.MessagesPerSecond.Should().Be(double.NegativeInfinity);
    }

    #endregion

    #region PackageCredentials Tests

    [Fact]
    public void PackageCredentials_DefaultConstructor_ShouldInitializeWithDefaults()
    {
        // Act
        var packageCredentials = new PackageCredentials();

        // Assert
        packageCredentials.Should().NotBeNull();
        packageCredentials.PackageId.Should().Be(0);
        packageCredentials.Username.Should().BeNull();
        packageCredentials.Password.Should().BeNull();
        packageCredentials.MessageFormat.Should().Be("json");
    }

    [Fact]
    public void PackageCredentials_SetAllProperties_ShouldSetAllCorrectly()
    {
        // Arrange & Act
        var packageCredentials = new PackageCredentials
        {
            PackageId = 12345,
            Username = "testuser",
            Password = "testpass",
            MessageFormat = "xml"
        };

        // Assert
        packageCredentials.PackageId.Should().Be(12345);
        packageCredentials.Username.Should().Be("testuser");
        packageCredentials.Password.Should().Be("testpass");
        packageCredentials.MessageFormat.Should().Be("xml");
    }

    [Fact]
    public void PackageCredentials_SetPackageId_ShouldSetCorrectly()
    {
        // Arrange
        var credentials = new PackageCredentials();
        var packageId = 123;

        // Act
        credentials.PackageId = packageId;

        // Assert
        credentials.PackageId.Should().Be(packageId);
    }

    [Fact]
    public void PackageCredentials_SetUsername_ShouldSetCorrectly()
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
    public void PackageCredentials_SetPassword_ShouldSetCorrectly()
    {
        // Arrange
        var credentials = new PackageCredentials();
        var password = "testpass";

        // Act
        credentials.Password = password;

        // Assert
        credentials.Password.Should().Be(password);
    }

    [Fact]
    public void PackageCredentials_SetMessageFormat_ShouldSetCorrectly()
    {
        // Arrange
        var credentials = new PackageCredentials();
        var messageFormat = "xml";

        // Act
        credentials.MessageFormat = messageFormat;

        // Assert
        credentials.MessageFormat.Should().Be(messageFormat);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(999)]
    [InlineData(12345)]
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
    [InlineData("user1")]
    [InlineData("user@domain.com")]
    [InlineData("admin")]
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
    [InlineData("pass1")]
    [InlineData("p@ssw0rd!")]
    [InlineData("123456")]
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
    public void PackageCredentials_DefaultValues_ShouldBeCorrect()
    {
        // Act
        var credentials = new PackageCredentials();

        // Assert
        credentials.PackageId.Should().Be(0);
        credentials.Username.Should().BeNull();
        credentials.Password.Should().BeNull();
        credentials.MessageFormat.Should().Be("json");
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void PackageCredentials_SetUsernameToNullOrEmpty_ShouldAcceptValue(string username)
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
    public void PackageCredentials_SetPasswordToNullOrEmpty_ShouldAcceptValue(string password)
    {
        // Arrange
        var credentials = new PackageCredentials();

        // Act
        credentials.Password = password;

        // Assert
        credentials.Password.Should().Be(password);
    }

    [Fact]
    public void PackageCredentials_SetZeroPackageId_ShouldAcceptZero()
    {
        // Arrange
        var credentials = new PackageCredentials();

        // Act
        credentials.PackageId = 0;

        // Assert
        credentials.PackageId.Should().Be(0);
    }

    [Fact]
    public void PackageCredentials_SetNegativePackageId_ShouldAcceptNegativeValue()
    {
        // Arrange
        var credentials = new PackageCredentials();

        // Act
        credentials.PackageId = -1;

        // Assert
        credentials.PackageId.Should().Be(-1);
    }

    [Fact]
    public void PackageCredentials_SetMaxPackageId_ShouldAcceptMaxValue()
    {
        // Arrange
        var credentials = new PackageCredentials();

        // Act
        credentials.PackageId = int.MaxValue;

        // Assert
        credentials.PackageId.Should().Be(int.MaxValue);
    }

    [Fact]
    public void PackageCredentials_SetMinPackageId_ShouldAcceptMinValue()
    {
        // Arrange
        var credentials = new PackageCredentials();

        // Act
        credentials.PackageId = int.MinValue;

        // Assert
        credentials.PackageId.Should().Be(int.MinValue);
    }

    [Fact]
    public void PackageCredentials_WithCompleteValidData_ShouldMaintainAllProperties()
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

    [Fact]
    public void PackageCredentials_SetLongUsername_ShouldAcceptLongValue()
    {
        // Arrange
        var credentials = new PackageCredentials();
        var longUsername = new string('a', 1000);

        // Act
        credentials.Username = longUsername;

        // Assert
        credentials.Username.Should().Be(longUsername);
        credentials.Username.Length.Should().Be(1000);
    }

    [Fact]
    public void PackageCredentials_SetLongPassword_ShouldAcceptLongValue()
    {
        // Arrange
        var credentials = new PackageCredentials();
        var longPassword = new string('b', 1000);

        // Act
        credentials.Password = longPassword;

        // Assert
        credentials.Password.Should().Be(longPassword);
        credentials.Password.Length.Should().Be(1000);
    }

    [Fact]
    public void PackageCredentials_SetSpecialCharacters_ShouldAcceptSpecialCharacters()
    {
        // Arrange
        var credentials = new PackageCredentials();

        // Act
        credentials.Username = "test@user.com";
        credentials.Password = "p@ssw0rd!#$%";

        // Assert
        credentials.Username.Should().Be("test@user.com");
        credentials.Password.Should().Be("p@ssw0rd!#$%");
    }

    [Fact]
    public void PackageCredentials_SetUnicodeCharacters_ShouldAcceptUnicodeCharacters()
    {
        // Arrange
        var credentials = new PackageCredentials();

        // Act
        credentials.Username = "тестовый_пользователь";
        credentials.Password = "密码测试";

        // Assert
        credentials.Username.Should().Be("тестовый_пользователь");
        credentials.Password.Should().Be("密码测试");
    }

    [Fact]
    public void PackageCredentials_SetEmptyMessageFormat_ShouldAcceptEmptyValue()
    {
        // Arrange
        var credentials = new PackageCredentials();

        // Act
        credentials.MessageFormat = "";

        // Assert
        credentials.MessageFormat.Should().BeEmpty();
    }

    [Fact]
    public void PackageCredentials_BusinessLogic_ShouldValidateCorrectly()
    {
        // Arrange
        var validCredentials = new PackageCredentials
        {
            PackageId = 123,
            Username = "user",
            Password = "pass"
        };

        var invalidCredentials = new PackageCredentials
        {
            PackageId = 0,
            Username = "",
            Password = ""
        };

        // Act & Assert
        var isValidCredentialsValid = validCredentials.PackageId > 0 && 
                                     !string.IsNullOrEmpty(validCredentials.Username) && 
                                     !string.IsNullOrEmpty(validCredentials.Password);

        var isInvalidCredentialsValid = invalidCredentials.PackageId > 0 && 
                                       !string.IsNullOrEmpty(invalidCredentials.Username) && 
                                       !string.IsNullOrEmpty(invalidCredentials.Password);

        isValidCredentialsValid.Should().BeTrue();
        isInvalidCredentialsValid.Should().BeFalse();
    }

    #endregion
} 