using System.Threading.Tasks;
using Authentication.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Authentication.Web.Controllers
{

  // [Authorize]
  [Route("api/[controller]")]
  [ApiController]
  public class UsersController : ControllerBase
  {
    private readonly IUserService _userService;

    public UsersController(IUserService userService)
    {
      _userService = userService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
      var users = await _userService.GetAllAsync();

      return Ok(users);
    }
  }
}