using CosManagement.Dtos.Clients;
using CosManagement.Interfaces;
using MediatR;

namespace CosManagement.CQRS.Clients.Queries.GetClient;

public class GetClientQuery : IRequest<GetClientDto>, IGetQuery
{
	public Guid Id { get; set; }
}