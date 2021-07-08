using System.Collections.Generic;
using FoodyNotes.Entities.Entities;
using FoodyNotes.Infrastructure.Interfaces;

namespace FoodyNotes.UseCases
{
  public class UserService : IUserService
  {
    private readonly IApplicationDbContext _context;

    public UserService(IApplicationDbContext context)
    {
      _context = context;

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
  }
}