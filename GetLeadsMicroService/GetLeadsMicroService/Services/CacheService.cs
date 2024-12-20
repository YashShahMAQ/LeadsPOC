using GetLeadsMicroService.Models;
using Microsoft.Extensions.Caching.Memory;
using System.Collections.Concurrent;

namespace GetLeadsMicroService.Services
{
    public class CacheService : ICacheService
    {
        private readonly ILogger<CacheService> _logger;

        // In-memory cache: Key = InterestArea, Value = CacheEntry
        private readonly ConcurrentDictionary<string, CacheEntry> _cache = new();

        public CacheService(ILogger<CacheService> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<List<Lead>> GetCachedLeadsAsync(string key)
        {
            try
            {
                if (_cache.TryGetValue(key, out var cacheEntry))
                {
                    // Check expiration
                    if (cacheEntry.ExpirationTime > DateTime.UtcNow)
                    {
                        _logger.LogInformation("Cache hit for key: {Key}", key);
                        return await Task.FromResult(cacheEntry.Data);
                    }

                    // Cache expired
                    _logger.LogWarning("Cache expired for key: {Key}", key);
                    _cache.TryRemove(key, out _);
                }
                else
                {
                    _logger.LogWarning("Cache miss for key: {Key}", key);
                }

                return await Task.FromResult<List<Lead>>(null); // Cache miss or expired
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting cached leads for key: {Key}", key);
                throw;
            }
        }

        public async Task SetCacheAsync(string key, List<Lead> value, TimeSpan ttl)
        {
            try
            {
                var expirationTime = DateTime.UtcNow.Add(ttl);
                var cacheEntry = new CacheEntry
                {
                    Data = value,
                    ExpirationTime = expirationTime
                };

                _cache[key] = cacheEntry; // Add or update the cache

                _logger.LogInformation("Cache set for key: {Key} with TTL: {TTL} seconds", key, ttl.TotalSeconds);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while setting cache for key: {Key}", key);
                throw;
            }
        }

        public void RemoveExpiredCache()
        {
            try
            {
                var expiredKeys = _cache
                    .Where(entry => entry.Value.ExpirationTime <= DateTime.UtcNow)
                    .Select(entry => entry.Key)
                    .ToList();

                foreach (var key in expiredKeys)
                {
                    _cache.TryRemove(key, out _);
                    _logger.LogInformation("Removed expired cache for key: {Key}", key);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while removing expired cache entries");
                throw;
            }
        }

        private class CacheEntry
        {
            public List<Lead> Data { get; set; }
            public DateTime ExpirationTime { get; set; }
        }
    }
}
