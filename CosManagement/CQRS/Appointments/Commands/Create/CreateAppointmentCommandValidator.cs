using FluentValidation;

namespace CosManagement.CQRS.Appointments.Commands.Create;

public class CreateAppointmentCommandValidator : AbstractValidator<CreateAppointmentCommand>
{
	public CreateAppointmentCommandValidator()
	{
		RuleFor(r => r.Date)
			.NotEmpty().WithMessage("Appointment date cannot be empty");

		RuleFor(r => r.Note)
			.MaximumLength(500).WithMessage("First name length must not exceed 500.");

		RuleFor(r => r.ClientId)
			.NotEmpty().WithMessage("Client must be provided");
	}
}