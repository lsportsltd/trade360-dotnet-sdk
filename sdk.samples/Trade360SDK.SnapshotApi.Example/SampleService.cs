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
                        try
                        {
                            await HandleMenuChoice(choice, cancellationToken);
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex, "An error occurred while retrieving data");
                        }
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
            Console.WriteLine();
            Console.WriteLine("Select an option (type 'exit' to quit):");
            Console.WriteLine("--- InPlay ---");
            Console.WriteLine("1.  InPlay - Get Fixtures");
            Console.WriteLine("2.  InPlay - Get Events");
            Console.WriteLine("3.  InPlay - Get Fixture Markets");
            Console.WriteLine("4.  InPlay - Get Livescore");
            Console.WriteLine("5.  InPlay - Get Outright Leagues Fixtures");
            Console.WriteLine("6.  InPlay - Get Outright Leagues Markets");
            Console.WriteLine("7.  InPlay - Get Outright Leagues Events");
            Console.WriteLine("--- Prematch ---");
            Console.WriteLine("8.  Prematch - Get Fixtures");
            Console.WriteLine("9.  Prematch - Get Events");
            Console.WriteLine("10. Prematch - Get Fixture Markets");
            Console.WriteLine("11. Prematch - Get Livescore");
            Console.WriteLine("12. Prematch - Get Outright Fixtures");
            Console.WriteLine("13. Prematch - Get Outright Livescore");
            Console.WriteLine("14. Prematch - Get Outright Markets");
            Console.WriteLine("15. Prematch - Get Outright Events");
            Console.WriteLine("16. Prematch - Get Outright Leagues Fixtures");
            Console.WriteLine("17. Prematch - Get Outright Leagues Markets");
            Console.WriteLine("18. Prematch - Get Outright Leagues Events");
            Console.WriteLine();
        }

        private async Task HandleMenuChoice(string choice, CancellationToken cancellationToken)
        {
            switch (choice)
            {
                // InPlay — all ISnapshotInplayApiClient endpoints
                case "1":
                    await GetFixtures(_snapshotInplayApiClient, cancellationToken);
                    break;
                case "2":
                    await GetEvents(_snapshotInplayApiClient, cancellationToken);
                    break;
                case "3":
                    await GetFixtureMarkets(_snapshotInplayApiClient, cancellationToken);
                    break;
                case "4":
                    await GetLivescore(_snapshotInplayApiClient, cancellationToken);
                    break;
                case "5":
                    await GetOutrightLeaguesFixtures(_snapshotInplayApiClient, cancellationToken);
                    break;
                case "6":
                    await GetOutrightLeaguesMarkets(_snapshotInplayApiClient, cancellationToken);
                    break;
                case "7":
                    await GetOutrightLeaguesEvents(_snapshotInplayApiClient, cancellationToken);
                    break;

                // Prematch — all ISnapshotPrematchApiClient endpoints
                case "8":
                    await GetFixtures(_snapshotPrematchApiClient, cancellationToken);
                    break;
                case "9":
                    await GetEvents(_snapshotPrematchApiClient, cancellationToken);
                    break;
                case "10":
                    await GetFixtureMarkets(_snapshotPrematchApiClient, cancellationToken);
                    break;
                case "11":
                    await GetLivescore(_snapshotPrematchApiClient, cancellationToken);
                    break;
                case "12":
                    await GetOutrightFixtures(_snapshotPrematchApiClient, cancellationToken);
                    break;
                case "13":
                    await GetOutrightLivescore(_snapshotPrematchApiClient, cancellationToken);
                    break;
                case "14":
                    await GetOutrightMarkets(_snapshotPrematchApiClient, cancellationToken);
                    break;
                case "15":
                    await GetOutrightEvents(_snapshotPrematchApiClient, cancellationToken);
                    break;
                case "16":
                    await GetOutrightLeaguesFixtures(_snapshotPrematchApiClient, cancellationToken);
                    break;
                case "17":
                    await GetOutrightLeaguesMarkets(_snapshotPrematchApiClient, cancellationToken);
                    break;
                case "18":
                    await GetOutrightLeaguesEvents(_snapshotPrematchApiClient, cancellationToken);
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
                Sports = new List<int>() { 6046 },
                Fixtures = new List<int>()  {/* List of fixture IDs, e.g., 12345678, 23456789 */ },
                Leagues = new List<int>() { 170 },
                Locations = new List<int>() { 171 }
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
                Sports = new List<int>() { 6046 },
                Fixtures = new List<int>() { /* List of fixture IDs, e.g., 12345678, 23456789 */ },
                Leagues = new List<int>() { /* List of league IDs, e.g., 1111, 2222 */ },
                Locations = new List<int>() { 171 }
            };

            var response = await snapshotInplayApiClient.GetEvents(request, cancellationToken);
            _logger.LogInformation("GetEvents ended with response count: {Count}", response.Count());
        }

        private async Task GetFixtureMarkets(ISnapshotInplayApiClient snapshotInplayApiClient, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Starting GetFixtureMarkets...");

            var request = new GetMarketRequestDto()
            {
                Sports = new List<int>() { 6046 },
                Fixtures = new List<int>() { 171 },
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
                Sports = new List<int>() { 6046 },
                Fixtures = new List<int>() { 171 },
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
                Sports = new List<int>() { 6046 },
                Fixtures = new List<int>() { /* List of fixture IDs, e.g., 12345678, 23456789 */ },
                Leagues = new List<int>() { /* List of league IDs, e.g., 1111, 2222 */ },
                Locations = new List<int>() { 4 }
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
                Sports = new List<int>() { 35232 },
                Fixtures = new List<int>() { /* List of fixture IDs, e.g., 12345678, 23456789 */ },
                Leagues = new List<int>() { /* List of league IDs, e.g., 1111, 2222 */ },
                Locations = new List<int>() { 73 }
            };

            var response = await snapshotPrematchApiClient.GetEvents(request, cancellationToken);
            _logger.LogInformation("GetEvents ended with response count: {Count}", response.Count());
        }

        private async Task GetFixtureMarkets(ISnapshotPrematchApiClient snapshotPrematchApiClient, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Starting GetFixtureMarkets...");

            var request = new GetMarketRequestDto()
            {
                Sports = new List<int>() { 452674 },
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
                Sports = new List<int>() { 452674 },
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
                // Fixture outrights: competition sport (e.g. Horse Racing). Football 6046 returns null Body here.
                Sports = new List<int>() { 687888 },
            };

            var response = await snapshotPrematchApiClient.GetOutrightFixture(request, cancellationToken);
            _logger.LogInformation("GetOutrightFixtures ended with response count: {Count}", response.Count());
        }

        private async Task GetOutrightLivescore(ISnapshotPrematchApiClient snapshotPrematchApiClient, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Starting GetOutrightLivescore...");

            var request = new GetOutrightLivescoreRequestDto()
            {
                Sports = new List<int>() { 687888 },
            };

            var response = await snapshotPrematchApiClient.GetOutrightScores(request, cancellationToken);
            _logger.LogInformation("GetOutrightLivescore ended with response count: {Count}", response.Count());
        }

        private async Task GetOutrightMarkets(ISnapshotPrematchApiClient snapshotPrematchApiClient, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Starting GetOutrightMarkets...");

            var request = new GetOutrightMarketsRequestDto()
            {
                Sports = new List<int>() { 687888 },
            };

            var response = await snapshotPrematchApiClient.GetOutrightFixtureMarkets(request, cancellationToken);
            _logger.LogInformation("GetOutrightMarkets ended with response count: {Count}", response.Count());
        }

        private async Task GetOutrightEvents(ISnapshotPrematchApiClient snapshotPrematchApiClient, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Starting GetOutrightEvents...");

            var request = new GetOutrightMarketsRequestDto()
            {
                Sports = new List<int>() { 687888 },
            };

            var response = await snapshotPrematchApiClient.GetOutrightEvents(request, cancellationToken);
            _logger.LogInformation("GetOutrightEvents ended with response count: {Count}", response.Count());
        }

        private async Task GetOutrightLeaguesFixtures(ISnapshotPrematchApiClient snapshotPrematchApiClient, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Starting GetOutrightLeaguesFixtures...");

            var request = new GetFixturesRequestDto()
            {
                // League outrights use regular sports (Football). Competition sport 687888 returns null Body here.
                Sports = new List<int>() { 6046 },
            };

            var response = await snapshotPrematchApiClient.GetOutrightLeagues(request, cancellationToken);
            _logger.LogInformation("GetOutrightLeaguesFixtures ended with response count: {Count}", response.Count());
        }

        private async Task GetOutrightLeaguesMarkets(ISnapshotPrematchApiClient snapshotPrematchApiClient, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Starting GetOutrightLeaguesMarkets...");

            var request = new GetMarketRequestDto()
            {
                Sports = new List<int>() { 6046 },
            };

            var response = await snapshotPrematchApiClient.GetOutrightLeaguesMarkets(request, cancellationToken);
            _logger.LogInformation("GetOutrightLeaguesMarkets ended with response count: {Count}", response.Count());
        }
        
        private async Task GetOutrightLeaguesEvents(ISnapshotPrematchApiClient snapshotPrematchApiClient, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Starting GetOutrightLeaguesEvents...");

            var request = new GetOutrightFixturesRequestDto()
            {
                Sports = new List<int>() { 6046 },
            };

            var response = await snapshotPrematchApiClient.GetOutrightLeagueEvents(request, cancellationToken);
            _logger.LogInformation("GetOutrightLeaguesEvents ended with response count: {Count}", response.Count());
        }
        
        
        private async Task GetOutrightLeaguesFixtures(ISnapshotInplayApiClient snapshotInplayApiClient, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Starting GetOutrightLeaguesFixtures...");

            var request = new GetFixturesRequestDto()
            {
                Sports = new List<int>() { 6046 },
            };

            var response = await snapshotInplayApiClient.GetOutrightLeagues(request, cancellationToken);
            _logger.LogInformation("GetOutrightLeaguesFixtures ended with response count: {Count}", response.Count());
        }

        private async Task GetOutrightLeaguesMarkets(ISnapshotInplayApiClient snapshotInplayApiClient, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Starting GetOutrightLeaguesMarkets...");

            var request = new GetMarketRequestDto()
            {
                Sports = new List<int>() { 6046 },
            };

            var response = await snapshotInplayApiClient.GetOutrightLeaguesMarkets(request, cancellationToken);
            _logger.LogInformation("GetOutrightLeaguesMarkets ended with response count: {Count}", response.Count());
        }
        
        private async Task GetOutrightLeaguesEvents(ISnapshotInplayApiClient snapshotInplayApiClient, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Starting GetOutrightLeaguesEvents...");

            var request = new GetOutrightFixturesRequestDto()
            {
                Sports = new List<int>() { 6046 },
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
