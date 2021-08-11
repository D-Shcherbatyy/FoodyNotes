using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FoodyNotes.Infrastructure.Interfaces;
using FoodyNotes.Infrastructure.Interfaces.Authentication;
using FoodyNotes.Infrastructure.Interfaces.Authentication.Dtos;
using FoodyNotes.UseCases.Authentication.Commands;
using FoodyNotes.UseCases.Exceptions;
using MediatR;

namespace FoodyNotes.UseCases.Authentication.Handlers
{
  public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, RefreshTokenResponseDto>
  {
    private readonly IDbContext _dbContext;
    private readonly ITokenService _tokenService;

    public RefreshTokenCommandHandler(IDbContext dbContext, ITokenService tokenService)
    {
      _dbContext = dbContext;
      _tokenService = tokenService;

    }
    
    public async Task<RefreshTokenResponseDto> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
      var user = _tokenService.GetUserByRefreshToken(request.CurrentRefreshToken);
      var refreshToken = user.RefreshTokens.Single(x => x.Token == request.CurrentRefreshToken);

      if (refreshToken.IsRevoked)
      {
        // revoke all descendant tokens in case this token has been compromised
        _tokenService.RevokeDescendantRefreshTokens(refreshToken, user, request.IpAddress, $"Attempted reuse of revoked ancestor token: {request.CurrentRefreshToken}");
        
        await _dbContext.UpdateAndSaveUser(user);
      }

      if (!refreshToken.IsActive)
        throw new AppException("Invalid token");

      // replace old refresh token with a new one (rotate token)
      var newRefreshToken = _tokenService.RotateRefreshToken(refreshToken, request.IpAddress);
      user.RefreshTokens.Add(newRefreshToken);

      // remove old refresh tokens from user
      _tokenService.RemoveOldRefreshTokens(user);

      // save changes to db
      await _dbContext.UpdateAndSaveUser(user);

      // generate new jwt
      var jwtToken = _tokenService.GenerateJwtToken(user);

      return new RefreshTokenResponseDto { RefreshToken = newRefreshToken, JwtToken = jwtToken };
    }
  }
}