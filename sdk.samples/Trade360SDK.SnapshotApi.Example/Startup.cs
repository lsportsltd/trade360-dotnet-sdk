using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Trade360SDK.SnapshotApi.Configuration;
using Trade360SDK.SnapshotApi.Entities.Requests;
using Trade360SDK.SnapshotApi.Interfaces;

namespace Trade360SDK.SnapshotApi.Example
{
    public class Startup : IHostedService
    {
        private readonly ILogger<Startup> _logger;
        private readonly ISnapshotInplayApiClient _snapshotInplayApiClient;
        private readonly ISnapshotPrematchApiClient _snapshotPrematchApiClient;

        public Startup(ILogger<Startup> logger, ISnapshotInplayApiClient snapshotInplayApiClient, IOptionsMonitor<SnapshotApiSettings> settingsMonitor, ISnapshotPrematchApiClient snapshotPrematchApiClient)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _snapshotInplayApiClient = snapshotInplayApiClient;
            _snapshotPrematchApiClient = snapshotPrematchApiClient;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            try
            {
                // Expose only the GetFixtures method
                await GetFixtures(_snapshotPrematchApiClient, cancellationToken);

                // Uncomment and implement other methods as needed
                await GetEvents(_snapshotPrematchApiClient, cancellationToken);
                await GetFixtureMarkets(_snapshotPrematchApiClient, cancellationToken);
                await GetLivescore(_snapshotPrematchApiClient, cancellationToken);
                await GetOutrightFixtures(_snapshotPrematchApiClient, cancellationToken);
                await GetOutrightLivescore(_snapshotPrematchApiClient, cancellationToken);
                await GetOutrightMarkets(_snapshotPrematchApiClient, cancellationToken);
                await GetOutrightEvents(_snapshotPrematchApiClient, cancellationToken);
                await GetOutrightLeaguesFixtures(_snapshotPrematchApiClient, cancellationToken);
                await GetOutrightLeaguesMarkets(_snapshotPrematchApiClient, cancellationToken);

                // Uncomment and implement inplay methods as needed
                //await GetEvents(_snapshotInplayApiClient, cancellationToken);
                //await GetFixtureMarkets(_snapshotInplayApiClient, cancellationToken);
                //await GetLivescore(_snapshotInplayApiClient, cancellationToken);
                //await GetFixtures(_snapshotInplayApiClient, cancellationToken);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving data");
            }
        }

        private async Task GetFixtures(ISnapshotPrematchApiClient snapshotPrematchApiClient, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Starting GetFixtures...");

            var request = new GetFixturesRequestDto()
            {
                Sports = new List<int>() { /* List of sport IDs, e.g., 1234, 2345 */ },
                Fixtures = new List<int>() { /* List of fixture IDs, e.g., 12345678, 23456789 */ },
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
                Fixtures = new List<int>() { /* List of fixture IDs, e.g., 12345678, 23456789 */ },
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
                Fixtures = new List<int>() { /* List of fixture IDs, e.g., 12345678, 23456789 */ },
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


        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Service is stopping.");
            return Task.CompletedTask;
        }
    }
}
