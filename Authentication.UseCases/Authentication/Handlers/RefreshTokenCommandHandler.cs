using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Authentication.Entities.Authentication.Entities;
using Authentication.Infrastructure.Interfaces.Authentication;
using Authentication.Infrastructure.Interfaces.Authentication.Dtos;
using Authentication.Infrastructure.Interfaces.Persistence;
using Authentication.UseCases.Authentication.Commands;
using Authentication.UseCases.Exceptions;
using MediatR;

namespace Authentication.UseCases.Authentication.Handlers
{
  public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, RefreshTokenResponseDto>
  {
    private readonly IRepositoryBase<User, string> _userRepo;
    private readonly IJwtTokenService _jwtTokenService;
    private readonly IRefreshTokenService _refreshTokenService;

    public RefreshTokenCommandHandler(IRepositoryBase<User, string> userRepo, IJwtTokenService jwtTokenService, IRefreshTokenService refreshTokenService)
    {
      _userRepo = userRepo;
      _jwtTokenService = jwtTokenService;
      _refreshTokenService = refreshTokenService;

    }
    
    public async Task<RefreshTokenResponseDto> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
      var user = await _refreshTokenService.GetUserByRefreshTokenAsync(request.CurrentRefreshToken);
      var refreshToken = user.RefreshTokens.Single(x => x.Token == request.CurrentRefreshToken);

      if (refreshToken.IsRevoked)
      {
        // ALARM! ALARM!
        // revoke all descendant tokens in case this token has been compromised
        _refreshTokenService.RevokeDescendantRefreshTokens(refreshToken, user, request.IpAddress, $"Attempted reuse of revoked ancestor token: {request.CurrentRefreshToken}");
        
        _userRepo.Update(user);
        await _userRepo.SaveChangesAsync(cancellationToken);
      }

      if (!refreshToken.IsActive)
        throw new AppException("Invalid token");

      // replace old refresh token with a new one (rotate token)
      var newRefreshToken = _refreshTokenService.RotateRefreshToken(refreshToken, request.IpAddress);
      user.RefreshTokens.Add(newRefreshToken);

      // remove old refresh tokens from user
      _refreshTokenService.RemoveOldRefreshTokens(user);

      // save changes to db
      _userRepo.Update(user);
      await _userRepo.SaveChangesAsync(cancellationToken);

      // generate new jwt
      var jwtToken = _jwtTokenService.GenerateJwtToken(user);

      return new RefreshTokenResponseDto { RefreshToken = newRefreshToken, JwtToken = jwtToken };
    }
  }
}