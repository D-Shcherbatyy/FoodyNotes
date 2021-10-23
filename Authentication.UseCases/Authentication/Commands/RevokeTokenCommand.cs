using Authentication.Infrastructure.Interfaces;
using MediatR;

namespace Authentication.UseCases.Authentication.Commands
{
  public class RevokeTokenCommand : IRequest, IValidatable
  {

    public string RefreshToken { get; set; }
    public string IpAddress { get; set; }
  }
}