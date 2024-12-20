using Microsoft.EntityFrameworkCore;
using PullLeadsMicroService.Data;
using PullLeadsMicroService.Models;

namespace PullLeadsMicroService.Repositories
{
    public class LeadRepository : ILeadRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly ILogger<LeadRepository> _logger;

        public LeadRepository(ApplicationDbContext dbContext, ILogger<LeadRepository> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        /// <summary>
        /// Fetches all existing email addresses from the database.
        /// </summary>
        public async Task<List<string>> GetExistingEmailsAsync()
        {
            try
            {
                _logger.LogInformation("Fetching existing email addresses from the database.");
                return await _dbContext.Leads.Select(lead => lead.EmailAddress).ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError("Error fetching existing emails: {Message}", ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Bulk inserts new leads into the database.
        /// </summary>
        public async Task BulkInsertAsync(IEnumerable<Lead> leads)
        {
            try
            {
                _logger.LogInformation("Inserting {Count} new leads into the database.", leads.Count());

                await _dbContext.Leads.AddRangeAsync(leads);
                await _dbContext.SaveChangesAsync();

                _logger.LogInformation("Successfully inserted new leads into the database.");
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError("Database update error during bulk insert: {Message}", ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError("Unexpected error during bulk insert: {Message}", ex.Message);
                throw;
            }
        }
    }
}
