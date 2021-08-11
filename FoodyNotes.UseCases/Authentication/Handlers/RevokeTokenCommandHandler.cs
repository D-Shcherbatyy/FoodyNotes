using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FoodyNotes.Infrastructure.Interfaces;
using FoodyNotes.Infrastructure.Interfaces.Authentication;
using FoodyNotes.UseCases.Authentication.Commands;
using FoodyNotes.UseCases.Exceptions;
using MediatR;

namespace FoodyNotes.UseCases.Authentication.Handlers
{
  public class RevokeTokenCommandHandler : AsyncRequestHandler<RevokeTokenCommand>
  {
    private readonly IDbContext _dbContext;
    private readonly ITokenService _tokenService;

    public RevokeTokenCommandHandler(IDbContext dbContext, ITokenService tokenService)
    {
      _dbContext = dbContext;
      _tokenService = tokenService;

    }

    protected override async Task Handle(RevokeTokenCommand request, CancellationToken cancellationToken)
    {
      var user = _tokenService.GetUserByRefreshToken(request.RefreshToken);
      var refreshToken = user.RefreshTokens.Single(x => x.Token == request.RefreshToken);

      if (!refreshToken.IsActive)
        throw new AppException("Invalid token");

      // revoke token and save
      _tokenService.RevokeRefreshToken(refreshToken, request.IpAddress, "Revoked without replacement");
      
      await _dbContext.UpdateAndSaveUser(user);
    }
  }
}