using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;

namespace RedisCacheProvider
{
    /// <summary>
    /// Stores .NET Objects in Redis.
    /// *IMPORTANT* Stored Objects must implement ISerializable as the person class does in the tests,
    /// If you do not want to write this implementation then consider using the DataContract version of this class.
    /// </summary>
    public class RedisBinaryCacheProvider : RedisCacheProviderBase
    {
        private BinaryFormatter mBFormatter;

        internal RedisBinaryCacheProvider(RedisConnectionSettings connection)
            : base(connection)
        {
            mBFormatter = new BinaryFormatter();
        }

        public override async Task<T> Get<T>(string key)
        {
            Open();
            byte[] base64String = await Redis.Strings.Get(RedisDBNumber, key);

            if (base64String == null)
                return null; //Key not found.

            MemoryStream s = new MemoryStream(GetBytesFromBase64EncodedBytes(base64String));

            return mBFormatter.Deserialize(s) as T;
        }

        public override async Task Add<T>(string key, T entry, DateTime? utcExpiry = null)
        {
            Open();
            await AddOrSet(key, entry, utcExpiry);
        }

        public override async Task Set<T>(string key, T entry, DateTime? utcExpiry = null)
        {
            Open();
            await AddOrSet(key, entry, utcExpiry);
        }

        private async Task AddOrSet<T>(string key, T entry, DateTime? utcExpiry = null) where T : class
        {
            MemoryStream ms = new MemoryStream();
            mBFormatter.Serialize(ms, entry);

            if (utcExpiry != null)
                await Redis.Strings.Set(RedisDBNumber, key, Convert.ToBase64String(ms.ToArray()),
                                                  Helpers.GetExpiryTimeFromNow(utcExpiry));
            else
                await Redis.Strings.Set(RedisDBNumber, key, Convert.ToBase64String(ms.ToArray()));
        }
    }
}
