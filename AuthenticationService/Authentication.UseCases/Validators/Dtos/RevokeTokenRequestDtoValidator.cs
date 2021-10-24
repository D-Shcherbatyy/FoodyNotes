using Authentication.Infrastructure.Interfaces.Authentication.Dtos;
using FluentValidation;

namespace Authentication.UseCases.Validators.Dtos
{
  public class RevokeTokenRequestDtoValidator : AbstractValidator<RevokeTokenRequestDto>
  {
    public RevokeTokenRequestDtoValidator()
    {
      RuleFor(x => x.RefreshToken).NotEmpty();
    }
  }
}