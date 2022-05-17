using FluentValidation;

namespace CosManagement.CQRS.Categories.Commands.Update;

public class UpdateCategoryCommandValidator : AbstractValidator<UpdateCategoryCommand>
{
	public UpdateCategoryCommandValidator()
	{
		RuleFor(r => r.Dto!.Name)
			.NotEmpty().WithMessage("Name cannot be empty")
			.MaximumLength(500).WithMessage("Name length must not exceed 500.");
	}
}