using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Trade360SDK.SnapshotApi.Entities.Requests;
using Trade360SDK.SnapshotApi.Interfaces;

namespace Trade360SDK.SnapshotApi.Example
{
    public class SampleService : IHostedService
    {
        private readonly ILogger<SampleService> _logger;
        private readonly ISnapshotInplayApiClient _snapshotInplayApiClient;
        private readonly ISnapshotPrematchApiClient _snapshotPrematchApiClient;

        public SampleService(ILogger<SampleService> logger, ISnapshotInplayApiClient snapshotInplayApiClient, ISnapshotPrematchApiClient snapshotPrematchApiClient)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _snapshotInplayApiClient = snapshotInplayApiClient;
            _snapshotPrematchApiClient = snapshotPrematchApiClient;
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
            Console.WriteLine("1. Snapshot API - Get Fixtures");
            Console.WriteLine("2. Snapshot API - Get Events");
            Console.WriteLine("3. Snapshot API - Get Fixture Markets");
            Console.WriteLine("4. Snapshot API - Get Livescore");
            Console.WriteLine("5. Snapshot API - Get Outright Fixtures");
            Console.WriteLine("6. Snapshot API - Get Outright Livescore");
            Console.WriteLine("7. Snapshot API - Get Outright Markets");
            Console.WriteLine("8. Snapshot API - Get Outright Events");
            Console.WriteLine("9. Snapshot API - Get Outright Leagues Fixtures");
            Console.WriteLine("10. Snapshot API - Get Outright Leagues Markets");
            Console.WriteLine("11. Snapshot API - Get Outright Leagues Events");
        }

        private async Task HandleMenuChoice(string choice, CancellationToken cancellationToken)
        {
            switch (choice)
            {
                case "1":
                    await GetFixtures(_snapshotInplayApiClient, cancellationToken);
                    //await GetFixtures(_snapshotPrematchApiClient, cancellationToken);
                    break;
                case "2":
                    //await GetEvents(_snapshotPrematchApiClient, cancellationToken);
                    await GetEvents(_snapshotInplayApiClient, cancellationToken);
                    break;
                case "3":
                    //await GetFixtureMarkets(_snapshotPrematchApiClient, cancellationToken);
                    await GetFixtureMarkets(_snapshotInplayApiClient, cancellationToken);
                    break;
                case "4":
                    //await GetLivescore(_snapshotPrematchApiClient, cancellationToken);
                    await GetLivescore(_snapshotInplayApiClient, cancellationToken);
                    break;
                case "5":
                    await GetOutrightFixtures(_snapshotPrematchApiClient, cancellationToken);
                    break;
                case "6":
                    await GetOutrightLivescore(_snapshotPrematchApiClient, cancellationToken);
                    break;
                case "7":
                    await GetOutrightMarkets(_snapshotPrematchApiClient, cancellationToken);
                    break;
                case "8":
                    await GetOutrightEvents(_snapshotPrematchApiClient, cancellationToken);
                    break;
                case "9":
                    await GetOutrightLeaguesFixtures(_snapshotPrematchApiClient, cancellationToken);
                    await GetOutrightLeaguesFixtures(_snapshotInplayApiClient, cancellationToken);
                    break;
                case "10":
                    await GetOutrightLeaguesMarkets(_snapshotPrematchApiClient, cancellationToken);
                    await GetOutrightLeaguesMarkets(_snapshotInplayApiClient, cancellationToken);
                    break;
                case "11":
                    await GetOutrightLeaguesEvents(_snapshotPrematchApiClient, cancellationToken);
                    await GetOutrightLeaguesEvents(_snapshotInplayApiClient, cancellationToken);
                    break;
                default:
                    Console.WriteLine("Invalid choice. Please try again.");
                    break;
            }
        }

        private async Task GetFixtures(ISnapshotInplayApiClient snapshotInplayApiClient, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Starting GetFixtures...");

            var request = new GetFixturesRequestDto()
            {
                Sports = new List<int>() { /* List of sport IDs, e.g., 1234, 2345 */ },
                Fixtures = new List<int>() { 24770989 },
                Leagues = new List<int>() { /* List of league IDs, e.g., 1111, 2222 */ },
                Locations = new List<int>() { /* List of location IDs, e.g., 3333, 4444 */ }
            };

            var response = await snapshotInplayApiClient.GetFixtures(request, cancellationToken);
            _logger.LogInformation("GetFixtures ended with response count: {Count}", response.Count());
        }

        // Example of other methods to be uncommented and used as needed
        
