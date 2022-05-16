using Ardalis.ApiEndpoints;
using CosManagement.Common.Models;
using CosManagement.CQRS.Appointments.Queries.GetAppointments;
using CosManagement.Dtos.Appointments;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace CosManagement.Endpoints.Category;

[Authorize]
public class GetAppointmentsWithPagination : EndpointBaseAsync
	.WithRequest<GetAppointmentsWithPaginationQuery>
	.WithActionResult<PaginatedList<GetAppointmentDto>>
{
	private readonly IMediator _mediator;

	public GetAppointmentsWithPagination(IMediator mediator)
	{
		_mediator = mediator;
	}

	[HttpGet("api/appointments")]
	[SwaggerOperation(
	Summary = "Get paginated list of appointments  with 10 entires",
	Description = "Get paginated list of appointments with 10 entires",
	OperationId = "Appointments.GetAll",
	Tags = new[] { "AppointmentsEndpoints" })]
	public override async Task<ActionResult<PaginatedList<GetAppointmentDto>>> HandleAsync([FromQuery] GetAppointmentsWithPaginationQuery query, CancellationToken cancellationToken = default)
	{
		return Ok(await _mediator.Send(query, cancellationToken));
	}
}