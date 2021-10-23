using Authentication.Infrastructure.Interfaces;
using Authentication.Infrastructure.Interfaces.Authentication.Dtos;
using MediatR;

namespace Authentication.UseCases.Authentication.Commands
{
  public class RefreshTokenCommand : IRequest<RefreshTokenResponseDto>, IValidatable
  {
    public string CurrentRefreshToken { get; set; }
    public string IpAddress { get; set; }
  }
}