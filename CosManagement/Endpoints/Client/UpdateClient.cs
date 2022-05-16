using Ardalis.ApiEndpoints;
using CosManagement.Attributes;
using CosManagement.CQRS.Clients.Commands.Update;
using CosManagement.Dtos.Clients;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace CosManagement.Endpoints.Client;

public class UpdateClientRequest
{
	[FromRoute(Name = "id")] public Guid Id { get; set; }
	[FromBody] public UpdateClientDto? ClientDto { get; set; }
}

[Authorize]
public class UpdateClient : EndpointBaseAsync
	.WithRequest<UpdateClientRequest>
	.WithActionResult
{
	private readonly IMediator _mediator;

	public UpdateClient(IMediator mediator)
	{
		_mediator = mediator;
	}

	[HttpPut("api/clients/{id}")]
	[SwaggerOperation(
	Summary = "Update client by id",
	Description = "Update client by id",
	OperationId = "Clients.Update",
	Tags = new[] { "ClientsEndpoints" })]
	public override async Task<ActionResult> HandleAsync([FormMultiSource] UpdateClientRequest request, CancellationToken cancellationToken = default)
	{
		await _mediator.Send(new UpdateClientCommand { Id = request.Id, Dto = request.ClientDto }, cancellationToken);
		return Ok();
	}
}