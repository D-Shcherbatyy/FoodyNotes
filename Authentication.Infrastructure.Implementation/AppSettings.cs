namespace Authentication.Infrastructure.Implementation
{
  public class AppSettings
  {
    public string Secret { get; set; }
    public int RefreshTokenTTL { get; set; }
  }
}