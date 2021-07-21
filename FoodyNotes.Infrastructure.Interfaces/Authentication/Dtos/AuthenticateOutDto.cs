using System.Text.Json.Serialization;
using FoodyNotes.Entities.Authentication.Entities;

namespace FoodyNotes.Infrastructure.Interfaces.Authentication.Dtos
{
  public class AuthenticateOutDto
  {
    public string Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Username { get; set; }
    public string JwtToken { get; set; }

    [JsonIgnore] // refresh token is returned in http only cookie
    public string RefreshToken { get; set; }

    public AuthenticateOutDto(User user, string jwtToken, string refreshToken)
    {
      Id = user.Id;
      JwtToken = jwtToken;
      RefreshToken = refreshToken;
    }
  }
}