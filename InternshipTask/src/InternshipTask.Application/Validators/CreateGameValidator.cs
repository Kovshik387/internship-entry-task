using FluentValidation;
using InternshipTask.Application.DTOs;

namespace InternshipTask.Application.Validators;

public class CreateGameValidator :  AbstractValidator<CreateGameDto>
{
    public CreateGameValidator()
    {
        RuleFor(x => x.PlayerIdO)
            .NotNull()
            .NotEmpty()
            .WithMessage("PlayerIdO is required.");
        RuleFor(x => x.PlayerIdX)
            .NotNull()
            .NotEmpty()
            .WithMessage("PlayerIdX is required.");
        RuleFor(x => x.PlayerIdO != x.PlayerIdX)
            .NotNull()
            .NotEmpty()
            .WithMessage("PlayerIdO is required.");
    }
}