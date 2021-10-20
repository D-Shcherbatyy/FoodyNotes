using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FoodyNotes.Entities.Authentication.Entities;
using FoodyNotes.Infrastructure.Interfaces.Authentication;
using FoodyNotes.Infrastructure.Interfaces.Authentication.Dtos;
using FoodyNotes.Infrastructure.Interfaces.Persistence;
using FoodyNotes.UseCases.Authentication.Commands;
using MediatR;

namespace FoodyNotes.UseCases.Authentication.Handlers
{
  public class AuthenticateCommandHandler : IRequestHandler<AuthenticateCommand, AuthenticateResponseDto>
  {
    private readonly IRepositoryBase<User, string> _userRepo;
    private readonly IJwtTokenService _jwtTokenService;
    private readonly IRefreshTokenService _refreshTokenService;
    private readonly IGoogleService _googleService;

    public AuthenticateCommandHandler(IRepositoryBase<User, string> userRepo,
      IJwtTokenService jwtTokenService,
      IRefreshTokenService refreshTokenService,
      IGoogleService googleService)
    {
      _userRepo = userRepo;
      _jwtTokenService = jwtTokenService;
      _refreshTokenService = refreshTokenService;
      _googleService = googleService;
    }
    
    public async Task<AuthenticateResponseDto> Handle(AuthenticateCommand request, CancellationToken cancellationToken)
    {
      var userId = await _googleService.GetUserIdByIdTokenAsync(request.RequestDto.IdToken);
      var user = await _userRepo.GetByIdAsync(userId, cancellationToken);

      var refreshToken = _refreshTokenService.GenerateRefreshToken(request.IpAddress);
      
      // create user if it doesn't exist in db
      if (user == null)
      {
        user = new User { Id = userId, RefreshTokens = new List<RefreshToken> { refreshToken }};

        await _userRepo.AddAsync(user, cancellationToken);
        await _userRepo.SaveChangesAsync(cancellationToken);
      }
      else
      {
        user.RefreshTokens.Add(refreshToken);
        
        _refreshTokenService.RemoveOldRefreshTokens(user);

        await _userRepo.SaveChangesAsync(cancellationToken);
      }

      var jwtToken = _jwtTokenService.GenerateJwtToken(user);

      return new AuthenticateResponseDto(user, jwtToken, refreshToken.Token);
    }
  }
}