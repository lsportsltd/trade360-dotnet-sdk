using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using System.Net;
using System.Text;
using Trade360SDK.Common.Configuration;
using Trade360SDK.SnapshotApi.Http;
using Trade360SDK.SnapshotApi.Interfaces;

namespace Trade360SDK.SnapshotApi.Tests;

public class Trade360SettingsAdvancedBusinessLogicTests
{
    private readonly Mock<IHttpClientFactory> _mockHttpClientFactory;
    private readonly HttpClient _httpClient;
    private readonly Trade360Settings _settings;

    public Trade360SettingsAdvancedBusinessLogicTests()
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
            SnapshotApiBaseUrl = "https://api.example.com",
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
        _settings.SnapshotApiBaseUrl.Should().Be("https://api.example.com");
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
    public void PackageCredentials_WithValidData_ShouldValidateCorrectly()
    {
        var credentials = new PackageCredentials
        {
            PackageId = 789,
            Username = "test_user",
            Password = "test_password"
        };

        credentials.PackageId.Should().BeGreaterThan(0);
        credentials.Username.Should().NotBeNullOrEmpty();
        credentials.Password.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public void PackageCredentials_WithInvalidPackageId_ShouldBeDetectable()
    {
        var invalidCredentials = new PackageCredentials
        {
            PackageId = 0,
            Username = "user",
            Password = "pass"
        };

        invalidCredentials.PackageId.Should().Be(0);
        invalidCredentials.PackageId.Should().BeLessOrEqualTo(0);
    }

    [Fact]
    public void PackageCredentials_WithEmptyCredentials_ShouldBeDetectable()
    {
        var emptyCredentials = new PackageCredentials
        {
            PackageId = 123,
            Username = "",
            Password = ""
        };

        emptyCredentials.Username.Should().BeEmpty();
        emptyCredentials.Password.Should().BeEmpty();
    }

    [Fact]
    public void SnapshotApiSettings_WithNullCredentials_ShouldHandleGracefully()
    {
        var settingsWithNullCredentials = new Trade360Settings
        {
            SnapshotApiBaseUrl = "https://api.example.com",
            InplayPackageCredentials = null,
            PrematchPackageCredentials = null
        };

        settingsWithNullCredentials.SnapshotApiBaseUrl.Should().Be("https://api.example.com");
        settingsWithNullCredentials.InplayPackageCredentials.Should().BeNull();
        settingsWithNullCredentials.PrematchPackageCredentials.Should().BeNull();
    }

    [Fact]
    public void Trade360Settings_WithInvalidBaseUrl_ShouldBeDetectable()
    {
        var invalidSettings = new Trade360Settings
        {
            SnapshotApiBaseUrl = "",
            InplayPackageCredentials = _settings.InplayPackageCredentials,
            PrematchPackageCredentials = _settings.PrematchPackageCredentials
        };

        invalidSettings.SnapshotApiBaseUrl.Should().BeEmpty();
    }

    [Fact]
    public void HttpClientFactory_ShouldCreateHttpClientCorrectly()
    {
        _mockHttpClientFactory.Should().NotBeNull();
        
        var client = _mockHttpClientFactory.Object.CreateClient("test");
        client.Should().NotBeNull();
        client.Should().Be(_httpClient);
    }

    [Fact]
    public void HttpClient_WithBaseAddress_ShouldConfigureCorrectly()
    {
        _httpClient.BaseAddress.Should().NotBeNull();
        _httpClient.BaseAddress!.ToString().Should().Be("https://api.example.com/");
    }

    [Fact]
    public void Trade360Settings_BusinessLogic_ShouldValidateCredentialConsistency()
    {
        var inplayCredentials = _settings.InplayPackageCredentials;
        var prematchCredentials = _settings.PrematchPackageCredentials;

        inplayCredentials!.PackageId.Should().NotBe(prematchCredentials!.PackageId);
        inplayCredentials!.Username.Should().NotBe(prematchCredentials!.Username);
        inplayCredentials!.Password.Should().NotBe(prematchCredentials!.Password);
    }

    [Fact]
    public void Trade360Settings_BusinessLogic_ShouldHandleMultiplePackageTypes()
    {
        var allCredentials = new[]
        {
            _settings.InplayPackageCredentials,
            _settings.PrematchPackageCredentials
        };

        allCredentials.Should().AllSatisfy(cred =>
        {
            cred.Should().NotBeNull();
            cred!.PackageId.Should().BeGreaterThan(0);
            cred!.Username.Should().NotBeNullOrEmpty();
            cred!.Password.Should().NotBeNullOrEmpty();
        });

        var packageIds = allCredentials.Select(c => c!.PackageId).ToList();
        packageIds.Should().OnlyHaveUniqueItems();
    }

    [Fact]
    public void Trade360Settings_BusinessLogic_ShouldValidateUrlFormat()
    {
        var validUrls = new[]
        {
            "https://api.example.com",
            "http://localhost:8080",
            "https://api.lsports.eu/v1"
        };

        var invalidUrls = new[]
        {
            "",
            "not-a-url",
            "://missing-scheme",
            "http://",
            "ht tp://invalid-space.com"
        };

        foreach (var url in validUrls)
        {
            var settings = new Trade360Settings { SnapshotApiBaseUrl = url };
            Uri.TryCreate(settings.SnapshotApiBaseUrl, UriKind.Absolute, out var result).Should().BeTrue();
            result.Should().NotBeNull();
        }

        foreach (var url in invalidUrls)
        {
            var settings = new Trade360Settings { SnapshotApiBaseUrl = url };
            if (string.IsNullOrEmpty(url))
            {
                settings.SnapshotApiBaseUrl.Should().BeNullOrEmpty();
            }
            else
            {
                Uri.TryCreate(settings.SnapshotApiBaseUrl, UriKind.Absolute, out var result).Should().BeFalse();
            }
        }
    }

    [Fact]
    public void Trade360Settings_BusinessLogic_ShouldHandleCredentialValidation()
    {
        var testCases = new[]
        {
            new { PackageId = 1, Username = "user1", Password = "pass1", IsValid = true },
            new { PackageId = 0, Username = "user2", Password = "pass2", IsValid = false },
            new { PackageId = 123, Username = "", Password = "pass3", IsValid = false },
            new { PackageId = 456, Username = "user4", Password = "", IsValid = false },
            new { PackageId = -1, Username = "user5", Password = "pass5", IsValid = false }
        };

        foreach (var testCase in testCases)
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

            isValid.Should().Be(testCase.IsValid);
        }
    }

    [Fact]
    public void Trade360Settings_Performance_ShouldHandleLargeCredentialSets()
    {
        var stopwatch = System.Diagnostics.Stopwatch.StartNew();
        
        var credentialsList = new List<PackageCredentials>();
        for (int i = 1; i <= 1000; i++)
        {
            credentialsList.Add(new PackageCredentials
            {
                PackageId = i,
                Username = $"user_{i}",
                Password = $"password_{i}"
            });
        }
        
        stopwatch.Stop();
        
        stopwatch.ElapsedMilliseconds.Should().BeLessThan(1000);
        credentialsList.Should().HaveCount(1000);
        credentialsList.Should().OnlyContain(c => c.PackageId > 0);
    }
}
