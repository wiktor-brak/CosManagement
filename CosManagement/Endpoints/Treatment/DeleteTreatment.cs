using Ardalis.ApiEndpoints;
using CosManagement.CQRS.Treatments.Commands.Delete;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace CosManagement.Endpoints.Treatment;

[Authorize]
public class DeleteTreatment : EndpointBaseAsync
	.WithRequest<Guid>
	.WithActionResult
{
	private readonly IMediator _mediator;

	public DeleteTreatment(IMediator mediator)
	{
		_mediator = mediator;
	}

	[HttpDelete("api/treatments/{id}")]
	[SwaggerOperation(
	Summary = "Delete treatment by id",
	Description = "Delete treatment by id",
	OperationId = "Treatment.Delete",
	Tags = new[] { "TreatmentsEndpoints" })]
	public override async Task<ActionResult> HandleAsync(Guid id, CancellationToken cancellationToken = default)
	{
		await _mediator.Send(new DeleteTreatmentCommand { Id = id }, cancellationToken);
		return NoContent();
	}
}