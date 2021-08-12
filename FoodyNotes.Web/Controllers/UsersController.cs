using FoodyNotes.Infrastructure.Interfaces;
using FoodyNotes.UseCases;
using FoodyNotes.Web.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace FoodyNotes.Web.Controllers
{

  [Authorize]
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
    public IActionResult GetAll()
    {
      var users = _userService.GetAll();

      return Ok(users);
    }
  }
}