﻿using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Trade360SDK.Common.Configuration;
using Trade360SDK.CustomersApi.Entities.MetadataApi.Requests;
using Trade360SDK.CustomersApi.Entities.SubscriptionApi.Requests;
using Trade360SDK.CustomersApi.Interfaces;

namespace Trade360SDK.CustomersApi.Example
{
    public class SampleService : IHostedService
    {
        private readonly ILogger<SampleService> _logger;
        private readonly IMetadataApiClient _inplayMetadataApiClient;
        private readonly ISubscriptionApiClient _inplaySubscriptionApiClient;
        private readonly IPackageDistributionApiClient _inplayPackageDistributionApiClient;

        public SampleService(ILogger<SampleService> logger, ICustomersApiFactory customersApiFactory, IOptionsMonitor<Trade360Settings> settingsMonitor)
        {
            var settings = settingsMonitor.CurrentValue;
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _inplayPackageDistributionApiClient = customersApiFactory.CreatePackageDistributionHttpClient(settings.CustomersApiBaseUrl, settings.InplayPackageCredentials);
            _inplayMetadataApiClient = customersApiFactory.CreateMetadataHttpClient(settings.CustomersApiBaseUrl, settings.InplayPackageCredentials);
            _inplaySubscriptionApiClient = customersApiFactory.CreateSubscriptionHttpClient(settings.CustomersApiBaseUrl, settings.InplayPackageCredentials);
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            try
            {
                while (true)
                {
                    ShowMenu();
                    var choice = Console.ReadLine();

                    if (choice is "exit") break;

                    if (choice != null)
                    {
                        await HandleMenuChoice(choice, cancellationToken);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving data");
            }
        }

        private void ShowMenu()
        {
            Console.WriteLine("Select an option:");
            Console.WriteLine("1. Metadata API - Get Fixture Metadata");
            Console.WriteLine("2. Metadata API - Get Competitions");
            Console.WriteLine("3. Metadata API - Get Translations");
            Console.WriteLine("4. Metadata API - Get Markets");
            Console.WriteLine("5. Metadata API - Get Sports");
            Console.WriteLine("6. Metadata API - Get Locations");
            Console.WriteLine("7. Metadata API - Get Leagues");
            Console.WriteLine("8. Subscription API - Subscribe to Fixture");
            Console.WriteLine("9. Subscription API - Unsubscribe from Fixture");
            Console.WriteLine("10. Subscription API - Subscribe to League");
            Console.WriteLine("11. Subscription API - Unsubscribe from League");
            Console.WriteLine("12. Subscription API - Get Subscribed Fixtures");
            Console.WriteLine("13. Subscription API - Subscribe to Outright Competition");
            Console.WriteLine("14. Subscription API - Unsubscribe from Outright Competition");
            Console.WriteLine("15. Subscription API - Get Inplay Fixture Schedule");
            Console.WriteLine("16. Subscription API - Get All Manual Suspensions");
            Console.WriteLine("17. Subscription API - Add Manual Suspension");
            Console.WriteLine("18. Subscription API - Remove Manual Suspension");
            Console.WriteLine("19. Package Distribution API - Get Distribution Status");
            Console.WriteLine("20. Package Distribution API - Start Distribution");
            Console.WriteLine("Type 'exit' to quit");
        }

        private async Task HandleMenuChoice(string choice, CancellationToken cancellationToken)
        {
            switch (choice)
            {
                case "1":
                    await GetFixtureMetadata(_inplayMetadataApiClient, cancellationToken);
                    break;
                case "2":
                    await GetCompetitions(_inplayMetadataApiClient, cancellationToken);
                    break;
                case "3":
                    await GetTranslations(_inplayMetadataApiClient, cancellationToken);
                    break;
                case "4":
                    await GetMarkets(_inplayMetadataApiClient, cancellationToken);
                    break;
                case "5":
                    await GetSports(_inplayMetadataApiClient, cancellationToken);
                    break;
                case "6":
                    await GetLocations(_inplayMetadataApiClient, cancellationToken);
                    break;
                case "7":
                    await GetLeagues(_inplayMetadataApiClient, cancellationToken);
                    break;
                case "8":
                    await SubscribeToFixture(_inplaySubscriptionApiClient, cancellationToken);
                    break;
                case "9":
                    await UnsubscribeFromFixture(_inplaySubscriptionApiClient, cancellationToken);
                    break;
                case "10":
                    await SubscribeToLeague(_inplaySubscriptionApiClient, cancellationToken);
                    break;
                case "11":
                    await UnsubscribeFromLeague(_inplaySubscriptionApiClient, cancellationToken);
                    break;
                case "12":
                    await GetSubscribedFixtures(_inplaySubscriptionApiClient, cancellationToken);
                    break;
                case "13":
                    await SubscribeToOutrightCompetition(_inplaySubscriptionApiClient, cancellationToken);
                    break;
                case "14":
                    await UnsubscribeFromOutrightCompetition(_inplaySubscriptionApiClient, cancellationToken);
                    break;
                case "15":
                    await GetInplayFixtureSchedule(_inplaySubscriptionApiClient, cancellationToken);
                    break;
                case "16":
                    await GetAllManualSuspensionsAsync(_inplaySubscriptionApiClient, cancellationToken);
                    break;
                case "17":
                    await AddManualSuspensionAsync(_inplaySubscriptionApiClient, cancellationToken);
                    break;
                case "18":
                    await RemoveManualSuspensionAsync(_inplaySubscriptionApiClient, cancellationToken);
                    break;
                case "19":
                    await GetDistributionStatus(_inplayPackageDistributionApiClient, cancellationToken);
                    break;
                case "20":
                    await StartDistribution(_inplayPackageDistributionApiClient, cancellationToken);
                    break;
                default:
                    Console.WriteLine("Invalid choice. Please try again.");
                    break;
            }
        }

        private async Task SubscribeToFixture(ISubscriptionApiClient subscriptionApiClient, CancellationToken cancellationToken)
        {
            var request = new FixtureSubscriptionRequestDto
            {
                Fixtures = new[] { 12345 }
            };
            var response = await subscriptionApiClient.SubscribeByFixture(request, cancellationToken);
            Console.WriteLine($"Send subscription request to {response.Fixtures?.Count} fixtures");
        }

        private async Task UnsubscribeFromFixture(ISubscriptionApiClient subscriptionApiClient, CancellationToken cancellationToken)
        {
            var request = new FixtureSubscriptionRequestDto
            {
                Fixtures = new[] { 12345 }
            };
            var response = await subscriptionApiClient.UnSubscribeByFixture(request, cancellationToken);
            Console.WriteLine($"Send unSubscription request to {response.Fixtures?.Count} fixtures");
        }

        private async Task SubscribeToLeague(ISubscriptionApiClient subscriptionApiClient, CancellationToken cancellationToken)
        {
            var request = new LeagueSubscriptionRequestDto
            {
                Subscriptions = new List<LeagueSubscription>()
                {
                    new()
                    {
                        SportId = 6046,
                        LocationId = 142,
                        LeagueId = 5
                    }
                }
            };
            var response = await subscriptionApiClient.SubscribeByLeague(request, cancellationToken);
            Console.WriteLine($"Send Subscription request to {response.Subscription?.Count} fixtures");
        }

        private async Task UnsubscribeFromLeague(ISubscriptionApiClient subscriptionApiClient, CancellationToken cancellationToken)
        {
            var request = new LeagueSubscriptionRequestDto
            {
                Subscriptions = new List<LeagueSubscription>()
                {
                    new()
                    {
                        SportId = 6046,
                        LocationId = 142,
                        LeagueId = 5
                    }
                }
            };
            var response = await subscriptionApiClient.UnSubscribeByLeague(request, cancellationToken);
            Console.WriteLine($"Send UnSubscription request to {response.Subscription?.Count} fixtures");
        }

        private async Task GetSubscribedFixtures(ISubscriptionApiClient subscriptionApiClient, CancellationToken cancellationToken)
        {
            var request = new GetSubscriptionRequestDto
            {
                SportIds = new List<int>() { 6046 }
            };
            var response = await subscriptionApiClient.GetSubscriptions(request, cancellationToken);
            Console.WriteLine($"{response.Fixtures?.Count()} Subscribed fixtures retrieved.");
        }

        private async Task SubscribeToOutrightCompetition(ISubscriptionApiClient subscriptionApiClient, CancellationToken cancellationToken)
        {
            var request = new CompetitionSubscriptionRequestDto
            {
                Subscriptions = new List<CompetitionSubscription>()
                {
                    new()
                    {
                        SportId = 6046
                    }
                }
            };
            var response = await subscriptionApiClient.SubscribeByCompetition(request, cancellationToken);
            Console.WriteLine($"{response.Subscription?.Count} Subscribed fixtures retrieved.");
        }

        private async Task UnsubscribeFromOutrightCompetition(ISubscriptionApiClient subscriptionApiClient, CancellationToken cancellationToken)
        {
            var request = new CompetitionSubscriptionRequestDto
            {
                Subscriptions = new List<CompetitionSubscription>()
                {
                    new()
                    {
                        SportId = 6046
                    }
                }
            };
            var response = await subscriptionApiClient.UnSubscribeByCompetition(request, cancellationToken);
            Console.WriteLine($"{response.Subscription?.Count} Competition unsubscribed.");
        }

        private async Task GetInplayFixtureSchedule(ISubscriptionApiClient subscriptionApiClient, CancellationToken cancellationToken)
        {
            var request = new GetFixtureScheduleRequestDto
            {
                SportIds = new[] { 6046 }
            };
            var response = await subscriptionApiClient.GetInplayFixtureSchedule(request, cancellationToken);
            Console.WriteLine($"{response.Fixtures?.Count()} Fixture schedule retrieved.");
        }

        private async Task GetFixtureMetadata(IMetadataApiClient metadataApiClient, CancellationToken cancellationToken)
        {
            var request = new GetFixtureMetadataRequestDto
            {
                FromDate = DateTime.Now,
                ToDate = DateTime.Now.AddDays(2)
            };
            var response = await metadataApiClient.GetFixtureMetadataAsync(request, cancellationToken);
            Console.WriteLine($"{response.SubscribedFixtures?.Count()} Fixture metadata retrieved.");
        }

        private async Task GetCompetitions(IMetadataApiClient metadataApiClient, CancellationToken cancellationToken)
        {
            var request = new GetCompetitionsRequestDto
            {
                LocationIds = new[] { 1 },
                SubscriptionStatus = 0
            };
            var response = await metadataApiClient.GetCompetitionsAsync(request, cancellationToken);
            Console.WriteLine($"{response.Competitions?.Count()} Competitions retrieved.");
        }

        private async Task GetTranslations(IMetadataApiClient metadataApiClient, CancellationToken cancellationToken)
        {
            var request = new GetTranslationsRequestDto
            {
                SportIds = new[] { 6046 },
                Languages = new[] { 4 }
            };
            var response = await metadataApiClient.GetTranslationsAsync(request, cancellationToken);
            Console.WriteLine($"Count of translations received Sports: {response.Sports?.Count}, Leagues: {response.Leagues?.Count}, Locations: {response.Locations?.Count} Translations retrieved.");
        }

        private async Task GetDistributionStatus(IPackageDistributionApiClient packageDistributionApiClient, CancellationToken cancellationToken)
        {
            await packageDistributionApiClient.GetDistributionStatusAsync(cancellationToken);
            Console.WriteLine("Distribution status retrieved.");
        }

        private async Task StartDistribution(IPackageDistributionApiClient packageDistributionApiClient, CancellationToken cancellationToken)
        {
            await packageDistributionApiClient.StartDistributionAsync(cancellationToken);
            Console.WriteLine("Distribution started.");
        }

        private async Task GetMarkets(IMetadataApiClient metadataApiClient, CancellationToken cancellationToken)
        {
            var request = new GetMarketsRequestDto
            {
                MarketIds = new List<int> { 1, 2 },
                IsSettleable = true,
                SportIds = new List<int> { 6046 }
            };
            var response = await metadataApiClient.GetMarketsAsync(request, cancellationToken);
            Console.WriteLine("Markets received:");
            foreach (var market in response)
            {
                Console.WriteLine($"MarketId: {market.Id}, MarketName: {market.Name}");
            }
        }

        private async Task GetSports(IMetadataApiClient metadataApiClient, CancellationToken cancellationToken)
        {
            var response = await metadataApiClient.GetSportsAsync(cancellationToken);
            Console.WriteLine("Sports entities received:");
            foreach (var sport in response)
            {
                Console.WriteLine($"SportId: {sport.Id}, SportName: {sport.Name}");
            }
        }

        private async Task GetLocations(IMetadataApiClient metadataApiClient, CancellationToken cancellationToken)
        {
            var response = await metadataApiClient.GetLocationsAsync(cancellationToken);
            Console.WriteLine("Locations entities received:");
            foreach (var location in response)
            {
                Console.WriteLine($"LocationId: {location.Id}, LocationName: {location.Name}");
            }
        }

        private async Task GetLeagues(IMetadataApiClient metadataApiClient, CancellationToken cancellationToken)
        {
            var sportsResults = await metadataApiClient.GetSportsAsync(cancellationToken);
            var footballSportEntity = sportsResults.FirstOrDefault(x => x.Name == "Football");
            if (footballSportEntity == null)
            {
                Console.WriteLine("Football sport entity not found.");
                return;
            }

            var request = new GetLeaguesRequestDto
            {
                SportIds = new List<int> { footballSportEntity.Id }
            };
            var response = await metadataApiClient.GetLeaguesAsync(request, cancellationToken);
            var enumerable = response.ToList();
            Console.WriteLine($"Response returned {enumerable.Count} leagues:");
            foreach (var league in enumerable)
            {
                Console.WriteLine($"LeagueId: {league.Id}, LeagueName: {league.Name}");
            }
        }

        private async Task GetAllManualSuspensionsAsync(ISubscriptionApiClient subscriptionApiClient, CancellationToken cancellationToken)
        {
            var response = await subscriptionApiClient.GetAllManualSuspensions(cancellationToken);
            Console.WriteLine($"{response.Suspensions?.Count} Manual suspensions retrieved.");
        }

        private async Task AddManualSuspensionAsync(ISubscriptionApiClient subscriptionApiClient, CancellationToken cancellationToken)
        {
            var request = new ChangeManualSuspensionRequestDto
            {
                Suspensions = new List<Suspension>()
                {
                    new()
                    {
                        FixtureId = 13176576,
                        Markets = new List<Market>
                        {
                           new()
                           {
                               MarketId = 2755,
                               Line = "-0.25"
                           }
                        }
                    }
                }
            };
            var response = await subscriptionApiClient.AddManualSuspension(request, cancellationToken);
            Console.WriteLine($"{response.Suspensions?.Count} Manual suspension added.");
        }

        private async Task RemoveManualSuspensionAsync(ISubscriptionApiClient subscriptionApiClient, CancellationToken cancellationToken)
        {
            var request = new ChangeManualSuspensionRequestDto
            {
                Suspensions = new List<Suspension>()
                {
                    new()
                    {
                        FixtureId = 13176576,
                        Markets = new List<Market>
                        {
                           new()
                           {
                               MarketId = 1439,
                               Line = "-2.5"
                           }
                        }
                    }
                }
            };
            var response = await subscriptionApiClient.RemoveManualSuspension(request, cancellationToken);
            Console.WriteLine($"{response.Suspensions?.Count} Manual suspension removed.");
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Service is stopping.");
            return Task.CompletedTask;
        }
    }
}