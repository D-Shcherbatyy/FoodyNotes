using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;

namespace FoodyNotes.Infrastructure.Implementation.PipelineBehaviors
{
  public class LoggingPipelineBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
  {
    private readonly ILogger<LoggingPipelineBehaviour<TRequest, TResponse>> _logger;
    
    public LoggingPipelineBehaviour(ILogger<LoggingPipelineBehaviour<TRequest, TResponse>> logger)
    {
      _logger = logger;
    }
    public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
    {
      _logger.LogInformation($"Handling {typeof(TRequest).Name}");

      foreach (var prop in request.GetType().GetProperties())
      {
        _logger.LogInformation("{Property} : {@Value}", prop.Name, prop.GetValue(request, null));
      }

      var response = await next();
      
      _logger.LogInformation($"Handled {typeof(TResponse).Name}");
      
      return response;
    }
  }
}