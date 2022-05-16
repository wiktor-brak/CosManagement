using FluentValidation;

namespace CosManagement.CQRS.Treatments.Commands.Create;

public class CreateTreatmentCommandValidator : AbstractValidator<CreateTreatmentCommand>
{
	public CreateTreatmentCommandValidator()
	{
		RuleFor(r => r.Name)
			.NotEmpty().WithMessage("Name cannot be empty")
			.MaximumLength(500).WithMessage("Name length must not exceed 500.");

		RuleFor(r => r.CategoryId)
			.NotEmpty().WithMessage("Category cannot be empty");

		RuleFor(r => r.BasePrice)
			.NotEmpty().WithMessage("Price cannot be empty");
	}
}