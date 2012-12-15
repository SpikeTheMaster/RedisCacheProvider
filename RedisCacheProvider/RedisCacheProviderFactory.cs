using System;
using RedisCacheProvider.CacheProviders;

namespace RedisCacheProvider
{
    public static class RedisCacheProviderFactory
    {
        public static ICacheProvider GetCacheProvider(RedisConnectionSettings connection, SeralisationType seralisationType)
        {
            switch (seralisationType)
            {
                case SeralisationType.Binary:
                    return new RedisBinaryCacheProvider(connection);
                case SeralisationType.DataContract:
                    return new RedisDataContractCacheProvider(connection);
                default:
                    throw new ArgumentOutOfRangeException("seralisationType");
            }
        }
    }
}
