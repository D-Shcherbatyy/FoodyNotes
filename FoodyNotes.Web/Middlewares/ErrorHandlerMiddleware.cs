using System;
using System.Collections.Generic;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using FoodyNotes.UseCases.Exceptions;
using Google.Apis.Auth;
using Microsoft.AspNetCore.Http;

namespace FoodyNotes.Web.Middlewares
{
  public class ErrorHandlerMiddleware
  {
    private readonly RequestDelegate _next;

    public ErrorHandlerMiddleware(RequestDelegate next)
    {
      _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
      try
      {
        await _next(context);
      }
      catch (Exception error)
      {
        var response = context.Response;
        response.ContentType = "application/json";

        switch(error)
        {
          case InvalidJwtException:
          case AppException:
            // custom application error
            response.StatusCode = (int)HttpStatusCode.BadRequest;
            break;
          case KeyNotFoundException:
            // not found error
            response.StatusCode = (int)HttpStatusCode.NotFound;
            break;
          default:
            // unhandled error
            response.StatusCode = (int)HttpStatusCode.InternalServerError;
            break;
        }

        var result = JsonSerializer.Serialize(new { message = error?.Message });
        await response.WriteAsync(result);
      }
    }
  }
}