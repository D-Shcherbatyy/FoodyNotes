using System.Threading;
using System.Threading.Tasks;
using FoodyNotes.Infrastructure.Interfaces.Authentication.Dtos;
using FoodyNotes.UseCases.Authentication.Commands;
using MediatR;

namespace FoodyNotes.Infrastructure.Implementation.Authentication
{
  public class TestPipelineBehavior : IPipelineBehavior<AuthenticateCommand, AuthenticateResponseDto>
  {

    public async Task<AuthenticateResponseDto> Handle(AuthenticateCommand request, CancellationToken cancellationToken, RequestHandlerDelegate<AuthenticateResponseDto> next)
    {
      var response = await next();

      return response;
    }
  }
}