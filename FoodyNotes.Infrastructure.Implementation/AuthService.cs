using System.Collections.Generic;
using System.Linq;
using FoodyNotes.Entities.Authentication.Entities;
using FoodyNotes.Infrastructure.Interfaces;
using FoodyNotes.Infrastructure.Interfaces.Authentication;
using FoodyNotes.UseCases.Exceptions;
using Google.Apis.Auth;

namespace FoodyNotes.Infrastructure.Implementation
{
  public class AuthService : IAuthService
  {
    private readonly ITokenService _tokenService;
    private readonly IDbContext _dbContext;

    public AuthService(IDbContext dbContext, ITokenService tokenService)
    {
      _tokenService = tokenService;
      _dbContext = dbContext;
    }

    public string GetUserIdFromJwtToken(string token)
    {
      return _tokenService.GetClaimsByToken(token)?.First(x => x.Type == "id").Value;
    }

    public User GetUserById(string userId)
    {
      var user = _dbContext.Users.Find(userId);

      if (user == null)
        throw new KeyNotFoundException("User not found");

      return user;
    }
  }
}