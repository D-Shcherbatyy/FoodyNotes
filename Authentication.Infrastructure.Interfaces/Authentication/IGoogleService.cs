using System.Threading.Tasks;

namespace Authentication.Infrastructure.Interfaces.Authentication
{
  public interface IGoogleService
  {
    Task<string> GetUserIdByIdTokenAsync(string idToken);
  }
}