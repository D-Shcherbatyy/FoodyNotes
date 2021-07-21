using FoodyNotes.Infrastructure.Interfaces.Authentication;
using FoodyNotes.Infrastructure.Interfaces.Authentication.Dtos;
using FoodyNotes.Web.Attributes;
using FoodyNotes.Web.Models;
using FoodyNotes.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace FoodyNotes.Web.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class AuthController : ControllerBase
  {
    private readonly IAuthService _authService;
    private readonly ITokenService _tokenService;
    private readonly HttpService _httpService;

    public AuthController(IAuthService authService, ITokenService tokenService, HttpService httpService)
    {
      _authService = authService;
      _tokenService = tokenService;
      _httpService = httpService;
    }
    
    private string IpAddress => _httpService.GetIpAddress(Request, HttpContext);

    [HttpPost("authenticate")]
    public IActionResult Authenticate([FromBody] AuthenticateInDto model)
    {
      var response = _authService.Authenticate(model, IpAddress);
      
      _httpService.SetTokenCookie(Response, response.RefreshToken);
      
      return Ok(response);
    }

    [AllowAnonymous]
    [HttpPost("refresh-token")]
    public IActionResult RefreshToken()
    {
      var refreshToken = Request.Cookies["refreshToken"];
      var response = _tokenService.RefreshToken(refreshToken, IpAddress);
      _httpService.SetTokenCookie(Response, response.RefreshToken);

      return Ok(response);
    }

    [HttpPost("revoke-token")]
    public IActionResult RevokeToken(RevokeTokenRequest model)
    {
      // accept refresh token in request body or cookie
      var token = model.Token ?? Request.Cookies["refreshToken"];

      if (string.IsNullOrEmpty(token))
        return BadRequest(new { message = "Token is required" });

      _tokenService.RevokeToken(token, IpAddress);

      return Ok(new { message = "Token revoked" });
    }

  }
}