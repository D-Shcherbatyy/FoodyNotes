using System.Threading.Tasks;
using Authentication.Infrastructure.Interfaces.Authentication.Dtos;
using Authentication.UseCases.Authentication.Commands;
using Authentication.Web.Attributes;
using Authentication.Web.Services;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Authentication.Web.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class AuthController : ControllerBase
  {
    private readonly IMediator _mediator;
    private readonly HttpService _httpService;

    public AuthController(IMediator mediator, HttpService httpService)
    {
      _mediator = mediator;
      _httpService = httpService;
    }
    
    private string IpAddress => _httpService.GetIpAddress(Request, HttpContext);

    [HttpPost("authenticate")]
    public async Task<IActionResult> Authenticate([FromBody] AuthenticateRequestDto model)
    {
      var response = await _mediator.Send(new AuthenticateCommand { RequestDto = model, IpAddress = IpAddress });
      
      _httpService.SetTokenCookie(Response, response.RefreshToken);
      
      return Ok(response);
    }

    [AllowAnonymous]
    [HttpPost("refresh-token")]
    public async Task<IActionResult> RefreshToken()
    {
      var refreshToken = Request.Cookies["refreshToken"];

      var response = await _mediator.Send(new RefreshTokenCommand { CurrentRefreshToken = refreshToken, IpAddress = IpAddress });

      _httpService.SetTokenCookie(Response, response.RefreshToken.Token);

      return Ok(response.JwtToken);
    }

    [HttpPost("revoke-token")]
    public async Task<IActionResult> RevokeToken(RevokeTokenRequestDto model)
    {
      // accept refresh token in request body or cookie
      var refreshToken = model.RefreshToken ?? Request.Cookies["refreshToken"];

      if (string.IsNullOrEmpty(refreshToken))
        return BadRequest(new { message = "Token is required" });

      await _mediator.Send(new RevokeTokenCommand { RefreshToken = refreshToken, IpAddress = IpAddress });

      return Ok(new { message = "Token revoked" });
    }

  }
}