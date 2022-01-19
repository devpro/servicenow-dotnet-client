using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using RabbidsIncubator.ServiceNowClient.Domain.Repositories;

namespace RabbidsIncubator.ServiceNowClient.Infrastructure.InMemory.Repositories
{
    /// <summary>
    /// In-memory cache implementation.
    /// </summary>
    /// <remarks>
    /// See https://docs.microsoft.com/en-us/aspnet/core/performance/caching/memory.
    /// </remarks>
    public class InMemoryCacheRepository : ICacheRepository
    {
        private readonly IMemoryCache _memoryCache;

        private readonly InMemoryConfiguration _configuration;

        public InMemoryCacheRepository(IMemoryCache memoryCache, InMemoryConfiguration configuration)
        {
            _memoryCache = memoryCache;
            _configuration = configuration;
        }

        public void Clean(string key)
        {
            throw new NotImplementedException();
        }

        public void CleanAll()
        {
            throw new NotImplementedException();
        }

        public bool Contains<T>(string key)
        {
            return !_memoryCache.TryGetValue<T>(key, out _);
        }

        public T? Get<T>(string key) where T : class
        {
            if (!_memoryCache.TryGetValue<T>(key, out var cacheEntry))
            {
                return default;
            }
            return cacheEntry;
        }

        public Task<T> GetOrSetAsync<T>(string key, Func<ICacheEntry, Task<T>> method)
        {
            return _memoryCache.GetOrCreateAsync(key, method);
        }

        public void Set<T>(string key, T value)
        {
            var cacheEntryOptions = new MemoryCacheEntryOptions()
                .SetSlidingExpiration(TimeSpan.FromSeconds(_configuration.GeneralTimeoutInSeconds));

            _memoryCache.Set(key, value, cacheEntryOptions);
        }
    }
}
