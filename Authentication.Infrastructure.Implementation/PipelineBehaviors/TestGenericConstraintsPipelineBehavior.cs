using System.Threading;
using System.Threading.Tasks;
using Authentication.Infrastructure.Interfaces;
using MediatR;

namespace Authentication.Infrastructure.Implementation.PipelineBehaviors
{
  public class TestGenericConstraintsPipelineBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
   where TRequest : IValidatable 
   where TResponse : class
  {

    public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
    {
      var response = await next();

      return response;
    }
  }
}