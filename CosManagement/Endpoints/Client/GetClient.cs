using Ardalis.ApiEndpoints;
using CosManagement.CQRS.Clients.Queries.GetClient;
using CosManagement.Dtos.Clients;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CosManagement.Endpoints.Client;

public class GetClient : EndpointBaseAsync
	.WithRequest<Guid>
	.WithActionResult<GetClientDto>
{
	private readonly IMediator _mediator;

	public GetClient(IMediator mediator)
	{
		_mediator = mediator;
	}

	[HttpGet("clients/{id}")]
	public override async Task<ActionResult<GetClientDto>> HandleAsync(Guid id, CancellationToken cancellationToken = default)
	{
		return await _mediator.Send(new GetClientQuery { Id = id }, cancellationToken);
	}
}