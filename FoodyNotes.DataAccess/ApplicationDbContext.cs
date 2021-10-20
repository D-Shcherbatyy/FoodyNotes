using FoodyNotes.Entities.Authentication.Entities;
using FoodyNotes.Entities.Authentication.Enums;
using Microsoft.EntityFrameworkCore;

namespace FoodyNotes.DataAccess.Postgres
{
  public class ApplicationDbContext : DbContext
  {
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
      : base(options)
    {
    }
    
#pragma warning disable 108,114
    public DbSet<User> Users { get; set; }
#pragma warning restore 108,114

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      modelBuilder.HasDefaultSchema("auth_schema");
      // modelBuilder.HasPostgresEnum<Role>();
      
      base.OnModelCreating(modelBuilder);

      modelBuilder.Entity<User>().OwnsMany(x => x.RefreshTokens);
    }
  }

}