using System.Collections.Generic;
using System.Linq;
using FoodyNotes.Entities.Authentication.Entities;
using FoodyNotes.Infrastructure.Interfaces;
using FoodyNotes.Infrastructure.Interfaces.Authentication;
using FoodyNotes.Infrastructure.Interfaces.Authentication.Dtos;
using FoodyNotes.UseCases.Exceptions;
using Google.Apis.Auth;

namespace FoodyNotes.Infrastructure.Implementation
{
  public class AuthService : IAuthService
  {
    private readonly ITokenService _tokenService;
    private readonly IDbContext _context;

    public AuthService(IDbContext context, ITokenService tokenService)
    {
      _tokenService = tokenService;
      _context = context;
    }
    
    public AuthenticateOutDto Authenticate(AuthenticateInDto model, string ipAddress)
    {
      var settings = new GoogleJsonWebSignature.ValidationSettings
      {
        Audience = new[] { "674537541571-4q73096qq9tj9fimnuehdeefl5po6n0f.apps.googleusercontent.com" }
      };

      var payload = GoogleJsonWebSignature.ValidateAsync(model.IdToken, settings).Result;
      
      var user = _context.Users.SingleOrDefault(x => x.Id == payload.Subject);

      // validate
      if (user == null)
        throw new AppException("Username or password is incorrect");

      // authentication successful so generate jwt and refresh tokens
      var jwtToken = _tokenService.GenerateJwtToken(user);
      var refreshToken = _tokenService.GenerateRefreshToken(ipAddress);
      user.RefreshTokens.Add(refreshToken);

      // remove old refresh tokens from user
      _tokenService.RemoveOldRefreshTokens(user);

      // save changes to db
      _context.UpdateAndSaveUser(user);

      return new AuthenticateOutDto(user, jwtToken, refreshToken.Token);
    }

    public string GetUserIdFromJwtToken(string token)
    {
      return _tokenService.GetClaimsByToken(token)?.First(x => x.Type == "id").Value;
    }

    public User GetUserById(string userId)
    {
      var user = _context.Users.Find(userId);

      if (user == null)
        throw new KeyNotFoundException("User not found");

      return user;
    }
  }
}