using Ardalis.ApiEndpoints;
using CosManagement.CQRS.Appointments.Queries.GetAppointment;
using CosManagement.Dtos.Appointments;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace CosManagement.Endpoints.Appointment;

[Authorize]
public class GetAppointment : EndpointBaseAsync
	.WithRequest<Guid>
	.WithActionResult<GetAppointmentDto>
{
	private readonly IMediator _mediator;

	public GetAppointment(IMediator mediator)
	{
		_mediator = mediator;
	}

	[HttpGet("api/appointments/{id}")]
	[SwaggerOperation(
	Summary = "Get one appointment by id (GUID)",
	Description = "Get one appointment by id (GUID)",
	OperationId = "Appointments.Get",
	Tags = new[] { "AppointmentsEndpoints" })]
	public override async Task<ActionResult<GetAppointmentDto>> HandleAsync(Guid id, CancellationToken cancellationToken = default)
	{
		return await _mediator.Send(new GetAppointmentQuery { Id = id }, cancellationToken);
	}
}