using System.Collections.Generic;
using FoodyNotes.Entities.Authentication.Entities;

namespace FoodyNotes.Infrastructure.Interfaces
{
  public interface IUserService
  {
    User GetById(string id);
    IEnumerable<User> GetAll();
    string GetUserIdFromJwtToken(string token);
  }
}