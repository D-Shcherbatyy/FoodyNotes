using FoodyNotes.Entities.Authentication.Entities;
using FoodyNotes.Infrastructure.Interfaces.Authentication.Dtos;

namespace FoodyNotes.Infrastructure.Interfaces.Authentication
{
  public interface IAuthService
  {
    AuthenticateOutDto Authenticate(AuthenticateInDto model, string ipAddress);
    string GetUserIdFromJwtToken(string token);

    User GetUserById(string userId);
  }
}