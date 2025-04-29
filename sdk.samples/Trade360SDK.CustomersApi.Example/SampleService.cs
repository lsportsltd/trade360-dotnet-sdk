using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Trade360SDK.Common.Configuration;
using Trade360SDK.CustomersApi.Entities.MetadataApi.Requests;
using Trade360SDK.CustomersApi.Entities.SubscriptionApi.Requests;
using Trade360SDK.CustomersApi.Entities.SubscriptionApi.Responses;
using Trade360SDK.CustomersApi.Interfaces;
using Suspension = Trade360SDK.CustomersApi.Entities.SubscriptionApi.Requests.Suspension;
using System.ComponentModel;

namespace Trade360SDK.CustomersApi.Example
{
    public class SampleService : IHostedService
    {
        private readonly ILogger<SampleService> _logger;
        private readonly IMetadataHttpClient _prematchMetadataHttpClient;
        private readonly ISubscriptionHttpClient _prematchSubscriptionHttpClient;
        private readonly IPackageDistributionHttpClient _prematchPackageDistributionHttpClient;
        private readonly IMetadataHttpClient _inplayMetadataApiClient;
        private readonly ISubscriptionHttpClient _inplaySubscriptionHttpClient;
        private readonly IPackageDistributionHttpClient _inplayPackageDistributionHttpClient;
        private readonly Dictionary<MenuOption, Func<CancellationToken, Task>> _menuActions;

