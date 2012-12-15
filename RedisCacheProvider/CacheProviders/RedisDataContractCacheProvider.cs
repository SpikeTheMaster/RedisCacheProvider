using System;
using System.IO;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using System.Xml;

namespace RedisCacheProvider.CacheProviders
{
    /// <summary>
    /// Uses DataContract serialisation to store .NET Objects.
    /// *IMPORTANT* Must have DataContract + appropriate Member Attributes.
    /// </summary>
    public class RedisDataContractCacheProvider : RedisCacheProviderBase
    {
        internal RedisDataContractCacheProvider(RedisConnectionSettings settings)
            : base(settings)
        {
        }

        public override async Task<T> Get<T>(string key)
        {
            Open();
            DataContractSerializer serializer = new DataContractSerializer(typeof (T));

            byte[] base64String = await Redis.Strings.Get(RedisDBNumber, key);

            if (base64String == null)
                return null; //Key not found.

            return
                serializer.ReadObject(new XmlTextReader(new MemoryStream(GetBytesFromBase64EncodedBytes(base64String))))
                as T;
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
            DataContractSerializer serializer = new DataContractSerializer(typeof (T));
            MemoryStream ms = new MemoryStream();
            serializer.WriteObject(ms, entry);

            if (utcExpiry != null)
                await Redis.Strings.Set(RedisDBNumber, key, Convert.ToBase64String(ms.ToArray()),
                                        Helpers.GetExpiryTimeFromNow(utcExpiry));
            else
                await Redis.Strings.Set(RedisDBNumber, key, Convert.ToBase64String(ms.ToArray()));
        }
    }
}
