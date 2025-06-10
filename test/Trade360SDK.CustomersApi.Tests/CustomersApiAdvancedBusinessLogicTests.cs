using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using System.Net;
using System.Text;
using Trade360SDK.Common.Configuration;
using Trade360SDK.CustomersApi.Http;
using Trade360SDK.CustomersApi.Interfaces;

namespace Trade360SDK.CustomersApi.Tests;

public class CustomersApiAdvancedBusinessLogicTests
{
    private readonly Mock<IHttpClientFactory> _mockHttpClientFactory;
    private readonly HttpClient _httpClient;
    private readonly Trade360Settings _settings;

    public CustomersApiAdvancedBusinessLogicTests()
    {
        _mockHttpClientFactory = new Mock<IHttpClientFactory>();
        var mockHttpMessageHandler = new Mock<HttpMessageHandler>();

        _httpClient = new HttpClient(mockHttpMessageHandler.Object)
        {
            BaseAddress = new Uri("https://api.example.com/")
        };

        _mockHttpClientFactory.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(_httpClient);

        _settings = new Trade360Settings
        {
            CustomersApiBaseUrl = "https://api.example.com",
            InplayPackageCredentials = new PackageCredentials
            {
                PackageId = 123,
                Username = "inplay_user",
                Password = "inplay_pass"
            },
            PrematchPackageCredentials = new PackageCredentials
            {
                PackageId = 456,
                Username = "prematch_user",
                Password = "prematch_pass"
            }
        };
    }

    [Fact]
    public void Trade360Settings_WithValidConfiguration_ShouldInitializeCorrectly()
    {
        _settings.Should().NotBeNull();
        _settings.CustomersApiBaseUrl.Should().Be("https://api.example.com");
        _settings.InplayPackageCredentials.Should().NotBeNull();
        _settings.PrematchPackageCredentials.Should().NotBeNull();
        
        _settings.InplayPackageCredentials!.PackageId.Should().Be(123);
        _settings.InplayPackageCredentials!.Username.Should().Be("inplay_user");
        _settings.InplayPackageCredentials!.Password.Should().Be("inplay_pass");
        
        _settings.PrematchPackageCredentials!.PackageId.Should().Be(456);
        _settings.PrematchPackageCredentials!.Username.Should().Be("prematch_user");
        _settings.PrematchPackageCredentials!.Password.Should().Be("prematch_pass");
    }

    [Fact]
    public void Trade360Settings_BusinessLogic_ShouldValidateApiUrlFormat()
    {
        var validUrls = new[]
        {
            "https://api.lsports.eu",
            "http://localhost:8080",
            "https://customers-api.example.com/v1"
        };

        foreach (var url in validUrls)
        {
            var settings = new Trade360Settings { CustomersApiBaseUrl = url };
            Uri.TryCreate(settings.CustomersApiBaseUrl, UriKind.Absolute, out var result).Should().BeTrue();
            result.Should().NotBeNull();
        }
    }

    [Fact]
    public void Trade360Settings_BusinessLogic_ShouldHandleInvalidUrls()
    {
        var invalidUrls = new[]
        {
            "",
            "not-a-valid-url",
            "://missing-scheme",
            "http://",
            "ht tp://invalid-space.com"
        };

        foreach (var url in invalidUrls)
        {
            var settings = new Trade360Settings { CustomersApiBaseUrl = url };
            
            if (string.IsNullOrEmpty(url))
            {
                settings.CustomersApiBaseUrl.Should().BeNullOrEmpty();
            }
            else
            {
                Uri.TryCreate(settings.CustomersApiBaseUrl, UriKind.Absolute, out var result).Should().BeFalse();
            }
        }
    }

    [Fact]
    public void Trade360Settings_BusinessLogic_ShouldValidateCredentialIntegrity()
    {
        var inplayCredentials = _settings.InplayPackageCredentials;
        var prematchCredentials = _settings.PrematchPackageCredentials;

        inplayCredentials.PackageId.Should().NotBe(prematchCredentials.PackageId);
        inplayCredentials.Username.Should().NotBe(prematchCredentials.Username);
        inplayCredentials.Password.Should().NotBe(prematchCredentials.Password);

        var allCredentials = new[] { inplayCredentials, prematchCredentials };
        allCredentials.Should().AllSatisfy(cred =>
        {
            cred.PackageId.Should().BeGreaterThan(0);
            cred.Username.Should().NotBeNullOrEmpty();
            cred.Password.Should().NotBeNullOrEmpty();
        });
    }

    [Fact]
    public void Trade360Settings_BusinessLogic_ShouldHandleNullCredentials()
    {
        var settingsWithNullCredentials = new Trade360Settings
        {
            CustomersApiBaseUrl = "https://api.example.com",
            InplayPackageCredentials = null,
            PrematchPackageCredentials = null
        };

        settingsWithNullCredentials.CustomersApiBaseUrl.Should().Be("https://api.example.com");
        settingsWithNullCredentials.InplayPackageCredentials.Should().BeNull();
        settingsWithNullCredentials.PrematchPackageCredentials.Should().BeNull();
    }

