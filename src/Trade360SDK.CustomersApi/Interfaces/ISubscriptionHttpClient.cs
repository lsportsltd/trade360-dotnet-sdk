using System.Threading;
using System.Threading.Tasks;
using Trade360SDK.CustomersApi.Entities.MetadataApi.Requests;
using Trade360SDK.CustomersApi.Entities.MetadataApi.Responses;
using Trade360SDK.CustomersApi.Entities.SubscriptionApi.Requests;
using Trade360SDK.CustomersApi.Entities.SubscriptionApi.Responses;

namespace Trade360SDK.CustomersApi.Interfaces
{
    public interface ISubscriptionHttpClient
    {
        Task<GetFixtureMetadataCollectionResponse> GetFixtureMetadataAsync(GetFixtureMetadataRequestDto requestDto, CancellationToken cancellationToken);
        Task<PackageQuotaResponse> GetPackageQuotaAsync(CancellationToken cancellationToken);
        Task<FixtureScheduleCollectionResponse> GetInplayFixtureSchedule(GetFixtureScheduleRequestDto requestDto, CancellationToken cancellationToken);
        Task<FixtureSubscriptionCollectionResponse> SubscribeByFixture(FixtureSubscriptionRequestDto requestDto, CancellationToken cancellationToken);
        Task<FixtureSubscriptionCollectionResponse> UnSubscribeByFixture(FixtureSubscriptionRequestDto requestDto, CancellationToken cancellationToken);
        Task<LeagueSubscriptionCollectionResponse> SubscribeByLeague(LeagueSubscriptionRequestDto requestDto, CancellationToken cancellationToken);
        Task<LeagueSubscriptionCollectionResponse> UnSubscribeByLeague(LeagueSubscriptionRequestDto requestDto, CancellationToken cancellationToken);
        Task<GetSubscriptionResponse> GetSubscriptions(GetSubscriptionRequestDto requestDto, CancellationToken cancellationToken);
        Task<CompetitionSubscriptionCollectionResponse> SubscribeByCompetition(CompetitionSubscriptionRequestDto requestDto, CancellationToken cancellationToken);
        Task<CompetitionSubscriptionCollectionResponse> UnSubscribeByCompetition(CompetitionSubscriptionRequestDto requestDto, CancellationToken cancellationToken);
        Task<GetManualSuspensionResponse> GetAllManualSuspensions(CancellationToken cancellationToken);
        Task<ChangeManualSuspensionResponse> AddManualSuspension(ChangeManualSuspensionRequestDto requestDto, CancellationToken cancellationToken);
        Task<ChangeManualSuspensionResponse> RemoveManualSuspension(ChangeManualSuspensionRequestDto requestDto, CancellationToken cancellationToken);
    }
}