using Ardalis.ApiEndpoints;
using CosManagement.CQRS.Clients.Queries.GetClient;
using CosManagement.Dtos.Clients;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace CosManagement.Endpoints.Client;

[Authorize]
public class GetClient : EndpointBaseAsync
	.WithRequest<Guid>
	.WithActionResult<GetClientDto>
{
	private readonly IMediator _mediator;

	public GetClient(IMediator mediator)
	{
		_mediator = mediator;
	}

	[HttpGet("api/clients/{id}", Name = "GetClient")]
	[SwaggerOperation(
	Summary = "Get one client by id (GUID)",
	Description = "Get one client by id (GUID)",
	OperationId = "Clients.Get",
	Tags = new[] { "ClientsEndpoints" })]
	public override async Task<ActionResult<GetClientDto>> HandleAsync(Guid id, CancellationToken cancellationToken = default)
	{
		return Ok(await _mediator.Send(new GetClientQuery { Id = id }, cancellationToken));
	}
}