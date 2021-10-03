using System.Collections.Generic;
using System.Threading.Tasks;
using FoodyNotes.Entities.Authentication.Entities;

namespace FoodyNotes.Infrastructure.Interfaces
{
  public interface IUserService
  {
    Task<User> GetByIdAsync(string id);
    Task<IEnumerable<User>> GetAllAsync();
    string GetUserIdFromJwtToken(string token);
  }
}