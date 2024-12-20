using Newtonsoft.Json;
using PullLeadsMicroService.Models;
using PullLeadsMicroService.Repositories;

namespace PullLeadsMicroService.Services
{
    public class PullService : IPullService
    {
        private readonly ILogger<PullService> _logger;
        private readonly HttpClient _httpClient;
        private readonly ILeadRepository _leadRepository;
        private readonly string _externalApiUrl;
        private readonly string _apiKey;
        public PullService(
            ILogger<PullService> logger,
            IConfiguration configuration,
            ILeadRepository leadRepository)
        {
            _logger = logger;
            _leadRepository = leadRepository;

            _httpClient = new HttpClient();

            // Read values directly from appsettings.json
            _externalApiUrl = configuration["ExternalApi:Url"];
            _apiKey = configuration["ExternalApi:ApiKey"];

            // Add API key to HTTP headers
            _httpClient.DefaultRequestHeaders.Add("X-Api-Key", _apiKey);
        }
        public async Task PullDataAsync()
        {
            try
            {
                _logger.LogInformation("Starting data pull from External API at {Time}.", DateTime.UtcNow);

                // Fetch data from the External API
                var response = await _httpClient.GetAsync(_externalApiUrl);
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();
                var leads = JsonConvert.DeserializeObject<List<Lead>>(content);

                if (leads == null)
                {
                    _logger.LogWarning("No leads were fetched from the External API.");
                    return;
                }

                _logger.LogInformation("Fetched {Count} leads from the External API.", leads.Count);

                // Fetch existing emails from the database
                var existingEmails = await _leadRepository.GetExistingEmailsAsync();

                // Filter out duplicates based on EmailAddress
                List<Lead> newLeads = leads.Where(lead => !existingEmails.Contains(lead.EmailAddress))
                                           .Select(lead => new Lead
                                           {
                                               EmailAddress = lead.EmailAddress,
                                               ContactPerson = lead.ContactPerson,
                                               InterestArea = lead.InterestArea,
                                           })
                                           .ToList();

                if (newLeads.Any())
                {
                    // Insert new leads into the database
                    await _leadRepository.BulkInsertAsync(newLeads);
                    _logger.LogInformation("Inserted {Count} new leads into the database.", newLeads.Count);
                }
                else
                {
                    _logger.LogInformation("No new leads to insert.");
                }
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError("HTTP error while fetching data from External API: {Message}", ex.Message);
                throw;
            }
            catch (JsonException ex)
            {
                _logger.LogError("Error deserializing API response: {Message}", ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError("Unexpected error in PullService: {Message}", ex.Message);
                throw;
            }
        }
    }
}
