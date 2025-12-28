using Microsoft.AspNetCore.Mvc;

namespace MoviesProxy.Controllers
{
    [ApiController]
    public class HealthController : Controller
    {
        /// <summary>
        /// Health check
        /// </summary>
        /// <response code="200">Health check passed</response>
        [HttpGet("health")]
        public IActionResult Health()
        {
            return Ok();
        }
    }
}
