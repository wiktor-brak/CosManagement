using Ardalis.ApiEndpoints;
using CosManagement.Common.Models;
using CosManagement.CQRS.Clients.Queries.GetClientsWithPagination;
using CosManagement.Dtos.Clients;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace CosManagement.Endpoints.Client;

[Authorize]
public class GetClientsWithPagination : EndpointBaseAsync
	.WithRequest<GetClientsWithPaginationQuery>
	.WithActionResult<PaginatedList<GetClientDto>>
{
	private readonly IMediator _mediator;

	public GetClientsWithPagination(IMediator mediator)
	{
		_mediator = mediator;
	}

	[HttpGet("api/clients")]
	[SwaggerOperation(
	Summary = "Get paginated list of clients with 10 entires",
	Description = "Get paginated list of clients with 10 entires",
	OperationId = "Clients.GetAll",
	Tags = new[] { "ClientsEndpoints" })]
	public override async Task<ActionResult<PaginatedList<GetClientDto>>> HandleAsync([FromQuery] GetClientsWithPaginationQuery query, CancellationToken cancellationToken = default)
	{
		return Ok(await _mediator.Send(query, cancellationToken));
	}
}