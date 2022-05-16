using CosManagement.Entities;
using CosManagement.Mappings;

namespace CosManagement.Dtos.Clients;

public class CreateClientDto : BaseClientDto, IMapFrom<Client>
{
	public Guid Id { get; set; }
}