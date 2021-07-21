using System.Collections.Generic;
using FoodyNotes.Entities.Authentication.Enums;

namespace FoodyNotes.Entities.Authentication.Entities
{
  public class User
  {
    public string Id { get; set; }
    public Role[] Roles { get; set; }
    public List<RefreshToken> RefreshTokens { get; set; }
  }
}