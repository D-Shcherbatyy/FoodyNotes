using System;
using FoodyNotes.Infrastructure.Interfaces.Authentication.Tokens;
using FoodyNotes.Web.Attributes;
using FoodyNotes.Web.Models;
using Google.Apis.Auth;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FoodyNotes.Web.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class AuthController : ControllerBase
  {
    private readonly ITokenService _tokenService;

    public AuthController(ITokenService tokenService)
    {
      _tokenService = tokenService;
    }

    [HttpPost("authenticate")]
    public IActionResult Authenticate([FromBody] AuthenticateRequest data)
    {
      var settings = new GoogleJsonWebSignature.ValidationSettings
      {
        Audience = new[] { "674537541571-4q73096qq9tj9fimnuehdeefl5po6n0f.apps.googleusercontent.com" }
      };

      var payload = GoogleJsonWebSignature.ValidateAsync(data.IdToken, settings).Result;

      //todo: add internal authentication here
      return Ok(payload);
    }

    [AllowAnonymous]
    [HttpPost("refresh-token")]
    public IActionResult RefreshToken()
    {
      var refreshToken = Request.Cookies["refreshToken"];
      var response = _tokenService.RefreshToken(refreshToken, GetIpAddress());
      SetTokenCookie(response.RefreshToken);

      return Ok(response);
    }

    [HttpPost("revoke-token")]
    public IActionResult RevokeToken(RevokeTokenRequest model)
    {
      // accept refresh token in request body or cookie
      var token = model.Token ?? Request.Cookies["refreshToken"];

      if (string.IsNullOrEmpty(token))
        return BadRequest(new { message = "Token is required" });

      _tokenService.RevokeToken(token, GetIpAddress());

      return Ok(new { message = "Token revoked" });
    }

    private void SetTokenCookie(string refreshToken)
    {
      // append cookie with refresh token to the http response
      var cookieOptions = new CookieOptions
      {
        HttpOnly = true,
        Expires = DateTime.UtcNow.AddDays(7)
      };
      Response.Cookies.Append("refreshToken", refreshToken, cookieOptions);
    }

    private string GetIpAddress()
    {
      // get source ip address for the current request
      if (Request.Headers.ContainsKey("X-Forwarded-For"))
        return Request.Headers["X-Forwarded-For"];

      return HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
    }

  }
}