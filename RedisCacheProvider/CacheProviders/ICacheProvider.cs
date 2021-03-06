using System;
using System.Threading.Tasks;

namespace RedisCacheProvider.CacheProviders
{
    public interface ICacheProvider
    {
        Task<T> Get<T>(string key) where T : class;
        Task Add<T>(string key, T entry, DateTime? utcExpiry = null) where T : class;
        Task Set<T>(string key, T entry, DateTime? utcExpiry = null) where T : class;
        Task Remove<T>(string key) where T : class;

        void ClearCache();
    }
}