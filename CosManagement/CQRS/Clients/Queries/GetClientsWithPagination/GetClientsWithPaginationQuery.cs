using CosManagement.Common.Models;
using CosManagement.Dtos.Clients;
using MediatR;

namespace CosManagement.CQRS.Clients.Queries.GetClientsWithPagination;

public class GetClientsWithPaginationQuery : IRequest<PaginatedList<GetClientDto>>
{
	public int PageNumber { get; set; } = 1;
	public int PageSize { get; set; } = 10;
}