using FluentValidation;

namespace CosManagement.CQRS.Clients.Commands.Update;

public class UpdateClientCommandValidator : AbstractValidator<UpdateClientCommand>
{
	public UpdateClientCommandValidator()
	{
		RuleFor(r => r.Dto!.FirstName)
			.NotEmpty().WithMessage("First name cannot be empty")
			.MaximumLength(100).WithMessage("First name length must not exceed 100.");

		RuleFor(r => r.Dto!.LastName)
			.NotEmpty().WithMessage("Last name cannot be empty")
			.MaximumLength(100).WithMessage("First name length must not exceed 100.");

		RuleFor(r => r.Dto!.Email)
			.EmailAddress().WithMessage("Provide correct email address")
			.MaximumLength(200).WithMessage("Email length must not exceed 200.");

		RuleFor(r => r.Dto!.Phone)
			.MaximumLength(12).WithMessage("Phone number length must not exceed 100.");

		RuleFor(r => r.Dto!.AdditionalInformations)
			.MaximumLength(1000).WithMessage("Additional information length must not exceed 100.");
	}
}