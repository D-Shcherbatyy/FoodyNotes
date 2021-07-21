using FoodyNotes.Entities.Authentication.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace FoodyNotes.Infrastructure.Interfaces
{
  public interface IDbContext
  {
    DbSet<User> Users { get; }
    int UpdateAndSaveUser(User user);
  }
}