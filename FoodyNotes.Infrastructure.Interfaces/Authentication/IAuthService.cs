using FoodyNotes.Entities.Entities;

namespace FoodyNotes.Infrastructure.Interfaces.Authentication
{
  public interface IAuthService
  {
    string GetUserIdFromJwtToken(string token);

    AuthUser GetUserById(string userId);
  }
}