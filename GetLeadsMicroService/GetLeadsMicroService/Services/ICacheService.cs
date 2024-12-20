using GetLeadsMicroService.Models;

namespace GetLeadsMicroService.Services
{
    public interface ICacheService
    {
        Task<List<Lead>> GetCachedLeadsAsync(string key);
        Task SetCacheAsync(string key, List<Lead> value, TimeSpan ttl);
        void RemoveExpiredCache();
    }
}
