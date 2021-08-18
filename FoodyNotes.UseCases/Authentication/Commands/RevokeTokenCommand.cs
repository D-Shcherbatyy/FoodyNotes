using FoodyNotes.Infrastructure.Interfaces;
using MediatR;

namespace FoodyNotes.UseCases.Authentication.Commands
{
  public class RevokeTokenCommand : IRequest, IValidatable
  {

    public string RefreshToken { get; set; }
    public string IpAddress { get; set; }
  }
}