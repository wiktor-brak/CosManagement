using FluentValidation;

namespace CosManagement.CQRS.Categories.Commands.Create;

public class CreateCategoryCommandValidator : AbstractValidator<CreateCategoryCommand>
{
	public CreateCategoryCommandValidator()
	{
		RuleFor(r => r.Name)
			.NotEmpty().WithMessage("Name cannot be empty")
			.MaximumLength(500).WithMessage("Name length must not exceed 500.");
	}
}