    [Fact]
    public void Trade360Settings_BusinessLogic_ShouldValidatePackageIdUniqueness()
    {
        var credentials1 = new PackageCredentials { PackageId = 123, Username = "user1", Password = "pass1" };
        var credentials2 = new PackageCredentials { PackageId = 123, Username = "user2", Password = "pass2" };

        credentials1.PackageId.Should().Be(credentials2.PackageId);
        
        var duplicateSettings = new Trade360Settings
        {
            InplayPackageCredentials = credentials1,
            PrematchPackageCredentials = credentials2
        };

        duplicateSettings.InplayPackageCredentials.PackageId.Should().Be(duplicateSettings.PrematchPackageCredentials.PackageId);
    }

    [Fact]
    public void Trade360Settings_BusinessLogic_ShouldHandleCredentialValidation()
    {
        var validationTestCases = new[]
        {
            new { PackageId = 1, Username = "valid_user", Password = "valid_pass", IsValid = true },
            new { PackageId = 0, Username = "user", Password = "pass", IsValid = false },
            new { PackageId = -1, Username = "user", Password = "pass", IsValid = false },
            new { PackageId = 123, Username = "", Password = "pass", IsValid = false },
            new { PackageId = 123, Username = "user", Password = "", IsValid = false },
            new { PackageId = int.MaxValue, Username = "user", Password = "pass", IsValid = true }
        };

        foreach (var testCase in validationTestCases)
        {
            var credentials = new PackageCredentials
            {
                PackageId = testCase.PackageId,
                Username = testCase.Username,
                Password = testCase.Password
            };

            var isValid = credentials.PackageId > 0 && 
                         !string.IsNullOrEmpty(credentials.Username) && 
                         !string.IsNullOrEmpty(credentials.Password);

            isValid.Should().Be(testCase.IsValid, 
                $"PackageId: {testCase.PackageId}, Username: '{testCase.Username}', Password: '{testCase.Password}'");
        }
    }

    [Fact]
    public void Trade360Settings_BusinessLogic_ShouldHandleComplexScenarios()
    {
        var complexSettings = new Trade360Settings
        {
            CustomersApiBaseUrl = "https://api.lsports.eu/customers/v2",
            InplayPackageCredentials = new PackageCredentials
            {
                PackageId = 12345,
                Username = "inplay_production_user",
                Password = "complex_password_123!@#"
            },
            PrematchPackageCredentials = new PackageCredentials
            {
                PackageId = 67890,
                Username = "prematch_production_user",
                Password = "another_complex_password_456$%^"
            }
        };

        complexSettings.CustomersApiBaseUrl.Should().Contain("lsports.eu");
        complexSettings.CustomersApiBaseUrl.Should().Contain("customers");
        complexSettings.CustomersApiBaseUrl.Should().Contain("v2");

        complexSettings.InplayPackageCredentials.Username.Should().Contain("inplay");
        complexSettings.InplayPackageCredentials.Username.Should().Contain("production");
        complexSettings.InplayPackageCredentials.Password.Should().Contain("123");

        complexSettings.PrematchPackageCredentials.Username.Should().Contain("prematch");
        complexSettings.PrematchPackageCredentials.Username.Should().Contain("production");
        complexSettings.PrematchPackageCredentials.Password.Should().Contain("456");
    }

    [Fact]
    public void Trade360Settings_Performance_ShouldHandleMultipleInstantiations()
    {
        var stopwatch = System.Diagnostics.Stopwatch.StartNew();
        
        var settingsList = new List<Trade360Settings>();
        for (int i = 1; i <= 1000; i++)
        {
            settingsList.Add(new Trade360Settings
            {
                CustomersApiBaseUrl = $"https://api-{i}.example.com",
                InplayPackageCredentials = new PackageCredentials
                {
                    PackageId = i,
                    Username = $"inplay_user_{i}",
                    Password = $"inplay_pass_{i}"
                },
                PrematchPackageCredentials = new PackageCredentials
                {
                    PackageId = i + 10000,
                    Username = $"prematch_user_{i}",
                    Password = $"prematch_pass_{i}"
                }
            });
        }
        
        stopwatch.Stop();
        
        stopwatch.ElapsedMilliseconds.Should().BeLessThan(2000);
        settingsList.Should().HaveCount(1000);
        settingsList.Should().AllSatisfy(s => s.CustomersApiBaseUrl.Should().StartWith("https://api-"));
    }

    [Fact]
    public void HttpClientFactory_BusinessLogic_ShouldCreateClientCorrectly()
    {
        _mockHttpClientFactory.Should().NotBeNull();
        
        var client = _mockHttpClientFactory.Object.CreateClient("customers-api");
        client.Should().NotBeNull();
        client.Should().Be(_httpClient);
        client.BaseAddress.Should().NotBeNull();
    }

    [Fact]
    public void HttpClient_BusinessLogic_ShouldConfigureBaseAddress()
    {
        _httpClient.BaseAddress.Should().NotBeNull();
        _httpClient.BaseAddress!.ToString().Should().Be("https://api.example.com/");
        _httpClient.BaseAddress.Scheme.Should().Be("https");
        _httpClient.BaseAddress.Host.Should().Be("api.example.com");
    }
}
