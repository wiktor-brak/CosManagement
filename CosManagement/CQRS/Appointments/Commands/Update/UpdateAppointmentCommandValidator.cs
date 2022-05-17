using FluentValidation;

namespace CosManagement.CQRS.Appointments.Commands.Update;

public class UpdateAppointmentCommandValidator : AbstractValidator<UpdateAppointmentCommand>
{
	public UpdateAppointmentCommandValidator()
	{
		RuleFor(r => r.Dto!.Date)
			.NotEmpty().WithMessage("Appointment date cannot be empty");

		RuleFor(r => r.Dto!.Note)
			.MaximumLength(500).WithMessage("First name length must not exceed 500.");

		RuleFor(r => r.Dto!.ClientId)
			.NotEmpty().WithMessage("Client must be provided");
	}
}