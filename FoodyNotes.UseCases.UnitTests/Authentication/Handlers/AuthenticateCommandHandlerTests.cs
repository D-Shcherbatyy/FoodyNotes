using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using AutoFixture.AutoMoq;
using FluentAssertions;
using FoodyNotes.Entities.Authentication.Entities;
using FoodyNotes.Infrastructure.Interfaces;
using FoodyNotes.Infrastructure.Interfaces.Authentication;
using FoodyNotes.Infrastructure.Interfaces.Persistence;
using FoodyNotes.UseCases.Authentication.Commands;
using FoodyNotes.UseCases.Authentication.Handlers;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace FoodyNotes.UseCases.UnitTests.Authentication.Handlers
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