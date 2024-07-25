using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Trade360SDK.CustomersApi.Configuration;
using Trade360SDK.CustomersApi.Entities.MetadataApi.Requests;
using Trade360SDK.CustomersApi.Entities.SubscriptionApi.Requests;
using Trade360SDK.CustomersApi.Interfaces;

namespace Trade360SDK.CustomersApi.Example
{
    public class Startup : IHostedService
    {
        private readonly ILogger<Startup> _logger;
        private readonly ICustomersApiFactory _customerApiFactory;
        private readonly IOptionsMonitor<CustomersApiSettings> _settingsMonitor;

        public Startup(ILogger<Startup> logger, ICustomersApiFactory customersApiFactory, IOptionsMonitor<CustomersApiSettings> settingsMonitor)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _customerApiFactory = customersApiFactory;
            _settingsMonitor = settingsMonitor;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            try
            {
                // Retrieve the API settings for in-play data from the configuration.
                // You can initialize settings for pre-match type as well, create using the factory more instances for different types
                var customersApiSettings = _settingsMonitor.Get("CustomersApiInplaySettings");
                //Initialize Api Clients (Metadata, PackageDistribution, Subscription)
                var packageDistributionApiClient = _customerApiFactory.CreatePackageDistributionHttpClient(customersApiSettings);
                var metadataApiClient = _customerApiFactory.CreateMetadataHttpClient(customersApiSettings);
                var subscriptionApiClient = _customerApiFactory.CreateSubscriptionHttpClient(customersApiSettings);


                //Subscription Api Examples
                
                await SubscribeToFixture(subscriptionApiClient, cancellationToken);
                await UnsubscribeFromFixture(subscriptionApiClient, cancellationToken);
                await SubscribeToLeague(subscriptionApiClient, cancellationToken);
                await UnsubscribeFromLeague(subscriptionApiClient, cancellationToken);
                await GetInplayFixtureSchedule(subscriptionApiClient, cancellationToken);
                await SubscribeToOutrightCompetition(subscriptionApiClient, cancellationToken);
                await UnsubscribeFromOutrightCompetition(subscriptionApiClient, cancellationToken);
                await GetSubscribedFixtures(subscriptionApiClient, cancellationToken);

                await GetAllManualSuspensionsAsync(subscriptionApiClient, cancellationToken);
                await AddManualSuspensionAsync(subscriptionApiClient, cancellationToken);
                await RemoveManualSuspensionAsync(subscriptionApiClient, cancellationToken);

                //Metadata api examples
                await GetFixtureMetadata(metadataApiClient, cancellationToken);
                await GetCompetitions(metadataApiClient, cancellationToken);
                await GetTranslations(metadataApiClient, cancellationToken);
                await GetDistributionStatus(packageDistributionApiClient, cancellationToken);
                await StartDistribution(packageDistributionApiClient, cancellationToken);
                await GetMarkets(metadataApiClient, cancellationToken);
                await GetSports(metadataApiClient, cancellationToken);
                await GetLocations(metadataApiClient, cancellationToken);
                await GetLeagues(metadataApiClient, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving data");
            }
        }

        private async Task SubscribeToFixture(ISubscriptionApiClient subscriptionApiClient, CancellationToken cancellationToken)
        {
            Console.WriteLine("Subscribing to fixture. Press any key to continue.");
            Console.ReadKey();

            var request = new FixtureSubscriptionRequestDto
            {
                Fixtures = new[] { 12345 }
            };
            var response = await subscriptionApiClient.SubscribeByFixture(request, cancellationToken);
            Console.WriteLine($"Send subscription request to {response.Fixtures.Count} fixtures");
        }

        private async Task UnsubscribeFromFixture(ISubscriptionApiClient subscriptionApiClient, CancellationToken cancellationToken)
        {
            Console.WriteLine("Unsubscribing from fixture. Press any key to continue.");
            Console.ReadKey();

            var request = new FixtureSubscriptionRequestDto
            {
                Fixtures = new[] { 12345 }
            };
            var response = await subscriptionApiClient.UnSubscribeByFixture(request, cancellationToken);
            Console.WriteLine($"Send unSubscription request to {response.Fixtures.Count} fixtures");
        }

