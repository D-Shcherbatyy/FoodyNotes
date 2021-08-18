using FoodyNotes.Infrastructure.Interfaces;
using FoodyNotes.Infrastructure.Interfaces.Authentication.Dtos;
using MediatR;

namespace FoodyNotes.UseCases.Authentication.Commands
{
  public class RefreshTokenCommand : IRequest<RefreshTokenResponseDto>, IValidatable
  {
    public string CurrentRefreshToken { get; set; }
    public string IpAddress { get; set; }
  }
}