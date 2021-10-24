using System.Threading;
using System.Threading.Tasks;
using Authentication.Entities.Authentication.Entities;
using Authentication.Infrastructure.Interfaces.Authentication;
using Authentication.Infrastructure.Interfaces.Persistence;
using Authentication.UseCases.Authentication.Commands;
using Authentication.UseCases.Authentication.Handlers;
using AutoFixture;
using AutoFixture.AutoMoq;
using Moq;
using Xunit;

namespace Authentication.UseCases.UnitTests.Authentication.Handlers
{
  public class AuthenticateCommandHandlerTests
  {
    private Mock<IRepositoryBase<User, string>> _mockIUserRepo;
    private Mock<IJwtTokenService> _mockJwtTokenService;
    // private Mock<IRefreshTokenService> _mockIRefreshTokenService;
    private Mock<IGoogleService> _mockIGoogleService;
    private readonly IFixture _fixture;

    public AuthenticateCommandHandlerTests()
    {
      _fixture = new Fixture().Customize(new AutoMoqCustomization());

      _mockIUserRepo = _fixture.Freeze<Mock<IRepositoryBase<User, string>>>();
      _mockJwtTokenService = _fixture.Freeze<Mock<IJwtTokenService>>();
      _mockIGoogleService = _fixture.Freeze<Mock<IGoogleService>>();
    }
    
    [Fact]
    public async Task Handle_UserExists_ShouldUpdateUser()
    {
      var authenticateCommandHandler = _fixture.Create<AuthenticateCommandHandler>();
      var authenticateCommand = _fixture.Create<AuthenticateCommand>();
      
      var user1 = _fixture.Create<User>();

      _mockIGoogleService.Setup(x => x.GetUserIdByIdTokenAsync(It.IsAny<string>())).ReturnsAsync(user1.Id);

      _mockIUserRepo.Setup(x => x.GetByIdAsync(user1.Id, CancellationToken.None)).ReturnsAsync(user1);

      var authenticateResponseDto = await authenticateCommandHandler.Handle(authenticateCommand, CancellationToken.None);
      
      _mockIUserRepo.Verify(x => x.SaveChangesAsync(CancellationToken.None), Times.Once);
    }
  }
}