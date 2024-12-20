using GetLeadsMicroService.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GetLeadsMicroService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LeadsController : ControllerBase
    {
        private readonly ILeadService _leadService;
        private readonly ILogger<LeadsController> _logger;

        public LeadsController(ILeadService leadService, ILogger<LeadsController> logger)
        {
            _leadService = leadService ?? throw new ArgumentNullException(nameof(leadService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet("getleads")]
        [Authorize] // Secure endpoint with JWT Authentication
        public async Task<IActionResult> GetLeads([FromQuery] string sortParam, [FromQuery] string searchParam)
        {
            try
            {
                _logger.LogInformation("Received request to fetch leads with sortParam: {SortParam} and searchParam: {SearchParam}", sortParam, searchParam);

                var leads = await _leadService.GetLeadsAsync(sortParam, searchParam);

                if (leads == null || leads.Count == 0)
                {
                    _logger.LogWarning("No leads found for searchParam: {SearchParam}", searchParam);
                    return NotFound("No leads found for the given criteria.");
                }

                _logger.LogInformation("Successfully retrieved {Count} leads for searchParam: {SearchParam}", leads.Count, searchParam);
                return Ok(leads);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred while fetching leads.");
                return StatusCode(500, new { Error = ex.Message });
            }
        }
    }
}
