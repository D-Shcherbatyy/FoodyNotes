using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using FoodyNotes.Entities.Authentication.Entities;
using FoodyNotes.Infrastructure.Interfaces;
using FoodyNotes.Infrastructure.Interfaces.Authentication;
using FoodyNotes.UseCases.Exceptions;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace FoodyNotes.Infrastructure.Implementation
{
  public class TokenService : ITokenService
  {
    private readonly AppSettings _appSettings;
    private readonly IDbContext _context;

    public TokenService(IOptions<AppSettings> appSettings, IDbContext context)
    {
      _appSettings = appSettings.Value;
      _context = context;
    }

    public string GenerateJwtToken(User user)
    {
      // generate token that is valid for 15 minutes
      var tokenHandler = new JwtSecurityTokenHandler();
      var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
      var tokenDescriptor = new SecurityTokenDescriptor
      {
        Subject = new ClaimsIdentity(new[] { new Claim("id", user.Id) }),
        Expires = DateTime.UtcNow.AddMinutes(15),
        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
      };
      var token = tokenHandler.CreateToken(tokenDescriptor);

      return tokenHandler.WriteToken(token);
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
    
    public IEnumerable<Claim> GetClaimsByToken(string token)
    {
      if (token == null)
        return null;

      var tokenHandler = new JwtSecurityTokenHandler();
      var key = Encoding.ASCII.GetBytes(_appSettings.Secret);

      try
      {
        tokenHandler.ValidateToken(token, new TokenValidationParameters
        {
          ValidateIssuerSigningKey = true,
          IssuerSigningKey = new SymmetricSecurityKey(key),
          ValidateIssuer = false,
          ValidateAudience = false,
          // set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
          ClockSkew = TimeSpan.Zero
        }, out var validatedToken);

        return ((JwtSecurityToken)validatedToken).Claims;
      }
      catch
      {
        // return null if validation fails
        return null;
      }
    }

    public User GetUserByRefreshToken(string token)
    {
      var user = _context.Users.SingleOrDefault(u => u.RefreshTokens.Any(t => t.Token == token));

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