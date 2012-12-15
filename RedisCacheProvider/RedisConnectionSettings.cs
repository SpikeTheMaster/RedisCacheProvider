namespace RedisCacheProvider
{
    public class RedisConnectionSettings
    {
        public string Host { get; set; }
        public int Port { get; set; }
        public int Timeout { get; set; }
        public string Password { get; set; }
        public int MaxUnsent { get; set; }
        public int SyncTimeout { get; set; }

        public RedisConnectionSettings()
        {
            Host = Properties.Settings.Default.RedisHost;
            Port = Properties.Settings.Default.RedisPort;
            Timeout = Properties.Settings.Default.RedisTimeout;
            Password = Properties.Settings.Default.RedisPassword;
            MaxUnsent = Properties.Settings.Default.RedisMaxUnsent;
            SyncTimeout = Properties.Settings.Default.RedisSyncTimeout;
        }
    }
}