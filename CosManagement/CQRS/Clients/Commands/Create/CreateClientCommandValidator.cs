using FluentValidation;

namespace CosManagement.CQRS.Clients.Commands.Create;

public class CreateClientCommandValidator : AbstractValidator<CreateClientCommand>
{
	public CreateClientCommandValidator()
	{
		RuleFor(r => r.FirstName)
			.NotEmpty().WithMessage("First name cannot be empty")
			.MaximumLength(100).WithMessage("First name length must not exceed 100.");

		RuleFor(r => r.LastName)
			.NotEmpty().WithMessage("First name cannot be empty")
			.MaximumLength(100).WithMessage("First name length must not exceed 100.");

		RuleFor(r => r.Email)
			.EmailAddress().WithMessage("Provide correct email address")
			.MaximumLength(200).WithMessage("First name length must not exceed 100.");

		RuleFor(r => r.Phone)
			.MaximumLength(12).WithMessage("Phone number length must not exceed 100.");

		RuleFor(r => r.AdditionalInformations)
			.MaximumLength(1000).WithMessage("Additional information length must not exceed 100.");
	}
}