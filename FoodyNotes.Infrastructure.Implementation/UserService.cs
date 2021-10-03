using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FoodyNotes.Entities.Authentication.Entities;
using FoodyNotes.Infrastructure.Interfaces;
using FoodyNotes.Infrastructure.Interfaces.Authentication;
using FoodyNotes.Infrastructure.Interfaces.Persistence;

namespace FoodyNotes.Infrastructure.Implementation
{
  public class UserService : IUserService
  {
    private readonly IRepositoryBase<User, string> _userRepo;
    private readonly IJwtTokenService _jwtTokenService;

    public UserService(IRepositoryBase<User, string> userRepo, IJwtTokenService jwtTokenService)
    {
      _userRepo = userRepo;
      _jwtTokenService = jwtTokenService;

    }

    public async Task<User> GetByIdAsync(string id)
    {
      var user = await _userRepo.GetByIdAsync(id);

      if (user == null)
        throw new KeyNotFoundException("User not found");

      return user;
    }

    public async Task<IEnumerable<User>> GetAllAsync()
    {
      return await _userRepo.GetAllAsync();
    }

    public string GetUserIdFromJwtToken(string token)
    {
      return _jwtTokenService.GetClaimsByToken(token)?.First(x => x.Type == "id").Value;
    }
  }
}