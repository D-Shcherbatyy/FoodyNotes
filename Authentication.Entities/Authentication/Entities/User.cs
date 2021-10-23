using System.Collections.Generic;

namespace Authentication.Entities.Authentication.Entities
{
  public class User : Entity<string>
  {
    //public List<Role> Roles { get; set; }
    public List<RefreshToken> RefreshTokens { get; set; }
  }
}