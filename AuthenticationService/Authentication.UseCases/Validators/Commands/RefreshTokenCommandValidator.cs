using Authentication.UseCases.Authentication.Commands;
using FluentValidation;

namespace Authentication.UseCases.Validators.Commands
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