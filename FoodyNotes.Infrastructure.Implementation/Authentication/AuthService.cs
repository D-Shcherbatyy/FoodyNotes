using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using FoodyNotes.Entities.Entities;
using FoodyNotes.Infrastructure.Interfaces;
using FoodyNotes.Infrastructure.Interfaces.Authentication;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace FoodyNotes.Infrastructure.Implementation.Authentication
{
  public class AuthService : IAuthService
  {
    private readonly IApplicationDbContext _context;
    private readonly AppSettings _appSettings;

    public AuthService(IOptions<AppSettings> appSettings, IApplicationDbContext context)
    {
      _context = context;
      _appSettings = appSettings.Value;

    }

    public string GetUserIdFromJwtToken(string token)
    {
      return GetValidatedToken(token)?.Claims.First(x => x.Type == "id").Value;
    }

    public AuthUser GetUserById(string userId)
    {
      var user = _context.AuthUsers.Find(userId);

      if (user == null)
        throw new KeyNotFoundException("User not found");

      return user;
    }

    private JwtSecurityToken GetValidatedToken(string token)
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

        return (JwtSecurityToken)validatedToken;
      }
      catch
      {
        // return null if validation fails
        return null;
      }
    }
  }
}