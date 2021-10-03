using System.Collections.Generic;

namespace FoodyNotes.Entities.Authentication.Entities
{
  public class User : Entity<string>
  {
    //public List<Role> Roles { get; set; }
    public List<RefreshToken> RefreshTokens { get; set; }
  }
}