using Ardalis.ApiEndpoints;
using CosManagement.Common.Models;
using CosManagement.CQRS.Treatments.Queries.GetTreatmentWithPagination;
using CosManagement.Dtos.Treatments;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace CosManagement.Endpoints.Treatment;

[Authorize]
public class GetTreatmetnsWithPagination : EndpointBaseAsync
	.WithRequest<GetTreatmentsWithPaginationQuery>
	.WithActionResult<PaginatedList<GetTreatmentDto>>
{
	private readonly IMediator _mediator;

	public GetTreatmetnsWithPagination(IMediator mediator)
	{
		_mediator = mediator;
	}

	[HttpGet("api/treatments")]
	[SwaggerOperation(
	Summary = "Get paginated list  of treatments with 10 entires",
	Description = "Get paginated list of treatments with 10 entires",
	OperationId = "Treatments.GetAll",
	Tags = new[] { "TreatmentsEndpoints" })]
	public override async Task<ActionResult<PaginatedList<GetTreatmentDto>>> HandleAsync([FromQuery] GetTreatmentsWithPaginationQuery query, CancellationToken cancellationToken = default)
	{
		return await _mediator.Send(query, cancellationToken);
	}
}