using ExternalAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace ExternalAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LeadsController : ControllerBase
    {
        private readonly LeadService _leadService;

        public LeadsController()
        {
            _leadService = new LeadService();
        }

        [HttpGet]
        public IActionResult GetLeads()
        {
            try
            {
                var leads = _leadService.GetLeads();

                if (leads == null || !leads.Any())
                {
                    return NotFound("No leads available.");
                }

                return Ok(leads);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An unexpected error occurred: {ex.Message}");
            }
        }
    }
}
