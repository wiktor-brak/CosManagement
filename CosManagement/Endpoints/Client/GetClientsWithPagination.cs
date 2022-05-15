using Ardalis.ApiEndpoints;
using CosManagement.Common.Models;
using CosManagement.CQRS.Clients.Queries.GetClientsWithPagination;
using CosManagement.Dtos.Clients;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CosManagement.Endpoints.Client;

public class GetClientsWithPagination : EndpointBaseAsync
	.WithRequest<GetClientsWithPaginationQuery>
	.WithActionResult<PaginatedList<GetClientDto>>
{
	private readonly IMediator _mediator;

	public GetClientsWithPagination(IMediator mediator)
	{
		_mediator = mediator;
	}

	[HttpGet("clients")]
	public override async Task<ActionResult<PaginatedList<GetClientDto>>> HandleAsync([FromQuery] GetClientsWithPaginationQuery query, CancellationToken cancellationToken = default)
	{
		return await _mediator.Send(query, cancellationToken);
	}
}