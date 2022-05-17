using Ardalis.ApiEndpoints;
using CosManagement.Attributes;
using CosManagement.CQRS.Treatments.Commands.Update;
using CosManagement.Dtos.Treatments;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace CosManagement.Endpoints.Client;

public class UpdateTreatmentRequest
{
	[FromRoute(Name = "id")] public Guid Id { get; set; }
	[FromBody] public UpdateTreatmentDto? ClientDto { get; set; }
}

[Authorize]
public class UpdateTreatment : EndpointBaseAsync
	.WithRequest<UpdateTreatmentRequest>
	.WithActionResult
{
	private readonly IMediator _mediator;

	public UpdateTreatment(IMediator mediator)
	{
		_mediator = mediator;
	}

	[HttpPut("api/treatments/{id}")]
	[SwaggerOperation(
	Summary = "Update treatment by id",
	Description = "Update treatment by id",
	OperationId = "Treatments.Update",
	Tags = new[] { "TreatmentsEndpoints" })]
	public override async Task<ActionResult> HandleAsync([FormMultiSource] UpdateTreatmentRequest request, CancellationToken cancellationToken = default)
	{
		await _mediator.Send(new UpdateTreatmentCommand { Id = request.Id, Dto = request.ClientDto }, cancellationToken);
		return Ok();
	}
}