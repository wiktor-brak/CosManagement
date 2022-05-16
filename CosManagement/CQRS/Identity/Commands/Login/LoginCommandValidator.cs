using FluentValidation;

namespace CosManagement.CQRS.Identity.Commands.Login;

public class LoginCommandValidator : AbstractValidator<LoginCommand>
{
	public LoginCommandValidator()
	{
		RuleFor(l => l.Username)
			.NotEmpty().WithMessage("Your email address cannot be empty")
			.EmailAddress().WithMessage("You must provide correct email address");

		RuleFor(l => l.Password)
			.NotEmpty().WithMessage("Your password cannot be empty");
	}
}