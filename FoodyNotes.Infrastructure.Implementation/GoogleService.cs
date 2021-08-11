using System.Threading.Tasks;
using FoodyNotes.Infrastructure.Interfaces.Authentication;
using Google.Apis.Auth;

namespace FoodyNotes.Infrastructure.Implementation
{
  public class GoogleService : IGoogleService
  {
    public async Task<string> GetUserIdByIdToken(string idToken)
    {
      var payload = await ValidateIdToken(idToken);

      return payload.Subject;
    }
    
    private static async Task<GoogleJsonWebSignature.Payload> ValidateIdToken(string idToken)
    {
      var settings = new GoogleJsonWebSignature.ValidationSettings
      {
        Audience = new[] { "674537541571-4q73096qq9tj9fimnuehdeefl5po6n0f.apps.googleusercontent.com" }
      };

      return await GoogleJsonWebSignature.ValidateAsync(idToken, settings);
    }
  }
}