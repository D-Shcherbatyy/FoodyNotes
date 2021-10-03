using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace FoodyNotes.Entities.Authentication.Entities
{
  public class RefreshToken : Entity<int>
  {
    public string Token { get; set; }
    public DateTime Expires { get; set; }
    public DateTime Created { get; set; }
    public string CreatedByIp { get; set; }
    public DateTime? Revoked { get; set; }
    public string RevokedByIp { get; set; }
    public string ReplacedByToken { get; set; }
    public string ReasonRevoked { get; set; }
    public bool IsExpired => DateTime.UtcNow >= Expires;
    public bool IsRevoked => Revoked != null;
    public bool IsActive => !IsRevoked && !IsExpired;

    public string UserId { get; set; }

    public User User { get; set; }
  }
}