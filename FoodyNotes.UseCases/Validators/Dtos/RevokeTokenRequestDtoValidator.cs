using FluentValidation;
using FoodyNotes.Infrastructure.Interfaces.Authentication.Dtos;

namespace FoodyNotes.UseCases.Validators
{
  public class RevokeTokenRequestDtoValidator : AbstractValidator<RevokeTokenRequestDto>
  {
    public RevokeTokenRequestDtoValidator()
    {
      RuleFor(x => x.RefreshToken).NotEmpty();
    }
  }
}