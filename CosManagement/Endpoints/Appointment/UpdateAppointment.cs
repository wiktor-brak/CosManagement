using Ardalis.ApiEndpoints;
using CosManagement.Attributes;
using CosManagement.CQRS.Appointments.Commands.Update;
using CosManagement.Dtos.Appointments;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace CosManagement.Endpoints.Appointment;

public class UpdateAppointmentRequest
{
	[FromRoute(Name = "id")] public Guid Id { get; set; }
	[FromBody] public UpdateAppointmentDto? AppointmentDto { get; set; }
}

[Authorize]
public class UpdateAppointment : EndpointBaseAsync
	.WithRequest<UpdateAppointmentRequest>
	.WithActionResult
{
	private readonly IMediator _mediator;

	public UpdateAppointment(IMediator mediator)
	{
		_mediator = mediator;
	}

	[HttpPut("api/appointments/{id}")]
	[SwaggerOperation(
	Summary = "Update appointment by id",
	Description = "Update appointment by id",
	OperationId = "Appointments.Update",
	Tags = new[] { "AppointmentsEndpoints" })]
	public override async Task<ActionResult> HandleAsync([FormMultiSource] UpdateAppointmentRequest request, CancellationToken cancellationToken = default)
	{
		await _mediator.Send(new UpdateAppointmentCommand { Id = request.Id, Dto = request.AppointmentDto }, cancellationToken);
		return Ok();
	}
}