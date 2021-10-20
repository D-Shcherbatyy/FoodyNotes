using System.Threading.Tasks;
using FoodyNotes.Infrastructure.Interfaces;
using FoodyNotes.Web.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace FoodyNotes.Web.Controllers
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