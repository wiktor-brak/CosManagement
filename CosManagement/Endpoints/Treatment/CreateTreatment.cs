using Ardalis.ApiEndpoints;
using CosManagement.CQRS.Treatments.Commands.Create;
using CosManagement.Dtos.Treatments;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace CosManagement.Endpoints.Treatment;

[Authorize]
public class CreateTreatment : EndpointBaseAsync
	.WithRequest<CreateTreatmentCommand>
	.WithActionResult<CreateTreatmentDto>
{
	private readonly IMediator _mediator;

	public CreateTreatment(IMediator mediator)
	{
		_mediator = mediator;
	}

	[HttpPost("api/treatments")]
	[SwaggerOperation(
	Summary = "Creates new treatment",
	Description = "Creates new treatment",
	OperationId = "Treatment.Create",
	Tags = new[] { "TreatmentsEndpoints" })]
	public override async Task<ActionResult<CreateTreatmentDto>> HandleAsync(CreateTreatmentCommand command, CancellationToken cancellationToken = default)
	{
		var treatment = await _mediator.Send(command, cancellationToken);
		return CreatedAtRoute("GetTreatment", new { id = treatment.Id }, treatment);
	}
}