using GetLeadsMicroService.Models;

namespace GetLeadsMicroService.Repositories
{
    public interface ILeadRepository
    {
        Task<List<Lead>> GetLeadsAsync(string sortParam, string searchParam);
    }
}
