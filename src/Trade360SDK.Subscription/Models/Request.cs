namespace Trade360SDK.Subscription.Models
{
    internal class Request
    {
        public int PackageId { get; }
        public string UserName { get; }
        public string Password { get; }

        public Request(int packageId, string userName, string password)
        {
            PackageId = packageId;
            UserName = userName;
            Password = password;
        }
    }
}
