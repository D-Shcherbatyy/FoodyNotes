using System.Threading;
using System.Threading.Tasks;
using Authentication.Infrastructure.Interfaces.Authentication.Dtos;
using Authentication.UseCases.Authentication.Commands;
using MediatR;

namespace Authentication.Infrastructure.Implementation.PipelineBehaviors
{
  public class TestPipelineBehavior : IPipelineBehavior<AuthenticateCommand, AuthenticateResponseDto>
  {

    public async Task<AuthenticateResponseDto> Handle(AuthenticateCommand request, CancellationToken cancellationToken, RequestHandlerDelegate<AuthenticateResponseDto> next)
    {
      //pre
      
      var response = await next();

      // post
      
      return response;
    }
  }
}