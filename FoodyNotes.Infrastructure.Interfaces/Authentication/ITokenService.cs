using System.Collections.Generic;
using System.Security.Claims;
using FoodyNotes.Entities.Authentication.Entities;

namespace FoodyNotes.Infrastructure.Interfaces.Authentication
{
  public interface ITokenService
  {
    User GetUserByRefreshToken(string token);
    string GenerateJwtToken(User user);
    RefreshToken GenerateRefreshToken(string ipAddress);
    IEnumerable<Claim> GetClaimsByToken(string token);
    void RemoveOldRefreshTokens(User user);
    RefreshToken RotateRefreshToken(RefreshToken refreshToken, string ipAddress);
    void RevokeDescendantRefreshTokens(RefreshToken refreshToken, User user, string ipAddress, string reason);
    void RevokeRefreshToken(RefreshToken refreshToken, string requestIpAddress, string revokedWithoutReplacement, string replacedByToken = null);
  }

}