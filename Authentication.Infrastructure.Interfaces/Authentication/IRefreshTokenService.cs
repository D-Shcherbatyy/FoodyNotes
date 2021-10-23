using System.Threading.Tasks;
using Authentication.Entities.Authentication.Entities;

namespace Authentication.Infrastructure.Interfaces.Authentication
{
  public interface IRefreshTokenService
  {
    RefreshToken GenerateRefreshToken(string ipAddress);
    Task<User> GetUserByRefreshTokenAsync(string token);
    void RemoveOldRefreshTokens(User user);
    RefreshToken RotateRefreshToken(RefreshToken refreshToken, string ipAddress);
    void RevokeDescendantRefreshTokens(RefreshToken refreshToken, User user, string ipAddress, string reason);
    void RevokeRefreshToken(RefreshToken refreshToken, string requestIpAddress, string revokedWithoutReplacement, string replacedByToken = null);
  }
}