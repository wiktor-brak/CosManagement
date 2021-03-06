using Ardalis.ApiEndpoints;
using CosManagement.CQRS.Appointments.Commands.Create;
using CosManagement.Dtos.Appointments;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace CosManagement.Endpoints.Appointment;

[Authorize]
public class CreateAppointment : EndpointBaseAsync
	.WithRequest<UpdateApptointmentCommand>
	.WithActionResult<CreateAppointmentDto>
{
	private readonly IMediator _mediator;

	public CreateAppointment(IMediator mediator)
	{
		_mediator = mediator;
	}

	[HttpPost("api/appointments")]
	[SwaggerOperation(
	Summary = "Creates new appointment",
	Description = "Creates new appointment",
	OperationId = "Appointments.Create",
	Tags = new[] { "AppointmentsEndpoints" })]
	public override async Task<ActionResult<CreateAppointmentDto>> HandleAsync(UpdateApptointmentCommand command, CancellationToken cancellationToken = default)
	{
		var appointment = await _mediator.Send(command, cancellationToken);
		return CreatedAtRoute("GetAppointment", new { id = appointment.Id }, appointment);
	}
}