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
        private readonly ISnapshotApiFactory _snapshotApiFactory;
        private readonly IOptionsMonitor<SnapshotApiSettings> _settingsMonitor;

        public Startup(ILogger<Startup> logger, ISnapshotApiFactory snapshotApiFactory, IOptionsMonitor<SnapshotApiSettings> settingsMonitor)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _snapshotApiFactory = snapshotApiFactory;
            _settingsMonitor = settingsMonitor;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            try
            {
                var snapshotInplayApiSettings = _settingsMonitor.Get("SnapshotInplaySettings");
                var snapshotPrematchApiSettings = _settingsMonitor.Get("SnapshotPrematchSettings");

                var inplaySnapshotClient = _snapshotApiFactory.CreateInplayHttpClient(snapshotInplayApiSettings);
                var prematchSnapshotClient = _snapshotApiFactory.CreatePrematchHttpClient(snapshotPrematchApiSettings);

                // Expose only the GetFixtures method
                //await GetFixtures(prematchSnapshotClient, cancellationToken);

                // Uncomment and implement other methods as needed
                 await GetEvents(prematchSnapshotClient, cancellationToken);
                // await GetFixtureMarkets(prematchSnapshotClient, cancellationToken);
                // await GetLivescore(prematchSnapshotClient, cancellationToken);
                // await GetOutrightFixtures(prematchSnapshotClient, cancellationToken);
                // await GetOutrightLivescore(prematchSnapshotClient, cancellationToken);
                // await GetOutrightMarkets(prematchSnapshotClient, cancellationToken);
                // await GetOutrightEvents(prematchSnapshotClient, cancellationToken);
                // await GetOutrightLeaguesFixtures(prematchSnapshotClient, cancellationToken);
                // await GetOutrightLeaguesMarkets(prematchSnapshotClient, cancellationToken);

                // Uncomment and implement inplay methods as needed
                // await GetEvents(inplaySnapshotClient, cancellationToken);
                // await GetFixtureMarkets(inplaySnapshotClient, cancellationToken);
                // await GetLivescore(inplaySnapshotClient, cancellationToken);
                // await GetFixtures(inplaySnapshotClient, cancellationToken);

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
                Fixtures = new List<int>() { 13384002/* List of fixture IDs, e.g., 12345678, 23456789 */ },
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