        private async Task SubscribeToLeague(ISubscriptionApiClient subscriptionApiClient, CancellationToken cancellationToken)
        {
            Console.WriteLine("Subscribing to league. Press any key to continue.");
            Console.ReadKey();

            var request = new LeagueSubscriptionRequestDto
            {
                Subscriptions = new List<LeagueSubscription>()
                {
                    new LeagueSubscription()
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
            Console.WriteLine("Unsubscribing from league. Press any key to continue.");
            Console.ReadKey();

            var request = new LeagueSubscriptionRequestDto
            {
                Subscriptions = new List<LeagueSubscription>()
                {
                    new LeagueSubscription()
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
            Console.WriteLine("Getting subscribed fixtures. Press any key to continue.");
            Console.ReadKey();

            var request = new GetSubscriptionRequestDto
            {
                SportIds = new List<int>() { 6046 }
            };
            var response = await subscriptionApiClient.GetSubscriptions(request, cancellationToken);
            Console.WriteLine("Subscribed fixtures retrieved.");
        }

        private async Task SubscribeToOutrightCompetition(ISubscriptionApiClient subscriptionApiClient, CancellationToken cancellationToken)
        {
            Console.WriteLine("Subscribing to competition. Press any key to continue.");
            Console.ReadKey();

            var request = new CompetitionSubscriptionRequestDto
            {
                Subscriptions = new List<CompetitionSubscription>()
                {
                    new CompetitionSubscription()
                    {
                        SportId = 6046
                    }
                }
            };
            var response = await subscriptionApiClient.SubscribeByCompetition(request, cancellationToken);
            Console.WriteLine("Competition subscribed.");
        }

        private async Task UnsubscribeFromOutrightCompetition(ISubscriptionApiClient subscriptionApiClient, CancellationToken cancellationToken)
        {
            Console.WriteLine("Unsubscribing from competition. Press any key to continue.");
            Console.ReadKey();

            var request = new CompetitionSubscriptionRequestDto
            {
                Subscriptions = new List<CompetitionSubscription>()
                {
                    new CompetitionSubscription()
                    {
                        SportId = 6046
                    }
                }
            };
            var response = await subscriptionApiClient.UnSubscribeByCompetition(request, cancellationToken);
            Console.WriteLine("Competition unsubscribed.");
        }


        private async Task GetInplayFixtureSchedule(ISubscriptionApiClient subscriptionApiClient, CancellationToken cancellationToken)
        {
            Console.WriteLine("Get fixture schedule response");
            Console.ReadKey();

            var request = new GetFixtureScheduleRequestDto
            {
                SportIds = new[] { 6046 }
            };
            var response = await subscriptionApiClient.GetInplayFixtureSchedule(request, cancellationToken);
            Console.WriteLine("Fixture schedule retrieved.");
        }

        private async Task GetFixtureMetadata(IMetadataApiClient metadataApiClient, CancellationToken cancellationToken)
        {
            Console.WriteLine("Get fixture metadata");
            Console.ReadKey();

            var request = new GetFixtureMetadataRequestDto
            {
                FromDate = DateTime.Now,
                ToDate = DateTime.Now.AddDays(2)
            };
            var response = await metadataApiClient.GetFixtureMetadataAsync(request, cancellationToken);
            Console.WriteLine("Fixture metadata retrieved.");
        }

        private async Task GetCompetitions(IMetadataApiClient metadataApiClient, CancellationToken cancellationToken)
        {
            Console.WriteLine("Get competitions");
            Console.ReadKey();

            var request = new GetCompetitionsRequestDto
            {
                LocationIds = new[] { 1 },
                SubscriptionStatus = 0
            };
            var response = await metadataApiClient.GetCompetitionsAsync(request, cancellationToken);
            Console.WriteLine("Competitions retrieved.");
        }

        private async Task GetTranslations(IMetadataApiClient metadataApiClient, CancellationToken cancellationToken)
        {
            Console.WriteLine("Starting translations");
            Console.ReadKey();

            var request = new GetTranslationsRequestDto
            {
                SportIds = new[] { 6046 },
                Languages = new[] { 4 }
            };
            var response = await metadataApiClient.GetTranslationsAsync(request, cancellationToken);
            Console.WriteLine("Translations retrieved.");
        }

        private async Task GetDistributionStatus(IPackageDistributionApiClient packageDistributionApiClient, CancellationToken cancellationToken)
        {
            Console.WriteLine("Get distribution status");
            Console.ReadKey();

            var response = await packageDistributionApiClient.GetDistributionStatusAsync(cancellationToken);
            Console.WriteLine("Distribution status retrieved.");
        }

        private async Task StartDistribution(IPackageDistributionApiClient packageDistributionApiClient, CancellationToken cancellationToken)
        {
            Console.WriteLine("Starting distribution");
            Console.ReadKey();

            var response = await packageDistributionApiClient.StartDistributionAsync(cancellationToken);
            Console.WriteLine("Distribution started.");
        }

        private async Task GetMarkets(IMetadataApiClient metadataApiClient, CancellationToken cancellationToken)
        {
            Console.WriteLine("Press any key to receive LSports market entities");
            Console.ReadKey();

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
            Console.WriteLine("Press any key to receive LSports sports entities");
            Console.ReadKey();

            var response = await metadataApiClient.GetSportsAsync(cancellationToken);
            Console.WriteLine("Sports entities received:");
            foreach (var sport in response)
            {
                Console.WriteLine($"SportId: {sport.Id}, SportName: {sport.Name}");
            }
        }

        private async Task GetLocations(IMetadataApiClient metadataApiClient, CancellationToken cancellationToken)
        {
            Console.WriteLine("Press any key to receive LSports locations entities");
            Console.ReadKey();

            var response = await metadataApiClient.GetLocationsAsync(cancellationToken);
            Console.WriteLine("Locations entities received:");
            foreach (var location in response)
            {
                Console.WriteLine($"LocationId: {location.Id}, LocationName: {location.Name}");
            }
        }

        private async Task GetLeagues(IMetadataApiClient metadataApiClient, CancellationToken cancellationToken)
        {
            Console.WriteLine("Press any key to receive LSports leagues for football");
            Console.ReadKey();

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
            Console.WriteLine($"Response returned {response.Count()} leagues:");
            foreach (var league in response)
            {
                Console.WriteLine($"LeagueId: {league.Id}, LeagueName: {league.Name}");
            }
        }

        private async Task GetAllManualSuspensionsAsync(ISubscriptionApiClient subscriptionApiClient, CancellationToken cancellationToken)
        {
            Console.WriteLine("Getting all manual suspensions. Press any key to continue.");
            Console.ReadKey();

            var response = await subscriptionApiClient.GetAllManualSuspensions(cancellationToken);
            Console.WriteLine("Manual suspensions retrieved.");
        }

        private async Task AddManualSuspensionAsync(ISubscriptionApiClient subscriptionApiClient, CancellationToken cancellationToken)
        {
            Console.WriteLine("Adding manual suspension. Press any key to continue.");
            Console.ReadKey();

            var request = new ChangeManualSuspensionRequestDto
            {
                Suspensions = new List<Suspension>()
                {
                    new Suspension()
                    {
                        FixtureId = 13176576,
                        Markets = new List<Market>
                        {
                           new Market()
                           {
                               MarketId = 2755,
                               Line = "-0.25"
                           }
                        }
                    }
                }
            };
            var response = await subscriptionApiClient.AddManualSuspension(request, cancellationToken);
            Console.WriteLine("Manual suspension added.");
        }

        private async Task RemoveManualSuspensionAsync(ISubscriptionApiClient subscriptionApiClient, CancellationToken cancellationToken)
        {
            Console.WriteLine("Removing manual suspension. Press any key to continue.");
            Console.ReadKey();

            var request = new ChangeManualSuspensionRequestDto
            {
                Suspensions = new List<Suspension>()
                {
                    new Suspension()
                    {
                        FixtureId = 13176576,
                        Markets = new List<Market>
                        {
                           new Market()
                           {
                               MarketId = 1439,
                               Line = "-2.5"
                           }
                        }
                    }
                }
            };
            var response = await subscriptionApiClient.RemoveManualSuspension(request, cancellationToken);
            Console.WriteLine("Manual suspension removed.");
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Service is stopping.");
            return Task.CompletedTask;
        }
    }
}
