using System.Threading.Tasks;
using FoodyNotes.Infrastructure.Interfaces.Authentication;
using FoodyNotes.Infrastructure.Interfaces.Authentication.Dtos;
using FoodyNotes.UseCases.Authentication.Commands;
using FoodyNotes.Web.Attributes;
using FoodyNotes.Web.Models;
using FoodyNotes.Web.Services;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FoodyNotes.Web.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class AuthController : ControllerBase
  {
    private readonly IMediator _mediator;
    private readonly IAuthService _authService;
    private readonly ITokenService _tokenService;
    private readonly HttpService _httpService;

    public AuthController(IMediator mediator, IAuthService authService, ITokenService tokenService, HttpService httpService)
    {
      _mediator = mediator;
      _authService = authService;
      _tokenService = tokenService;
      _httpService = httpService;
    }
    
    private string IpAddress => _httpService.GetIpAddress(Request, HttpContext);

    [HttpPost("authenticate")]
    public async Task<IActionResult> Authenticate([FromBody] AuthenticateRequestDto model)
    {
      var response = await _mediator.Send(new AuthenticateCommand { RequestDto = model, IpAddress = IpAddress });
      //var response = await _authService.Authenticate(model, IpAddress);
      
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

      return Ok();
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