using System.Collections.Generic;
using System.Linq;
using FoodyNotes.Entities.Authentication.Entities;
using FoodyNotes.Infrastructure.Interfaces;
using FoodyNotes.Infrastructure.Interfaces.Authentication;

namespace FoodyNotes.UseCases
{
  public class UserService : IUserService
  {
    private readonly IDbContext _context;
    private readonly IJwtTokenService _jwtTokenService;

    public UserService(IDbContext context, IJwtTokenService jwtTokenService)
    {
      _context = context;
      _jwtTokenService = jwtTokenService;

    }

    public User GetById(string id)
    {
      var user = _context.Users.Find(id);

      if (user == null)
        throw new KeyNotFoundException("User not found");

      return user;
    }

    public IEnumerable<User> GetAll()
    {
      return _context.Users;
    }

    public string GetUserIdFromJwtToken(string token)
    {
      return _jwtTokenService.GetClaimsByToken(token)?.First(x => x.Type == "id").Value;
    }
  }
}