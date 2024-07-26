namespace Trade360SDK.Common.Configuration
{
    public class Trade360Settings
    {
        public string? CustomersApiBaseUrl { get; set; }
        public string? SnapshotApiBaseUrl { get; set; }
        public PackageCredentials? InplayPackageCredentials { get; set; }
        public PackageCredentials? PrematchPackageCredentials { get; set; }
    }

    public class PackageCredentials
    {
        public int PackageId { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }
    }
}
