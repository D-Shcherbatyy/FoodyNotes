using FoodyNotes.Entities.Entities;
using FoodyNotes.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FoodyNotes.DataAccess.MsSql
{
  public class ApplicationDbContext : DbContext, IApplicationDbContext
  {
    public DbSet<AuthUser> AuthUsers { get; set; }
    public DbSet<User> Users { get; set; }

    public ApplicationDbContext()
    {

    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
      base.OnConfiguring(optionsBuilder);

      optionsBuilder.UseInMemoryDatabase("TestDb");
    }
  }

}