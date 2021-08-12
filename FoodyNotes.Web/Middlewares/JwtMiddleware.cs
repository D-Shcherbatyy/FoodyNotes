using System.Linq;
using System.Threading.Tasks;
using FoodyNotes.Infrastructure.Interfaces;
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

    public async Task Invoke(HttpContext context, IUserService userService)
    {
      var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
      var userId = userService.GetUserIdFromJwtToken(token);

      if (userId != null)
      {
        // attach user to context on successful jwt validation
        context.Items["User"] = userService.GetById(userId);
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