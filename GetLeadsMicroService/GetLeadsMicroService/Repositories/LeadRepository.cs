using GetLeadsMicroService.Data;
using GetLeadsMicroService.Models;
using Microsoft.EntityFrameworkCore;

namespace GetLeadsMicroService.Repositories
{
    public class LeadRepository : ILeadRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly ILogger<LeadRepository> _logger;

        public LeadRepository(ApplicationDbContext dbContext, ILogger<LeadRepository> logger)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        public async Task<List<Lead>> GetLeadsAsync(string sortParam, string searchParam)
        {
            try
            {
                _logger.LogInformation("Fetching leads from the database...");

                // Start building the query
                IQueryable<Lead> query = _dbContext.Leads;

                // Filter by InterestArea if provided
                if (!string.IsNullOrEmpty(searchParam))
                {
                    _logger.LogInformation("Applying filter for InterestArea: {searchParam}", searchParam);
                    query = query.Where(l => l.InterestArea.ToLower() == searchParam.ToLower());
                }

                // Apply sorting based on EmailAddress
                if (sortParam?.ToLower() == "asc")
                {
                    _logger.LogInformation("Sorting leads by EmailAddress in ascending order.");
                    query = query.OrderBy(l => l.EmailAddress);
                }
                else if (sortParam?.ToLower() == "desc")
                {
                    _logger.LogInformation("Sorting leads by EmailAddress in descending order.");
                    query = query.OrderByDescending(l => l.EmailAddress);
                }
                else
                {
                    _logger.LogWarning("No valid sort parameter provided. Returning unsorted results.");
                }

                var leads = await query.ToListAsync();

                // Null or empty check
                if (leads == null || leads.Count == 0)
                {
                    _logger.LogWarning("No leads found in the database for the given filters.");
                    return new List<Lead>();
                }

                _logger.LogInformation("Successfully fetched {Count} leads from the database.", leads.Count);
                return leads;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching leads from the database.");
                throw new Exception("An error occurred while fetching leads. Please try again later.", ex);
            }
        }
    }
}
