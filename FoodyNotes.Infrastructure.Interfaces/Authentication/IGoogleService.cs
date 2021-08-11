using System.Threading.Tasks;

namespace FoodyNotes.Infrastructure.Interfaces.Authentication
{
  public interface IGoogleService
  {
    Task<string> GetUserIdByIdToken(string idToken);
  }
}