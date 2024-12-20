using GetLeadsMicroService.Models;
using GetLeadsMicroService.Repositories;

namespace GetLeadsMicroService.Services
{
    public class LeadService : ILeadService
    {
        private readonly ILeadRepository _leadRepository;
        private readonly ICacheService _cacheService;
        private readonly ILogger<LeadService> _logger;

        public LeadService(
            ILeadRepository leadRepository,
            ICacheService cacheService,
            ILogger<LeadService> logger)
        {
            _leadRepository = leadRepository ?? throw new ArgumentNullException(nameof(leadRepository));
            _cacheService = cacheService ?? throw new ArgumentNullException(nameof(cacheService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<List<Lead>> GetLeadsAsync(string sortParam, string searchParam)
        {
            try
            {
                if (string.IsNullOrEmpty(searchParam))
                {
                    throw new ArgumentException("Search parameter (InterestArea) must not be null or empty.");
                }

                _logger.LogInformation("Fetching leads for InterestArea: {SearchParam}, with sorting: {SortParam}", searchParam, sortParam);

                // Check Cache
                var cachedLeads = await _cacheService.GetCachedLeadsAsync(searchParam);
                if (cachedLeads != null)
                {
                    _logger.LogInformation("Returning leads from cache for InterestArea: {SearchParam}", searchParam);
                    return cachedLeads;
                }

                // Fetch from Database if not in Cache
                var leadsFromDb = await _leadRepository.GetLeadsAsync(sortParam, searchParam);

                // Store in Cache
                if (leadsFromDb.Any())
                {
                    var ttl = TimeSpan.FromMinutes(10); // Cache TTL is 10 minutes
                    await _cacheService.SetCacheAsync(searchParam, leadsFromDb, ttl);
                    _logger.LogInformation("Stored leads in cache for InterestArea: {SearchParam} with TTL: {TTL} minutes.", searchParam, ttl.TotalMinutes);
                }
                else
                {
                    _logger.LogWarning("No leads found in database for InterestArea: {SearchParam}", searchParam);
                }

                return leadsFromDb;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching leads.");
                throw new Exception("An error occurred while fetching leads. Please try again later.", ex);
            }
        }
    }
}
