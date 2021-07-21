using System.ComponentModel.DataAnnotations;

namespace FoodyNotes.Infrastructure.Interfaces.Authentication.Dtos
{
  public class AuthenticateInDto
  {
     [Required]
     public string IdToken { get; set; }
  }
}