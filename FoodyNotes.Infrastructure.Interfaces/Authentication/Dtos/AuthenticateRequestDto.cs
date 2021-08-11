using System.ComponentModel.DataAnnotations;

namespace FoodyNotes.Infrastructure.Interfaces.Authentication.Dtos
{
  public class AuthenticateRequestDto
  {
     [Required]
     public string IdToken { get; set; }
  }
}