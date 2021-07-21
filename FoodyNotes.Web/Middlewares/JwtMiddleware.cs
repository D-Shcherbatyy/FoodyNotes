using System.Linq;
using System.Threading.Tasks;
using FoodyNotes.Infrastructure.Interfaces.Authentication;
using FoodyNotes.UseCases;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace FoodyNotes.Web.Middlewares
{
  public class JwtMiddleware
  {
    private readonly RequestDelegate _next;

    public JwtMiddleware(RequestDelegate next)
    {
      _next = next;
    }

    public async Task Invoke(HttpContext context, IAuthService authService, IUserService userService)
    {
      var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
      var userId = authService.GetUserIdFromJwtToken(token);

      if (userId != null)
      {
        // attach user to context on successful jwt validation
        context.Items["User"] = authService.GetUserById(userId);
      }

      await _next(context);
    }
  }
  
  public static class JwtMiddlewareExtensions
  {
    public static IApplicationBuilder UseJwtMiddleware(this IApplicationBuilder builder)
    {
      return builder.UseMiddleware<JwtMiddleware>();
    }
  }

}