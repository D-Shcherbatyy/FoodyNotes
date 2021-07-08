using System.Collections.Generic;
using FoodyNotes.Entities.Enums;

namespace FoodyNotes.Entities.Entities
{
  public class AuthUser
  {
    public string Id { get; set; }
    public Role[] Roles { get; set; }
    public List<RefreshToken> RefreshTokens { get; set; }
  }
}