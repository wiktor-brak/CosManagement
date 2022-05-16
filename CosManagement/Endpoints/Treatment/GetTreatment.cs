using Ardalis.ApiEndpoints;
using CosManagement.CQRS.Treatments.Queries;
using CosManagement.Dtos.Treatments;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace CosManagement.Endpoints.Treatment;

[Authorize]
public class GetTreatment : EndpointBaseAsync
	.WithRequest<Guid>
	.WithActionResult<GetTreatmentDto>
{
	private readonly IMediator _mediator;

	public GetTreatment(IMediator mediator)
	{
		_mediator = mediator;
	}

	[HttpGet("api/treatments/{id}", Name = "GetTreatment")]
	[SwaggerOperation(
	Summary = "Get one treatment by id (GUID)",
	Description = "Get one treatment by id (GUID)",
	OperationId = "Treatments.Get",
	Tags = new[] { "TreatmentsEndpoints" })]
	public override async Task<ActionResult<GetTreatmentDto>> HandleAsync(Guid id, CancellationToken cancellationToken = default)
	{
		return Ok(await _mediator.Send(new GetTreatmentQuery { Id = id }, cancellationToken));
	}
}