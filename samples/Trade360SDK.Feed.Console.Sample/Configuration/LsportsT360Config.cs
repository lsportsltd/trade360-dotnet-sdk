namespace Trade360SDK.Feed.Example.Configuration
{
    public class LsportsT360Config
    {
        public RmqConnection InPlayRmqConnection { get; set; }
        public RmqConnection PreMatchRmqConnection { get; set; }
        public AccountConfig Account { get; set; }
        public string CustomersApiUrl { get; set; }
    }

    public class RmqConnection
    {
        public string RabbitmqHost { get; set; }
        public int RmqPort { get; set; }
        public string RmqVHost { get; set; }
    }

    public class AccountConfig
    {
        public int InPlayPackageId { get; set; }
        public int PreMatchPackageId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
