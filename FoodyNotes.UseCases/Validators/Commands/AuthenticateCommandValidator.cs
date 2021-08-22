using FluentValidation;
using FoodyNotes.UseCases.Authentication.Commands;

namespace FoodyNotes.UseCases.Validators.Commands
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