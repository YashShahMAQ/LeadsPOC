using GetLeadsMicroService.Models;

namespace GetLeadsMicroService.Services
{
    public interface ILeadService
    {
        Task<List<Lead>> GetLeadsAsync(string sortParam, string searchParam);
    }
}
