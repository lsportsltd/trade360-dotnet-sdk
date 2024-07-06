using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Trade360SDK.SnapshotApi.Configuration;
using Trade360SDK.SnapshotApi.Entities.Requests;
using Trade360SDK.SnapshotApi.Interfaces;

namespace Trade360SDK.Feed.Example
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
                // Retrieve the API settings for in-play data from the configuration.
                // You can initialize settings for pre-match type as well, create using the factory more instances for different types
                var snapshotInplayApiSettings = _settingsMonitor.Get("SnapshotInplaySettings");
                var snapshotPrematchApiSettings = _settingsMonitor.Get("SnapshotPrematchSettings");
                //Initialize Inplay Snapshot Client
                var inplaySnapshotClient = _snapshotApiFactory.CreateInplayHttpClient(snapshotInplayApiSettings);
                var prematchSnapshotClient = _snapshotApiFactory.CreatePrematchHttpClient(snapshotPrematchApiSettings);

                //Prematch Api
                //await GetEvents(prematchSnapshotClient, cancellationToken);
                await GetFixtureMarkets(prematchSnapshotClient, cancellationToken);
                await GetLivescore(prematchSnapshotClient, cancellationToken);
                await GetFixtures(prematchSnapshotClient, cancellationToken);

                //Inplay Api
                //await GetEvents(inplaySnapshotClient, cancellationToken);
                //await GetFixtureMarkets(inplaySnapshotClient, cancellationToken);
                //await GetLivescore(inplaySnapshotClient, cancellationToken);
                //await GetFixtures(inplaySnapshotClient, cancellationToken);
                
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving data");
            }
        }

        private async Task GetFixtures(ISnapshotPrematchApiClient snapshotPrematchApiClient, CancellationToken cancellationToken)
        {
            Console.WriteLine("GetFixtures Example. Press any key to continue.");
            Console.ReadKey();

            var request = new GetFixturesRequestDto()
            {
                Sports = new List<int>() { 6046 },
                Fixtures = new List<int>() { 13213748 }
            };
            var response = await snapshotPrematchApiClient.GetFixtures(request, cancellationToken);
            Console.WriteLine("Get fixtures Ended.");
        }

        private async Task GetLivescore(ISnapshotPrematchApiClient snapshotPrematchApiClient, CancellationToken cancellationToken)
        {
            Console.WriteLine("GetLivescore Example. Press any key to continue.");
            Console.ReadKey();

            var request = new GetLivescoreRequestDto()
            {
                Sports = new List<int>() { 6046 },
                Fixtures = new List<int>() { 13213748 }
            };
            var response = await snapshotPrematchApiClient.GetLivescore(request, cancellationToken);
            Console.WriteLine("GetLivescore Ended.");
        }

        private async Task GetFixtureMarkets(ISnapshotPrematchApiClient snapshotPrematchApiClient, CancellationToken cancellationToken)
        {
            Console.WriteLine("GetFixtureMarkets Example. Press any key to continue.");
            Console.ReadKey();

            var request = new GetMarketRequestDto()
            {
                Sports = new List<int>() { 6046 },
                Fixtures = new List<int>() { 13213748 }
            };
            var response = await snapshotPrematchApiClient.GetFixtureMarkets(request, cancellationToken);
            Console.WriteLine("GetFixtureMarkets Ended.");
        }

        private async Task GetEvents(ISnapshotPrematchApiClient snapshotPrematchApiClient, CancellationToken cancellationToken)
        {
            Console.WriteLine("GetEvents Example. Press any key to continue.");
            Console.ReadKey();

            var request = new GetMarketRequestDto()
            {
                Sports = new List<int>() { 6046 },
                Fixtures = new List<int>() { 13213748 }
            };
            var response = await snapshotPrematchApiClient.GetEvents(request, cancellationToken);
            Console.WriteLine("GetEvents Ended.");
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Service is stopping.");
            return Task.CompletedTask;
        }
    }
}
