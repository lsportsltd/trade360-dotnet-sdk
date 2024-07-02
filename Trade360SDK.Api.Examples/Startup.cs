using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Trade360SDK.CustomersApi.MetadataApi.Requests;
using Trade360SDK.Metadata;

namespace Trade360SDK.Feed.Example
{
    public class Startup : IHostedService
    {
        private readonly ILogger<Startup> _logger;
        private readonly IMetadataClient _MetadataClient;
        private readonly IPackageDistributionClient _packageDistributionClient;

        public Startup(ILogger<Startup> logger, IMetadataClient MetadataClient, IPackageDistributionClient packageDistributionClient)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _MetadataClient = MetadataClient;
            _packageDistributionClient = packageDistributionClient;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            try
            {
                Console.WriteLine("Get Distribution Status");
                Console.ReadKey();

                var getDistributionStatus = await _packageDistributionClient.GetDistributionStatusAsync(cancellationToken);


                Console.WriteLine("Starting Distribution");
                Console.ReadKey();

                var startDistributionResponse = await _packageDistributionClient.StartDistributionAsync(cancellationToken);

                Console.WriteLine("Press any key to receive LSports markets entities");
                Console.ReadKey();

                var marketRequest = new GetMarketsRequestDto()
                {
                    MarketIds = new List<int> { 1, 2 },
                    IsSettleable = true,
                    SportIds = new List<int> { 6046 },
                };
                var marketsResults = await _MetadataClient.GetMarketsAsync(marketRequest, cancellationToken);
                Console.WriteLine("Sports entities received:");
                foreach (var sport in marketsResults)
                {
                    Console.WriteLine($"SportId: {sport.Id}, SportName: {sport.Name}");
                }

                Console.WriteLine("Press any key to receive LSports sports entities");
                Console.ReadKey();

                var sportsResults = await _MetadataClient.GetSportsAsync(cancellationToken);
                Console.WriteLine("Sports entities received:");
                foreach (var sport in sportsResults)
                {
                    Console.WriteLine($"SportId: {sport.Id}, SportName: {sport.Name}");
                }

                Console.WriteLine("\nPress any key to receive LSports locations entities");
                Console.ReadKey();

                var locationsResults = await _MetadataClient.GetLocationsAsync(cancellationToken);
                Console.WriteLine("Locations entities received:");
                foreach (var location in locationsResults)
                {
                    Console.WriteLine($"LocationId: {location.Id}, LocationName: {location.Name}");
                }

                Console.WriteLine("\nPress any key to receive LSports leagues for football");
                Console.ReadKey();

                var footballSportEntity = sportsResults.FirstOrDefault(x => x.Name == "Football");
                if (footballSportEntity == null)
                {
                    Console.WriteLine("Football sport entity not found.");
                }
                else
                {
                    var getLeagueRequest = new GetLeaguesRequestDto()
                    {
                        SportIds = new List<int>(6046)
                    };
                    var leagueResults = await _MetadataClient.GetLeaguesAsync(getLeagueRequest, cancellationToken);
                    Console.WriteLine($"Response returned {leagueResults.Count()} leagues:");
                    foreach (var league in leagueResults)
                    {
                        Console.WriteLine($"LeagueId: {league.Id}, LeagueName: {league.Name}");
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving data");
            }
        }

        public async Task StopAsync(CancellationToken cancellationToken) 
        {
          
        }
    }
}
