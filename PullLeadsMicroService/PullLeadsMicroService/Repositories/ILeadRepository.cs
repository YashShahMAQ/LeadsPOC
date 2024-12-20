using PullLeadsMicroService.Models;

namespace PullLeadsMicroService.Repositories
{
    public interface ILeadRepository
    {
        /// <summary>
        /// Fetches all existing email addresses from the database.
        /// </summary>
        Task<List<string>> GetExistingEmailsAsync();

        /// <summary>
        /// Bulk inserts new leads into the database.
        /// </summary>
        /// <param name="leads">List of leads to insert.</param>
        Task BulkInsertAsync(IEnumerable<Lead> leads);
    }
}
