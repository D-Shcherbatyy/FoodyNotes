using System;
using Microsoft.AspNetCore.Http;

namespace FoodyNotes.Web.Services
{
  public class HttpService
  {
    public void SetTokenCookie(HttpResponse response, string refreshToken)
    {
      // append cookie with refresh token to the http response
      var cookieOptions = new CookieOptions
      {
        HttpOnly = true,
        Expires = DateTime.UtcNow.AddDays(7)
      };
      
      response.Cookies.Append("refreshToken", refreshToken, cookieOptions);
    }

    public string GetIpAddress(HttpRequest request, HttpContext context)
    {
      // get source ip address for the current request
      if (request.Headers.ContainsKey("X-Forwarded-For"))
        return request.Headers["X-Forwarded-For"];

      return context.Connection.RemoteIpAddress.MapToIPv4().ToString();
    }
  }
}