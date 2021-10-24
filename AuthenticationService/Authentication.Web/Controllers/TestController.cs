using Authentication.Infrastructure.Implementation;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

// using Serilog;

namespace Authentication.Web.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class TestController : ControllerBase
  {
    private readonly ILogger _logger;
    private readonly IWebHostEnvironment _environment;
    private readonly AppSettings _appSettings;

    public TestController(ILogger<TestController> logger, IWebHostEnvironment environment, IOptions<AppSettings> options)
    {
      _logger = logger;
      _environment = environment;
      _appSettings = options.Value;
    }
    
    [HttpGet]
    public IActionResult Test()
    {
      _logger.Log(LogLevel.Information, "----- Test");
      
      var response = new
      {
        _environment.EnvironmentName,
        _appSettings.Secret,
        _appSettings.RefreshTokenTTL
      };
      
      return Ok(response);
    }
  }
}