using Dogshouse.DTOs;
using FluentValidation;

namespace Dogshouse.Validation;

public class CreateDogDtoValidator : AbstractValidator<CreateDogDto>
{
    public CreateDogDtoValidator()
    {
        RuleFor(d => d.Name)
            .Cascade(CascadeMode.Continue)
            .NotEmpty().WithMessage("Name cannot be empty.")
            .MaximumLength(50).WithMessage("Name cannot exceed 50 characters.");

        RuleFor(d => d.Color)
            .Cascade(CascadeMode.Continue)
            .MaximumLength(50).WithMessage("Color cannot exceed 50 characters.");

        RuleFor(d => d.TailLength)
            .Cascade(CascadeMode.Continue)
            .GreaterThanOrEqualTo(0).WithMessage("Tail length cannot be negative.");

        RuleFor(d => d.Weight)
            .Cascade(CascadeMode.Continue)
            .GreaterThan(0).WithMessage("Weight must be greater than 0.");
    }
}
