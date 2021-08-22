using FluentValidation;
using FoodyNotes.UseCases.Authentication.Commands;

namespace FoodyNotes.UseCases.Validators.Commands
{
  public class RefreshTokenCommandValidator : AbstractValidator<RefreshTokenCommand>
  {
    public RefreshTokenCommandValidator()
    {
      RuleFor(x => x.IpAddress).NotEmpty();
      RuleFor(x => x.CurrentRefreshToken).NotEmpty();
    }
  }
}