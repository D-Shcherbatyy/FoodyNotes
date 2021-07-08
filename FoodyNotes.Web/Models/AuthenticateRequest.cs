using System.ComponentModel.DataAnnotations;

namespace FoodyNotes.Web.Models
{
  public class AuthenticateRequest
  {
    [Required]
    public string IdToken { get; set; }
  }
}