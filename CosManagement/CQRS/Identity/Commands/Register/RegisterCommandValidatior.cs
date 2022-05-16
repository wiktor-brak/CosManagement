using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CosManagement.CQRS.Identity.Commands.Register;

public class RegisterCommandValidatior : AbstractValidator<RegisterCommand>
{
	public RegisterCommandValidatior()
	{
		RuleFor(r => r.Email)
			.NotEmpty().WithMessage("Your email address cannot be empty")
			.EmailAddress().WithMessage("Your input must be email address");

		RuleFor(r => r.Password)
			.NotEmpty().WithMessage("Your password cannot be empty")
			.MinimumLength(6).WithMessage("Your password length must be at least 6.")
			.MaximumLength(100).WithMessage("Your password length must not exceed 100.")
			.Matches(@"[A-Z]+").WithMessage("Your password must contain at least one uppercase letter.")
			.Matches(@"[a-z]+").WithMessage("Your password must contain at least one lowercase letter.")
			.Matches(@"[0-9]+").WithMessage("Your password must contain at least one number.")
			.Matches(@"[^a-zA-Z\d\s:]+").WithMessage("Your password must contain at least one non-alphanumeric character");

		RuleFor(r => r.ConfirmPassoword)
			.Equal(r => r.Password).WithMessage("Your confirmation password doesn't match your password");
	}
}