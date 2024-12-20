using Microsoft.AspNetCore.Mvc;
using PullLeadsMicroService.Services;

namespace PullLeadsMicroService.Controllers
{
    public class PullController : ControllerBase
    {
        private readonly IPullService _pullService;

        public PullController(IPullService pullService)
        {
            _pullService = pullService;
        }

        [HttpPost("trigger")]
        public async Task<IActionResult> TriggerPull()
        {
            try
            {
                await _pullService.PullDataAsync();
                return Ok("Data pull triggered successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }
    }
}
