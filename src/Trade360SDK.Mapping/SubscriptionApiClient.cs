using AutoMapper;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Trade360SDK.Api.Abstraction.Interfaces;
using Trade360SDK.Api.Abstraction.SubscriptionApi.Requests;
using Trade360SDK.Api.Abstraction.SubscriptionApi.Responses;
using Trade360SDK.Api.Common.Models.Requests.Base;
using Trade360SDK.Common;
using Trade360SDK.Common.Metadata.Responses;

namespace Trade360SDK.Api.Subscription
{
    public class SubscriptionApiClient : BaseHttpClient, ISubscriptionApiClient
    {
        private readonly IMapper _mapper;
        public SubscriptionApiClient(HttpClient httpClient, CustomersApiSettings settings, IMapper mapper)
            : base(httpClient, settings)
        {
            httpClient.BaseAddress = new System.Uri(settings.BaseUrl);
            _mapper = mapper;
        }

        public Task<PackageQuotaResponse> GetPackageQuotaAsync(CancellationToken cancelationToken)
            => GetEntityAsync<PackageQuotaResponse>("/package/GetPackageQuota", new BaseRequest(), cancelationToken);

        public async Task<FixtureScheduleCollectionResponse> GetInplayFixtureSchedule(GetFixtureScheduleRequestDto requestDto, CancellationToken cancellationToken)
        {
            var request = _mapper.Map<GetFixtureScheduleRequest>(requestDto);

            var response = await PostEntityAsync<FixtureScheduleCollectionResponse>(
                "Fixtures/InPlaySchedule",
                request,
                cancellationToken);
            return response;
        }

        public async Task<FixtureSubscribtionCollectionResponse> SubscribeByFixture(FixtureSubscriptionRequestDto requestDto, CancellationToken cancellationToken)
        {
            var request = _mapper.Map<FixtureSubscriptionRequest>(requestDto);

            var response = await PostEntityAsync<FixtureSubscribtionCollectionResponse>(
                "Fixtures/Subscribe",
                request,
                cancellationToken);
            return response;
        }

        public async Task<FixtureSubscribtionCollectionResponse> UnSubscribeByFixture(FixtureSubscriptionRequestDto requestDto, CancellationToken cancellationToken)
        {
            var request = _mapper.Map<FixtureSubscriptionRequest>(requestDto);

            var response = await PostEntityAsync<FixtureSubscribtionCollectionResponse>(
                "Fixtures/UnSubscribe",
                request,
                cancellationToken);
            return response;
        }

        public async Task<LeagueSubscribtionCollectionResponse> SubscribeByLeague(LeagueSubscriptionRequestDto requestDto, CancellationToken cancellationToken)
        {
            var request = _mapper.Map<LeagueSubscriptionRequest>(requestDto);

            var response = await PostEntityAsync<LeagueSubscribtionCollectionResponse>(
                "Leagues/Subscribe",
                request,
                cancellationToken);
            return response;
        }

        public async Task<LeagueSubscribtionCollectionResponse> UnSubscribeByLeague(LeagueSubscriptionRequestDto requestDto, CancellationToken cancellationToken)
        {
            var request = _mapper.Map<LeagueSubscriptionRequest>(requestDto);

            var response = await PostEntityAsync<LeagueSubscribtionCollectionResponse>(
                "Leagues/UnSubscribe",
                request,
                cancellationToken);
            return response;
        }

        public async Task<GetSubscriptionResponse> GetSubscritptions(GetSubscriptionRequestDto requestDto, CancellationToken cancellationToken)
        {
            var request = _mapper.Map<GetSubscriptionRequest>(requestDto);

            var response = await PostEntityAsync<GetSubscriptionResponse>(
                "Fixtures/Get",
                request,
                cancellationToken);
            return response;
        }

        public async Task<CompetitionSubscribtionCollectionResponse> SubscribeByCompetition(CompetitionSubscriptionRequestDto requestDto, CancellationToken cancellationToken)
        {
            var request = _mapper.Map<CompetitionSubscriptionRequest>(requestDto);

            var response = await PostEntityAsync<CompetitionSubscribtionCollectionResponse>(
                "Outright/Subscribe",
                request,
                cancellationToken);
            return response;
        }

        public async Task<CompetitionSubscribtionCollectionResponse> UnSubscribeByCompetition(CompetitionSubscriptionRequestDto requestDto, CancellationToken cancellationToken)
        {
            var request = _mapper.Map<CompetitionSubscriptionRequest>(requestDto);

            var response = await PostEntityAsync<CompetitionSubscribtionCollectionResponse>(
                "Outright/UnSubscribe",
                request,
                cancellationToken);
            return response;
        }

        public async Task<GetManualSuspensionResponse> GetAllManualSuspensions(CancellationToken cancellationToken)
        {
            var response = await PostEntityAsync<GetManualSuspensionResponse>(
                "Markets/ManualSuspension/GetAll",
                new BaseRequest(),
                cancellationToken);
            return response;
        }

        public async Task<ChangeManualSuspensionResponse> AddManualSuspension(ChangeManualSuspensionRequestDto requestDto, CancellationToken cancellationToken)
        {
            var request = _mapper.Map<ChangeManualSuspensionRequest>(requestDto);

            var response = await PostEntityAsync<ChangeManualSuspensionResponse>(
                "Markets/ManualSuspension/Activate",
                request,
                cancellationToken);
            return response;
        }

        public async Task<ChangeManualSuspensionResponse> RemoveManualSuspension(ChangeManualSuspensionRequestDto requestDto, CancellationToken cancellationToken)
        {
            var request = _mapper.Map<ChangeManualSuspensionRequest>(requestDto);

            var response = await PostEntityAsync<ChangeManualSuspensionResponse>(
                "Markets/ManualSuspension/Deactivate ",
                request,
                cancellationToken);
            return response;
        }
    }
}
