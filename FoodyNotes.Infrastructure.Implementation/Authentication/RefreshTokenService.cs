using System;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using FoodyNotes.Entities.Authentication.Entities;
using FoodyNotes.Infrastructure.Interfaces.Authentication;
using FoodyNotes.Infrastructure.Interfaces.Persistence;
using FoodyNotes.UseCases.Exceptions;
using Microsoft.Extensions.Options;

namespace FoodyNotes.Infrastructure.Implementation.Authentication
{
  public class RefreshTokenService : IRefreshTokenService
  {
    private readonly IRepositoryBase<User, string> _userRepo;
    private readonly AppSettings _appSettings;

    public RefreshTokenService(IOptions<AppSettings> appSettings, IRepositoryBase<User, string> userRepo)
    {
      _userRepo = userRepo;
      _appSettings = appSettings.Value;
    }

    public RefreshToken GenerateRefreshToken(string ipAddress)
    {
      // generate token that is valid for 7 days
      using var rngCryptoServiceProvider = new RNGCryptoServiceProvider();
      var randomBytes = new byte[64];
      rngCryptoServiceProvider.GetBytes(randomBytes);
      var refreshToken = new RefreshToken
      {
        Token = Convert.ToBase64String(randomBytes),
        Expires = DateTime.UtcNow.AddDays(7),
        Created = DateTime.UtcNow,
        CreatedByIp = ipAddress
      };

      return refreshToken;
    }

    public void RemoveOldRefreshTokens(User user)
    {
      // remove old inactive refresh tokens from user based on TTL in app settings
      user.RefreshTokens.RemoveAll(x =>
        !x.IsActive &&
        x.Created.AddDays(_appSettings.RefreshTokenTTL) <= DateTime.UtcNow);
    }

    public async Task<User> GetUserByRefreshTokenAsync(string token)
    {
      var users = await _userRepo.GetAllAsync();
      var user = users.SingleOrDefault(u => u.RefreshTokens.Any(t => t.Token == token));
      
      if (user == null)
        throw new AppException("Invalid token");

      return user;
    }

    public RefreshToken RotateRefreshToken(RefreshToken refreshToken, string ipAddress)
    {
      var newRefreshToken = GenerateRefreshToken(ipAddress);
      RevokeRefreshToken(refreshToken, ipAddress, "Replaced by new token", newRefreshToken.Token);

      return newRefreshToken;
    }

    public void RevokeDescendantRefreshTokens(RefreshToken refreshToken, User user, string ipAddress, string reason)
    {
      // recursively traverse the refresh token chain and ensure all descendants are revoked
      if (!string.IsNullOrEmpty(refreshToken.ReplacedByToken))
      {
        var childToken = user.RefreshTokens.SingleOrDefault(x => x.Token == refreshToken.ReplacedByToken);
        if (childToken.IsActive)
          RevokeRefreshToken(childToken, ipAddress, reason);
        else
          RevokeDescendantRefreshTokens(childToken, user, ipAddress, reason);
      }
    }

    public void RevokeRefreshToken(RefreshToken token, string ipAddress, string reason = null, string replacedByToken = null)
    {
      token.Revoked = DateTime.UtcNow;
      token.RevokedByIp = ipAddress;
      token.ReasonRevoked = reason;
      token.ReplacedByToken = replacedByToken;
    }
  }
}