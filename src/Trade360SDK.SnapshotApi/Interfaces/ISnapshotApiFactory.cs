using Trade360SDK.SnapshotApi.Configuration;

namespace Trade360SDK.SnapshotApi.Interfaces
{
    public interface ISnapshotApiFactory
    {
        ISnapshotInplayApiClient CreateInplayHttpClient(SnapshotApiSettings settings);
        ISnapshotPrematchApiClient CreatePrematchHttpClient(SnapshotApiSettings settings);
    }
}