        public SampleService(ILogger<SampleService> logger, ICustomersApiFactory customersApiFactory, IOptionsMonitor<Trade360Settings> settingsMonitor)
        {
            var settings = settingsMonitor.CurrentValue;
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _prematchPackageDistributionHttpClient = customersApiFactory.CreatePackageDistributionHttpClient(settings.CustomersApiBaseUrl, settings.PrematchPackageCredentials);
            _prematchMetadataHttpClient = customersApiFactory.CreateMetadataHttpClient(settings.CustomersApiBaseUrl, settings.PrematchPackageCredentials);
            _prematchSubscriptionHttpClient = customersApiFactory.CreateSubscriptionHttpClient(settings.CustomersApiBaseUrl, settings.PrematchPackageCredentials);
            _inplayPackageDistributionHttpClient = customersApiFactory.CreatePackageDistributionHttpClient(settings.CustomersApiBaseUrl, settings.InplayPackageCredentials);
            _inplayMetadataApiClient = customersApiFactory.CreateMetadataHttpClient(settings.CustomersApiBaseUrl, settings.InplayPackageCredentials);
            _inplaySubscriptionHttpClient = customersApiFactory.CreateSubscriptionHttpClient(settings.CustomersApiBaseUrl, settings.InplayPackageCredentials);
            
            _menuActions = new Dictionary<MenuOption, Func<CancellationToken, Task>>
            {
                { MenuOption.GetFixtureMetadata, async ct => await GetFixtureMetadata(_prematchSubscriptionHttpClient, ct) },
                { MenuOption.GetCompetitions, async ct => await GetCompetitions(_prematchMetadataHttpClient, ct) },
                { MenuOption.GetTranslations, async ct => await GetTranslations(_prematchMetadataHttpClient, ct) },
                { MenuOption.GetMarkets, async ct => await GetMarkets(_prematchMetadataHttpClient, ct) },
                { MenuOption.GetSports, async ct => await GetSports(_prematchMetadataHttpClient, ct) },
                { MenuOption.GetLocations, async ct => await GetLocations(_prematchMetadataHttpClient, ct) },
                { MenuOption.GetLeagues, async ct => await GetLeagues(_prematchMetadataHttpClient, ct) },
                { MenuOption.SubscribeToFixture, async ct => await SubscribeToFixture(_prematchSubscriptionHttpClient, ct) },
                { MenuOption.UnsubscribeFromFixture, async ct => await UnsubscribeFromFixture(_prematchSubscriptionHttpClient, ct) },
                { MenuOption.SubscribeToLeague, async ct => await SubscribeToLeague(_prematchSubscriptionHttpClient, ct) },
                { MenuOption.UnsubscribeFromLeague, async ct => await UnsubscribeFromLeague(_prematchSubscriptionHttpClient, ct) },
                { MenuOption.GetSubscribedFixtures, async ct => await GetSubscribedFixtures(_prematchSubscriptionHttpClient, ct) },
                { MenuOption.SubscribeToOutrightCompetition, async ct => await SubscribeToOutrightCompetition(_prematchSubscriptionHttpClient, ct) },
                { MenuOption.UnsubscribeFromOutrightCompetition, async ct => await UnsubscribeFromOutrightCompetition(_prematchSubscriptionHttpClient, ct) },
                { MenuOption.GetInplayFixtureSchedule, async ct => await GetInplayFixtureSchedule(_inplaySubscriptionHttpClient, ct) },
                { MenuOption.GetAllManualSuspensions, async ct => await GetAllManualSuspensionsAsync(_prematchSubscriptionHttpClient, ct) },
                { MenuOption.AddManualSuspension, async ct => await AddManualSuspensionAsync(_prematchSubscriptionHttpClient, ct) },
                { MenuOption.RemoveManualSuspension, async ct => await RemoveManualSuspensionAsync(_prematchSubscriptionHttpClient, ct) },
                { MenuOption.GetPackageQuota, async ct => await GetPackageQuota(_inplaySubscriptionHttpClient, ct) },
                { MenuOption.GetDistributionStatus, async ct => await GetDistributionStatus(_prematchPackageDistributionHttpClient, ct) },
                { MenuOption.StartDistribution, async ct => await StartDistribution(_prematchPackageDistributionHttpClient, ct) },
                { MenuOption.GetIncidents, async ct => await GetIncidentsAsync(_prematchMetadataHttpClient, ct)}
            };
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

        // private void ShowMenu()
        // {
        //     Console.WriteLine("Select an option:");
        //     Console.WriteLine("1. Metadata API - Get Fixture Metadata");
        //     Console.WriteLine("2. Metadata API - Get Competitions");
        //     Console.WriteLine("3. Metadata API - Get Translations");
        //     Console.WriteLine("4. Metadata API - Get Markets");
        //     Console.WriteLine("5. Metadata API - Get Sports");
        //     Console.WriteLine("6. Metadata API - Get Locations");
        //     Console.WriteLine("7. Metadata API - Get Leagues");
        //     Console.WriteLine("8. Subscription API - Subscribe to Fixture");
        //     Console.WriteLine("9. Subscription API - Unsubscribe from Fixture");
        //     Console.WriteLine("10. Subscription API - Subscribe to League");
        //     Console.WriteLine("11. Subscription API - Unsubscribe from League");
        //     Console.WriteLine("12. Subscription API - Get Subscribed Fixtures");
        //     Console.WriteLine("13. Subscription API - Subscribe to Outright Competition"); //TODO: Double check models. + Outright league
        //     Console.WriteLine("14. Subscription API - Unsubscribe from Outright Competition"); //TODO: Double check models. + Outright league
        //     Console.WriteLine("15. Subscription API - Get Inplay Fixture Schedule");
        //     Console.WriteLine("16. Subscription API - Get All Manual Suspensions");
        //     Console.WriteLine("17. Subscription API - Add Manual Suspension");
        //     Console.WriteLine("18. Subscription API - Remove Manual Suspension");
        //     Console.WriteLine("19. Subscription API - Get Package Quota");
        //     Console.WriteLine("20. Package Distribution API - Get Distribution Status");
        //     Console.WriteLine("21. Package Distribution API - Start Distribution");
        //     Console.WriteLine("Type 'exit' to quit");
        // }
        
        private void ShowMenu()
        {
            Console.WriteLine("Select an option:");
            foreach (var option in _menuActions.Keys.OrderBy(k => (int)k))
            {
                Console.WriteLine($"{(int)option}. {GetEnumDescription(option)}");
            }
            Console.WriteLine("Type 'exit' to quit");
        }

        // private async Task HandleMenuChoice(string choice, CancellationToken cancellationToken)
        // {
        //     switch (choice)
        //     {
        //         case "1":
        //             await GetFixtureMetadata(_prematchSubscriptionHttpClient, cancellationToken);
        //             break;
        //         case "2":
        //             await GetCompetitions(_prematchMetadataHttpClient, cancellationToken);
        //             break;
        //         case "3":
        //             await GetTranslations(_prematchMetadataHttpClient, cancellationToken); 
        //             break;
        //         case "4":
        //             await GetMarkets(_prematchMetadataHttpClient, cancellationToken);
        //             break;
        //         case "5":
        //             await GetSports(_prematchMetadataHttpClient, cancellationToken);
        //             break;
        //         case "6":
        //             await GetLocations(_prematchMetadataHttpClient, cancellationToken);
        //             break;
        //         case "7":
        //             await GetLeagues(_prematchMetadataHttpClient, cancellationToken);
        //             break;
        //         case "8":
        //             await SubscribeToFixture(_prematchSubscriptionHttpClient, cancellationToken);
        //             break;
        //         case "9":
        //             await UnsubscribeFromFixture(_prematchSubscriptionHttpClient, cancellationToken);
        //             break;
        //         case "10":
        //             await SubscribeToLeague(_prematchSubscriptionHttpClient, cancellationToken);
        //             break;
        //         case "11":
        //             await UnsubscribeFromLeague(_prematchSubscriptionHttpClient, cancellationToken);
        //             break;
        //         case "12":
        //             await GetSubscribedFixtures(_prematchSubscriptionHttpClient, cancellationToken);
        //             break;
        //         case "13":
        //             await SubscribeToOutrightCompetition(_prematchSubscriptionHttpClient, cancellationToken);
        //             break;
        //         case "14":
        //             await UnsubscribeFromOutrightCompetition(_prematchSubscriptionHttpClient, cancellationToken);
        //             break;
        //         case "15":
        //             await GetInplayFixtureSchedule(_inplaySubscriptionHttpClient, cancellationToken);
        //             break;
        //         case "16":
        //             await GetAllManualSuspensionsAsync(_prematchSubscriptionHttpClient, cancellationToken);
        //             break;
        //         case "17":
        //             await AddManualSuspensionAsync(_prematchSubscriptionHttpClient, cancellationToken);
        //             break;
        //         case "18":
        //             await RemoveManualSuspensionAsync(_prematchSubscriptionHttpClient, cancellationToken);
        //             break;
        //         case "19":
        //             await GetPackageQuota(_inplaySubscriptionHttpClient, cancellationToken);
        //             break;
        //         case "20":
        //             await GetDistributionStatus(_prematchPackageDistributionHttpClient, cancellationToken);
        //             break;
        //         case "21":
        //             await StartDistribution(_prematchPackageDistributionHttpClient, cancellationToken);
        //             break;
        //         default:
        //             Console.WriteLine("Invalid choice. Please try again.");
        //             break;
        //     }
        // }
        
        private async Task HandleMenuChoice(string choice, CancellationToken cancellationToken)
        {
            if (int.TryParse(choice, out var numericChoice) && Enum.IsDefined(typeof(MenuOption), numericChoice))
            {
                var menuOption = (MenuOption)numericChoice;
                if (_menuActions.TryGetValue(menuOption, out var action))
                {
                    await action(cancellationToken);
                    return;
                }
            }

            Console.WriteLine("Invalid choice. Please try again.");
        }

        private async Task GetPackageQuota(ISubscriptionHttpClient subscriptionApiClient, CancellationToken cancellationToken)
        {
            await subscriptionApiClient.GetPackageQuotaAsync(cancellationToken);
            Console.WriteLine($"Send GetPackageQuota request.");
        }

        private async Task SubscribeToFixture(ISubscriptionHttpClient subscriptionApiClient, CancellationToken cancellationToken)
        {
            var request = new FixtureSubscriptionRequestDto
            {
                Fixtures = new[] { 12345 }
            };
            var response = await subscriptionApiClient.SubscribeByFixture(request, cancellationToken);
            Console.WriteLine($"Send subscription request to {response.Fixtures?.Count} fixtures");
        }

        private async Task UnsubscribeFromFixture(ISubscriptionHttpClient subscriptionApiClient, CancellationToken cancellationToken)
        {
            var request = new FixtureSubscriptionRequestDto
            {
                Fixtures = new[] { 12345 }
            };
            var response = await subscriptionApiClient.UnSubscribeByFixture(request, cancellationToken);
            Console.WriteLine($"Send unSubscription request to {response.Fixtures?.Count} fixtures");
        }

        private async Task SubscribeToLeague(ISubscriptionHttpClient subscriptionApiClient, CancellationToken cancellationToken)
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

        private async Task UnsubscribeFromLeague(ISubscriptionHttpClient subscriptionApiClient, CancellationToken cancellationToken)
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

        private async Task GetSubscribedFixtures(ISubscriptionHttpClient subscriptionApiClient, CancellationToken cancellationToken)
        {
            var request = new GetSubscriptionRequestDto
            {
                SportIds = new List<int>() { 6046 }
            };
            var response = await subscriptionApiClient.GetSubscriptions(request, cancellationToken);
            Console.WriteLine($"{response.Fixtures?.Count()} Subscribed fixtures retrieved.");
        }

        private async Task SubscribeToOutrightCompetition(ISubscriptionHttpClient subscriptionApiClient, CancellationToken cancellationToken)
        {
            var request = new CompetitionSubscriptionRequestDto
            {
                Subscriptions = new List<CompetitionSubscription>()
                {
                    new()
                    {
                        SportId = 687888
                    }
                }
            };
            var response = await subscriptionApiClient.SubscribeByCompetition(request, cancellationToken);
            Console.WriteLine($"{response.Subscription?.Count} Subscribed fixtures retrieved.");
        }

        private async Task UnsubscribeFromOutrightCompetition(ISubscriptionHttpClient subscriptionApiClient, CancellationToken cancellationToken)
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

        private async Task GetInplayFixtureSchedule(ISubscriptionHttpClient subscriptionApiClient, CancellationToken cancellationToken)
        {
            var request = new GetFixtureScheduleRequestDto
            {
                SportIds = new[] { 6046 }
            };
            var response = await subscriptionApiClient.GetInplayFixtureSchedule(request, cancellationToken);
            Console.WriteLine($"{response.Fixtures?.Count()} Fixture schedule retrieved.");
        }

        private async Task GetFixtureMetadata(ISubscriptionHttpClient subscriptionApiClient, CancellationToken cancellationToken)
        {
            var request = new GetFixtureMetadataRequestDto
            {
                FromDate = DateTime.Now,
                ToDate = DateTime.Now.AddDays(2)
            };
            var response = await subscriptionApiClient.GetFixtureMetadataAsync(request, cancellationToken);
            Console.WriteLine($"{response.SubscribedFixtures?.Count()} Fixture metadata retrieved.");
        }

        private async Task GetCompetitions(IMetadataHttpClient metadataApiClient, CancellationToken cancellationToken)
        {
            var request = new GetCompetitionsRequestDto
            {
                LocationIds = new[] { 1 },
                SubscriptionStatus = 0
            };
            var response = await metadataApiClient.GetCompetitionsAsync(request, cancellationToken);
            Console.WriteLine($"{response.Competitions?.Count()} Competitions retrieved.");
        }

        private async Task GetTranslations(IMetadataHttpClient metadataApiClient, CancellationToken cancellationToken)
        {
            var request = new GetTranslationsRequestDto
            {
                SportIds = new[] { 6046 },
                Languages = new[] { 4 }
            };
            var response = await metadataApiClient.GetTranslationsAsync(request, cancellationToken);
            Console.WriteLine($"Count of translations received Sports: {response.Sports?.Count}, Leagues: {response.Leagues?.Count}, Locations: {response.Locations?.Count} Translations retrieved.");
        }

        private async Task GetDistributionStatus(IPackageDistributionHttpClient packageDistributionApiClient, CancellationToken cancellationToken)
        {
            await packageDistributionApiClient.GetDistributionStatusAsync(cancellationToken);
            Console.WriteLine("Distribution status retrieved.");
        }

        private async Task StartDistribution(IPackageDistributionHttpClient packageDistributionApiClient, CancellationToken cancellationToken)
        {
            await packageDistributionApiClient.StartDistributionAsync(cancellationToken);
            Console.WriteLine("Distribution started.");
        }

        private async Task GetMarkets(IMetadataHttpClient metadataApiClient, CancellationToken cancellationToken)
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

        private async Task GetSports(IMetadataHttpClient metadataApiClient, CancellationToken cancellationToken)
        {
            var response = await metadataApiClient.GetSportsAsync(cancellationToken);
            Console.WriteLine("Sports entities received:");
            foreach (var sport in response)
            {
                Console.WriteLine($"SportId: {sport.Id}, SportName: {sport.Name}");
            }
        }

        private async Task GetLocations(IMetadataHttpClient metadataApiClient, CancellationToken cancellationToken)
        {
            var response = await metadataApiClient.GetLocationsAsync(cancellationToken);
            Console.WriteLine("Locations entities received:");
            foreach (var location in response)
            {
                Console.WriteLine($"LocationId: {location.Id}, LocationName: {location.Name}");
            }
        }

        private async Task GetLeagues(IMetadataHttpClient metadataApiClient, CancellationToken cancellationToken)
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

        private async Task GetAllManualSuspensionsAsync(ISubscriptionHttpClient subscriptionApiClient, CancellationToken cancellationToken)
        {
            var response = await subscriptionApiClient.GetAllManualSuspensions(cancellationToken);
            Console.WriteLine($"{response.Suspensions?.Count} Manual suspensions retrieved.");
        }

        private async Task AddManualSuspensionAsync(ISubscriptionHttpClient subscriptionApiClient, CancellationToken cancellationToken)
        {
            var request = new ChangeManualSuspensionRequestDto
            {
                Suspensions = new List<Suspension>()
                {
                    new()
                    {
                        FixtureId = 13176576,
                        Markets = new List<SuspendedMarket>
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

        private async Task RemoveManualSuspensionAsync(ISubscriptionHttpClient subscriptionApiClient, CancellationToken cancellationToken)
        {
            var request = new ChangeManualSuspensionRequestDto
            {
                Suspensions = new List<Suspension>()
                {
                    new()
                    {
                        FixtureId = 13176576,
                        Markets = new List<SuspendedMarket>
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

        private async Task GetIncidentsAsync(IMetadataHttpClient metadataApiClient,
            CancellationToken cancellationToken)
        {
            var request = new GetIncidentsRequestDto
            {
                Filters = new IncidentFilter
                {
                    Ids = null,
                    Sports = null,
                    CreationDate = null,
                    SearchText = null
                }
            };
            
            var response = await metadataApiClient.GetIncidentsAsync(request, cancellationToken);
            var incidentsList = response.ToList();
            Console.WriteLine($"Response returned {incidentsList.Count} Incidents:");
            foreach (var incident in incidentsList)
            {
                Console.WriteLine($"IncidentId: {incident.IncidentId}, LeagueName: {incident.IncidentName}");
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Service is stopping.");
            return Task.CompletedTask;
        }


        private enum MenuOption
        {
            [Description("Metadata API - Get Fixture Metadata")]
            GetFixtureMetadata = 1,

            [Description("Metadata API - Get Competitions")]
            GetCompetitions,

            [Description("Metadata API - Get Translations")]
            GetTranslations,

            [Description("Metadata API - Get Markets")]
            GetMarkets,

            [Description("Metadata API - Get Sports")]
            GetSports,

            [Description("Metadata API - Get Locations")]
            GetLocations,

            [Description("Metadata API - Get Leagues")]
            GetLeagues,
            
            [Description("Metadata API - Get Incidents")]
            GetIncidents,

            [Description("Subscription API - Subscribe to Fixture")]
            SubscribeToFixture,

            [Description("Subscription API - Unsubscribe from Fixture")]
            UnsubscribeFromFixture,

            [Description("Subscription API - Subscribe to League")]
            SubscribeToLeague,

            [Description("Subscription API - Unsubscribe from League")]
            UnsubscribeFromLeague,

            [Description("Subscription API - Get Subscribed Fixtures")]
            GetSubscribedFixtures,

            [Description("Subscription API - Subscribe to Outright Competition")]
            SubscribeToOutrightCompetition,

            [Description("Subscription API - Unsubscribe from Outright Competition")]
            UnsubscribeFromOutrightCompetition,

            [Description("Subscription API - Get Inplay Fixture Schedule")]
            GetInplayFixtureSchedule,

            [Description("Subscription API - Get All Manual Suspensions")]
            GetAllManualSuspensions,

            [Description("Subscription API - Add Manual Suspension")]
            AddManualSuspension,

            [Description("Subscription API - Remove Manual Suspension")]
            RemoveManualSuspension,

            [Description("Subscription API - Get Package Quota")]
            GetPackageQuota,

            [Description("Package Distribution API - Get Distribution Status")]
            GetDistributionStatus,

            [Description("Package Distribution API - Start Distribution")]
            StartDistribution
        }
        
        private static string GetEnumDescription(Enum value)
        {
            var field = value.GetType().GetField(value.ToString());
            if (field != null && Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) is DescriptionAttribute attribute)
            {
                return attribute.Description;
            }
            return value.ToString();
        }
    }
}
