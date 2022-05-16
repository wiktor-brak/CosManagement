using Ardalis.ApiEndpoints;
using CosManagement.CQRS.Clients.Commands.Delete;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace CosManagement.Endpoints.Client;

[Authorize]
public class DeleteClient : EndpointBaseAsync
	.WithRequest<Guid>
	.WithActionResult
{
	private readonly IMediator _mediator;

	public DeleteClient(IMediator mediator)
	{
		_mediator = mediator;
	}

	[HttpDelete("api/clients/{id}")]
	[SwaggerOperation(
	Summary = "Delete client by id",
	Description = "Delete client by id",
	OperationId = "Clients.Delete",
	Tags = new[] { "ClientsEndpoints" })]
	public override async Task<ActionResult> HandleAsync(Guid id, CancellationToken cancellationToken = default)
	{
		await _mediator.Send(new DeleteClientCommand { Id = id }, cancellationToken);
		return NoContent();
	}
}