using FluentValidation;
using FoodyNotes.Infrastructure.Interfaces.Authentication.Dtos;

namespace FoodyNotes.UseCases.Validators
{
  public class AuthenticateRequestDtoValidator : AbstractValidator<AuthenticateRequestDto>
  {
    public AuthenticateRequestDtoValidator()
    {
      RuleFor(x => x.IdToken).NotEmpty();
    }
  }
}