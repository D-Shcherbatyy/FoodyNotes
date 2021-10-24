using Authentication.Infrastructure.Interfaces.Authentication.Dtos;
using FluentValidation;

namespace Authentication.UseCases.Validators.Dtos
{
  public class AuthenticateRequestDtoValidator : AbstractValidator<AuthenticateRequestDto>
  {
    public AuthenticateRequestDtoValidator()
    {
      RuleFor(x => x.IdToken).NotEmpty();
    }
  }
}