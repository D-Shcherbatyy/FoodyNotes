using MediatR;

namespace FoodyNotes.UseCases.Authentication.Commands
{
  public class RevokeTokenCommand : IRequest
  {

    public string RefreshToken { get; set; }
    public string IpAddress { get; set; }
  }
}