        private async Task GetEvents(ISnapshotInplayApiClient snapshotInplayApiClient, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Starting GetEvents...");

            var request = new GetMarketRequestDto()
            {
                Sports = new List<int>() { /* List of sport IDs, e.g., 1234, 2345 */ },
                Fixtures = new List<int>() { /* List of fixture IDs, e.g., 12345678, 23456789 */ },
                Leagues = new List<int>() { /* List of league IDs, e.g., 1111, 2222 */ },
                Locations = new List<int>() { /* List of location IDs, e.g., 3333, 4444 */ }
            };

            var response = await snapshotInplayApiClient.GetEvents(request, cancellationToken);
            _logger.LogInformation("GetEvents ended with response count: {Count}", response.Count());
        }

        private async Task GetFixtureMarkets(ISnapshotInplayApiClient snapshotInplayApiClient, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Starting GetFixtureMarkets...");

            var request = new GetMarketRequestDto()
            {
                Sports = new List<int>() { /* List of sport IDs, e.g., 1234, 2345 */ },
                Fixtures = new List<int>() { /* List of fixture IDs, e.g., 12345678, 23456789 */ },
                Leagues = new List<int>() { /* List of league IDs, e.g., 1111, 2222 */ },
                Locations = new List<int>() { /* List of location IDs, e.g., 3333, 4444 */ }
            };

            var response = await snapshotInplayApiClient.GetFixtureMarkets(request, cancellationToken);
            _logger.LogInformation("GetFixtureMarkets ended with response count: {Count}", response.Count());
        }

        private async Task GetLivescore(ISnapshotInplayApiClient snapshotInplayApiClient, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Starting GetLivescore...");

            var request = new GetLivescoreRequestDto()
            {
                Sports = new List<int>() { /* List of sport IDs, e.g., 1234, 2345 */ },
                Fixtures = new List<int>() {  },
                Leagues = new List<int>() { /* List of league IDs, e.g., 1111, 2222 */ },
                Locations = new List<int>() { /* List of location IDs, e.g., 3333, 4444 */ }
            };

            var response = await snapshotInplayApiClient.GetLivescore(request, cancellationToken);
            _logger.LogInformation("GetLivescore ended with response count: {Count}", response.Count());
        }


        private async Task GetFixtures(ISnapshotPrematchApiClient snapshotPrematchApiClient, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Starting GetFixtures...");

            var request = new GetFixturesRequestDto()
            {
                Sports = new List<int>() { /* List of sport IDs, e.g., 1234, 2345 */ },
                Fixtures = new List<int>() { 24770989 },
                Leagues = new List<int>() { /* List of league IDs, e.g., 1111, 2222 */ },
                Locations = new List<int>() { /* List of location IDs, e.g., 3333, 4444 */ }
            };

            var response = await snapshotPrematchApiClient.GetFixtures(request, cancellationToken);
            _logger.LogInformation("GetFixtures ended with response count: {Count}", response.Count());
        }

        // Example of other methods to be uncommented and used as needed
        
        private async Task GetEvents(ISnapshotPrematchApiClient snapshotPrematchApiClient, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Starting GetEvents...");

            var request = new GetMarketRequestDto()
            {
                Sports = new List<int>() { /* List of sport IDs, e.g., 1234, 2345 */ },
                Fixtures = new List<int>() { /* List of fixture IDs, e.g., 12345678, 23456789 */ },
                Leagues = new List<int>() { /* List of league IDs, e.g., 1111, 2222 */ },
                Locations = new List<int>() { /* List of location IDs, e.g., 3333, 4444 */ }
            };

            var response = await snapshotPrematchApiClient.GetEvents(request, cancellationToken);
            _logger.LogInformation("GetEvents ended with response count: {Count}", response.Count());
        }

        private async Task GetFixtureMarkets(ISnapshotPrematchApiClient snapshotPrematchApiClient, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Starting GetFixtureMarkets...");

            var request = new GetMarketRequestDto()
            {
                Sports = new List<int>() { /* List of sport IDs, e.g., 1234, 2345 */ },
                Fixtures = new List<int>() { /* List of fixture IDs, e.g., 12345678, 23456789 */ },
                Leagues = new List<int>() { /* List of league IDs, e.g., 1111, 2222 */ },
                Locations = new List<int>() { /* List of location IDs, e.g., 3333, 4444 */ }
            };

            var response = await snapshotPrematchApiClient.GetFixtureMarkets(request, cancellationToken);
            _logger.LogInformation("GetFixtureMarkets ended with response count: {Count}", response.Count());
        }

