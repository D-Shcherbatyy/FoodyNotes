using Authentication.UseCases.Authentication.Commands;
using FluentValidation;

namespace Authentication.UseCases.Validators.Commands
{
  public class AuthenticateCommandValidator : AbstractValidator<AuthenticateCommand>
  {
    public AuthenticateCommandValidator()
    {
      RuleFor(x => x.IpAddress).NotEmpty();
      RuleFor(x => x.RequestDto).NotNull();
      RuleFor(x => x.RequestDto.IdToken).NotEmpty();
    }
  }
}