using System.Threading.Tasks;
using FoodyNotes.Entities.Authentication.Entities;
using FoodyNotes.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FoodyNotes.DataAccess.Postgres
{
  public class ApplicationDbContext : DbContext, IDbContext
  {
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
      : base(options)
    {
    }
    
    public DbSet<User> Users { get; set; }
    public Task<int> UpdateAndSaveUser(User user)
    {
      Update(user);
      
      return SaveChangesAsync();
    }

    Task<int> IDbContext.SaveChangesAsync()
    {
     return SaveChangesAsync();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      base.OnModelCreating(modelBuilder);

      modelBuilder.Entity<User>().OwnsMany(x => x.RefreshTokens);
    }
  }

}