namespace Trade360SDK.Feed.Configuration
{
    public class RmqConnectionSettings
    {
        public string Host { get; set; }
        public int Port { get; set; }
        public string VirtualHost { get; set; }
        public int PackageId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public ushort PrefetchCount { get; set; } = 100; // Default 100;
        public bool DispatchConsumersAsync { get; set; } = true; // Default true;
        public bool AutomaticRecoveryEnabled { get; set; } = true; // Default true;
        public bool AutoAck { get; set; } = true; // Default true;
        public int RequestedHeartbeatSeconds { get; set; } = 30; // Default 30 seconds
        public int NetworkRecoveryInterval { get; set; } = 30; // Default 30 seconds
    }

}