        private async Task GetLivescore(ISnapshotPrematchApiClient snapshotPrematchApiClient, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Starting GetLivescore...");

            var request = new GetLivescoreRequestDto()
            {
                Sports = new List<int>() { /* List of sport IDs, e.g., 1234, 2345 */ },
                Fixtures = new List<int>() { /* List of fixture IDs, e.g., 12345678, 23456789 */ },
                Leagues = new List<int>() { /* List of league IDs, e.g., 1111, 2222 */ },
                Locations = new List<int>() { /* List of location IDs, e.g., 3333, 4444 */ }
            };

            var response = await snapshotPrematchApiClient.GetLivescore(request, cancellationToken);
            _logger.LogInformation("GetLivescore ended with response count: {Count}", response.Count());
        }

        private async Task GetOutrightFixtures(ISnapshotPrematchApiClient snapshotPrematchApiClient, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Starting GetOutrightFixtures...");

            var request = new GetOutrightFixturesRequestDto()
            {
                Sports = new List<int>() { /* List of sport IDs, e.g., 1234, 2345 */ },
                Fixtures = new List<int>() { 24775553/* List of fixture IDs, e.g., 12345678, 23456789 */ },
                Tournaments = new List<int>() { /* List of league IDs, e.g., 1111, 2222 */ },
                Locations = new List<int>() { /* List of location IDs, e.g., 3333, 4444 */ }
            };

            var response = await snapshotPrematchApiClient.GetOutrightFixture(request, cancellationToken);
            _logger.LogInformation("GetOutrightFixtures ended with response count: {Count}", response.Count());
        }

        private async Task GetOutrightLivescore(ISnapshotPrematchApiClient snapshotPrematchApiClient, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Starting GetOutrightLivescore...");

            var request = new GetOutrightLivescoreRequestDto()
            {
                Sports = new List<int>() { /* List of sport IDs, e.g., 1234, 2345 */ },
                Fixtures = new List<int>() { /* List of fixture IDs, e.g., 12345678, 23456789 */ },
                Tournaments = new List<int>() { /* List of league IDs, e.g., 1111, 2222 */ },
                Locations = new List<int>() { /* List of location IDs, e.g., 3333, 4444 */ }
            };

            var response = await snapshotPrematchApiClient.GetOutrightScores(request, cancellationToken);
            _logger.LogInformation("GetOutrightLivescore ended with response count: {Count}", response.Count());
        }

        private async Task GetOutrightMarkets(ISnapshotPrematchApiClient snapshotPrematchApiClient, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Starting GetOutrightMarkets...");

            var request = new GetOutrightMarketsRequestDto()
            {
                Sports = new List<int>() { /* List of sport IDs, e.g., 1234, 2345 */ },
                Fixtures = new List<int>() { /* List of fixture IDs, e.g., 12345678, 23456789 */ },
                Tournaments = new List<int>() { /* List of league IDs, e.g., 1111, 2222 */ },
                Locations = new List<int>() { /* List of location IDs, e.g., 3333, 4444 */ }
            };

            var response = await snapshotPrematchApiClient.GetOutrightFixtureMarkets(request, cancellationToken);
            _logger.LogInformation("GetOutrightMarkets ended with response count: {Count}", response.Count());
        }

        private async Task GetOutrightEvents(ISnapshotPrematchApiClient snapshotPrematchApiClient, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Starting GetOutrightEvents...");

            var request = new GetOutrightMarketsRequestDto()
            {
                Sports = new List<int>() { /* List of sport IDs, e.g., 1234, 2345 */ },
                Fixtures = new List<int>() { /* List of fixture IDs, e.g., 12345678, 23456789 */ },
                Tournaments = new List<int>() { /* List of league IDs, e.g., 1111, 2222 */ },
                Locations = new List<int>() { /* List of location IDs, e.g., 3333, 4444 */ }
            };

            var response = await snapshotPrematchApiClient.GetOutrightEvents(request, cancellationToken);
            _logger.LogInformation("GetOutrightEvents ended with response count: {Count}", response.Count());
        }

        private async Task GetOutrightLeaguesFixtures(ISnapshotPrematchApiClient snapshotPrematchApiClient, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Starting GetOutrightLeaguesFixtures...");

            var request = new GetFixturesRequestDto()
            {
                Sports = new List<int>() { /* List of sport IDs, e.g., 1234, 2345 */ },
                Fixtures = new List<int>() {24593410 /* List of fixture IDs, e.g., 12345678, 23456789 */ },
                Leagues = new List<int>() { /* List of league IDs, e.g., 1111, 2222 */ },
                Locations = new List<int>() { /* List of location IDs, e.g., 3333, 4444 */ }
            };

            var response = await snapshotPrematchApiClient.GetOutrightLeagues(request, cancellationToken);
            _logger.LogInformation("GetOutrightLeaguesFixtures ended with response count: {Count}", response.Count());
        }

