using CosManagement.Dtos.Clients;
using MediatR;

namespace CosManagement.CQRS.Clients.Queries.GetClient;

public class GetClientQuery : IRequest<GetClientDto>
{
	public Guid Id { get; set; }
}