using System.Threading;
using System.Threading.Tasks;
using Trade360SDK.Api.Abstraction.SubscriptionApi.Requests;
using Trade360SDK.Api.Abstraction.SubscriptionApi.Responses;
using Trade360SDK.Common.Metadata.Responses;

namespace Trade360SDK.Api.Abstraction.Interfaces
{
    public interface ISubscriptionApiClient
    {
        Task<PackageQuotaResponse> GetPackageQuotaAsync(CancellationToken cancellationToken);
        Task<FixtureScheduleCollectionResponse> GetInplayFixtureSchedule(GetFixtureScheduleRequestDto requestDto, CancellationToken cancellationToken);
        Task<FixtureSubscribtionCollectionResponse> SubscribeByFixture(FixtureSubscriptionRequestDto requestDto, CancellationToken cancellationToken);
        Task<FixtureSubscribtionCollectionResponse> UnSubscribeByFixture(FixtureSubscriptionRequestDto requestDto, CancellationToken cancellationToken);
        Task<LeagueSubscribtionCollectionResponse> SubscribeByLeague(LeagueSubscriptionRequestDto requestDto, CancellationToken cancellationToken);
        Task<LeagueSubscribtionCollectionResponse> UnSubscribeByLeague(LeagueSubscriptionRequestDto requestDto, CancellationToken cancellationToken);
        Task<GetSubscriptionResponse> GetSubscritptions(GetSubscriptionRequestDto requestDto, CancellationToken cancellationToken);
        Task<CompetitionSubscribtionCollectionResponse> SubscribeByCompetition(CompetitionSubscriptionRequestDto requestDto, CancellationToken cancellationToken);
        Task<CompetitionSubscribtionCollectionResponse> UnSubscribeByCompetition(CompetitionSubscriptionRequestDto requestDto, CancellationToken cancellationToken);
        Task<GetManualSuspensionResponse> GetAllManualSuspensions(CancellationToken cancellationToken);
        Task<ChangeManualSuspensionResponse> AddManualSuspension(ChangeManualSuspensionRequestDto requestDto, CancellationToken cancellationToken);
        Task<ChangeManualSuspensionResponse> RemoveManualSuspension(ChangeManualSuspensionRequestDto requestDto, CancellationToken cancellationToken);
    }
}