using System.Text.Json.Serialization;
using Authentication.Entities.Authentication.Entities;

namespace Authentication.Infrastructure.Interfaces.Authentication.Dtos
{
  public class AuthenticateResponseDto
  {
    public string Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Username { get; set; }
    public string JwtToken { get; set; }

    [JsonIgnore] // refresh token is returned in http only cookie
    public string RefreshToken { get; set; }

    public AuthenticateResponseDto(User user, string jwtToken, string refreshToken)
    {
      Id = user.Id;
      JwtToken = jwtToken;
      RefreshToken = refreshToken;
    }
  }
}