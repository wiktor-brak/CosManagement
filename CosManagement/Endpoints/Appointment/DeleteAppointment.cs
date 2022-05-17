using Ardalis.ApiEndpoints;
using CosManagement.CQRS.Appointments.Commands.Delete;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace CosManagement.Endpoints.Appointment;

[Authorize]
public class DeleteAppointment : EndpointBaseAsync
	.WithRequest<Guid>
	.WithActionResult
{
	private readonly IMediator _mediator;

	public DeleteAppointment(IMediator mediator)
	{
		_mediator = mediator;
	}

	[HttpDelete("api/appointments/{id}")]
	[SwaggerOperation(
	Summary = "Delete appointments by id",
	Description = "Delete appointments by id",
	OperationId = "Appointments.Delete",
	Tags = new[] { "AppointmentsEndpoints" })]
	public override async Task<ActionResult> HandleAsync(Guid id, CancellationToken cancellationToken = default)
	{
		await _mediator.Send(new DeleteAppointmentCommand { Id = id }, cancellationToken);
		return NoContent();
	}
}