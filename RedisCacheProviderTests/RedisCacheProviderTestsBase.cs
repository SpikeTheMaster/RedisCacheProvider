using System;
using System.Threading;
using NUnit.Framework;
using RedisCacheProvider;
using RedisCacheProvider.CacheProviders;

namespace RedisCacheProviderTests
{
    public abstract class RedisCacheProviderTestsBase
    {
        private readonly ICacheProvider mProvider;
        protected abstract ICacheProvider GetCacheProvider();

        private Person mPerson;
        private DateTime expiryTime;

        public RedisCacheProviderTestsBase()
        {
            mProvider = GetCacheProvider();
        }

        private const string Name = "Frank";
        private const int Age = 91;

        [SetUp]
        public void Setup()
        {
            mPerson = new Person(Name, Age);
        }

        [Test]
        public void CanAddNewPerson()
        {
            mProvider.Add("Bob", new Person("Bob", 10)).Wait();
            Assert.IsTrue(true);
        }

        [Test]
        public void CanGetNewPerson()
        {
            mProvider.Add(Name, mPerson).Wait();
            Assert.IsTrue(mProvider.Get<Person>(Name).Result.Age == Age);
        }

        [Test]
        public void CanGetCachedPersonAndCacheExpires()
        {
            DateTime utcNow = DateTime.UtcNow;
            expiryTime = utcNow.AddSeconds(3);

            mProvider.Add(Name, mPerson, expiryTime).Wait();
            Assert.AreEqual(Name, mProvider.Get<Person>(Name).Result.Name);
            
            while (DateTime.UtcNow <= expiryTime.AddSeconds(1)) //Add a bit of leeway here
            {
                Thread.Sleep(100);
            }
            
            Assert.IsNull(mProvider.Get<Person>(Name).Result);
        }

        [TearDown]
        public void TearDown()
        {
            mProvider.ClearCache();
        }
    }

    public class RedisDataContractCacheProviderTests : RedisCacheProviderTestsBase
    {
        protected override ICacheProvider GetCacheProvider()
        {
            return RedisCacheProviderFactory.GetCacheProvider(new RedisConnectionSettings(),
                                                              SeralisationType.DataContract);
        }
    }

    public class RedisBinaryCacheProviderTests : RedisCacheProviderTestsBase
    {
        protected override ICacheProvider GetCacheProvider()
        {
            return RedisCacheProviderFactory.GetCacheProvider(new RedisConnectionSettings(), SeralisationType.Binary);
        }
    }
}
