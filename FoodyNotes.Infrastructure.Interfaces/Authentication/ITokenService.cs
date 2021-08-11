using System.Collections.Generic;
using System.Security.Claims;
using FoodyNotes.Entities.Authentication.Entities;
using FoodyNotes.Infrastructure.Interfaces.Authentication.Dtos;

namespace FoodyNotes.Infrastructure.Interfaces.Authentication
{
  public interface ITokenService
  {
    AuthenticateResponseDto RefreshToken(string token, string ipAddress);
    void RevokeToken(string token, string ipAddress);
    string GenerateJwtToken(User user);
    RefreshToken GenerateRefreshToken(string ipAddress);
    IEnumerable<Claim> GetClaimsByToken(string token);
    void RemoveOldRefreshTokens(User user);
  }

}