using FluentValidation;
using InternshipTask.Application.DTOs;

namespace InternshipTask.Application.Validators;

public class MakeMoveValidator : AbstractValidator<MakeMoveDto>
{
    public MakeMoveValidator()
    {
        RuleFor(x => x.PlayerId)
            .NotNull()
            .NotEmpty()
            .WithMessage("PlayerId is required.");
        RuleFor(x => x.Column)
            .GreaterThan(0)
            .NotNull()
            .WithMessage("Column is required.");
        RuleFor(x => x.Row)
            .GreaterThan(0)
            .NotNull()
            .WithMessage("Row is required.");
        RuleFor(x => x.GameId)
            .NotNull()
            .NotEmpty()
            .WithMessage("GameId is required.");
    }
}