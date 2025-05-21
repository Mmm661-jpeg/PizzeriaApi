using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace PizzeriaApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly ILogger<TestController> _logger;
        public TestController(ILogger<TestController> logger)
        {
            _logger = logger;
        }

        [HttpGet("test-auth")]
        public IActionResult TestAuth()
        {
            if (User.Identity?.IsAuthenticated == true)
                return Ok(User.Claims.Select(c => new { c.Type, c.Value }));
            else
                return Unauthorized();
        }

        [HttpGet("test-logging")]
        
        public IActionResult TestLogging()
        {
            _logger.LogInformation("Test logging endpoint hit");
            _logger.LogInformation("Hello from Azure at {Time}", DateTime.UtcNow);
            _logger.LogWarning("Test warning");
            _logger.LogError("Test error");
            return Ok("Logging is being tested");
        }
    }

    
}
