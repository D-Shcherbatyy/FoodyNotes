using System.Threading.Tasks;
using FoodyNotes.Entities.Authentication.Entities;
using FoodyNotes.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace FoodyNotes.DataAccess.MsSql
{
  public class ApplicationDbContext : DbContext, IDbContext
  {
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