using FoodyNotes.Entities.Authentication.Entities;

namespace FoodyNotes.Infrastructure.Interfaces.Authentication
{
  public interface IAuthService
  {
    string GetUserIdFromJwtToken(string token);

    User GetUserById(string userId);
  }
}