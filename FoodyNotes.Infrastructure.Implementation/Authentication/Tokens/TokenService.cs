using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using FoodyNotes.DataAccess.MsSql;
using FoodyNotes.Entities.Entities;
using FoodyNotes.Infrastructure.Implementation.Exceptions;
using FoodyNotes.Infrastructure.Interfaces;
using FoodyNotes.Infrastructure.Interfaces.Authentication;
using FoodyNotes.Infrastructure.Interfaces.Authentication.Tokens;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace FoodyNotes.Infrastructure.Implementation.Authentication.Tokens
{
  public class TokenService : ITokenService
  {
    private readonly AppSettings _appSettings;
    private readonly ApplicationDbContext _context;

    public TokenService(IOptions<AppSettings> appSettings, IApplicationDbContext context)
    {
      _appSettings = appSettings.Value;
      _context = context as ApplicationDbContext;
    }

    public IAuthenticateResponse RefreshToken(string token, string ipAddress)
    {
      var user = GetUserByRefreshToken(token);
      var refreshToken = user.RefreshTokens.Single(x => x.Token == token);

      if (refreshToken.IsRevoked)
      {
        // revoke all descendant tokens in case this token has been compromised
        RevokeDescendantRefreshTokens(refreshToken, user, ipAddress,
          $"Attempted reuse of revoked ancestor token: {token}");
        _context.Update(user);
        _context.SaveChanges();
      }

      if (!refreshToken.IsActive)
        throw new AppException("Invalid token");

      // replace old refresh token with a new one (rotate token)
      var newRefreshToken = RotateRefreshToken(refreshToken, ipAddress);
      user.RefreshTokens.Add(newRefreshToken);

      // remove old refresh tokens from user
      RemoveOldRefreshTokens(user);

      // save changes to db
      _context.Update(user);
      _context.SaveChanges();

      // generate new jwt
      var jwtToken = GenerateJwtToken(user);

      return new AuthenticateResponse(user, jwtToken, newRefreshToken.Token);
    }

    public void RevokeToken(string token, string ipAddress)
    {
      var user = GetUserByRefreshToken(token);
      var refreshToken = user.RefreshTokens.Single(x => x.Token == token);

      if (!refreshToken.IsActive)
        throw new AppException("Invalid token");

      // revoke token and save
      RevokeRefreshToken(refreshToken, ipAddress, "Revoked without replacement");
      _context.Update(user);
      _context.SaveChanges();
    }

    private string GenerateJwtToken(AuthUser user)
    {
      // generate token that is valid for 15 minutes
      var tokenHandler = new JwtSecurityTokenHandler();
      var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
      var tokenDescriptor = new SecurityTokenDescriptor
      {
        Subject = new ClaimsIdentity(new[] { new Claim("id", user.Id) }),
        Expires = DateTime.UtcNow.AddMinutes(15),
        SigningCredentials =
          new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
      };
      var token = tokenHandler.CreateToken(tokenDescriptor);

      return tokenHandler.WriteToken(token);
    }

    private RefreshToken GenerateRefreshToken(string ipAddress)
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

    private AuthUser GetUserByRefreshToken(string token)
    {
      var user = _context.AuthUsers.SingleOrDefault(u => u.RefreshTokens.Any(t => t.Token == token));

      if (user == null)
        throw new AppException("Invalid token");

      return user;
    }

    private RefreshToken RotateRefreshToken(RefreshToken refreshToken, string ipAddress)
    {
      var newRefreshToken = GenerateRefreshToken(ipAddress);
      RevokeRefreshToken(refreshToken, ipAddress, "Replaced by new token", newRefreshToken.Token);

      return newRefreshToken;
    }

    private void RemoveOldRefreshTokens(AuthUser user)
    {
      // remove old inactive refresh tokens from user based on TTL in app settings
      user.RefreshTokens.RemoveAll(x =>
        !x.IsActive &&
        x.Created.AddDays(_appSettings.RefreshTokenTTL) <= DateTime.UtcNow);
    }

    private void RevokeDescendantRefreshTokens(RefreshToken refreshToken, AuthUser user, string ipAddress,
      string reason)
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

    private void RevokeRefreshToken(RefreshToken token, string ipAddress, string reason = null,
      string replacedByToken = null)
    {
      token.Revoked = DateTime.UtcNow;
      token.RevokedByIp = ipAddress;
      token.ReasonRevoked = reason;
      token.ReplacedByToken = replacedByToken;
    }
  }
}