        private async Task GetOutrightLeaguesMarkets(ISnapshotPrematchApiClient snapshotPrematchApiClient, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Starting GetOutrightLeaguesMarkets...");

            var request = new GetMarketRequestDto()
            {
                Sports = new List<int>() { /* List of sport IDs, e.g., 1234, 2345 */ },
                Fixtures = new List<int>() { /* List of fixture IDs, e.g., 12345678, 23456789 */ },
                Leagues = new List<int>() { /* List of league IDs, e.g., 1111, 2222 */ },
                Locations = new List<int>() { /* List of location IDs, e.g., 3333, 4444 */ }
            };

            var response = await snapshotPrematchApiClient.GetOutrightLeaguesMarkets(request, cancellationToken);
            _logger.LogInformation("GetOutrightLeaguesMarkets ended with response count: {Count}", response.Count());
        }
        
        private async Task GetOutrightLeaguesEvents(ISnapshotPrematchApiClient snapshotPrematchApiClient, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Starting GetOutrightLeaguesEvents...");

            var request = new GetOutrightFixturesRequestDto()
            {
                Sports = new List<int>() { /* List of sport IDs, e.g., 1234, 2345 */ },
                Fixtures = new List<int>() { /* List of fixture IDs, e.g., 12345678, 23456789 */ },
                Tournaments = new List<int>() { /* List of league IDs, e.g., 1111, 2222 */ },
                Locations = new List<int>() { /* List of location IDs, e.g., 3333, 4444 */ }
            };

            var response = await snapshotPrematchApiClient.GetOutrightLeagueEvents(request, cancellationToken);
            _logger.LogInformation("GetOutrightLeaguesEvents ended with response count: {Count}", response.Count());
        }
        
        
                private async Task GetOutrightLeaguesFixtures(ISnapshotInplayApiClient snapshotInplayApiClient, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Starting GetOutrightLeaguesFixtures...");

            var request = new GetFixturesRequestDto()
            {
                Sports = new List<int>() { /* List of sport IDs, e.g., 1234, 2345 */ },
                Fixtures = new List<int>() { /* List of fixture IDs, e.g., 12345678, 23456789 */ },
                Leagues = new List<int>() { /* List of league IDs, e.g., 1111, 2222 */ },
                Locations = new List<int>() { /* List of location IDs, e.g., 3333, 4444 */ }
            };

            var response = await snapshotInplayApiClient.GetOutrightLeagues(request, cancellationToken);
            _logger.LogInformation("GetOutrightLeaguesFixtures ended with response count: {Count}", response.Count());
        }

        private async Task GetOutrightLeaguesMarkets(ISnapshotInplayApiClient snapshotInplayApiClient, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Starting GetOutrightLeaguesMarkets...");

            var request = new GetMarketRequestDto()
            {
                Sports = new List<int>() { /* List of sport IDs, e.g., 1234, 2345 */ },
                Fixtures = new List<int>() { /* List of fixture IDs, e.g., 12345678, 23456789 */ },
                Leagues = new List<int>() { /* List of league IDs, e.g., 1111, 2222 */ },
                Locations = new List<int>() { /* List of location IDs, e.g., 3333, 4444 */ }
            };

            var response = await snapshotInplayApiClient.GetOutrightLeaguesMarkets(request, cancellationToken);
            _logger.LogInformation("GetOutrightLeaguesMarkets ended with response count: {Count}", response.Count());
        }
        
        private async Task GetOutrightLeaguesEvents(ISnapshotInplayApiClient snapshotInplayApiClient, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Starting GetOutrightLeaguesEvents...");

            var request = new GetOutrightFixturesRequestDto()
            {
                Sports = new List<int>() { /* List of sport IDs, e.g., 1234, 2345 */ },
                Fixtures = new List<int>() { /* List of fixture IDs, e.g., 12345678, 23456789 */ },
                Tournaments = new List<int>() { /* List of league IDs, e.g., 1111, 2222 */ },
                Locations = new List<int>() { /* List of location IDs, e.g., 3333, 4444 */ }
            };

            var response = await snapshotInplayApiClient.GetOutrightLeagueEvents(request, cancellationToken);
            _logger.LogInformation("GetOutrightLeaguesEvents ended with response count: {Count}", response.Count());
        }


        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Service is stopping.");
            return Task.CompletedTask;
        }
    }
}
