using System.Collections.Generic;
using System.Threading.Tasks;
using Authentication.Entities.Authentication.Entities;

namespace Authentication.Infrastructure.Interfaces
{
  public interface IUserService
  {
    Task<User> GetByIdAsync(string id);
    Task<IEnumerable<User>> GetAllAsync();
    string GetUserIdFromJwtToken(string token);
  }
}