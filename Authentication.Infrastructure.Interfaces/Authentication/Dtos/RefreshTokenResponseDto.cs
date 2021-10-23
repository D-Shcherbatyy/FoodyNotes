using Authentication.Entities.Authentication.Entities;

namespace Authentication.Infrastructure.Interfaces.Authentication.Dtos
{
  public class RefreshTokenResponseDto
  {
    public RefreshToken RefreshToken { get; set; }
    public string JwtToken { get; set; }
  }
}