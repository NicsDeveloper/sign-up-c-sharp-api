using FluentValidation;
using SignUpApi.Application.Commands;

namespace SignUpApi.Application.Validators
{
  public class UpdateUserProfileCommandValidator : AbstractValidator<UpdateUserProfileCommand>
  {
    public UpdateUserProfileCommandValidator()
    {
      _ = RuleFor(static x => x.UserId)
          .NotEmpty().WithMessage("ID do usuário é obrigatório");

      _ = RuleFor(static x => x.FirstName)
          .NotEmpty().WithMessage("Nome é obrigatório")
          .MaximumLength(50).WithMessage("Nome deve ter no máximo 50 caracteres")
          .Matches(@"^[a-zA-ZÀ-ÿ\s]+$").WithMessage("Nome deve conter apenas letras e espaços");

      _ = RuleFor(static x => x.LastName)
          .NotEmpty().WithMessage("Sobrenome é obrigatório")
          .MaximumLength(50).WithMessage("Sobrenome deve ter no máximo 50 caracteres")
          .Matches(@"^[a-zA-ZÀ-ÿ\s]+$").WithMessage("Sobrenome deve conter apenas letras e espaços");
    }
  }
}
