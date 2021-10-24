using System.Collections.Generic;
using System.Security.Claims;
using Authentication.Entities.Authentication.Entities;

namespace Authentication.Infrastructure.Interfaces.Authentication
{
  public interface IJwtTokenService
  {
    string GenerateJwtToken(User user);
    IEnumerable<Claim> GetClaimsByToken(string token);
  }

}