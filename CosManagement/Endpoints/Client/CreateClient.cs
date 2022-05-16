using Ardalis.ApiEndpoints;
using CosManagement.CQRS.Clients.Commands.Create;
using CosManagement.Dtos.Clients;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace CosManagement.Endpoints.Client;

[Authorize]
public class CreateClient : EndpointBaseAsync
	.WithRequest<CreateClientCommand>
	.WithActionResult<CreateClientDto>
{
	private readonly IMediator _mediator;

	public CreateClient(IMediator mediator)
	{
		_mediator = mediator;
	}

	[HttpPost("api/clients")]
	[SwaggerOperation(
	Summary = "Creates new client",
	Description = "Creates new client",
	OperationId = "Clients.Create",
	Tags = new[] { "ClientsEndpoints" })]
	public override async Task<ActionResult<CreateClientDto>> HandleAsync(CreateClientCommand command, CancellationToken cancellationToken = default)
	{
		var client = await _mediator.Send(command, cancellationToken);
		return CreatedAtRoute("GetClient", new { id = client.Id }, client);
	}
}