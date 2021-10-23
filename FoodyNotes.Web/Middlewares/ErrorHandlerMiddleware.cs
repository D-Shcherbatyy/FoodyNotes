using System;
using System.Collections.Generic;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using FluentValidation;
using FoodyNotes.UseCases.Exceptions;
using Google.Apis.Auth;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace FoodyNotes.Web.Middlewares
{
  public class ErrorHandlerMiddleware
  {
    private readonly RequestDelegate _next;
    private readonly ILogger<ErrorHandlerMiddleware> _logger;

    public ErrorHandlerMiddleware(RequestDelegate next, ILogger<ErrorHandlerMiddleware> logger)
    {
      _next = next;
      _logger = logger;
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
          case InvalidJwtException when error.Source == "Google.Apis.Auth":
          case ArgumentException when error.Source == "Google.Apis.Core":
            _logger.LogWarning("Exception from the library {errorSource} with the message {errorMessage}", error.Source, error.Message);
            response.StatusCode = (int)HttpStatusCode.BadRequest;
            break;
          case InvalidJwtException:
          case AppException:
          case ValidationException:
            response.StatusCode = (int)HttpStatusCode.BadRequest;
            break;
          case KeyNotFoundException:
            response.StatusCode = (int)HttpStatusCode.NotFound;
            break;
          default:
            response.StatusCode = (int)HttpStatusCode.InternalServerError;
            break;
        }

        var result = JsonSerializer.Serialize(new { message = error.Message });
        await response.WriteAsync(result);
      }
    }
  }
}