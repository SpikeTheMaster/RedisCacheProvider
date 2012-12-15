using System;
using System.Text;
using System.Threading.Tasks;
using BookSleeve;

namespace RedisCacheProvider
{
    public abstract class RedisCacheProviderBase : ICacheProvider
    {
        protected RedisConnection Redis;

        internal RedisCacheProviderBase(RedisConnectionSettings connection)
        {
            Redis = new RedisConnection(connection.Host,
                                                  connection.Port,
                                                  connection.Timeout,
                                                  connection.Password,
                                                  connection.MaxUnsent,
                                                  true, //Required to flush DB
                                                  connection.SyncTimeout);
        }

        protected int RedisDBNumber { get; private set; }
        public abstract Task<T> Get<T>(string key) where T : class;
        public abstract Task Add<T>(string key, T entry, DateTime? utcExpiry = null) where T : class;
        public abstract Task Set<T>(string key, T entry, DateTime? utcExpiry = null) where T : class;
        
        public void ClearCache()
        {
            Redis.Server.FlushDb(RedisDBNumber);
        }

        public async Task Remove<T>(string key) where T : class
        {
            await Redis.Keys.Remove(RedisDBNumber, key);
        }

        protected void Open()
        {
            if (Redis.State != RedisConnectionBase.ConnectionState.Open)
                Redis.Open();
        }

        protected byte[] GetBytesFromBase64EncodedBytes(byte[] base64String)
        {
            return Convert.FromBase64String(Encoding.ASCII.GetString(base64String));
        }
    }
}