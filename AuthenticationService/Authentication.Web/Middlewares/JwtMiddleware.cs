using System.Linq;
using System.Threading.Tasks;
using Authentication.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Http;

namespace Authentication.Web.Middlewares
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
        context.Items["User"] = await userService.GetByIdAsync(userId);
      }

      await _next(context);
    }
  }
}