using Authentication.Infrastructure.Interfaces;
using Authentication.Infrastructure.Interfaces.Authentication.Dtos;
using MediatR;

namespace Authentication.UseCases.Authentication.Commands
{
  public class AuthenticateCommand : IRequest<AuthenticateResponseDto>, IValidatable
  {
    public AuthenticateRequestDto RequestDto { get; set; }
    public string IpAddress { get; set; }
  }
}