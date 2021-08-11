using System.Collections.Generic;
using System.Security.Claims;
using FoodyNotes.Entities.Authentication.Entities;

namespace FoodyNotes.Infrastructure.Interfaces.Authentication
{
  public interface IJwtTokenService
  {
    string GenerateJwtToken(User user);
    IEnumerable<Claim> GetClaimsByToken(string token);
  }

}