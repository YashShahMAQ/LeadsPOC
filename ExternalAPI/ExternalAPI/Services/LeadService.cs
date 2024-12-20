using ExternalAPI.Models;
using Newtonsoft.Json;

namespace ExternalAPI.Services
{
    public class LeadService
    {
        private readonly string _filePath = Path.Combine(Directory.GetCurrentDirectory(), "Data", "leads.json");

        public List<Lead> GetLeads()
        {
            try
            {
                if (!File.Exists(_filePath))
                    return new List<Lead>();

                var jsonData = File.ReadAllText(_filePath);
                return JsonConvert.DeserializeObject<List<Lead>>(jsonData) ?? new List<Lead>();
            }
            catch (Exception ex)
            {
                throw new Exception($"An unexpected error occurred while retrieving leads: {ex.Message}");
            }
        }
    }
}
