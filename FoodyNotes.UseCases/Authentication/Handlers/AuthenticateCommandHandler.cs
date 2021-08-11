using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FoodyNotes.Entities.Authentication.Entities;
using FoodyNotes.Infrastructure.Interfaces;
using FoodyNotes.Infrastructure.Interfaces.Authentication;
using FoodyNotes.Infrastructure.Interfaces.Authentication.Dtos;
using FoodyNotes.UseCases.Authentication.Commands;
using MediatR;

namespace FoodyNotes.UseCases.Authentication.Handlers
{
  public class AuthenticateCommandHandler : IRequestHandler<AuthenticateCommand, AuthenticateResponseDto>
  {
    private readonly IDbContext _dbContext;
    private readonly ITokenService _tokenService;
    private readonly IGoogleService _googleService;

    public AuthenticateCommandHandler(IDbContext dbContext, ITokenService tokenService, IGoogleService googleService)
    {
      _dbContext = dbContext;
      _tokenService = tokenService;
      _googleService = googleService;
    }
    
    public async Task<AuthenticateResponseDto> Handle(AuthenticateCommand request, CancellationToken cancellationToken)
    {
      // return _authService.Authenticate(request.RequestDto, request.IpAddress);
      
      var userId = await _googleService.GetUserIdByIdToken(request.RequestDto.IdToken);
      
      var user = _dbContext.Users.SingleOrDefault(x => x.Id == userId);
      
      var refreshToken = _tokenService.GenerateRefreshToken(request.IpAddress);
      
      // validate
      if (user == null)
      {
        user = new User { Id = userId, RefreshTokens = new List<RefreshToken> { refreshToken }};
        
        _dbContext.Users.Add(user);

        await _dbContext.SaveChangesAsync();
      }
      else
      {
        user.RefreshTokens.Add(refreshToken);
        
        _tokenService.RemoveOldRefreshTokens(user);
        
        await _dbContext.UpdateAndSaveUser(user);
      }

      var jwtToken = _tokenService.GenerateJwtToken(user);

      return new AuthenticateResponseDto(user, jwtToken, refreshToken.Token);
    }
  }
}