using EventsService.Models;
using Microsoft.AspNetCore.Mvc;

namespace EventsService.Controllers
{
    [ApiController]
    [Route("api/events")]
    public class HealthController : Controller
    {
        /// <summary>
        /// Health check
        /// </summary>
        /// <response code="200">Health check passed</response>
        [HttpGet("health")]
        public IActionResult Health()
        {
            return Ok(new HealthCheckResponse { status = true });
        }
    }
}
