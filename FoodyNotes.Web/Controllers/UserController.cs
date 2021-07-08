using FoodyNotes.UseCases;
using FoodyNotes.Web.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace FoodyNotes.Web.Controllers
{

  [Authorize]
  public class UserController : ControllerBase
  {
    private readonly IUserService _userService;

    public UserController(IUserService userService)
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