using System.Text.Json.Serialization;

namespace FoodyNotes.Infrastructure.Interfaces.Authentication
{
  public interface IAuthenticateResponse
  {
    public string Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Username { get; set; }
    public string JwtToken { get; set; }

    [JsonIgnore] // refresh token is returned in http only cookie
    public string RefreshToken { get; set; }
  }
}