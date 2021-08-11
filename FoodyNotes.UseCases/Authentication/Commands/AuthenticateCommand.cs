using FoodyNotes.Infrastructure.Interfaces.Authentication.Dtos;
using MediatR;

namespace FoodyNotes.UseCases.Authentication.Commands
{
  public class AuthenticateCommand : IRequest<AuthenticateResponseDto>
  {
    public AuthenticateRequestDto RequestDto { get; set; }
    public string IpAddress { get; set; }
  }
}