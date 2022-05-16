using CosManagement.Dtos.Clients;
using CosManagement.Entities;
using CosManagement.Mappings;
using MediatR;

namespace CosManagement.CQRS.Clients.Commands.Create;

public class CreateClientCommand : IRequest<CreateClientDto>, IMapFrom<Client>
{
	public string? FirstName { get; set; }
	public string? LastName { get; set; }
	public string? Phone { get; set; }
	public string? Email { get; set; }
	public string? AdditionalInformations { get; set; }
}