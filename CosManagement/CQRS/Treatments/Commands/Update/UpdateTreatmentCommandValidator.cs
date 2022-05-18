using FluentValidation;

namespace CosManagement.CQRS.Treatments.Commands.Update;

public class UpdateTreatmentCommandValidator : AbstractValidator<UpdateTreatmentCommand>
{
	public UpdateTreatmentCommandValidator()
	{
		RuleFor(r => r.Dto!.Name)
			.NotEmpty().WithMessage("Name cannot be empty")
			.MaximumLength(500).WithMessage("Name length must not exceed 500.");

		RuleFor(r => r.Dto!.BasePrice)
			.NotEmpty().WithMessage("Base cannot be empty")
			.GreaterThan(0).WithMessage("Price must be greater than 0");
	}
}