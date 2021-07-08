using FoodyNotes.Entities.Entities;
using Microsoft.EntityFrameworkCore;

namespace FoodyNotes.Infrastructure.Interfaces
{
  public interface IApplicationDbContext
  {
    DbSet<User> Users { get; set; }
    DbSet<AuthUser> AuthUsers { get; set; }
  }
}