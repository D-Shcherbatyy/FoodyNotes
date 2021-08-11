using FoodyNotes.Entities.Authentication.Entities;

namespace FoodyNotes.Infrastructure.Interfaces.Authentication.Dtos
{
  public class RefreshTokenResponseDto
  {
    public RefreshToken RefreshToken { get; set; }
    public string JwtToken { get; set; }
  }
}