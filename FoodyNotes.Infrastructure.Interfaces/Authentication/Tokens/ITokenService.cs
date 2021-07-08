namespace FoodyNotes.Infrastructure.Interfaces.Authentication.Tokens
{
  public interface ITokenService
  {
    IAuthenticateResponse RefreshToken(string token, string ipAddress);
    void RevokeToken(string token, string ipAddress);
  }

}